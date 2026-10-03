using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystem
{

    public class Instructor
    {
        public int InstructorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public string PrintDetails()
        {
            return $"ID: {InstructorId} | Name: {Name} | Specialization: {Specialization}";
        }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public Instructor Instructor { get; set; }

        public string PrintDetails()
        {
            string instructorName = Instructor != null ? Instructor.Name : "None";
            return $"Course ID: {CourseId} | Title: {Title} | Instructor: {instructorName}";
        }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();

        public bool Enroll(Course course)
        {
            if (course == null || Courses.Any(c => c.CourseId == course.CourseId))
                return false;

            Courses.Add(course);
            return true;
        }

        public string PrintDetails()
        {
            string courseList = Courses.Count > 0
                ? string.Join(", ", Courses.Select(c => c.Title))
                : "None";
            return $"Student ID: {StudentId} | Name: {Name} | Age: {Age} | Enrolled Courses: [{courseList}]";
        }
    }

    public class StudentManager
    {
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

        public bool AddStudent(Student student)
        {
            if (Students.Any(s => s.StudentId == student.StudentId)) return false;
            Students.Add(student);
            return true;
        }

        public bool AddInstructor(Instructor instructor)
        {
            if (Instructors.Any(i => i.InstructorId == instructor.InstructorId)) return false;
            Instructors.Add(instructor);
            return true;
        }

        public bool AddCourse(Course course)
        {
            if (Courses.Any(c => c.CourseId == course.CourseId)) return false;
            Courses.Add(course);
            return true;
        }

        public Student FindStudent(int studentId) => Students.FirstOrDefault(s => s.StudentId == studentId);

        public Course FindCourse(int courseId) => Courses.FirstOrDefault(c => c.CourseId == courseId);

        public Instructor FindInstructor(int instructorId) => Instructors.FirstOrDefault(i => i.InstructorId == instructorId);

        public bool EnrollStudentInCourse(int studentId, int courseId)
        {
            Student student = FindStudent(studentId);
            Course course = FindCourse(courseId);

            if (student == null || course == null) return false;
            return student.Enroll(course);
        }

        public bool IsStudentEnrolledInCourse(int studentId, int courseId)
        {
            Student student = FindStudent(studentId);
            return student != null && student.Courses.Any(c => c.CourseId == courseId);
        }

        public string GetInstructorByCourseName(string courseName)
        {
            Course course = Courses.FirstOrDefault(c => c.Title.Equals(courseName, StringComparison.OrdinalIgnoreCase));
            return course?.Instructor != null ? course.Instructor.Name : "Course or Instructor not found";
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            StudentManager manager = new StudentManager();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n================ STUDENT MANAGEMENT SYSTEM ================");
                Console.WriteLine("1.  Add Student");
                Console.WriteLine("2.  Add Instructor");
                Console.WriteLine("3.  Add Course");
                Console.WriteLine("4.  Enroll Student in Course");
                Console.WriteLine("5.  Show All Students");
                Console.WriteLine("6.  Show All Courses");
                Console.WriteLine("7.  Show All Instructors");
                Console.WriteLine("8.  Find Student by ID or Name");
                Console.WriteLine("9.  Find Course by ID or Name");
                Console.WriteLine("10. Check if Student is Enrolled in Course (Bonus)");
                Console.WriteLine("11. Get Instructor Name by Course Name (Bonus)");
                Console.WriteLine("12. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter Student ID: ");
                        int sid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Student Name: ");
                        string sname = Console.ReadLine();
                        Console.Write("Enter Student Age: ");
                        int sage = int.Parse(Console.ReadLine());

                        bool sAdded = manager.AddStudent(new Student { StudentId = sid, Name = sname, Age = sage });
                        Console.WriteLine(sAdded ? "Student added successfully." : "Student ID already exists!");
                        break;

                    case "2":
                        Console.Write("Enter Instructor ID: ");
                        int iid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Instructor Name: ");
                        string iname = Console.ReadLine();
                        Console.Write("Enter Specialization: ");
                        string spec = Console.ReadLine();

                        bool iAdded = manager.AddInstructor(new Instructor { InstructorId = iid, Name = iname, Specialization = spec });
                        Console.WriteLine(iAdded ? "Instructor added successfully." : "Instructor ID already exists!");
                        break;

                    case "3":
                        Console.Write("Enter Course ID: ");
                        int cid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Course Title: ");
                        string ctitle = Console.ReadLine();
                        Console.Write("Enter Instructor ID for this Course: ");
                        int instId = int.Parse(Console.ReadLine());

                        Instructor instructor = manager.FindInstructor(instId);
                        if (instructor == null)
                        {
                            Console.WriteLine("Instructor not found! Create the instructor first.");
                        }
                        else
                        {
                            bool cAdded = manager.AddCourse(new Course { CourseId = cid, Title = ctitle, Instructor = instructor });
                            Console.WriteLine(cAdded ? "Course added successfully." : "Course ID already exists!");
                        }
                        break;

                    case "4":
                        Console.Write("Enter Student ID: ");
                        int enrollSid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Course ID: ");
                        int enrollCid = int.Parse(Console.ReadLine());

                        bool enrolled = manager.EnrollStudentInCourse(enrollSid, enrollCid);
                        Console.WriteLine(enrolled ? "Student enrolled successfully!" : "Enrollment failed (Check IDs or duplicate enrollment).");
                        break;

                    case "5":
                        Console.WriteLine("--- All Students ---");
                        if (!manager.Students.Any()) Console.WriteLine("No students available.");
                        foreach (var s in manager.Students) Console.WriteLine(s.PrintDetails());
                        break;

                    case "6":
                        Console.WriteLine("--- All Courses ---");
                        if (!manager.Courses.Any()) Console.WriteLine("No courses available.");
                        foreach (var c in manager.Courses) Console.WriteLine(c.PrintDetails());
                        break;

                    case "7":
                        Console.WriteLine("--- All Instructors ---");
                        if (!manager.Instructors.Any()) Console.WriteLine("No instructors available.");
                        foreach (var inst in manager.Instructors) Console.WriteLine(inst.PrintDetails());
                        break;

                    case "8":
                        Console.Write("Enter Student ID or Name: ");
                        string sSearch = Console.ReadLine();
                        var foundStudents = manager.Students.Where(s =>
                            s.StudentId.ToString() == sSearch ||
                            s.Name.Equals(sSearch, StringComparison.OrdinalIgnoreCase)).ToList();

                        if (foundStudents.Any())
                            foundStudents.ForEach(s => Console.WriteLine(s.PrintDetails()));
                        else
                            Console.WriteLine("No student found.");
                        break;

                    case "9":
                        Console.Write("Enter Course ID or Title: ");
                        string cSearch = Console.ReadLine();
                        var foundCourses = manager.Courses.Where(c =>
                            c.CourseId.ToString() == cSearch ||
                            c.Title.Equals(cSearch, StringComparison.OrdinalIgnoreCase)).ToList();

                        if (foundCourses.Any())
                            foundCourses.ForEach(c => Console.WriteLine(c.PrintDetails()));
                        else
                            Console.WriteLine("No course found.");
                        break;

                    case "10":
                        Console.Write("Enter Student ID: ");
                        int chkSid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Course ID: ");
                        int chkCid = int.Parse(Console.ReadLine());

                        bool isEnrolled = manager.IsStudentEnrolledInCourse(chkSid, chkCid);
                        Console.WriteLine(isEnrolled ? "Yes, student is enrolled." : "No, student is not enrolled.");
                        break;

                    case "11":
                        Console.Write("Enter Course Title: ");
                        string titleSearch = Console.ReadLine();
                        string teacherName = manager.GetInstructorByCourseName(titleSearch);
                        Console.WriteLine($"Instructor: {teacherName}");
                        break;

                    case "12":
                        running = false;
                        Console.WriteLine("Exiting application. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please choose 1-12.");
                        break;
                }
            }
        }
    }
}