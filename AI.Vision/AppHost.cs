var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama("ollama").WithDataVolume();
var visionModel = ollama.AddModel("vision-model", "llama3.2-vision:latest");

builder.AddProject<Projects.AI_Vision_Server>("ai-vision-server")
    .WithExternalHttpEndpoints()
    .WithReference(visionModel)
    .WaitFor(visionModel);

builder.Build().Run();
