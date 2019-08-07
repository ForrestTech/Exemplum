# Exemplum
An example micro services setup

## Docker 

Publish project
`dotnet publish .\src\QandA\QandA.csproj -o ./publish`
Build Container
`docker build -t exemplum -f Dockerfile .`
Run container
`docker run -d -p 8080:80 --name myapp exemplum`
