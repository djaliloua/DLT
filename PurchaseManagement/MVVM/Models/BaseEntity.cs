using System.ComponentModel.DataAnnotations;

namespace PurchaseManagement.MVVM.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
