using QuoteFeeder;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<Instrument[]>(provider => Instrument.ReadFromJson("./aex.json").ToArray());
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();