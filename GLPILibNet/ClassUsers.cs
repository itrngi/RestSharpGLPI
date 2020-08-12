using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLPILibNet
{
    public class ClassUsers
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        // public string UserEmail { get; set; }
        public string UserRealName { get; set; }
        public string UserFirstName { get; set; }


        public ClassUsers(string id, string userName, string userPhone, /*string userEmail,*/ string userRealName, string userFirstName)
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
