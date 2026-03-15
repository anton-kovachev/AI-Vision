using Microsoft.OpenApi;
using Microsoft.Extensions.AI;
using OllamaSharp;
using AI.Vision.Server.Utilities;

var builder = WebApplication.CreateBuilder(args);
var ollamaVisionModelEndpoint = builder.Configuration.GetConnectionString("vision-model");
var (endpointUrl, model) = OllamaConnectionStringParser.Parse(ollamaVisionModelEndpoint);


builder.Services.AddHealthChecks();
builder.Services.AddHttpClient("OllamaHClient", config =>
{
    config.BaseAddress = new Uri(endpointUrl);
    config.Timeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IChatClient>(sp =>
{
    var httpClient = sp.GetService<IHttpClientFactory>()!.CreateClient("OllamaHClient");
    return new OllamaApiClient(httpClient) { SelectedModel = model };
});
builder.Services.AddExceptionHandler(options =>
{
    options.ExceptionHandler = async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        if (exception != null)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = exception.Message });
        }
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AI Image Processing API",
        Description = "API for processing images (such as receipts) using an Ollama vision model."
    });
});

var app = builder.Build();
var chatClient = app.Services.GetRequiredService<IChatClient>();

app.UseExceptionHandler();

app.MapPost("api/scan-receipt", async (IFormFile file, IChatClient chatClient) =>
{
    var systemMessage = new ChatMessage(ChatRole.System,
        """
            You are a helpful accounting assistant reading detailed receipt data by first checking if the image is an actual receipt. 
            And if the provided image files is an receipt then: 
            Review the net and gross costs (price + tax) of each line item and calculating the receipt's total amount.
            Provide a detailed response of the extracted information in the form of a well formatted json object.

            The json object of the response should include only these fields and nothing more:

                1. An array of line items, where each line item includes the name, price, and code (if available). Property name: lineItems. Format: array of json objects.
                2. An array of taxes, where each tax includes the name, rate, and amount. Property name: taxes. Format: array of json objects.
                3. The subtotal amount (sum of line item prices before tax). Property name: subTotalAmount. Format: decimal.
                4. The total amount (sum of line item prices including tax). Property name: totalAmount. Format: decimal.
                5. The summary of the receipt, including suggestions on whether the purchases were cheap or expensive. Property name: summary. Format: string.
                6. The merchant that issued the receipt. Property name: merchant. Format: string.
                7. If the provided file data is not an image of a receipt than the json object should only have the property error filled in. Format: string.
                8. All property names should be in camelCase.
        """);

    var memoryStream = new MemoryStream();
    file.CopyTo(memoryStream);
    systemMessage.Contents.Add(new DataContent(memoryStream.ToArray(), file.ContentType));

    var response = await chatClient.GetResponseAsync([systemMessage]);
        
    var receiptSummary = JsonExtractor.ExtractAndParse<ReceiptSummary>(
        response.ToString(), 
        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (receiptSummary == null)
    {
        return Results.BadRequest(new { error = "Failed to extract receipt information." });
    }

    return Results.Ok(receiptSummary with { fileName = file.FileName });
})
.DisableAntiforgery()
.WithName("Receipt Scanner")
.WithDescription("Upload receipt image for scanning")
.Produces<ReceiptSummary>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AI Receipt Scanner API V1");
    options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});
app.Run();

record ReceiptSummary(string fileName, string merchant, IEnumerable<LineItem> lineItems, IEnumerable<Tax> taxes, decimal subTotalAmount, decimal totalAmount, string summary);
record LineItem(string name, string code, decimal price);
record Tax(string name, decimal rate, decimal amount);

