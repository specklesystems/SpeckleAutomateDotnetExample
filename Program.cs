using System.CommandLine;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using Objects.Geometry;
using Speckle.Newtonsoft.Json;

namespace SpeckleAutomateDotnetExample;

internal static class Program
{
  internal static async Task Main(string[] args)
  {
    var speckleProjectDataArg = new Argument<string>(
      name: "Speckle project data",
      description: "The values of the project / model / version that triggered this function"
    );
    var functionInputsArg = new Argument<string>(
      name: "Function inputs",
      description: "The values provided by the function user, matching the function input schema"
    );
    var speckleTokenArg = new Argument<string>(
      name: "Speckle token",
      description: "A token to talk to the Speckle server with"
    );
    var rootCommand = new RootCommand("Count objects matching the given speckle_type");
    rootCommand.AddArgument(speckleProjectDataArg);
    rootCommand.AddArgument(functionInputsArg);
    rootCommand.AddArgument(speckleTokenArg);
    rootCommand.SetHandler(
      async (speckleProjectData, functionInputs, speckleToken) =>
      {
        await RunFunction(speckleProjectData, functionInputs, speckleToken);
      },
      speckleProjectDataArg,
      functionInputsArg,
      speckleTokenArg
    );

    var generateSchemaCommand = new Command(
      "generate-schema",
      "Generate JSON schema for the function inputs"
    );
    generateSchemaCommand.SetHandler(() =>
    {
      var schema = GenerateFunctionInputSchema();
      Console.WriteLine(schema);
    });
    rootCommand.Add(generateSchemaCommand);

    await rootCommand.InvokeAsync(args);
  }

  static async Task RunFunction(
    string rawSpeckleProjectData,
    string rawFunctionInputs,
    string speckleToken
  )
  {
    var p = new Point();
    var speckleProjectData = JsonConvert.DeserializeObject<SpeckleProjectData>(
      rawSpeckleProjectData
    );
    var functionInputs = JsonConvert.DeserializeObject<FunctionInputs>(
      rawFunctionInputs
    );
    await AutomateFunction.Run(speckleProjectData, functionInputs, speckleToken);
  }

  static string GenerateFunctionInputSchema()
  {
    var generator = new JSchemaGenerator
    {
      ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
    var schema = generator.Generate(typeof(FunctionInputs));
    return schema.ToString(Newtonsoft.Json.Schema.SchemaVersion.Draft2019_09);
  }
}

/// <summary>
/// This class describes the user specified variables that the function wants to work with.
/// </summary>
class FunctionInputs
{
  [Required]
  public string SpeckleTypeToCount;
}

internal class SpeckleProjectData
{
  public string ProjectId;
  public string ModelId;
  public string VersionId;
  public string SpeckleServerUrl;
}