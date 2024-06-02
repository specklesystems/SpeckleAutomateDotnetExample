using System.Diagnostics.CodeAnalysis;
using GraphQL;
using Speckle.Automate.Sdk.Schema;
using Speckle.Automate.Sdk.Schema.Triggers;
using Speckle.Core.Api;
using Speckle.Core.Models;

namespace TestAutomateFunction;

public static class TestAutomateUtils
{
  [SuppressMessage("Security", "CA5394:Do not use insecure randomness")]
  public static string RandomString(int length)
  {
    Random rand = new();
    const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
    var chars = Enumerable.Range(0, length).Select(_ => pool[rand.Next(0, pool.Length)]);
    return new string(chars.ToArray());
  }

  public static Base TestObject()
  {
    Base rootObject = new() { ["foo"] = "bar" };
    return rootObject;
  }

  public static async Task<AutomationRunData> CreateTestRun(
    Client speckleClient
  ) {
        GraphQLRequest query =
            new(
                query: """
                    mutation CreateTestAutomation(
                        $automationId: ID!
                    ) {
                        projectAutomationMutations {
                            createTestAutomationRun($automationId) {
                                automationRunId
                                functionRunId
                                triggers {
                                    payload {
                                        modelId
                                        versionId
                                    }
                                    triggerType
                                }
                            }
                        }
                    }
                """,
                variables: new
                {
                    automationId = TestAutomateEnvironment.GetSpeckleAutomationId()
                }
            );

        var res = await speckleClient.ExecuteGraphQLRequest<object>(query);

        var data = new AutomationRunData()
        {
            ProjectId = TestAutomateEnvironment.GetSpeckleProjectId(),
            SpeckleServerUrl = TestAutomateEnvironment.GetSpeckleServerUrl(),
            AutomationId = TestAutomateEnvironment.GetSpeckleAutomationId(),
            AutomationRunId = "",
            FunctionRunId = "",
            Triggers = new List<AutomationRunTriggerBase>() {
                new VersionCreationTrigger(modelId: "", versionId: "")
            }
        };

        return data;
  }
}
