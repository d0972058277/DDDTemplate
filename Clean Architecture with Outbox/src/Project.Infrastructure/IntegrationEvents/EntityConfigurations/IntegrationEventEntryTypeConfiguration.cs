using Project.Infrastructure.IntegrationEvents.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.Infrastructure.IntegrationEvents.EntityConfigurations
{
    public class IntegrationEventEntryTypeConfiguration : IEntityTypeConfiguration<IntegrationEventEntry>
    {
        public void Configure(EntityTypeBuilder<IntegrationEventEntry> builder)
        {
            builder.ToTable("IntegrationEvent");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTimestamp)
                .IsRequired();

            builder.Property(e => e.State)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();

            builder.Property(e => e.TransactionId)
                .IsRequired();

            builder.HasIndex(e => e.TransactionId);
            builder.HasIndex(e => e.CreationTimestamp);
        }
    }
}