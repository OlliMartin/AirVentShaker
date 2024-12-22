using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Api;
using Oma.WndwCtrl.CoreAsp.Conventions;
using Oma.WndwCtrl.MgmtApi;
using Oma.WndwCtrl.MgmtApi.Model;
using Oma.WndwCtrl.MgmtApi.Workers;
using Scalar.AspNetCore;

MgmtApiService apiService = new();
await apiService.RunAsync();