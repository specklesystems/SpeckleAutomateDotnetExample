using Objects.Geometry;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Speckle.Core.Models;
using Speckle.Core.Models.Extensions;
using Speckle.Core.Transports;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// This class describes the user specified variables that the function wants to work with.
/// </summary>
/// This class is used to generate a JSON Schema to ensure that the user provided values
/// are valid and match the required schema.
class FunctionInputs
{
}

class AutomateFunction
{
  public static async Task<string> Run(
    SpeckleProjectData speckleProjectData,
    string speckleToken
  )
  {
    var account = new Account
    {
      token = speckleToken,
      serverInfo = new ServerInfo() { url = speckleProjectData.SpeckleServerUrl }
    };
    var client = new Client(account);

    var streamId = await client.StreamCreate(new StreamCreateInput { description = "Speckle Automate is Awesome ⭐", name = "Automated Stream" });
    var data = new Base();
    data["matteos-prop"] = "zis iz a test";
    var commitId = await Helpers.Send(streamId, data, "I'M A BOT", "automate");


    return $"{speckleProjectData.SpeckleServerUrl}/streams/{streamId}/commits/{commitId}";
  }
}
