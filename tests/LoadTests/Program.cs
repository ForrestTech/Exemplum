using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using NBomber.Plugins.Network.Ping;

var step = Step.Create("get_task_list",
    clientFactory: HttpClientFactory.Create(),
    execute: context =>
    {
        var request = Http.CreateRequest("GET", "https://localhost:5001/api/todolist")
            .WithHeader("Accept", "application/json");

        return Http.Send(request, context);
    });

var scenario = ScenarioBuilder
    .CreateScenario("task_list", step)
    .WithWarmUpDuration(TimeSpan.FromSeconds(5))
    .WithLoadSimulations(
        Simulation.InjectPerSec(rate: 50, during: TimeSpan.FromSeconds(10))
    );

// creates ping plugin that brings additional reporting data
var pingPluginConfig = PingPluginConfig.CreateDefault(new[] {"https://localhost:5001/api/todolist"});
var pingPlugin = new PingPlugin(pingPluginConfig);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithWorkerPlugins(pingPlugin)
    .Run();