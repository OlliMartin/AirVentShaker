using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Api;

IApiService apiService = new CtrlApiService();

await apiService.RunAsync(CancellationToken.None);