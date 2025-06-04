using Microsoft.Extensions.DependencyInjection;
using Speckle.Automate.Sdk;

namespace TestAutomateFunction;

public static class ServiceRegistration
{
  public static IServiceProvider GetServiceProvider()
  {
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddAutomateSdk();
    serviceCollection.AddSingleton<AutomateFunction>();
    return serviceCollection.BuildServiceProvider();
  }
}
