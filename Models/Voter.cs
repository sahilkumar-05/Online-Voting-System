using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONLINEVS_Project.Models
{
    [Table("Voters")]
    public class Voter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("VoterID")]
        public int voterid { get; set; }

        [Required, StringLength(13)]
        [Column("VoterCNIC")]
        public string votercnic { get; set; }

        [Required, StringLength(100)]
        [Column("Name")]
        public string name { get; set; }

        [Required, EmailAddress]
        [Column("Email")]
        public string email { get; set; } 

        [Required, StringLength(8)]
        [Column("Password")]
        public string PasswordHash { get; set; } 

        [Required, StringLength(11)]
        [Column("Contact_number")]
        public string c_number { get; set; }

        [Required]
        [Column("Age")]
        [Range(18, 120, ErrorMessage = "Age must be 18 or above")]
        public int Age { get; set; }

        public bool IsVerified { get; set; }

        // Navigation property for votes
        public ICollection<Voting> Votes { get; set; } = new List<Voting>();
    }
}
