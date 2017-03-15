using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContactsMobile.Models
{
    public class Contact
    {

        public int ContactId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Image { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; } 

        public byte[] ImageArray { get; set; }

        public string FullName
        {
            get { return $"{ FirstName} { LastName}"; }
        }

        public string ImaheFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return "Avatar.png";
                }

                return $"http://contactsbackend.azurewebsites.net{Image.Substring(1)}";
            }
        }

        public override int GetHashCode()
        {
            return ContactId;
        }
    }
}
