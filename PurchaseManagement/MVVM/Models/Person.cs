using SQLiteNetExtensions.Attributes;

namespace PurchaseManagement.MVVM.Models
{


    public class Person: BaseEntity
    {
        public string Name { get; set; }
        [ForeignKey(typeof(Car))]
        public int CarId { get; set; }

        [OneToOne]
        public Car Car { get; set; }
        public Person(string name)
        {
            Name = name;
        }
        public Person()
        {
            
        }
    }
    public class Car : BaseEntity
    {
        public string Name { get; set; }
        [ForeignKey(typeof(Person))]
        public int PersonId {  get; set; }
        [OneToOne]
        public Person Person { get; set; }
        public Car(int personId, string name)
        {
            Name = name;
            PersonId = personId;
        }
        public Car()
        {
            
        }
    }
}
