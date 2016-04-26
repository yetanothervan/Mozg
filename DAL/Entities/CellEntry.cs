using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class CellEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CellId { get; set; }
        public int TimeMoment { get; set; }
        public double Value { get; set; }
    }
}
