namespace Talabat.Core.Entites.Identity
{
    public class Address
    {
        public int Id { get; set; }

        public string FName { get; set; }        // fname ,lname  is name of reciever يعني اليوسر حد تاني يستلم مكانه الشحنة
         
        public string LName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string AppUserId { get; set; }  // (fk) because id of user is string

        public AppUser AppUser { get; set; }
    }
}

// in this bussiness every user has one address ( relation 1 : 1 )