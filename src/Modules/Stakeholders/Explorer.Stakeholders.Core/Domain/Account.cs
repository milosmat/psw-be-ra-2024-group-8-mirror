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

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Picture { get; private set; }
        public string Biography {  get; private set; }
        public string Motto {  get; private set; }
        public User User { get; private set; }
        public Account() { }
        public Account(int accountId, string firstName, string lastName, string picture, string biography, string motto, User user)
        {

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
