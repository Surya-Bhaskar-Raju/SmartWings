using SmartWings.Domain;
using System.Threading.Tasks;

namespace SmartWings.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
    }
}