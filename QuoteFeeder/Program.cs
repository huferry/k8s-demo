using QuoteFeeder;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<Instrument[]>(_ => Instrument.ReadFromJson("./aex.json").ToArray());
        services.AddHostedService<Worker>();
        services.AddSingleton<Sender, Sender>();
        services.AddSingleton<QueueConfig, QueueConfig>();
    })
    .Build();

await host.RunAsync();