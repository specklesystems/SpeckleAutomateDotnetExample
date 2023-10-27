using Speckle.Automate.Sdk;

// WARNING do not delete this call, this is the actual execution of your function
return await AutomationRunner
  .Main<FunctionInputs>(args, AutomateFunction.Run)
  .ConfigureAwait(false);
