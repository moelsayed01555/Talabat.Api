using System;
using System.Collections.Generic;
using System.Text;

namespace Talabat.Core.Entites.Order_Aggregate
{
    public class Address
    {
        public Address()
        {
            
        }
        public Address(string firstName, string lastName, string street, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
        }
        public string City { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
    }
}
