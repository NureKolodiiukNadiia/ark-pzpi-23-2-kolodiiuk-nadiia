using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Interfaces.Repositories;

namespace CoWorkingAccess.Infrastructure.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(CoWorkingAccessDbContext context) : base(context)
    {
    }
}