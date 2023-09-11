using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Plugins.Network.Ping;

var httpClient = new HttpClient();

var scenario =  Scenario.Create("get_task_list",
    async context =>
    {
        var request = Http.CreateRequest("GET", "https://localhost:5001/api/todolist")
            .WithHeader("Accept", "application/json");

        var response = await Http.Send(httpClient, request);
            
        return response;
    })
    .WithWarmUpDuration(TimeSpan.FromSeconds(5))
    .WithLoadSimulations(
        Simulation.Inject(rate: 100,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromMinutes(3))
    );

// creates ping plugin that brings additional reporting data
var pingPluginConfig = PingPluginConfig.CreateDefault(new[] {"https://localhost:5001/api/todolist"});
var pingPlugin = new PingPlugin(pingPluginConfig);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithWorkerPlugins(pingPlugin)
    .Run();