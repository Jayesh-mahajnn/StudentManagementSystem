using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IConversationRepository : IRepository<Conversation>
{
    Task<Conversation?> GetWithMessagesAsync(int conversationId, int userId);
}