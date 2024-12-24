namespace Oma.WndwCtrl.Abstractions.Model;

public record TransformationOutcome : IOutcome
{
    public TransformationOutcome() { }

    public TransformationOutcome(IOutcome outcome)
    {
        Success = outcome.Success;
        OutcomeRaw = outcome.OutcomeRaw;
    }
    
    public bool Success { get; set; }
    public string OutcomeRaw { get; set; } = string.Empty;
}