using Oma.WndwCtrl.Core.Model.Commands;
namespace Oma.WndwCtrl.Core.Interfaces;

public interface IStateQueryable
{
    public BaseCommand QueryCommand { get; }
}
