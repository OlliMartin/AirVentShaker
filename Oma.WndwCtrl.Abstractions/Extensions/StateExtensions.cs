using LanguageExt;
using Oma.WndwCtrl.Abstractions.Errors;
using static LanguageExt.Prelude;

namespace Oma.WndwCtrl.Abstractions.Extensions;

public delegate Task<Either<FlowError, (S State, A Outcome)>> MyState<S, A>(S state);

public static class StateExtensions
{
    public static MyState<S, B> BindAsync<S, A, B>(this MyState<S, A> ma, Func<A, MyState<S, B>> f) =>
        async state =>
        {
            try
            {
                return await ma(state).BindAsync(pairA => f(pairA.Item2)(pairA.Item1));
            }
            catch(Exception e)
            {
                return Left<FlowError>(new TechnicalError(e.Message, Code: 1337));
            }
        };
    
    public async static Task<Either<FlowError, (S State, A Outcome)>> RunAsync<S, A>(this MyState<S, A> ma, S state)
    {
        try
        {
            return await ma(state);
        }
        catch (Exception e)
        {
            return Left<FlowError>(new TechnicalError(e.Message, Code: 1337));
        }
    }
}