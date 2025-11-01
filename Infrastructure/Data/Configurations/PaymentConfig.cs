using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.PaymentMethod)
                .HasConversion(
                PM => PM.ToString(),
                PM => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), PM!))
                .IsRequired();

            builder.Property(p => p.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Status)
                .HasConversion(
                Sts => Sts.ToString(),
                Sts => (Status)Enum.Parse(typeof(Status), Sts!))
                .IsRequired();
        }
    }
}
