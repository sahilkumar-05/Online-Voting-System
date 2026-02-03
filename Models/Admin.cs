using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONLINEVS_Project.Models
{
    [Table("Admin")]
    public class Admin
    {
        [Key]
        [Required, StringLength(13)]
        [Column("Cnic")]
        public string AdminId { get; set; }

        [StringLength(100)]
        [Column("Name")]
        public string name { get; set; }

        [Required, EmailAddress]
        [Column("Email")]
        public string email { get; set; }

        
        [Required, StringLength(8)]
        [Column("password")]
        public string PasswordHash { get; set; }
    }
}
