using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Oma.WndwCtrl.Core.Interfaces;
using Oma.WndwCtrl.Core.Model.Commands;

namespace Oma.WndwCtrl.Core.Model;

/// <summary>
/// A read-only control that provides one or multiple data points
/// Can define a GET endpoint to query the _current_ state (ad-hoc execution)
/// </summary>
public class Sensor : IStateQueryable
{
    [JsonInclude]
    [JsonRequired]
    public BaseCommand QueryCommand { get; internal set; } = null!;
}
