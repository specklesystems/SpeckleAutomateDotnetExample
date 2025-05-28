using Microsoft.Extensions.DependencyInjection;
using Speckle.Automate.Sdk;

var serviceCollection = new ServiceCollection();
serviceCollection.AddAutomateSdk();
serviceCollection.AddSingleton<AutomateFunction>();
await using var container = serviceCollection.BuildServiceProvider();

var runner = container.GetRequiredService<IAutomationRunner>();
var function = container.GetRequiredService<AutomateFunction>();

// WARNING do not delete this call, this is the actual execution of your function
return await runner.Main<FunctionInputs>(args, function.Run);
