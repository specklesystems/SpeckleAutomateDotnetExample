using Objects.Geometry;
using Speckle.Automate.Sdk;
using Speckle.Core.Models.Extensions;

static class AutomateFunction
{
  public static async Task<int> Run(
    AutomationContext automationContext,
    FunctionInputs functionInputs
  )
  {
    Console.WriteLine("Starting execution");
    // HACK needed for the objects kit to initialize
    var p = new Point();
    
    Console.WriteLine("Receiving version");
    var commitObject = await automationContext.ReceiveVersion();

    Console.WriteLine("Received version: " + commitObject);
    try
    {
      var count = commitObject
                 .Flatten()
                 .Count(b => b.speckle_type == functionInputs.SpeckleTypeToCount);
      Console.WriteLine($"Counted {count} objects");
      return count;
    }
    catch (Exception e)
    {
      Console.WriteLine($"An error occurred {e}");
      return 0;
    }
  }
}
