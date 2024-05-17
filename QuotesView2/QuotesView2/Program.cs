using QuotesView2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
    policy.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod()));

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<QuoteReceiver, QuoteReceiver>();
builder.Services.AddSingleton<QueueConfig, QueueConfig>();

var app = builder.Build();

app.UseCors();
app.MapControllers();
app.MapHub<QuotesHub>("stream");
app.Services.GetService<QuoteReceiver>()?.Setup();
app.MapGet("/health", () => "I'm healty!");
app.Run();
