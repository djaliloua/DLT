namespace CameraApp.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }
    public class Camera: BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public override string ToString() => Name;
    }
    
}
