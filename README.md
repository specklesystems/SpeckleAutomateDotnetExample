<h1 align="center">
  <img src="https://speckle.systems/content/images/2022/06/logo-blue-2.png" width="450px"/><br/>
  Speckle | Automate Dotnet Example
</h1>
<h3 align="center">
    An example function compatible with Speckle Automate
</h3>
<p align="center"><b>Speckle</b> is data infrastructure for the AEC industry.</p><br/>

<p align="center">
  <a href="https://twitter.com/SpeckleSystems">
    <img src="https://img.shields.io/twitter/follow/SpeckleSystems?style=social" alt="Twitter Follow">
  </a>
  <a href="https://speckle.community">
    <img src="https://img.shields.io/discourse/users?server=https%3A%2F%2Fspeckle.community&amp;style=flat-square&amp;logo=discourse&amp;logoColor=white" alt="Community forum users"></a> 
  <a href="https://speckle.systems">
    <img src="https://img.shields.io/badge/https://-speckle.systems-royalblue?style=flat-square" alt="website">
  </a>
  <a href="https://speckle.guide/dev/">
    <img src="https://img.shields.io/badge/docs-speckle.guide-orange?style=flat-square&amp;logo=read-the-docs&amp;logoColor=white" alt="docs">
  </a>
</p>

# Example Dotnet function for Speckle Automate

This repository contains an example function that is compatible with Speckle Automate, the platform to register and deploy automations that interact with your Speckle data.

## Quick Start

1. Download or clone this repository, or better still generate it via the New Function wizard in Speckle Automate.
2. Modify the `AutomateFunction.cs` to include your specific logic.
3. Publish your changes (see Publishing Functions).

## Getting started

This is essentially a template function, designed to serve as a starting point for creating your own function. The function targets dotnet 7.0 and uses the Speckle.Automate.SDK NuGet package, as well as the Objects Kit.

At its core every Speckle Automate function is a CLI application with a specific, standardized set of available commands and arguments ([see below](#anatomy-of-a-function)). Each automate function is then built into a Docker image and published onto Speckle Automate.

The Speckle Automate function publishing process is already taken care of ([see Publishing functions](#publishing-a-function)) so you can concentrate on writing the code that matters to you.

### Repo structure

Here are some key files and folders you'll find in this repository:

- **Main Solution File (`SpeckleAutomateDotnetExample.sln`)**: This is the project's master file that references the function's project. See note at the end of this readme about renaming this.
- **Project Sub-folder (`SpeckleAutomateDotnetExample/`)**: This is where your function's code lives. Expand or replace the code in this sub-folder to make the function your own.
- **Docker related files (`.dockerignore`, `Dockerfile`)** -> Files like `.dockerignore` and `Dockerfile` are essential for deploying your function as a Docker container. Modify these only if you're familiar with Docker and it is absolutely necessary for your function's operation.
- **Github Action (`.github/workflows/main.yml`)** -> This action automates the function's release process, making it easier to publish updates.
- **Codespaces configuration (`.devcontainer/`)** -> This configuration ensures that GitHub Codespaces has the settings it needs to run this project effectively.
- **IDE Configuration Files (`.vscode/`, `.csharpierrc.json`)** -> Configuration files for Visual Studio Code are included, allowing you to tailor the editor settings to your preferences.

## Anatomy of a function

Every Speckle Automate function is fundamentally a Command Line Interface (CLI) application that receives its operational context via arguments. Here's what Speckle Automate currently supplies to functions:

1. **Project Context (Stringified JSON)**: This argument contains key data points such as the project, model, version, and server URL that initiated the function's execution.
2. **User-Defined Arguments (Stringified JSON)**: The function author is responsible for defining the schema for these user inputs. Speckle Automate ensures that the provided inputs match this schema.
3. **Speckle Token**: This token allows the function to perform actions as if it were the automation's owner.

These arguments are automatically provided by the automate platform every time an automation is run. The `Speckle.Automate.Sdk` simplifies parsing these inputs via its `AutomationRunner` class, intended to be the function's entry point.

### Function Boilerplate (`Program.cs`)

In this file, you'll find a call to `AutomationRunner.Main<TInput>`, which serves as your function's SDK entry point. This method handles argument parsing and accepts:

- `args` -> the arguments provided by Speckle Automate, and
- `Func<AutomationContext, TInput>` -> Your custom function that gets executed when the automation is triggered.

> [!NOTE]
> If your function requires no inputs, there is also `AutomationRunner.Main` (non-generic) which takes in a `Func<AutomationContext>` instead.

This sets up a CLI application with two commands:

1. the main function command, that implements the Speckle Automate function's anatomy
2. `generate-schema` -> a helper command that can generate the JSON Schema from the function author provided `FunctionInputs` class. This command is called whenever a new version of the function is published to automate.

### Function Implementation (`AutomateFunction.cs`)

The `AutomateFunction.cs` contains the actual function implementation. This is the file that you should modify to implement the function's functionality.

You'll modify the `Run` function to execute your specific logic here. The function receives an AutomationContext and, optionally, a struct for your desired input data:

- `AutomationContext` -> The context of your automation, which contains all the parsed information provided by the automate service as explained [above](#anatomy-of-a-function)
- **Optional**: a `struct` representing your desired input data (see [Function Inputs](#user-inputs-functioninputscs))

The template already contains an example implementation that will count how many objects of a particular type can be found on a version.

### User Inputs (`FunctionInputs.cs`)

The definition of the user-defined inputs required for the function to work. This will also be used by the `generate-schema` CLI command to inform Automate of the required inputs a user setting up an automation will need to provide.

This `struct` defines the user input types your function will accept. Supported data types include basic ones (e.g., `string`, `int`, `double`) and nested structs are possible with the same types as their parents.

## Publishing a function

Publishing your Speckle Automate function is streamlined through a GitHub Action we provide. Here's what you need to know:

1. **Secret Values**: To start publishing, you'll need two secrets: `SPECKLE_FUNCTION_ID` and `SPECKLE_FUNCTION_TOKEN`. These are provided when you register a new function on Speckle Automate.

2. **Build and Restore**: The GitHub Action restores any necessary packages and builds your function.

3. **Generate JSON Schema**: The Action will also generate a JSON schema based on your FunctionInputs class to inform Speckle Automate about the required user inputs.

4. **Docker Image**: Your function is then packaged into a Docker image, which is essential for its deployment.

5. **Version Registration**: The new Docker image is registered as a new version in Speckle Automate, making your function discoverable and usable.

Once this process has successfully finished, your function should be available and discoverable in Speckle.Automate

> [!NOTE]
> Register your function on Speckle Automate to get the needed secret values. This is automatically done for new functions by our GitHub Application, so no extra setup is required on your end.

> [!IMPORTANT]
> After adding the Secrets to the repository secrets on Github each commit to the `main` branch will trigger the publication of a new version.

## Changing the name of your solution/project

Before you dive too deeply into development, give your project a name that better suits its purpose. If you decide to rename your project or solution, remember also to update a few key areas to maintain functionality:

- **Dockerfile**: On line 4, you'll find a reference to the `SpeckleAutomateDotnetExample/` folder. Update this to match your new project name.
- **GitHub Actions Workflow**: In `.github/workflows/main.yml`, the working directory for the GitHub Action is declared as `SpeckleAutomateDotnetFolder/`. Update this to point to your newly named project folder.

Keeping these references up-to-date ensures that GitHub Actions and Docker can correctly find and operate your project.
