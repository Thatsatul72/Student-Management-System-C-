using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new StudentContext())
            {
                // Option A (Beginner): automatically create DB & table if not exists
                ctx.Database.EnsureCreated();

                // Option B (Advanced): use migrations instead of EnsureCreated (commands shown later)
            }

            var manager = new EfStudentManager();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Student Management System (SQLite + EF Core) ===\n");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Update Student");
                Console.WriteLine("3. Delete Student");
                Console.WriteLine("4. Search by Name");
                Console.WriteLine("5. Search by Roll Number");
                Console.WriteLine("6. List All Students");
                Console.WriteLine("7. Exit\n");
                Console.Write("Select an option: ");
                var opt = Console.ReadLine();

                try
                {
                    switch (opt)
                    {
                        case "1": AddStudentFlow(manager); break;
                        case "2": UpdateStudentFlow(manager); break;
                        case "3": DeleteStudentFlow(manager); break;
                        case "4": SearchByNameFlow(manager); break;
                        case "5": SearchByRollFlow(manager); break;
                        case "6": ListAllFlow(manager); break;
                        case "7": return;
                        default:
                            Console.WriteLine("Invalid option. Press Enter to continue...");
                            Console.ReadLine();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        static void AddStudentFlow(EfStudentManager manager)
        {
            Console.Clear();
            Console.WriteLine("--- Add Student ---");
            Console.Write("Roll Number: ");
            var roll = Console.ReadLine()?.Trim();
            Console.Write("Name: ");
            var name = Console.ReadLine()?.Trim();
            Console.Write("Age: ");
            var ageText = Console.ReadLine()?.Trim();
            Console.Write("Course: ");
            var course = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(roll) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ageText))
            {
                Console.WriteLine("Invalid input. Press Enter to return to menu...");
                Console.ReadLine();
                return;
            }

            if (!int.TryParse(ageText, out int age))
            {
                Console.WriteLine("Age must be a number. Press Enter to return to menu...");
                Console.ReadLine();
                return;
            }

            var s = new Student { RollNumber = roll, Name = name, Age = age, Course = course };
            manager.AddStudent(s);
            Console.WriteLine("Student added. Press Enter to continue...");
            Console.ReadLine();
        }

        static void UpdateStudentFlow(EfStudentManager manager)
        {
            Console.Clear();
            Console.WriteLine("--- Update Student ---");
            Console.Write("Enter Id to update: ");
            var idText = Console.ReadLine()?.Trim();
            if (!int.TryParse(idText, out int id))
            {
                Console.WriteLine("Invalid id. Press Enter...");
                Console.ReadLine();
                return;
            }

            var st = manager.GetById(id);
            if (st == null)
            {
                Console.WriteLine("Student not found. Press Enter...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Found: {st}");
            Console.Write("New Name (leave blank to keep): ");
            var name = Console.ReadLine();
            Console.Write("New Age (leave blank to keep): ");
            var ageText = Console.ReadLine();
            Console.Write("New Course (leave blank to keep): ");
            var course = Console.ReadLine();

            bool updated = manager.UpdateStudent(id, s =>
            {
                if (!string.IsNullOrWhiteSpace(name)) s.Name = name.Trim();
                if (!string.IsNullOrWhiteSpace(ageText) && int.TryParse(ageText, out int a)) s.Age = a;
                if (!string.IsNullOrWhiteSpace(course)) s.Course = course.Trim();
            });

            Console.WriteLine(updated ? "Student updated. Press Enter to continue..." : "Update failed.");
            Console.ReadLine();
        }

        static void DeleteStudentFlow(EfStudentManager manager)
        {
            Console.Clear();
            Console.WriteLine("--- Delete Student ---");
            Console.Write("Enter Id to delete: ");
            var idText = Console.ReadLine()?.Trim();
            if (!int.TryParse(idText, out int id))
            {
                Console.WriteLine("Invalid id. Press Enter...");
                Console.ReadLine();
                return;
            }

            Console.Write("Are you sure? (y/n): ");
            var confirm = Console.ReadLine();
            if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Cancelled. Press Enter...");
                Console.ReadLine();
                return;
            }

            var ok = manager.DeleteStudent(id);
            Console.WriteLine(ok ? "Deleted. Press Enter..." : "Student not found. Press Enter...");
            Console.ReadLine();
        }

        static void SearchByNameFlow(EfStudentManager manager)
        {
            Console.Clear();
            Console.WriteLine("--- Search by Name ---");
            Console.Write("Enter name or part of name: ");
            var q = Console.ReadLine()?.Trim();
            var res = manager.SearchByName(q ?? string.Empty);
            if (!res.Any())
            {
                Console.WriteLine("No students matched. Press Enter...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Found {res.Count} student(s):\n");
            foreach (var s in res) Console.WriteLine(s);
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        static void SearchByRollFlow(EfStudentManager manager)
        {
            Console.Clear();
            Console.WriteLine("--- Search by Roll Number ---");
            Console.Write("Enter roll number: ");
            var roll = Console.ReadLine()?.Trim();
            var s = manager.SearchByRoll(roll);
            if (s == null)
            {
                Console.WriteLine("Not found. Press Enter...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(s);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        static void ListAllFlow(EfStudentManager manager)
        {
            Console.Clear();
            Console.WriteLine("--- All Students ---\n");
            var all = manager.GetAll();
            if (!all.Any())
            {
                Console.WriteLine("No students yet.");
            }
            else
            {
                foreach (var s in all) Console.WriteLine(s);
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
