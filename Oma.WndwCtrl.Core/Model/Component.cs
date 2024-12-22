using System.Text.Json.Serialization;
namespace Oma.WndwCtrl.Core.Model;

[JsonPolymorphic()]
[JsonDerivedType(typeof(Button), typeDiscriminator: "button")]
[JsonDerivedType(typeof(Sensor), typeDiscriminator: "sensor")]
[JsonDerivedType(typeof(Switch), typeDiscriminator: "switch")]
public class Component
{

}
