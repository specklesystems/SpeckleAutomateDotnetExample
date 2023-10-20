# Example function for Speckle Automate

This repository contains an example function, that is compatible with Speckle's automation platform.

## Getting started

This is practically a template function, that can be used as a starter for creating your own function.

The function targets dotnet 7.0 and uses the Speckle.Automate.SDK nuget package, as well as the Objects Kit.

At it's core, a Speckle Automate function is a CLI application with a specific, standardized set of available commands and arguments (see below). Each automate function is then built into a docker image that is published onto Speckle Automate.

The entire publishing process is already taken care of (see Publishing functions below) so you can concentrate on writing the code that matters to you.

### Repo structure

Within this repo, you will find:

- `SpeckleAutomateDotnetExample.sln` -> The main solution for the function. Only holds a reference to one project.
- `SpeckleAutomateDotnetExample/` -> Sub-folder containing the function's project and code
- **Docker related files** (`.dockerignore`, `Dockerfile`) -> Used for deployment of your function. Only modify if you know what you're doing.
- **Github Action** (`.github/workflows/main.yml`) -> Used to publish the function on every release
- **Codespaces configuration** (`.devcontainer/`) -> Preconfigures GitHub Codespaces to run this project.
- **IDE Configuration** (`.vscode/`, `.csharpierrc.json`) -> Contains all necessary settings to get started writing a function in VSCode. Feel free to modify these files at will

## Anatomy of a function

At its core every Speckle automate function is a CLI application that gets its arguments from the execution context.
Speckle Automate at its current stage provides 3 arguments to the functions it executes:

1. A stringified JSON object, containing the references to the project, model, version and server url, which triggered the run of the given function run.
2. Another stringified JSON object, that contains the user provided arguments to the function. Defining the schema of the required arguments is the responsibility of the function author. Speckle automate will make sure, that the automation user inputs match the published schema.
3. A Speckle token that the function can use to act on behalf of the automation owner.

These arguments are automatically provided by the automate platform every time an automation is run. Additionally, the complexity of parsing the inputs is already dealt with in the `Speckle.Automate.Sdk`, which provides a convenient `AutomationRunner` class that is intended to be used as the entrypoint for any function.

### Program.cs - Function boilerplate

In the `Program.cs` file you'll find only a call to `AutomationRunner.Main<TInput>` (the SDK entrypoint for our functions). This method contains the parsing logic for the previously mentioned arguments and takes in:

- `args` -> the arguments provided by automate
- `Func<AutomationContext, TInput>` -> The user provided function that will be run when this automation is triggered.

This sets up a CLI application with two commands:

1. the main function command, that implements the Speckle Automate function's anatomy
2. `generate-schema` -> a helper command that can generate the JSON Schema from the function author provided `FunctionInputs` class. This command is called whenever a new version of the function is published to automate.

### AutomateFunction.cs

The `AutomateFunction.cs` contains the actual function implementation. This is the file that you should modify to implement the function's functionality.

The `Run` function takes 2 inputs:

- `AutomationContext` -> The context of your automation, which contains all the parsed information provided by the automate service as explained [above](#anatomy-of-a-function)
- **Optional** A `struct` representing your desired input data (see [Function Inputs](#functioninputscs))

The template already contains an example implementation that will count how many objects of a particular type can be found on a version.

### FunctionInputs.cs

The definition of the user-defined inputs required for the function to work. This will also be used by the `generate-schema` CLI command to inform Automate of the required inputs a user setting up an automation will need to provide.

This is just a `struct` with whatever properties you need. For now, we support basic types (string, int, double) and nested `struct`s with the same type limitations as its parent.

## Publishing a function

We publish a [github action](https://github.com/specklesystems/speckle-automate-github-composite-action) that bundles all the steps that are needed to publish a new version of a function. The automation requires two secret values, the `SPECKLE_FUNCTION_ID` and `SPECKLE_FUNCTION_TOKEN` both of these are provided, when you register a new function on Automate.

This will:

- Restore any packages and build your function
- Generate the json schema for your inputs
- Package your function as a Docker image
- Register the new image as a new version in Speckle Automate

Once this process has successfully finished, your function should be available and discoverable in Speckle.Automate

> [!NOTE]
> You need to register your function on Speckle Automate to get the required secret values. For new functions, this is automatically done by our GitHub Application, so no extra user setup is needed.

> [!IMPORTANT]
> After adding the Secrets to the repository secrets on Github each commit to the `main` branch will trigger the publication of a new version.
