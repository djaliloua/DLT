using System.ComponentModel;

namespace CameraApp.ViewModels
{
    public interface IClone<T> where T: class
    {
        T Clone();
    }
    public class CameraViewModel:BaseViewModel, IClone<CameraViewModel>, IDataErrorInfo
    {
        public int Id { get; set; } 
        private string _name;
        public string Name
        {
            get => _name;
            set => UpdateObservable(ref _name, value);
        }
        private string _address = "http://192.168.1.";
        public string Address
        {
            get => _address;
            set => UpdateObservable(ref _address, value);   
        }
        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => UpdateObservable(ref isActive, value);
        }

        public string Error
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    return "Name is required";
                }
                if (string.IsNullOrWhiteSpace(Address))
                {
                    return "Address is required";
                }
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        break;
                    case nameof(Address):
                        break;
                }
                if (string.IsNullOrWhiteSpace(Name))
                {
                    result = "Name is required";
                }
                if(string.IsNullOrWhiteSpace(Address))
                {
                    result = "Address is required";
                }
                return result;
            }
        }

        public CameraViewModel()
        {
            
        }
        public CameraViewModel(string name, string address)
        {
            Name = name;
            Address = address;
        }
        public override string ToString()
        {
            return $"{Name}({(IsActive ? "Active" : "Inactive")})"; 
        }

        public CameraViewModel Clone() => (CameraViewModel)MemberwiseClone();
        
    }
}
