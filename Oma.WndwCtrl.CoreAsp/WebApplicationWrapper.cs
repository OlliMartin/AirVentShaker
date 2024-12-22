using Microsoft.AspNetCore.Mvc;
using Oma.WndwCtrl.CoreAsp.Conventions;
using Scalar.AspNetCore;

namespace Oma.WndwCtrl.CoreAsp;

public class WebApplicationWrapper<TAssemblyDescriptor>
    where TAssemblyDescriptor: class
{
    protected WebApplication? Application { get; private set; }
    
    public Task RunAsync(CancellationToken cancelToken = default)
    {
        if (Application is not null)
        {
            throw new InvalidOperationException("Application is already running.");
        }
        
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        ConfigurationConfiguration(builder.Configuration);
        
        IMvcCoreBuilder mvcBuilder = builder.Services
            .AddMvcCore(opts =>
            {
                PreConfigureMvcOptions(opts);
                
                opts.Conventions.Add(new ContainingAssemblyApplicationModelConvention<TAssemblyDescriptor>());
                
                PostConfigureMvcOptions(opts);
            })
            .AddApiExplorer();

        PostConfigureMvc(mvcBuilder);
        
        builder.Services.AddOpenApi();
        
        ConfigureServices(builder.Services);
        
        Application = PostAppBuild(builder.Build());
        
        Application.MapControllers();
        Application.MapOpenApi();
        Application.MapScalarApiReference();
        
#if DEBUG
        Application.UseDeveloperExceptionPage();
#endif
        
        return PreAppRun(Application).RunAsync(cancelToken);
    }

    protected virtual IConfigurationBuilder ConfigurationConfiguration(IConfigurationBuilder configurationBuilder) =>
        configurationBuilder;
    
    protected virtual IMvcCoreBuilder PostConfigureMvc(IMvcCoreBuilder builder) => builder;
    protected virtual MvcOptions PreConfigureMvcOptions(MvcOptions options) => options;
    protected virtual MvcOptions PostConfigureMvcOptions(MvcOptions options) => options;
    protected virtual IServiceCollection ConfigureServices(IServiceCollection services) => services;
    
    protected virtual WebApplication PostAppBuild(WebApplication app) => app;
    protected virtual WebApplication PreAppRun(WebApplication app) => app;
}