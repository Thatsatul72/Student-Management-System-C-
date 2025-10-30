namespace StudentManagementSystem
{
    public class Student
    {
        public int Id { get; set; } // PK (auto-increment)
        public string RollNumber { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Course { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} | Roll: {RollNumber} | Name: {Name} | Age: {Age} | Course: {Course}";
        }
    }
}
