using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Infrastructure.Persistence.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Departments.Any()) return; // already seeded, don't duplicate

        var cse = new Department { Name = "Computer Science", Code = "CSE" };
        var ece = new Department { Name = "Electronics", Code = "ECE" };
        await context.Departments.AddRangeAsync(cse, ece);
        await context.SaveChangesAsync();

        var btech = new Course { Name = "B.Tech", DurationYears = 4, DepartmentId = cse.Id };
        await context.Courses.AddAsync(btech);
        await context.SaveChangesAsync();

        var dsa = new Subject { Name = "Data Structures", Code = "CS201", Credits = 4, CourseId = btech.Id };
        var os = new Subject { Name = "Operating Systems", Code = "CS202", Credits = 4, CourseId = btech.Id };
        await context.Subjects.AddRangeAsync(dsa, os);

        var teacher = new Teacher
        {
            FullName = "Dr. Anil Sharma",
            Email = "anil.sharma@college.edu",
            Phone = "9876543210",
            Gender = Gender.Male,
            DateOfJoining = new DateTime(2018, 6, 1),
            DepartmentId = cse.Id
        };
        await context.Teachers.AddAsync(teacher);

        var student = new Student
        {
            FullName = "Rahul Verma",
            Email = "rahul.verma@student.edu",
            Phone = "9123456780",
            Gender = Gender.Male,
            DateOfBirth = new DateTime(2004, 3, 15),
            EnrollmentNumber = "CSE2024001",
            DepartmentId = cse.Id,
            CourseId = btech.Id
        };
        await context.Students.AddAsync(student);

        await context.SaveChangesAsync();
    }
}