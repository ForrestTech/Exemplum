# Exemplum

![Exemplum](./logo.png)


> An exemplum (Latin for "example", pl. exempla, exempli gratia = "for example", abbr.: e.g.) is a moral anecdote, brief or extended, real or fictitious, used to illustrate a point.


[![Build Status](https://dev.azure.com/forrest-technologies/exemplum/_apis/build/status/3?api-version=5.1-preview.1)](https://dev.azure.com/forrest-technologies/exemplum/_apis/build/status/3?api-version=5.1-preview.1)

## Docker 

Publish project
`dotnet publish .\src\QandA\QandA.csproj -o ./publish`
Build Container
`docker build -t exemplum -f Dockerfile .`
Run container
`docker run -d -p 8080:80 --name myapp exemplum`
