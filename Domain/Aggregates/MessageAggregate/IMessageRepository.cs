using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Aggregates.MessageAggregate;

public interface IMessageRepository<out TC> : IRepository<Message, Guid, TC>
    where TC : DbContext
{
}