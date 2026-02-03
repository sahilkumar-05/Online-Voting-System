using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONLINEVS_Project.Models
{
    [Table("Votings")]
    public class Voting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteId { get; set; }

        [Required]
        public int voterid { get; set; }

        [Required]
        public int ElectionId { get; set; }

        [Required]
        public int Candidate_ID { get; set; }

        [Required]
        public DateTime CastAt { get; set; } = DateTime.Now;

        // Navigation properties
        
        [ForeignKey(nameof(voterid))]
       
        public Voter? Voter { get; set; }

        [ForeignKey(nameof(ElectionId))]
        public Election? Election { get; set; }

        
        [ForeignKey(nameof(Candidate_ID))]
        public Candidate? Candidate { get; set; }
    }
}
