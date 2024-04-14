using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataExporter.Model
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Text { get; set; }
        public int PolicyId { get; set; }

        [ForeignKey(nameof(PolicyId))]
        public Policy? Policy { get; set; }
    }
}
