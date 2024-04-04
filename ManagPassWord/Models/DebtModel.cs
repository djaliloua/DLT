using SQLite;

namespace ManagPassWord.Models
{
    
    [Table("Debts")]
    public class DebtModel
    {
        public static event Action<DebtModel> OnUiUpdate;
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                if(OnUiUpdate != null)
                    OnUiUpdate(this);
            }
        }
        [MaxLength(250)]
        public string Description
        {
            get => description;
            set
            {
                description = value;
                if (OnUiUpdate != null)
                    OnUiUpdate(this);
            }
        }
        public string Amount
        {
            get => amount;
            set
            {
                amount = value;
                if (OnUiUpdate != null)
                    OnUiUpdate(this);
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
                if (OnUiUpdate != null)
                    OnUiUpdate(this);
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
        protected virtual void OnOnUpdate(DebtModel model) => OnUiUpdate?.Invoke(model);
    }
   
}
