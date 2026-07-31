using AutoMapper;
using StudentManagementSystem.Application.DTOs.Department;
using StudentManagementSystem.Application.DTOs.Student;
using StudentManagementSystem.Application.DTOs.Teacher;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Application.DTOs.Timetable;
using StudentManagementSystem.Application.DTOs.Assignment;
using StudentManagementSystem.Application.DTOs.Announcement;
using StudentManagementSystem.Application.DTOs.Attendance;
using StudentManagementSystem.Application.DTOs.Marks;

namespace StudentManagementSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.CourseCount, opt => opt.MapFrom(src => src.Courses.Count))
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students.Count));

        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();


        CreateMap<Student, StudentDto>()
    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
    .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
    .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));

        CreateMap<CreateStudentDto, Student>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)));

        CreateMap<UpdateStudentDto, Student>();

        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

        CreateMap<CreateTeacherDto, Teacher>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)));

        CreateMap<UpdateTeacherDto, Teacher>();


        CreateMap<Domain.Entities.Timetable, TimetableDto>()
    .ForMember(d => d.DayOfWeek, o => o.MapFrom(s => s.DayOfWeek.ToString()))
    .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.Name))
    .ForMember(d => d.SubjectName, o => o.MapFrom(s => s.Subject.Name))
    .ForMember(d => d.TeacherName, o => o.MapFrom(s => s.Teacher.FullName));

        CreateMap<CreateTimetableDto, Domain.Entities.Timetable>()
            .ForMember(d => d.DayOfWeek, o => o.MapFrom(s => Enum.Parse<DayOfWeekEnum>(s.DayOfWeek, true)));

        CreateMap<UpdateTimetableDto, Domain.Entities.Timetable>()
            .ForMember(d => d.DayOfWeek, o => o.MapFrom(s => Enum.Parse<DayOfWeekEnum>(s.DayOfWeek, true)));

        CreateMap<Assignment, AssignmentDto>()
            .ForMember(d => d.SubjectName, o => o.MapFrom(s => s.Subject.Name))
            .ForMember(d => d.TeacherName, o => o.MapFrom(s => s.Teacher.FullName));

        CreateMap<CreateAssignmentDto, Assignment>();
        CreateMap<UpdateAssignmentDto, Assignment>();

        CreateMap<Announcement, AnnouncementDto>()
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.Name : null))
            .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course != null ? s.Course.Name : null))
            .ForMember(d => d.PostedByName, o => o.MapFrom(s => s.PostedByUser.FullName));

        CreateMap<CreateAnnouncementDto, Announcement>();
        CreateMap<UpdateAnnouncementDto, Announcement>();

        CreateMap<Attendance, AttendanceDto>()
    .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
    .ForMember(d => d.StudentName, o => o.MapFrom(s => s.Student.FullName))
    .ForMember(d => d.SubjectName, o => o.MapFrom(s => s.Subject.Name))
    .ForMember(d => d.MarkedByTeacherName, o => o.MapFrom(s => s.MarkedByTeacher.FullName));

        CreateMap<Marks, MarksDto>()
            .ForMember(d => d.ExamType, o => o.MapFrom(s => s.ExamType.ToString()))
            .ForMember(d => d.StudentName, o => o.MapFrom(s => s.Student.FullName))
            .ForMember(d => d.SubjectName, o => o.MapFrom(s => s.Subject.Name));
    }
}