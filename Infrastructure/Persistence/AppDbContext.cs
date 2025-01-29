using Domain.Aggregates.MessageAggregate;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Persistence;

public class AppDbContext: DbContext
{
    public AppDbContext() : base()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(opts =>
        {
            opts.HasKey(x => x.Id);
            opts.Property(x => x.Id).ValueGeneratedOnAdd();
            opts.Property(x => x.UserIpAddress).IsRequired();
            opts.Property(x => x.Content).HasColumnType("nvarchar(max)");
            opts.Property(x => x.SentAt).IsRequired();
            opts.Property(x => x.Rate);

            opts.Property(x => x.Sender)
                .HasColumnType("integer")
                .IsRequired();

            // Sender constraint.
            var accessibleSenderValues = string.Join(", ", EnumHelpers.ToArray<MessageSender>());
            opts.ToTable(tb => tb.HasCheckConstraint("CK_Message_Sender", $@"Sender IN ({accessibleSenderValues})"));

            // Rate constraint.
            opts.ToTable(tb => tb.HasCheckConstraint("CK_Message_Rate", "Rate >= 0"));

            // Indexes.
            opts.HasIndex(p => new { p.UserIpAddress, p.SentAt });
        });
    }
}