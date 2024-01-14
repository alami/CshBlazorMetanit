namespace HelloBlazorApp
{
    public class Student
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now; // дата поступления
        public bool HasWork { get; set; }                           // работает или нет
        public string Subject { get; set; } = "";                   // специальность
        public string[] Courses { get; set; } = [];   // доп. курсы
    }
}
