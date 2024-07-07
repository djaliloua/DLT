using System.ComponentModel.DataAnnotations;

namespace ManagPassWord.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
