using SmartWings.Domain;
using SmartWings.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using SmartWings.Infrastructure.Contracts;

namespace SmartWings.Infrastructure.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly FlightDbContext _context;

        public OtpRepository(FlightDbContext context)
        {
            _context = context;
        }

        // Add a new OTP record (For Password Reset)
        public async Task AddAsync(OtpRecord otpRecord)
        {
            await _context.OtpRecords.AddAsync(otpRecord);
            await _context.SaveChangesAsync();
        }

        // Get a valid OTP record by email and OTP (For Password Reset)
        public async Task<OtpRecord> GetValidOtpAsync(string email, string otp)
        {
            return await _context.OtpRecords
                .FirstOrDefaultAsync(x => x.Email == email && x.Otp == otp && !x.IsUsed);
        }

        public async Task UpdateAsync(OtpRecord otpRecord) // Update the OTP record (For Password Reset)
        {
            _context.OtpRecords.Update(otpRecord);
            await _context.SaveChangesAsync();
        }
        
        // Get an OTP record by OTP (For pre-registration validation)
        public async Task<OtpRecord> GetByOtpAsync(string otp)
        {
            return await _context.OtpRecords
                .FirstOrDefaultAsync(x => x.Otp == otp && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow);
        }
    }
}