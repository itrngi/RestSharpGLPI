using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Phone
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        // public string UserEmail { get; set; }
        public string UserRealName { get; set; }
        public string UserFirstName { get; set; }


        public Phone(string id, string userName, string userPhone, /*string userEmail,*/ string userRealName, string userFirstName)
        {
            this.Id = id;
            this.UserName = userName;

            this.UserPhone = userPhone;
            // this.UserEmail = userEmail;
            this.UserRealName = userRealName;
            this.UserFirstName = userFirstName;
        }
    }
}
