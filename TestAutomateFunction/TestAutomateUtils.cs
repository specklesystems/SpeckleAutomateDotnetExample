using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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

  public static async Task<AutomationRunData> CreateTestRun(
    Client speckleClient
  ) {
        GraphQLRequest query =
            new(
                query: """
                    mutation Mutation($projectId: ID!, $automationId: ID!) {
                        projectMutations {
                            automationMutations(projectId: $projectId) {
                                createTestAutomationRun(automationId: $automationId) {
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
                    }
                """,
                variables: new
                {
                    automationId = TestAutomateEnvironment.GetSpeckleAutomationId(),
                    projectId = TestAutomateEnvironment.GetSpeckleProjectId()
                }
            );

        dynamic res = await speckleClient.ExecuteGraphQLRequest<object>(query);

        var runData = res["projectMutations"]["automationMutations"]["createTestAutomationRun"];
        var triggerData = runData["triggers"][0]["payload"];

        string modelId = triggerData["modelId"];
        string versionId = triggerData["versionId"];

        var data = new AutomationRunData()
        {
            ProjectId = TestAutomateEnvironment.GetSpeckleProjectId(),
            SpeckleServerUrl = TestAutomateEnvironment.GetSpeckleServerUrl(),
            AutomationId = TestAutomateEnvironment.GetSpeckleAutomationId(),
            AutomationRunId = runData["automationRunId"],
            FunctionRunId = runData["functionRunId"],
            Triggers = new List<AutomationRunTriggerBase>() {
                new VersionCreationTrigger(modelId: modelId, versionId: versionId)
            }
        };

        return data;
  }
}
