namespace Oma.WndwCtrl.Abstractions;

public interface IOutcome
{
    bool Success { get; set; }
    string OutcomeRaw { get; set; }
}