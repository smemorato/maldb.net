using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Data;
using Polly;
using Dbseeder.Services;
using MyEfModels.Entities;
using Api.Mal;
using Api.Tenrai;
using Dbseeder.Services.Shared;
using Dbseeder.Services.Updaters;


class Program
{
    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        string connectionString =
            builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");

        builder.Services.AddDbContext<MyDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        builder.Services.Configure<MalApiOptions>(
            builder.Configuration.GetSection("MalApi"));

        builder.Services.AddScoped<RankingGenre>();
        builder.Services.AddScoped<RankingStudio>();
        builder.Services.AddScoped<RankingImporter>();
        builder.Services.AddScoped<UserListImporter>();
        builder.Services.AddScoped<AnimeIdResolver>();
        builder.Services.AddScoped<AnimeUpdateOrchestrator>();
        builder.Services.AddScoped<TenraiRateLimiter>();
        builder.Services.AddScoped<CharacterDetailsService>();
        builder.Services.AddScoped<PersonDetailsService>();
        builder.Services.AddScoped<TenraiUsageService>();
        builder.Services.AddTransient<TenraiRequestCounterHandler>();

        builder.Services.AddHttpClient<IMalApiClient, MalApiClient>();
        builder.Services.AddHttpClient<ITenraiApiClient, TenraiApiClient>()
            .AddHttpMessageHandler<TenraiRequestCounterHandler>()
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                PooledConnectionIdleTimeout = TimeSpan.FromSeconds(30),
                MaxConnectionsPerServer = 10
            })
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(2)));



        




        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            var tenraiOrchestrator = scope.ServiceProvider.GetRequiredService<AnimeUpdateOrchestrator>();
            var resolver = scope.ServiceProvider.GetRequiredService<AnimeIdResolver>();
            var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            
            var mal = scope.ServiceProvider.GetRequiredService<IMalApiClient>();
            var tenrai = scope.ServiceProvider.GetRequiredService<ITenraiApiClient>();




            var rankingImporter = scope.ServiceProvider.GetRequiredService<RankingImporter>();
            var userListimporter = scope.ServiceProvider.GetRequiredService<UserListImporter>();


            var characterDetailsUpdate = scope.ServiceProvider.GetRequiredService<CharacterDetailsService>();
            var PersonDetailsUpdate = scope.ServiceProvider.GetRequiredService<PersonDetailsService>();

            // await rankingImporter.ImportRankingAsync("all");
            await userListimporter.ImportUserListAsync("","smemorato");


            // 1. Resolve anime IDs from user list
            var ids = await resolver.ResolveFromAnimeTable(lastUpdate: new DateOnly(2026, 8, 2));

            // 2. Create the updater manually
            var AnimeDetailsupdater = new AnimeDetailsUpdater(tenrai, db);

            // 3. Run the orchestrator
            await tenraiOrchestrator.RunAsync(ids, AnimeDetailsupdater);


            // await animeCharacterUpdate.UpdateAnimeCharacter("smemorato");
            // await animeStaffUpdate.UpdateAnimeStaff("smemorato");
            await characterDetailsUpdate.UpdateCharacterDetails("smemorato");

        }

    }
}
