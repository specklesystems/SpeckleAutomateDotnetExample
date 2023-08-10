using Objects.Geometry;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Speckle.Core.Models.Extensions;
using Speckle.Core.Transports;
using SpeckleAutomateDotnetExample;

class AutomateFunction
{
  public static async Task<int> Run(
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

    return rootObject.Flatten().Count( b => b.speckle_type == functionInputs.SpeckleTypeToCount);
  }
}
