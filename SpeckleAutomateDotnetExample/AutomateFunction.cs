using Speckle.Automate.Sdk;
using Speckle.Sdk.Models.Extensions;

public class AutomateFunction
{
  public async Task Run(
    IAutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {
    Console.WriteLine("Starting execution");

    Console.WriteLine("Receiving version");
    var commitObject = await automationContext.ReceiveVersion();

    Console.WriteLine("Received version: " + commitObject);

    var count = commitObject
      .Flatten()
      .Count(b => b.speckle_type == functionInputs.SpeckleTypeToCount);

    Console.WriteLine($"Counted {count} objects");

    if (count < functionInputs.SpeckleTypeTargetCount)
    {
      automationContext.MarkRunFailed(
        $"Counted {count} objects where {functionInputs.SpeckleTypeTargetCount} were expected"
      );
      return;
    }

    automationContext.MarkRunSuccess($"Counted {count} objects");
  }
}
