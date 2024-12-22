using System;

namespace Authentication
{
    public class AuthenticationToken
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ValidityDuration { get; set; }
    }
}
