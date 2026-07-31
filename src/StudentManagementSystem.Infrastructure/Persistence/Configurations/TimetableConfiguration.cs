using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Configurations;

public class TimetableConfiguration : IEntityTypeConfiguration<Timetable>
{
    public void Configure(EntityTypeBuilder<Timetable> builder)
    {
        builder.HasQueryFilter(t => !t.IsDeleted);

        builder.HasOne(t => t.Course).WithMany().HasForeignKey(t => t.CourseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.Subject).WithMany().HasForeignKey(t => t.SubjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.Teacher).WithMany().HasForeignKey(t => t.TeacherId).OnDelete(DeleteBehavior.Restrict);

        // The core business rule: a teacher can't be double-booked in the same slot on the same day
        builder.HasIndex(t => new { t.TeacherId, t.DayOfWeek, t.StartTime })
            .IsUnique();
    }
}