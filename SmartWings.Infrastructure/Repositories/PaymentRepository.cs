using System.Threading.Tasks;
using SmartWings.Domain;
using SmartWings.Domain.Interfaces;
using SmartWings.Infrastructure.DataContext;

namespace SmartWings.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly FlightDbContext _context;

        public PaymentRepository(FlightDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
    }
}
