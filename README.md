<style>
.logo img{
    float:left;
    width:116px;
    height:116px;
}
</style>

<span class="logo">

![Logo](logo.png)

</span>

# Exemplum

> An exemplum (Latin for "example", pl. exempla, exempli gratia = "for example", abbr.: e.g.) an example or model, especially a story told to illustrate a moral point.

<br/>

[![.NET](https://github.com/ForrestTech/Exemplum/actions/workflows/build.yml/badge.svg)](https://github.com/ForrestTech/Exemplum/actions/workflows/build.yml)

<br/>

This is a solution template for creating a asp.net core applications. Single Page App (SPA) with Angular and ASP.NET
Core following the principles of Clean Architecture. You can create a new project based on this one by clicking the
above Use this template button.

## Todo

Things we need to add to the readme

1. Clean architecture, onion architecture and ports and adapters
2. Solution structure
3. DDD modelling some concepts and the fact that an example domain needs to be simple but often DDD is only needed for
   more complex domains
4. Command Query MediatR (Command happen against the aggregate route REST resource should reflect this)
5. Folder structure, feature folders (command validator and handler in a single file)
6. Dtos and mapping use of Automapper (Dto should have )
7. Logging
8. EF Core and how we configure it
9. Support for Auditable entities
10. Add details about query objects and how we encapsulate query logic
11. Domain events and transactions  (Transaction behaviour dont save changes in event handlers)
12. Test framework use of fluent assertions as a better model for assertions, test server setup, auto fixture,
    nsubstitute
13. 3rd party service calls, resilience and error handling
14. Health checks and the health check monitor ui
15. Secret management
16. Shout out to packages used and inspiration for the project
17. Nullable
18. Caching and why there is not a great cache story in the .net space well sort off
19. Generated client
20. Blazor component libraries
21. Not publishing a client or contracts !!?
22. Blazor css scoping

Code coverage commands

dotnet test /p:CollectCoverage=true /p:Exclude="[Exemplum.Infrastructure]*" /p:CoverletOutputFormat=opencover /p:
ExcludeByAttribute="CompilerGeneratedAttribute"

Can add this param to fail test on coverate /p:Threshold=20

dotnet tool install -g dotnet-reportgenerator-globaltool

reportgenerator -reports:".\coverage.opencover.xml" -targetdir:"CoverageResults" -reporttypes:HTML

## Docker 

