using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Speckle.Core.Models.GraphTraversal;
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


    // var kit = KitManager.GetDefaultKit();

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

    var traversalRule = TraversalRule
      .NewTraversalRule()
      .When(_ => true)
      .ContinueTraversing(DefaultTraversal.ElementsAliases);
    var gt = new GraphTraversal(traversalRule);
    var ret = gt.Traverse(rootObject).Select(b => b.current).ToList();

    return ret.Count( b => b.speckle_type == functionInputs.SpeckleTypeToCount);
  }
}
