using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AslHelp.Shared.Results;

namespace AslHelp.Shared.Tasks;

// public class TaskBuilder
//     : ICreationStage
//     , ICatchStage
//     , IFinalizeStage
// {
//     private readonly Dictionary<Type, List<IExceptionStage>> _exceptionStagesByType = [];

//     private Action? _onStart;
//     private TaskBuilderFunc? _delegate;
//     private Action? _onComplete;

//     ICreationStage ICreationStage.OnStart(Action action)
//     {
//         _onStart = action;
//         return this;
//     }

//     ICatchStage ICreationStage.Exec(Func<Result> func)
//     {
//         _delegate = new(func);
//         return this;
//     }

//     ICatchStage ICreationStage.Exec(Func<Task<Result>> func)
//     {
//         _delegate = new(func);
//         return this;
//     }

//     IExceptionStage<TException> ICatchStage.Catch<TException>()
//     {
//         ExceptionStage<TException> exception = new(this);
//         _exceptionStagesByType.Add(typeof(TException), exception);
//     }

//     ICatchStage ICatchStage.WithRetries(uint retries)
//     {
//         throw new NotImplementedException();
//     }

//     IFinalizeStage ICatchStage.WithTimeout(uint msTimeout)
//     {
//         throw new NotImplementedException();
//     }

//     IFinalizeStage IFinalizeStage.OnComplete(Action action)
//     {
//         _onComplete = action;
//         return this;
//     }

//     public Task RunAsync()
//     {
//         throw new NotImplementedException();
//     }
// }
