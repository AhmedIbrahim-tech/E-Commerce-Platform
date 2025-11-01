using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DeliveryConfig : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.Property(d => d.DeliveryMethod)
                .HasConversion(
                DM => DM.ToString(),
                DM => (DeliveryMethod)Enum.Parse(typeof(DeliveryMethod), DM!))
                .IsRequired();

            builder.Property(d => d.Description)
                .HasMaxLength(300);

            builder.Property(d => d.Cost)
                .HasColumnType("decimal(18,2)");
        }
    }
}
