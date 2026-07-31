using StudentManagementSystem.Domain.Common;

namespace StudentManagementSystem.Domain.Entities;

public class Announcement : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

    // Nullable = global announcement (visible to all) when both are null
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }

    public int PostedByUserId { get; set; }
    public User PostedByUser { get; set; } = null!;
}