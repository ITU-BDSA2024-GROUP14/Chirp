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

!!THIS NEEDS TO BE DONE!!!! IT HAS TO BE DONE AS ONE OF THE LAST THINGS:!!
Show a screenshot of your project board right before hand-in.
Briefly describe which tasks are still unresolved, i.e., which features are missing from your applications or which functionality is incomplete.
!!THIS NEEDS TO BE DONE!!!! IT HAS TO BE DONE AS ONE OF THE LAST THINGS!!

When the project description comes out, we read it together individually and discuss it as a group, to get a rough idea of how we want to tackle the individual problems.
Then the description is made into Github issues, with user stories and acceptance criteria, and added to the Github kanban board.

Later someone will assign themselves to the issue, create a branch and move the issue to “In Progress” on the kanban board.
They will complete the issue and check off the acceptance criteria.
When the issue is complete, and all tests pass, they will move it to “Pre approval” on the kanban board, and create a pull request.

Another developer will review the pull request, and either suggest changes, in which case the original developer fixes the problems, and request a re-review.

When it is approved in review and all tests pass, the branch is merged into main and the issue is moved to “Done” on the kanban board.

```plantuml
start
:Receive Project Description;

:Read indivudually and discuss
as group;

:Someone writes the issue
and it is added to kanban board;

:Someone assigns themselves to the issue
and moves it on the kanban board;

:Complete the issue, and check
the acceptance criteria off;

repeat :Complete an acceptance criteria;
repeat while (More acceptance criteria?) is (yes)
->no;

:Create pull request
and move it on the kanban board;

repeat :Someone reviews the code;
backward:Original developer fixes problems;
repeat while (Review approved and
checks pass?) is (no)
->yes;

:Branch is merged into main
and issue is closed;
end
```

### How to make _Chirp!_ work locally

There has to be some documentation on how to come from cloning your project to a running system.
That is, Adrian or Helge have to know precisely what to do in which order.
Likely, it is best to describe how we clone your project, which commands we have to execute, and what we are supposed to see then.

### How to run test suite locally

List all necessary steps that Adrian or Helge have to perform to execute your test suites.
Here, you can assume that we already cloned your repository in the step above.

Briefly describe what kinds of tests you have in your test suites and what they are testing.

## Ethics

### License

When deciding which license to use the most important consideration is whether any GPL libraries are used in the project, since the licence then must be GPL.
None of the libraries used has the GPL license, or any other copyleft license. 
Therefore the choice of license was left open, and the MIT license was chosen. 
The MIT license is one of the most permissive licenses, allowing the program to be used for almost anything as long as the original copyright notice and license are included. 
This also means the program can be used as is, and the developers have no responsibility for maintaining the product.
As discussed in the open source lecture, there are many advantages with using open source as your license.
Additionally, since this is a school project, it makes sense to both be as open source as possible, and to not take responsibility for maintaining the code longterm.

### LLMs, ChatGPT, CoPilot, and others

In the development process LLM were used sparingly to support the coding process.
GitHub copilot and the JetBrains AI was occasionally used to finish lines of code, when it came with good suggestions. 
It also wrote some of the documentation for the code and helped with some tests.
ChatGPT was used occasionally to suggest names, explain error messages, and other similar uses.

Often the answers were wrong or irrelevant though, especially regarding ChatGPT, however it rarely took long to figure out whether the answer was useful, making it a small problem.
It was helpful as support and probably sped up the coding process, but the final product most likely did not change because of it.
Since neither ever contributed significantly to the codebase it has not been added as a co-author to any commits.


