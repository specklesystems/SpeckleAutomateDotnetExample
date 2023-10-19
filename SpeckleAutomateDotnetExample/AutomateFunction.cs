using Objects;
using Objects.Geometry;
using Speckle.Automate.Sdk;
using Speckle.Core.Logging;
using Speckle.Core.Models.Extensions;

static class AutomateFunction
{
  public static async Task Run(
    AutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {
    Console.WriteLine("Starting execution");
    // INFO: Force objects kit to initialize
    _ = nameof(ObjectsKit);
    
    Console.WriteLine("Receiving version");
    var commitObject = await automationContext.ReceiveVersion();

    Console.WriteLine("Received version: " + commitObject);

    var count = commitObject
               .Flatten()
               .Count(b => b.speckle_type == functionInputs.SpeckleTypeToCount);
    automationContext.MarkRunSuccess($"Counted {count} objects");
  }
}
