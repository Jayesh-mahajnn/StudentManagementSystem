using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Configurations;

public class MarksConfiguration : IEntityTypeConfiguration<Marks>
{
    public void Configure(EntityTypeBuilder<Marks> builder)
    {
        builder.HasQueryFilter(m => !m.IsDeleted);

        builder.Property(m => m.ObtainedMarks).HasColumnType("decimal(6,2)");
        builder.Property(m => m.MaxMarks).HasColumnType("decimal(6,2)");

        builder.HasOne(m => m.Student).WithMany().HasForeignKey(m => m.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Subject).WithMany().HasForeignKey(m => m.SubjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.RecordedByTeacher).WithMany().HasForeignKey(m => m.RecordedByTeacherId).OnDelete(DeleteBehavior.Restrict);

        // Prevent duplicate marks for the same student/subject/exam type
        builder.HasIndex(m => new { m.StudentId, m.SubjectId, m.ExamType }).IsUnique();
    }
}