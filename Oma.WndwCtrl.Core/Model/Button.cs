using System.Text.Json.Serialization;
using Oma.WndwCtrl.Core.Model.Commands;
namespace Oma.WndwCtrl.Core.Model;

/// <summary>
/// A write-only control that can be just executed, indicating success/failure of the operation
/// </summary>
public class Button
{
    [JsonInclude]
    [JsonRequired]
    public BaseCommand Command { get; internal set; } = null!;
}
