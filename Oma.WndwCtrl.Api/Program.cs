using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Api;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { });
IApiService apiService = new CtrlApiService(loggerFactory.CreateLogger<CtrlApiService>());

await apiService.RunAsync(CancellationToken.None);