using StudentManagementSystem.Domain.Common;

namespace StudentManagementSystem.Domain.Entities;

public class Conversation : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Title { get; set; } = "New Conversation";

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}