using System.ComponentModel.DataAnnotations;

namespace Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
