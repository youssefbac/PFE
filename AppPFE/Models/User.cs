using System.ComponentModel.DataAnnotations;

namespace AppPFE.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? CodeAgence { get; set; }
        public string? NomAgence { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }



    }
}
