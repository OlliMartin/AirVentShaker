using Oma.WndwCtrl.MgmtApi;

MgmtApiService apiService = new();
await apiService.StartAsync();
await apiService.WaitForShutdownAsync();