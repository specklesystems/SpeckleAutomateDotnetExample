using System.ComponentModel.DataAnnotations;
using Objects.Geometry;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Speckle.Core.Models.Extensions;
using Speckle.Core.Transports;

/// <summary>
/// This class describes the user specified variables that the function wants to work with.
/// </summary>
/// This class is used to generate a JSON Schema to ensure that the user provided values
/// are valid and match the required schema.
struct FunctionInputs
{
  [Required]
  public string SpeckleTypeToCount;
}

class AutomateFunction
{
  public static async Task Run(
    SpeckleProjectData speckleProjectData,
    FunctionInputs functionInputs,
    string speckleToken
  )
  {
    var account = new Account
    {
      token = speckleToken,
      serverInfo = new ServerInfo() { url = speckleProjectData.SpeckleServerUrl }
    };
    var client = new Client(account);


    // HACK needed for the objects kit to initialize
    var p = new Point();

    var commit = await client.CommitGet(
      speckleProjectData.ProjectId,
      speckleProjectData.VersionId
    );

    var serverTransport = new ServerTransport(account, speckleProjectData.ProjectId);
    var rootObject = await Operations.Receive(
      commit.referencedObject,
      serverTransport,
      new MemoryTransport()
    );

    var count = rootObject?.Flatten().Count( b => b.speckle_type == functionInputs.SpeckleTypeToCount);

    Console.WriteLine(
      $"Found {count} elements that have the type {functionInputs.SpeckleTypeToCount}"
    );
  }
}
