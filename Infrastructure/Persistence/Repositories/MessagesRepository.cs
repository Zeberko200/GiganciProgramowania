using Domain.Aggregates.MessageAggregate;

namespace Infrastructure.Persistence.Repositories;

public sealed class MessagesRepository(AppDbContext context) : Repository<Message, Guid, AppDbContext>(context), IMessageRepository<AppDbContext>
{
}