namespace ContactsModule.Models
{
    public partial class Contacts
    {
        public int ContactId { get; set; }

        private string firstname;
        public string Firstname
        {
            get { return firstname; }
            set
            {
                firstname = value;

                if (firstname == null)
                {
                    firstname = "empty";

                }
            }
        }
        string lastname;
        public string Lastname
        {
            get { return lastname; }
            set
            {
                lastname = value;
                if (lastname == null)
                {
                    lastname = "empty";

                }
            }
        }
        string phonenumber;
        public string PhoneNumber {
            get { return phonenumber; }
            set
            {
                phonenumber = value;

                if (phonenumber == null)
                {
                    phonenumber = "empty";

                }
            }
        }
        string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;

                if (email == null)
                {
                    email = "empty";

                }
            }
        }
        string address;
        public string Address {

            get { return address; }
            set
            {
                address = value;

                if (address == null)
                {
                    address = "empty";

                }
            }
        }
        string job;
        public string Job {
            get { return job; }
            set
            {
                job = value;

                if (job == null)
                {
                    job = "empty";

                }
            }

        }
    }
}
