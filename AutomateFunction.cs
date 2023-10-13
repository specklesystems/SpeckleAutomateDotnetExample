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
    // HACK needed for the objects kit to initialize
    var p = new Point();

    var commitObject = await automationContext.ReceiveVersion();

    return commitObject
      .Flatten()
      .Count(b => b.speckle_type == functionInputs.SpeckleTypeToCount);
  }
}
