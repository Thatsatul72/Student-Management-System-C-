using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem
{
    public class EfStudentManager
    {
        public void AddStudent(Student s)
        {
            using var ctx = new StudentContext();
            ctx.Students.Add(s);
            ctx.SaveChanges();
        }

        public bool UpdateStudent(int id, Action<Student> updateAction)
        {
            using var ctx = new StudentContext();
            var st = ctx.Students.FirstOrDefault(x => x.Id == id);
            if (st == null) return false;
            updateAction(st);
            ctx.SaveChanges();
            return true;
        }

        public bool DeleteStudent(int id)
        {
            using var ctx = new StudentContext();
            var st = ctx.Students.FirstOrDefault(x => x.Id == id);
            if (st == null) return false;
            ctx.Students.Remove(st);
            ctx.SaveChanges();
            return true;
        }

        public List<Student> SearchByName(string namePart)
        {
            using var ctx = new StudentContext();
            return ctx.Students
                .Where(x => EF.Functions.Like(x.Name, $"%{namePart}%"))
                .OrderBy(x => x.RollNumber)
                .ToList();
        }

        public Student SearchByRoll(string rollNumber)
        {
            using var ctx = new StudentContext();
            return ctx.Students.FirstOrDefault(x => x.RollNumber.Equals(rollNumber, StringComparison.OrdinalIgnoreCase));
        }

        public Student GetById(int id)
        {
            using var ctx = new StudentContext();
            return ctx.Students.FirstOrDefault(x => x.Id == id);
        }

        public List<Student> GetAll()
        {
            using var ctx = new StudentContext();
            return ctx.Students.OrderBy(x => x.RollNumber).ToList();
        }
    }
}
