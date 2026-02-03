using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONLINEVS_Project.Models
{
    [Table("Elections")]
    public class Election
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("ElectionId")]
        public int ElectionId { get; set; }

        [Required, StringLength(200)]
        [Column("Title")]
        public string Title { get; set; }

        [Required]
        [Column("Description")]
        public string Description { get; set; }

        [Required]
        [Column("Start_Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("End_Date")]
        public DateTime EndDate { get; set; }

        public List<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
}
