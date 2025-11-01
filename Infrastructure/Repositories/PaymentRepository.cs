
namespace Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepositoryAsync<Payment>, IPaymentRepository
    {
        private readonly DbSet<Payment> _payments;
        public PaymentRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _payments = dbContext.Set<Payment>();
        }

        public async Task<Payment?> GetPaymentByTransactionId(string transactionId)
        {
            return await _payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<Payment?> GetPaymentByOrderId(Guid orderId)
        {
            return await _payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
    }
}
