using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);

        builder.HasOne(a => a.Student).WithMany().HasForeignKey(a => a.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Subject).WithMany().HasForeignKey(a => a.SubjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.MarkedByTeacher).WithMany().HasForeignKey(a => a.MarkedByTeacherId).OnDelete(DeleteBehavior.Restrict);

        // Prevent duplicate attendance entries for the same student/subject/day
        builder.HasIndex(a => new { a.StudentId, a.SubjectId, a.Date }).IsUnique();
    }
}