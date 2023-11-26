
using System;

namespace mct.models
{
    public class Registration
    {
        public Guid RegistrationId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int Zipcode { get; set; }
        public int Age { get; set; }
        public bool IsFirstTimer { get; set; }
    }
}

