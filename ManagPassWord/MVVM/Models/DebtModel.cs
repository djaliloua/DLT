using MVVM;

namespace ManagPassWord.MVVM.Models
{
    public class DebtModelDTO:BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => UpdateObservable(ref _id, value);
        }
        public string Name
        {
            get => name;
            set => UpdateObservable(ref name, value);
        }
     
        public string Description
        {
            get => description;
            set => UpdateObservable(ref description, value);
        }
        public double Amount
        {
            get => _amount;
            set => UpdateObservable(ref _amount, value); 
        }

        public bool IsCompleted
        {
            get => isCompleted;
            set => UpdateObservable(ref isCompleted, value, () =>
            {
                if (value)
                    PayementDate = DateTime.Now;
                else
                    PayementDate = DateTime.MinValue;
            });
        }
        private DateTime _debtDate;
        public DateTime DebtDate
        {
            get => _debtDate;
            set => UpdateObservable(ref _debtDate, value);
        }
        private DateTime _paymentDate;
        public DateTime PayementDate
        {
            get => _paymentDate;
            set => UpdateObservable(ref _paymentDate, value);
        }
        public DebtModelDTO(int id, string name, DateTime dbtDate)
        {
            Id = id;
            Name = name;
            DebtDate = dbtDate;
        }
        public DebtModelDTO(string name, double amount)
        {
            Name = name;
            Amount = amount;
            DebtDate = DateTime.Now;
        }
        public DebtModelDTO()
        {
            DebtDate = DateTime.Now;
        }
        // backing fields
        private bool isCompleted;
        private string name = "";
        private string description = "";
        private double _amount;
        public DebtModelDTO Clone() => MemberwiseClone() as DebtModelDTO;
    }

    public class DebtModel:BaseEntity
    {
        public string Name
        {
            get => name;
            set
            {
                name = value;
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
               
            }
        }
        public double Amount
        {
            get => _amount;
            set
            {
                _amount = value;
            }
        }
        
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                if (value)
                    PayementDate = DateTime.Now;
                else
                    PayementDate = DateTime.MinValue;
                
                
            }
        }
        public DateTime DebtDate { get; set; }
        public DateTime PayementDate { get; set;}
        public DebtModel(int id, string name, DateTime dbtDate)
        {
            Id = id;
            Name = name;
            DebtDate = dbtDate;
        }
        public DebtModel(string name, double amount)
        {
            Name = name;
            Amount = amount;
            DebtDate = DateTime.Now;
        }
        public DebtModel()
        {
            DebtDate = DateTime.Now;
        }
        // backing fields
        private bool isCompleted;
        private string name = "";
        private string description = "";
        private double _amount;
        public DebtModel Clone() => MemberwiseClone() as DebtModel;
    }
   
}
