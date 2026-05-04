namespace SyllabusApp.Domain.Constants;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string Dean = "Dean";
    public const string Lecturer = "Lecturer";
    public const string Student = "Student";

    public static readonly string[] All = { Admin, Dean, Lecturer, Student };
}