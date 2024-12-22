namespace Oma.WndwCtrl.Configuration.Model;

public class ComponentConfigurationAccessor
{
    public ComponentConfiguration? Configuration { get; set; } = new();

    // TODO for stand alone
    public static ComponentConfigurationAccessor FromFile() => new();
}