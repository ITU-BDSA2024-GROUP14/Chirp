## Design and architecture

### Domain model

Provide an illustration of your domain model.
Make sure that it is correct and complete.
In case you are using ASP.NET Identity, make sure to illustrate that accordingly.

### Architecture — In the small

Illustrate the organization of your code base.
That is, illustrate which layers exist in your (onion) architecture.
Make sure to illustrate which part of your code is residing in which layer.

### Architecture of deployed application

Illustrate the architecture of your deployed application.
Remember, you developed a client-server application.
Illustrate the server component and to where it is deployed, illustrate a client component, and show how these communicate with each other.

### User activities

Illustrate typical scenarios of a user journey through your _Chirp!_ application.
That is, start illustrating the first page that is presented to a non-authorized user, illustrate what a non-authorized user can do with your _Chirp!_ application, and finally illustrate what a user can do after authentication.

Make sure that the illustrations are in line with the actual behavior of your application.

### Sequence of functionality/calls trough _Chirp!_

With a UML sequence diagram, illustrate the flow of messages and data through your _Chirp!_ application.
Start with an HTTP request that is send by an unauthorized user to the root endpoint of your application and end with the completely rendered web-page that is returned to the user.

Make sure that your illustration is complete.
That is, likely for many of you there will be different kinds of "calls" and responses.
Some HTTP calls and responses, some calls and responses in C# and likely some more.
(Note the previous sentence is vague on purpose. I want that you create a complete illustration.)

## Process

### Build, test, release, and deployment

Illustrate with a UML activity diagram how your _Chirp!_ applications are build, tested, released, and deployed.
That is, illustrate the flow of activities in your respective GitHub Actions workflows.

Describe the illustration briefly, i.e., how your application is built, tested, released, and deployed.

### Team work

Show a screenshot of your project board right before hand-in.
Briefly describe which tasks are still unresolved, i.e., which features are missing from your applications or which functionality is incomplete.

Briefly describe and illustrate the flow of activities that happen from the new creation of an issue (task description), over development, etc. until a feature is finally merged into the `main` branch of your repository.

### How to make _Chirp!_ work locally

There has to be some documentation on how to come from cloning your project to a running system.
That is, Adrian or Helge have to know precisely what to do in which order.
Likely, it is best to describe how we clone your project, which commands we have to execute, and what we are supposed to see then.

### How to run test suite locally

#### How to run test suite

To run the test suite locally playwright and dotnet8 needs to be installed. To install playwright, make sure powershell is installed. 

- [_How to install powershell_](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.4)
- [_How to install dotnet_](https://learn.microsoft.com/en-us/dotnet/core/install/) 

After installing powershell on your local machine, run the following command in the root of the project
```sh
dotnet build
pwsh ./test/UITests/bin/Debug/net8.0/playwright.ps1
```
After the listed above programs is installed on your computer, run the following command in the project root to run all tests
```sh
dotnet test
```

#### Tests in _Chirp!_
To ensure requirements, prevent bugs, and new code does not break old code, the project contains Unit tests, integration tests, and End to End tests.

These tests can be found in the following folders
```sh
Project root
|-- test
    |-- Chirp.Infrastructure.Tests
    |-- IntegrationTests
    |-- UITests
```

##### Chirp.Infrastructure.Tests
The _Infrastructure test_ project contains unit tests for

- Cheeprepository
- ChirpService
- AuthorService

These tests cover all of the different methods located in their respective classes.

##### IntegrationTests
The _intregration test_ project contains integration tests, using http requests to test if the website contains implemented features in the html code.

##### UITests
The _UITests_ project uses playwright to create End to End test for the project, this tests the UI, and features that are that are relient on being logged in.

## Ethics

### License

State which software license you chose for your application.

### LLMs, ChatGPT, CoPilot, and others

State which LLM(s) were used during development of your project.
In case you were not using any, just state so.
In case you were using an LLM to support your development, briefly describe when and how it was applied.
Reflect in writing to which degree the responses of the LLM were helpful.
Discuss briefly if application of LLMs sped up your development or if the contrary was the case.
