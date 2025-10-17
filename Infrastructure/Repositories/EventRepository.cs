using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories;

public class EventRepository : BaseRepository<Event, ApplicationDbContext>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context) { }
}