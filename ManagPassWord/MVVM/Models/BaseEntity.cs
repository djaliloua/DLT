using System.ComponentModel.DataAnnotations;

namespace ManagPassWord.MVVM.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
