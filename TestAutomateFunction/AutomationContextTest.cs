using Microsoft.Extensions.DependencyInjection;
using Speckle.Automate.Sdk;
using Speckle.Automate.Sdk.Test;
using Speckle.Sdk.Api;
using Speckle.Sdk.Api.GraphQL.Models;
using Speckle.Sdk.Credentials;

namespace TestAutomateFunction;

[TestFixture]
public sealed class AutomationContextTest : IDisposable
{
  private IClient _client;
  private Account _account;
  private IAutomationRunner _runner;
  private AutomateFunction _function;

  [OneTimeSetUp]
  public void Setup()
  {
    var serviceProvider = ServiceRegistration.GetServiceProvider();
    _account = new Account
    {
      token = TestAutomateEnvironment.GetSpeckleToken(),
      serverInfo = new ServerInfo
      {
        url = TestAutomateEnvironment.GetSpeckleServerUrl().ToString()
      }
    };
    _client = serviceProvider.GetRequiredService<IClientFactory>().Create(_account);
    _runner = serviceProvider.GetRequiredService<IAutomationRunner>();
    _function = serviceProvider.GetRequiredService<AutomateFunction>();
  }

  [Test]
  public async Task TestFunctionRun()
  {
    var inputs = new FunctionInputs
    {
      SpeckleTypeToCount = "Base",
      SpeckleTypeTargetCount = 1
    };

    var automationRunData = await TestAutomateUtils.CreateTestRun(_client);
    var automationContext = await _runner.RunFunction(
      _function.Run,
      automationRunData,
      _account.token,
      inputs
    );

    Assert.That(automationContext.RunStatus, Is.EqualTo("SUCCEEDED"));
  }

  public void Dispose()
  {
    _client.Dispose();
    TestAutomateEnvironment.Clear();
  }
}
