using MVVM;
using SQLite;

namespace ManagPassWord.Models
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
        public string Amount
        {
            get => amount;
            set => UpdateObservable(ref amount, value); 
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
        public DebtModelDTO(string name, string amount)
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
        private string amount = "";
        public DebtModelDTO Clone() => MemberwiseClone() as DebtModelDTO;
    }
    [Table("Debts")]
    public class DebtModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name
        {
            get => name;
            set
            {
                name = value;
            }
        }
        [MaxLength(250)]
        public string Description
        {
            get => description;
            set
            {
                description = value;
               
            }
        }
        public string Amount
        {
            get => amount;
            set
            {
                amount = value;
                
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
        public DebtModel(string name, string amount)
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
        private string amount = "";
        public DebtModel Clone() => MemberwiseClone() as DebtModel;
    }
   
}
