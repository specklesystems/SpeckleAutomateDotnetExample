# Example function for Speckle Automate

This repository contains an example function, that is compatible with Speckle's automation platform.

## Getting started

This is practically a template function, that can be used as a starter for creating your own function.

The function targets dotnet 7.0 and uses the SpeckleCore SDK. The published function has to run on the official dotnet runtime linux container.

## Functional anatomy

The repo contains the boilerplate that builds up a function.
At its core every Speckle automate function is a cli application that gets its arguments from the execution context.
Speckle Automate at its current stage provides 3 values to the functions it executes:

1. A stringified JSON object, containing the references to the project, model, version and server url, which triggered the run of the given function run.
2. Another stringified JSON object, that contains the user provided arguments to the function. Defining the schema of the required arguments is the responsibility of the function author. Speckle automate will make sure, that the automation user inputs match the published schema.
3. A Speckle token that the function can use to act on behalf of the automation owner.

### Function boilerplate - Program.cs

Getting a function published to Speckle Automate requires has some requirements. 
In the `Program.cs` file this function defines most of the boilerplate code to meet these requirements. So unless you know what you are doing, you probably shouldn't change it.

The `Program.cs` sets up a CLI application with two commands:

1. the main function command, that implements the Speckle Automate function's anatomy
2. a helper command that can generate the JSON Schema from the function author provided `FunctionInputs` class. This command is called whenever a new version of the function is published to automate.

### Function functionality - AutomateFunction.cs

The `AutomateFunction.cs` contains the actual function implementation. This is the file that you should modify to implement the function's functionality.
The `Run` function takes the parsed arguments that match the 3 arguments schema described [above](#functional-anatomy).

## Github Action

We publish a [github action](https://github.com/specklesystems/speckle-automate-github-composite-action) that bundles all the steps that are needed to publish a new version of a function. The automation requires two secret values, the `SPECKLE_FUNCTION_ID` and `SPECKLE_FUNCTION_TOKEN` both of these are provided, when you register a new function on Automate.

> [!NOTE]
> You need to register your function on Speckle Automate to get the required secret values.

> [!IMPORTANT]
> After adding the Secrets to the repository secrets on Github each commit to the `main` branch will trigger the publication of a new version.

