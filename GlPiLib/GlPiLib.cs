using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


using static System.Net.Mime.MediaTypeNames;

namespace GlPiLib
{


    public class GlPiLib
    {

        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string AuthType { get; set; }
        public string AppTocken { get; set; }
        public string GLPIurl { get; set; }



        public string Pathfile { get; set; }
        public string Namefile { get; set; }
        public string Fpath { get; set; }
        public string Session { get; set; }
        public string SessionAdmin { get; set; }
        public RestClient clientses;

        public string ErrorLib { get; set; }


        public string SessionConnectResult { get; set; }


        /*
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }  
        public string UserRealName { get; set; }
        public string UserFirstName { get; set; }
        */


        public GlPiLib(string appTocken, string glpiUrl)
        {
            /*this.UserName = userName;
            this.UserPass = userPass;
            this.AuthType = authType;*/
            this.AppTocken = appTocken;
            this.GLPIurl = glpiUrl;
        }

        public string loginGlpi(string user, string pass, string authType/*, string appTocken*/)
        {
            string result = string.Empty;
            this.UserName = user;
            this.UserPass = pass;
            this.AuthType = authType;

            try
            {
                if (user != string.Empty && pass != string.Empty)
                {
                    string _Uri = this.GLPIurl + "/initSession/";
                    var client = new RestClient(_Uri);
                    // client.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
                    client.Authenticator = new HttpBasicAuthenticator(user, pass);
                    // client.AddDefaultHeader("Auth", "local");
                    client.AddDefaultHeader("Auth", this.AuthType);
                    client.AddDefaultHeader("App-Token", this.AppTocken);
                    var request = new RestRequest("resource", Method.GET);
                    IRestResponse response = client.Execute(request);

                    string resultJson = response.Content.ToString();

                    var details = Json.JsonParser.FromJson(resultJson);
                    this.SessionAdmin = details["session_token"].ToString();
                    Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
                    // textBox1.Text = sessionAdmin;// response.Content.ToString();
                    sessionConnect(this.SessionAdmin);

                    // adminlogin();
                    // getListUsers();
                    // listBox1.Items.Add("Start User");
                    result = "{\"loginGlpi\": \"" + this.SessionAdmin + "\"}";


                    //  MessageBox.Show(er.Message.ToString(), "Ошибка заявки ");

                }else
                {
                    result = "{\"loginGlpi\": \"нет данных для входа\"}";
                }
            }
            catch (Exception er)
            {
                result = "{\"loginGlpi_error\": \"" + er.Message.ToString() + "\"}";
            }
            return result;
        }

        public void closeSession(string getsession)
        {
            string _UriT = this.GLPIurl + "/killSession/";
            clientses = new RestClient(_UriT);
            clientses.AddDefaultHeader("Content-Type", "application/json");
            clientses.AddDefaultHeader("Session-Token", getsession);
            clientses.AddDefaultHeader("App-Token", this.AppTocken);
            var requestses = new RestRequest("resource", Method.GET);
            IRestResponse responseses = clientses.Execute(requestses);
        }

       /* private void adminlogin()
        {

            string _Uri = this.GLPIurl + "/initSession/";

            var client = new RestClient(_Uri);
            //  client.Authenticator = new HttpBasicAuthenticator("glpi", "glpi");
            // client.AddDefaultHeader("Auth", "local");
            client.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            client.AddDefaultHeader("Auth", "RNGI");
            client.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);


            var request = new RestRequest("resource", Method.GET);
            //  client.Execute(request);
            IRestResponse response = client.Execute(request);

            //var httpResponseMessage = request.ToString();
            // var response = new RestResponse();

            textBox1.Text = response.Content.ToString();

            var details = Json.JsonParser.FromJson(textBox1.Text);
            session = details["session_token"].ToString();
            Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
            textBox1.Text = session;// response.Content.ToString();
            sessionConnect(session);
        }*/

       private void sessionConnect(string getsession)
        {
            string _UriT = this.GLPIurl + "/Ticket/";
            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);

            clientses.AddDefaultHeader("Content-Type", "application/json");
            clientses.AddDefaultHeader("Session-Token", getsession);
            clientses.AddDefaultHeader("App-Token", this.AppTocken);
            var requestses = new RestRequest("resource", Method.GET);
            IRestResponse responseses = clientses.Execute(requestses);
            string result = responseses.Content.ToString();
            this.SessionConnectResult = result;
            /* var details = Json.JsonParser.FromJson(textBox1.Text);
             session = details["session_token"].ToString();
             Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
             textBox1.Text = session;// response.Content.ToString();
             sessionConnect(session);*/
        }

        public string getJsonString(string pathurl)
        {
            string _UriT = this.GLPIurl + "/"+ pathurl + "/";
            clientses = new RestClient(_UriT);
            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");
            clientses.AddDefaultHeader("Session-Token", this.SessionAdmin);
            clientses.AddDefaultHeader("App-Token", this.AppTocken);
            var requestses = new RestRequest("resource", Method.GET);
            IRestResponse responseses = clientses.Execute(requestses);
            string result = responseses.Content.ToString();
            return result;
            /* var details = Json.JsonParser.FromJson(textBox1.Text);
             session = details["session_token"].ToString();
             Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
             textBox1.Text = session;// response.Content.ToString();
             sessionConnect(session);*/
        }

        public string parseJSON(string jsonSTR)
        {
            /*   var jsonString = @"{""id"":""15"",""Name"":""West Wind"",
                           ""message"":""text""}";
               */
            dynamic json = JValue.Parse(jsonSTR);

            // values require casting
            string name = json.id;
            string company = json.Name;
            // label5.Text = name;
            return name;
        }

        public string parseJSONArray(string jsonArray)
        {
            string res = "";
            var jsonString = @"{""id"":""15"",""Name"":""West Wind"",
                        ""message"":""text""}";

            JArray jsonVal = JArray.Parse(jsonArray) as JArray;
            dynamic stringsJson = jsonVal;

            foreach (dynamic stringJson in stringsJson)
            {
                string name = stringJson.id;
                string company = stringJson.Name;
              //  label5.Text = name;
                res = name;
            }
            return res;
        }

        public string createTicket(string theme, string message)
        {
            string result = string.Empty;
            string fileaddres = string.Empty;
            var idjson = "";
            //создание заявки
            try
            {
                string _UriT = this.GLPIurl + "/Ticket";
                string JsonStringCreate = "{\"input\": [{\"name\" : \"" + theme + "\",\"content\": \"" + message + "\",\"status\": \"1\",\"type\": \"1\",\"urgency\": \"2\",\"_disablenotif\": \"true\",\"users_id_recipient\":\"101\"}]}";
                //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                // ,\"users_id_recipient\":\"2\"
                clientses = new RestClient(_UriT);
                clientses.AddDefaultHeader("Content-Type", "application/json");
                clientses.AddDefaultHeader("Session-Token", this.SessionAdmin);
                clientses.AddDefaultHeader("App-Token", this.AppTocken);

                IRestRequest request = new RestRequest("", Method.POST, DataFormat.Json);
                request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");

                request.AddJsonBody(JsonStringCreate);

                var response = clientses.Execute(request);
                Console.WriteLine("CreateTicket:");
                Console.WriteLine(response.Content);

                string resultJsonstr = response.Content.ToString();


                // var details = Json.JsonParser.FromJson(label4.Text);
                idjson = parseJSONArray(resultJsonstr);
                //   var idjson = parseJSON(label4.Text);
                //   label5.Text= idjson;
               // label6.Text = idjson.ToString();
                result = "{\"ticket_add\": \""+ idjson.ToString() + "\"}";
            }
            catch (Exception er) {
              //  MessageBox.Show(er.Message.ToString(), "Ошибка заявки ");
                result = "{\"ticket_error\": \"" + er.Message.ToString() + "\"}";
            }

            return result;
        }

            public string addFileToTicket(string ticketID, string pathfile,string namefile)
            {
            //добавление файла в заявку
            string result = string.Empty;
                try
            {
                if (namefile != string.Empty)
                {
                    string _UriTf = this.GLPIurl + "/Document";

                    var RSClient = new RestClient(_UriTf);

                    var requestf = new RestRequest("", Method.POST);
                    requestf.AddHeader("Session-Token", this.SessionAdmin);
                    requestf.AddHeader("App-Token", this.AppTocken);
                    requestf.AddHeader("Accept", "application/json");
                    requestf.AddHeader("Content-Type", "multipart/form-data");

                    requestf.AddQueryParameter("uploadManifest", "{\"input\": {\"name\": \"UploadFileTest_" + ticketID + "\",\"items_id\": \"" + ticketID + "\",\"itemtype\": \"Ticket\", \"_filename\": \"" + namefile + "\"}}");

                    // requestf.AddQueryParameter("uploadManifest", "{\"input\": {\"name\": \"UploadFileTest_"+ idjson.ToString() + "\",\"items_id\": \"" + idjson.ToString() + "\",\"itemtype\": \"Ticket\", \"_filename\": \"TicketScrshot_102320_0823_0.png\"}}");
                    // requestf.AddFile("test_"+ idjson, @"D:\TicketScrshot_102320_0823_0.png");   //            ,"items_id":76,"itemtype":"Ticket","      \"itemtype\": \"Ticket\",
                    requestf.AddFile("Screen_" + ticketID, @pathfile);

                    // MessageBox.Show(pathfile, "добавлениt файла");


                    IRestResponse responsef = RSClient.Execute(requestf);

                    var contentf = responsef.Content;

                   string resultJson = contentf;

                    //  label4.Text = responsef.Content.ToString();


                    // var details = Json.JsonParser.FromJson(label4.Text);
                    var idDOC = parseJSON(resultJson);
                    Console.WriteLine("ID Document:");
                    Console.WriteLine(idDOC);
                   // result += ",\"file_add\": \"" + namefile + "\"";
                    result = "{\"ticket_add\": \"" + ticketID + "\",\"file_add\": \"" + namefile + "\"}";

                }
            }
            catch (Exception er) {
               // MessageBox.Show(er.Message.ToString(), "Ошибка добавления файла");
                result = "{\"file_add_error\": \"" + er.Message.ToString() + "\"}";

            }
            finally
            {
                
                namefile = string.Empty;
                pathfile = string.Empty;

            }
            return result;            

        }




    }
}
