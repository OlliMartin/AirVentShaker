using System.Globalization;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oma.AirVentShaker.Api.Model;
using Oma.AirVentShaker.Api.Model.Events;
using Oma.AirVentShaker.Api.Model.Settings;
using Oma.WndwCtrl.Abstractions.Messaging.Interfaces;

namespace Oma.AirVentShaker.Api.Messaging.Consumers;

public sealed class TimeSeriesPersistorMessageConsumer : IMessageConsumer<GForceValueBatchEvent>, IDisposable
{
  private readonly InfluxDBClient _influx;

  private readonly ILogger<TimeSeriesPersistorMessageConsumer> _logger;
  private readonly GlobalState _globalState;
  private readonly InfluxSettings _influxSettings;

  public TimeSeriesPersistorMessageConsumer(
    ILogger<TimeSeriesPersistorMessageConsumer> logger,
    IOptions<InfluxSettings> influxOptions,
    GlobalState globalState)
  {
    _logger = logger;
    _globalState = globalState;

    _influxSettings = influxOptions.Value;
    
    _influx = new(
      _influxSettings.Url,
      _influxSettings.Token
    );
  }

  public void Dispose()
  {
    _influx.Dispose();
  }

  public bool IsSubscribedTo(IMessage message) => message is GForceValueBatchEvent;

  public Task OnExceptionAsync(IMessage message, Exception exception, CancellationToken cancelToken = default)
  {
    _logger.LogError(
      exception,
      "An unexpected error occurred processing event {type}.",
      message.GetType().Name
    );

    return Task.CompletedTask;
  }

  public async Task OnMessageAsync(GForceValueBatchEvent message, CancellationToken cancelToken = default)
  {
    if(_globalState.Stage is not TestStage.Calibrate and not TestStage.Run)
    {
      return;
    }
    
    IEnumerable<string> stepNames = message.DataPoints
      .DistinctBy(dp => dp.TestStep?.ToString() ?? "none")
      .Select(dp => dp.TestStep?.ToString() ?? "none");

    _logger.LogDebug(
      "Received {count} events. Average value: {avg} Test Steps: [{stepNames}]",
      message.DataPoints.Count,
      message.DataPoints.Average(dp => dp.NetForce),
      string.Join(", ", stepNames)
    );

    WriteApiAsync? writeApi = _influx.GetWriteApiAsync();

    List<PointData> points = message.DataPoints.Select(
      dp => PointData.Measurement("g-force")
        .Field("value", (double)dp.NetForce)
        .Field("target", (double)(dp.TestStep?.TargetGravitationalForce ?? 0))
        .Field("amplitude", dp.TestStep?.Amplitude ?? 0)
        .Timestamp(dp.AsOf, WritePrecision.Ms)
        .Tag("targetG", (dp.TestStep?.TargetGravitationalForce ?? -1).ToString(CultureInfo.InvariantCulture))
        .Tag("freq", (dp.TestStep?.Frequency ?? -1).ToString(CultureInfo.InvariantCulture))
        .Tag("test-name", dp.TestDefinition?.Name ?? "None")
    ).ToList();

    await writeApi.WritePointsAsync(points, _influxSettings.Bucket, _influxSettings.Org, cancelToken);
  }
}