using System.ComponentModel.DataAnnotations;
namespace HelloBlazorApp.Components.Pages
{
    public class Person
    {
        [PersonNameValidator(new[] { "admin" })]
        [Required (ErrorMessage = "Необходимо ввести имя")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина имени должна быть от {2} до {1} символов")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Необходимо ввести возраст")]
        [Range(1, 110, ErrorMessage = "Возраст должен находиться в диапазоне от {1} до {2}")]
        public int Age { get; set; }
        public DateTime Day { get; set; } = DateTime.Now;
        public bool Working { get; set; }
        public string Subject { get; set; } = "";
        public string[] Courses { get; set; } = [];
    }
}
