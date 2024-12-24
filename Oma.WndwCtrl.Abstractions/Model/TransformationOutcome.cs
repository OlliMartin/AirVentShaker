namespace Oma.WndwCtrl.Abstractions.Model;

public record TransformationOutcome : IOutcome
{
    public string OutcomeRaw { get; set; } = string.Empty;
}