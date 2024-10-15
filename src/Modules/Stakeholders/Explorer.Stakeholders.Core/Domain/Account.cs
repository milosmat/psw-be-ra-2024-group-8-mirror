using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Account : Entity
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public string Biography {  get; set; }
        public string Motto {  get; set; }
        public User User { get; set; }
        public Account() { }
        public Account(int accountId, string firstName, string lastName, string picture, string biography, string motto, User user)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
            Picture = picture;
            Biography = biography;
            Motto = motto;
            User = user;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) throw new ArgumentException("Invalid First Name");
            if (string.IsNullOrWhiteSpace(LastName)) throw new ArgumentException("Invalid Last Name");
            if (string.IsNullOrWhiteSpace(Picture)) throw new ArgumentException("Invalid Picture");
            if (string.IsNullOrWhiteSpace(Biography)) throw new ArgumentException("Invalid Biography");
            if (string.IsNullOrWhiteSpace(Biography)) throw new ArgumentException("Invalid Motto");
        }
    }
}
