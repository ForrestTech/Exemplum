# Exemplum

![Exemplum](./logo.png)


> An exemplum (Latin for "example", pl. exempla, exempli gratia = "for example", abbr.: e.g.) an example or model, especially a story told to illustrate a moral point.

### Todo

Things we need to add to the readme

1. Talk about how to structure folders structure things around features not concepts e.g. billing not entities this should be reflected in tests as well
2. Things we do when we read compared to what we do when we write
3. Test framework use of shoudly as a better model for assertions
4. Add details on how mappings are registered
5. Add details about query objects and how we encapsulate query logic
6. Dto only have read write props and say why


## Docker 

Publish project
`dotnet publish .\src\QandA\QandA.csproj -o ./publish`
Build Container
`docker build -t exemplum -f Dockerfile .`
Run container
`docker run -d -p 8080:80 --name myapp exemplum`
