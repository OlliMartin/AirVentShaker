using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Oma.WndwCtrl.Abstractions;
using Oma.WndwCtrl.Core.Executors;
using Oma.WndwCtrl.Core.Executors.Commands;
using Oma.WndwCtrl.Core.FlowExecutors;
using Oma.WndwCtrl.Core.Model;
using Oma.WndwCtrl.Core.Transformers;

namespace Oma.WndwCtrl.Core.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Extension methods")]
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCommandExecutors(this IServiceCollection services)
    {
        // TODO: Will cause problems when called multiple times.
        
        services.AddScoped<ICommandExecutor, CliCommandExecutor>()
            .AddScoped<IOutcomeTransformer, NoOpTransformer>()
            .AddKeyedScoped<ICommandExecutor, DelegatingCommandExecutor>(ServiceKeys.EntryExecutor)
            .AddScoped<AdHocFlowExecutor>();
        
        return services;
    }
}