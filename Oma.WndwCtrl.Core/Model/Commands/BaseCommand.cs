using System.Text.Json.Serialization;
namespace Oma.WndwCtrl.Core.Model.Commands;

[JsonPolymorphic]
[JsonDerivedType(typeof(CliCommand), typeDiscriminator: "cli")]
public class BaseCommand
{
    public int Retries { get; set; } = 3;

    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
}
