using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLPILibNet
{
    public class ClassTicket
    {
        public string Id { get; set; }
        public string ThemeTicket { get; set; }
        public string TextTicket { get; set; }
        // public string UserEmail { get; set; }
        public string ComentTicket { get; set; }
        public string StatusTicket { get; set; }
        public string DateTicket { get; set; }



        public ClassTicket(string id, string themeTicket, string textTicket, /*string userEmail,*/ string comentTicket, string statusTicket, string dateTicket)
        {
            this.Id = id;
            this.ThemeTicket = themeTicket;
            this.TextTicket = textTicket;
            // this.UserEmail = userEmail;
            this.ComentTicket = comentTicket;
            this.StatusTicket = statusTicket;
            this.DateTicket = dateTicket;

        }
    }
}
