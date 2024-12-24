using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Configuration.Extensions;
using Oma.WndwCtrl.Core.Extensions;

namespace Oma.WndwCtrl.Api.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Extension methods")]
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddComponentApi(this IServiceCollection services)
    {
        services.AddConfiguration()
            .AddCommandExecutors()
            .TryAddSingleton<IApiService, CtrlApiService>();
        
        return services;
    }
}