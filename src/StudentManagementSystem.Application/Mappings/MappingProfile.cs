using AutoMapper;
using StudentManagementSystem.Application.DTOs.Department;
using StudentManagementSystem.Application.DTOs.Student;
using StudentManagementSystem.Application.DTOs.Teacher;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

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
    }
}