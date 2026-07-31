using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence.Configurations;

public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);

        builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Message).IsRequired().HasMaxLength(2000);

        builder.HasOne(a => a.Department).WithMany().HasForeignKey(a => a.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

        builder.HasOne(a => a.Course).WithMany().HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

        builder.HasOne(a => a.PostedByUser).WithMany().HasForeignKey(a => a.PostedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}