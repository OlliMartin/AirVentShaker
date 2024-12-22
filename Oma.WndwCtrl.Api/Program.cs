using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Api;
using Oma.WndwCtrl.Configuration.Model;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { });

IApiService apiService = new CtrlApiService(
    loggerFactory.CreateLogger<CtrlApiService>(),
    await ComponentConfigurationAccessor.FromFileAsync()
);

await apiService.StartAsync(CancellationToken.None);