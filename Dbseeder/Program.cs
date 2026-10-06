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
using Challenges;


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

        
        
        builder.Services.AddHttpClient<AnimeChallengeRepository>();
        
        builder.Services.AddScoped<MatchingChallenge>();



        




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
            // await userListimporter.ImportUserListAsync("","smemorato");


            // 1. Resolve anime IDs from user list
            //var ids = await resolver.ResolveFromAnimeTable(username: "smemorato", startDate: "2024-06-01");
            var ids = await resolver.ResolveFromAnimeTable(username: "smemorato",startDate: "2013-07-06");
            var characterIds = await resolver.ResolveFromCharacterTable(username: "smemorato",startDate: "2013-07-06");

            // 2. Create the updater manually
            var animeDetailsupdater = new AnimeDetailsUpdater(tenrai, db);
            var animeRecommendationsUpdater = new AnimeRecommendationUpdater(tenrai, db);
            var animeCharacterUpdater = new AnimeCharacterUpdater(tenrai, db);
            var animeStaffupdater = new AnimeStaffUpdater(tenrai, db);
            var animeReviewsUpdater = new AnimeReviewsUpdater(tenrai, db);
            var animeEpisodesUpdater = new AnimeEpisodesUpdater(tenrai, db);
            var characterDetailsUpdater = new CharacterDetailsUpdater(tenrai, db);


            // 3. Run the orchestrator
            await tenraiOrchestrator.RunAsync(ids, animeDetailsupdater);
            // await tenraiOrchestrator.RunAsync(ids, animeRecommendationsUpdater);
            // await tenraiOrchestrator.RunAsync(ids, animeStaffupdater);
            // await tenraiOrchestrator.RunAsync(ids, animeReviewsUpdater);
            // await tenraiOrchestrator.RunAsync(ids, animeCharacterUpdater);
            // await tenraiOrchestrator.RunAsync(ids, animeDetailsupdater);
            //await tenraiOrchestrator.RunAsync(characterIds, characterDetailsUpdater)


            // await animeCharacterUpdate.UpdateAnimeCharacter("smemorato");
            // await animeStaffUpdate.UpdateAnimeStaff("smemorato");
            // await characterDetailsUpdate.UpdateCharacterDetails("smemorato");




        }



        using (var scope = app.Services.CreateScope())
        {
                        // 2. Resolve MatchingChallenge from the scope
            var matchingChallenge = scope.ServiceProvider.GetRequiredService<MatchingChallenge>();

            // 3. Execute the challenge with your arguments
            string mediaType = "TV"; // or "Movie"
            DateOnly startDate = new DateOnly(2025, 6, 26);
            string username = "smemorato";

            try
            {
                await matchingChallenge.RunMatchingChallenge(mediaType, startDate, username, 13);
                Console.WriteLine("Matching Challenge completed successfully!");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
