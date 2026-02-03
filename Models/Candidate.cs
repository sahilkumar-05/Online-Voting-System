using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONLINEVS_Project.Models
{
    [Table("Candidates")]
    public class Candidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Candidate_ID")]
        public int Candidate_ID { get; set; }

        [Required, StringLength(13)]
        [Column("Candidate_CNIC")]
        public string Candidate_Cnic { get; set; } = string.Empty;

        [Required, StringLength(150)]
        [Column("CandidateName")]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(200)]
        [Column("Party")]
        public string Party { get; set; } = string.Empty;

        [Required]
        public int ElectionId { get; set; }

        [ForeignKey(nameof(ElectionId))]
        public Election? Election { get; set; }
    }
}
