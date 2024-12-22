using System.Text.Json.Serialization;
using Oma.WndwCtrl.Core.Interfaces;
using Oma.WndwCtrl.Core.Model.Commands;

namespace Oma.WndwCtrl.Core.Model;

/// <summary>
/// A read+write control indicating on/off.
/// Can define a GET endpoint to query the _current_ state (ad-hoc execution)
/// Additionally, a POST:/on and POST:/off endpoint is hosted (potentially flip as well)
/// </summary>
public class Switch : IStateQueryable
{
    [JsonInclude]
    [JsonRequired]
    public BaseCommand QueryCommand { get; internal set; } = null!;

    [JsonInclude]
    [JsonRequired]
    public BaseCommand OnCommand { get; internal set; } = null!;

    [JsonInclude]
    [JsonRequired]
    public BaseCommand OffCommand { get; internal set; } = null!;
}
