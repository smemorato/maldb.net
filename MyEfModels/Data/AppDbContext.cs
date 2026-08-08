
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;


namespace MyEfModels.Data;

public class MyDbContext: DbContext
{
    public DbSet<TenraiUsageCounter> TenraiUsageCounters => Set<TenraiUsageCounter>();
    public DbSet<Anime> Animes { get; set; }
    public DbSet<AnimeExternalLink> AnimeExternalLinks { get; set; }
    public DbSet<AnimeGenre> AnimeGenres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<AnimeCompany> AnimeCompanies { get; set; }
    public DbSet<AnimeStaff> AnimeStaff { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<AnimeCharacter> AnimeCharacters { get; set; }
     public DbSet<AnimeCharacterVoiceActor> AnimeCharacterVoiceActors { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<AnimeRecommendation> AnimeRecommendations { get; set; }
    public DbSet<AnimeRelation> AnimeRelations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserList> UserList { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TenraiUsageCounter>(builder =>
        {
            builder.HasIndex(c => c.Date).IsUnique();
        });

        modelBuilder.Entity<Anime>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.MalId).IsUnique();
            builder.HasIndex(a => a.MediaType);
            builder.HasIndex(a => a.TitleRomanji);
        });

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Username).IsUnique();
            builder.Property(u => u.Username).IsRequired();
        });

        modelBuilder.Entity<UserList>(builder =>
        {
            builder.HasIndex(ul => ul.Status);
        });


        modelBuilder.Entity<AnimeRecommendation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Anime1)
                .WithMany(a => a.RecommendationsFrom)
                .HasForeignKey(e => e.Anime1Id)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Anime2)
                .WithMany(a => a.RecommendationsTo)
                .HasForeignKey(e => e.Anime2Id)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.Anime1Id, e.Anime2Id })
                .IsUnique();
        });

         modelBuilder.Entity<AnimeRelation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Anime1)
                .WithMany(a => a.RelationsFrom)
                .HasForeignKey(e => e.Anime1Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Anime2)
                .WithMany(a => a.RelationsTo)
                .HasForeignKey(e => e.Anime2Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.Anime1Id, e.Anime2Id, e.Type })
                .IsUnique();
        });


        modelBuilder.Entity<AnimeStaff>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Anime)
                .WithMany(a => a.Staff)
                .HasForeignKey(e => e.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Person)
                .WithMany(p => p.AnimeStaffRoles)
                .HasForeignKey(e => e.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.AnimeId, e.PersonId, e.Position })
                .IsUnique();
        });


        modelBuilder.Entity<AnimeCharacter>(entity =>
        {

            entity.HasKey(ac => ac.Id);

            entity.HasOne(ac => ac.Anime)
                .WithMany(a => a.AnimeCharacters)
                .HasForeignKey(ac => ac.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ac => ac.Character)
                .WithMany(c => c.AnimeCharacters)
                .HasForeignKey(ac => ac.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(ac => ac.Role)
                .HasMaxLength(50);

            entity.HasIndex(e => new { e.AnimeId, e.CharacterId })
                .IsUnique();

        });



        modelBuilder.Entity<AnimeCharacterVoiceActor>(entity =>
        {

            entity.HasKey(ac => ac.Id);

            entity.HasOne(acva => acva.AnimeCharacter)
                .WithMany(acva => acva.VoiceActors)
                .HasForeignKey(ac => ac.AnimeCharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(acva => acva.Person)
                .WithMany(acva => acva.VoiceActingRoles)
                .HasForeignKey(ac => ac.PersonId)
                .OnDelete(DeleteBehavior.Cascade);


            entity.HasIndex(e => new { e.AnimeCharacterId, e.PersonId, e.Language })
                .IsUnique();

        });


        modelBuilder.Entity<Person>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.MalId);
            
        });

        modelBuilder.Entity<Character>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Name);
            builder.HasIndex(c => c.MalId);
            
        });


        modelBuilder.Entity<AnimeExternalLink>(builder =>
        {
           builder.HasIndex(el => new {el.Url, el.AnimeId, el.Name}).IsUnique();
        });




    }


}