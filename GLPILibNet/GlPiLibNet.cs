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
using System.Collections.Generic;
using GLPILibNet;

namespace GlPiLibNet
{


    public class GlPiLibNet
    {
        public string SaUserName { get; set; }
        public string SaUserPass { get; set; }
        public string SaAuthType { get; set; }
        public string SaSession { get; set; }


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
       // public RestClient clientses;


        public string ErrorLib { get; set; }


        public string SessionConnectResult { get; set; }
        public string SessionConnectResultforButton { get; set; }



        /*
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }  
        public string UserRealName { get; set; }
        public string UserFirstName { get; set; }
        */


        public GlPiLibNet(string appTocken, string glpiUrl)
        {
            /*this.UserName = userName;
            this.UserPass = userPass;
            this.AuthType = authType;*/
            this.AppTocken = appTocken;
            this.GLPIurl = glpiUrl;
            this.SessionAdmin = string.Empty;
            this.SaSession = string.Empty;
            this.SessionConnectResult = "Отключен";
            this.SessionConnectResultforButton = "Войти";
        }


        private string loginGlpiSA()

        {
            Console.WriteLine(string.Concat("loginGlpiSA()"));
              string result = string.Empty;
             /*
              this.SaUserName = "glpi";
              this.SaUserPass = "glpi";
              this.SaAuthType = "local";*/
              
            try
            {
                if (this.SaUserName != string.Empty && this.SaUserPass != string.Empty)
                {
                    string _Uri = this.GLPIurl + "/initSession/";
                    var client = new RestClient(_Uri);
                    // client.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
                    client.Authenticator = new HttpBasicAuthenticator(this.SaUserName, this.SaUserPass);
                    // client.AddDefaultHeader("Auth", "local");
                    client.AddDefaultHeader("Auth", this.SaAuthType);
                    client.AddDefaultHeader("App-Token", this.AppTocken);
                    var request = new RestRequest("resource", Method.GET);
                    IRestResponse response = client.Execute(request);

                    string resultJson = response.Content.ToString();

                    var details = Json.JsonParser.FromJson(resultJson);
                    this.SaSession = details["session_token"].ToString();
                    Console.WriteLine(string.Concat("session_tokenSA: ", details["session_token"]));
                    // textBox1.Text = sessionAdmin;// response.Content.ToString();
                  //  sessionConnect(this.SessionAdmin);

                    // adminlogin();
                    // getListUsers();
                    // listBox1.Items.Add("Start User");
                    result = "{\"loginGlpiSA\": \"" + this.SaSession + "\"}";


                    //  MessageBox.Show(er.Message.ToString(), "Ошибка заявки ");

                }
                else
                {
                   // this.SaSession = string.Empty;
                    result = "{\"loginGlpiSA\": \"нет данных для входа\"}";
                }
            }
            catch (Exception er)
            {
                this.SaSession = string.Empty;
                result = "{\"loginGlpiSA_error\": \"" + er.Message.ToString() + "\"}";
            }
            return result;
        }

        public void authGlpi(string user, string pass, string authType/*, string appTocken*/) {
            this.UserName = user;
            this.UserPass = pass;
            this.AuthType = authType;
        }
        public string loginGlpi()

        {
            loginGlpiSA();
            Console.WriteLine(string.Concat("loginGlpi()"));
            string result = string.Empty;
           /* this.UserName = user;
            this.UserPass = pass;
            this.AuthType = authType;*/
            
            try
            {
                if (this.UserName != string.Empty && this.UserPass != string.Empty)
                {
                    string _Uri = this.GLPIurl + "/initSession/";
                    var client = new RestClient(_Uri);
                    // client.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
                    client.Authenticator = new HttpBasicAuthenticator(this.UserName, this.UserPass);
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
                  //  sessionConnect(this.SessionAdmin);

                    // adminlogin();
                    // getListUsers();
                    // listBox1.Items.Add("Start User");
                    result = "{\"loginGlpi\": \"" + this.SessionAdmin + "\"}";
                 //   // loginGlpiSA();

                    //  MessageBox.Show(er.Message.ToString(), "Ошибка заявки ");
                  
                }
                else
                {
                    this.SessionAdmin = string.Empty;
                    result = "{\"loginGlpi\": \"нет данных для входа\"}";
                }
            }
            catch (Exception er)
            {
                this.SessionAdmin = string.Empty;
                result = "{\"loginGlpi_error\": \"" + er.Message.ToString() + "\"}";
            }
            finally {
                if (this.SessionAdmin != string.Empty) this.SessionConnectResult = "Подключен"; else this.SessionConnectResult = "Отключен";
                if (this.SessionAdmin != string.Empty) this.SessionConnectResultforButton = "Выйти"; else this.SessionConnectResultforButton = "Войти";
                
            }
            return result;
        }

       // public void closeSession(string getsession)
             public void closeSession()
        {
            closeSessionSA();
            Console.WriteLine(string.Concat("closeSession()"));
            string _UriT = this.GLPIurl + "/killSession/";
            var clientses = new RestClient(_UriT);
            clientses.AddDefaultHeader("Content-Type", "application/json");
            clientses.AddDefaultHeader("Session-Token", this.SessionAdmin);// getsession);
            clientses.AddDefaultHeader("App-Token", this.AppTocken);
            var requestses = new RestRequest("resource", Method.GET);
            IRestResponse responseses = clientses.Execute(requestses);
            this.SessionAdmin = string.Empty;
            if (this.SessionAdmin != string.Empty) this.SessionConnectResult = "Подключен"; else this.SessionConnectResult = "Отключен";
            if (this.SessionAdmin != string.Empty) this.SessionConnectResultforButton = "Выйти"; else this.SessionConnectResultforButton = "Войти";
        }

        public void closeSessionSA()
        {
            Console.WriteLine(string.Concat("closeSessionSA()"));
            string _UriT = this.GLPIurl + "/killSession/";
            var clientses = new RestClient(_UriT);
            clientses.AddDefaultHeader("Content-Type", "application/json");
          //  clientses.AddDefaultHeader("Session-Token", this.SaSession);// getsession);
            clientses.AddDefaultHeader("Session-Token", this.SaSession);
            clientses.AddDefaultHeader("App-Token", this.AppTocken);
            var requestses = new RestRequest("resource", Method.GET);
            IRestResponse responseses = clientses.Execute(requestses);
            this.SaSession = string.Empty;
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
            Console.WriteLine(string.Concat("sessionConnect()"));
            string _UriT = this.GLPIurl + "/Ticket/";
            var clientses = new RestClient(_UriT);

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

        public string getTicketId(string idDOc)
        {
            Console.WriteLine(string.Concat("getTicketId()"));
             loginGlpiSA();
            string _UriT = this.GLPIurl;// + idDOc;

            var clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

           // clientses.AddDefaultHeader("Session-Token", this.SaSession);
            clientses.AddDefaultHeader("Session-Token", this.SaSession);

            clientses.AddDefaultHeader("App-Token", this.AppTocken);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            //var requestses = new RestRequest("Ticket/" + idDOc + "/Document_Item/", Method.GET);
            var requestses = new RestRequest("Ticket/" + idDOc + "/Ticket_User", Method.GET);

            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);



            string resultJson = responsesesg.Content.ToString();
            closeSessionSA();
            //label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            return resultJson;
           // 
        }
        public string getJsonString(string pathurl, int idItem, string itemPath)
        {
             loginGlpiSA();
            Console.WriteLine(string.Concat("getJsonString()"));
            // loginGlpi();
            string _UriT = this.GLPIurl;// + idDOc;

            var clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

           // clientses.AddDefaultHeader("Session-Token", this.SaSession);
            clientses.AddDefaultHeader("Session-Token", this.SaSession);


            clientses.AddDefaultHeader("App-Token", this.AppTocken);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            //var requestses = new RestRequest("Ticket/" + idDOc + "/Document_Item/", Method.GET);
            var requestses = new RestRequest(pathurl + "/" + idItem + "/" , Method.GET);
            if (idItem != -1 || idItem != 0)
                requestses = new RestRequest(pathurl + "/" + idItem + "/" + itemPath, Method.GET);
            if (idItem == -1 || idItem == 0)
                requestses = new RestRequest(pathurl + "/", Method.GET);

            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);

            string resultJson = responsesesg.Content.ToString();

            //label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            // closeSession();
            closeSessionSA();
            return resultJson;
            /* var details = Json.JsonParser.FromJson(textBox1.Text);
             session = details["session_token"].ToString();
             Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
             textBox1.Text = session;// response.Content.ToString();
             sessionConnect(session);*/
        }

        public List<ClassUsers> parseJSONUserArray(string jsonArray)
        {
            Console.WriteLine(string.Concat("parseJSONUserArray()"));
            List<string> usersGlpi = new List<string>();
            List<ClassUsers> listToBind = new List<ClassUsers>();

            string res = "";
            var jsonString = @"[{""id"":""15"",""Name"":""West Wind"",
                        ""message"":""text""}]";
            // jsonArray = jsonString;
            JArray jsonVal = JArray.Parse(jsonArray) as JArray;
            dynamic stringsJson = jsonVal;
            try
            {
                foreach (dynamic stringJson in stringsJson)
                {
                    string name = stringJson.id;
                    string company = stringJson.name;
                    string phone = stringJson.phone;
                    //  string email = stringJson.email;
                    string realname = stringJson.realname;
                    string firstname = stringJson.firstname;

                  //  label5.Text = name.ToString();
                    // res = name;
                    //  usersGlpi.Add(name + " : " + company);

                    //this.listBox1.DataSource = usersGlpi;
                    listToBind.Add(new ClassUsers(name, company, phone, realname, firstname));
                }
            }
            catch (Exception er) { 
               // MessageBox.Show(er.Message.ToString(), "parseJSONUserArray");
                listToBind.Add(new ClassUsers("parseJSONUserArray: ",er.Message.ToString(),null,null,null));
            }
            // listBox1.DataSource = usersGlpi;


            return listToBind;
        }
        public List<ClassUsers> getListUsers()
        {
            Console.WriteLine(string.Concat("getListUsers()"));
            loginGlpiSA();
            List<ClassUsers> usersGlpi = new List<ClassUsers>();
            string _UriT = this.GLPIurl;// + idDOc;
            var clientses = new RestClient(_UriT);
            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");
            //clientses.AddDefaultHeader("Session-Token", this.SaSession);
            clientses.AddDefaultHeader("Session-Token", this.SaSession);
            clientses.AddDefaultHeader("App-Token", this.AppTocken);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            //var requestses = new RestRequest("Ticket/" + idDOc + "/Document_Item/", Method.GET);
            var requestses = new RestRequest("User/?is_deleted=0&range=0-500&get_hateoas=0", Method.GET);
            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);
           
            /*
            richTextBox1.Text = responsesesg.Content;
            label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
                                                              // idjson = parseJSONArray(label6.Text);
            listBox1.DisplayMember = "UserName";
            listBox1.ValueMember = "Id"; // optional depending on your needs
            */

            usersGlpi= parseJSONUserArray(responsesesg.Content.ToString());
           closeSessionSA();
              return usersGlpi;
        }
    
        
       

        public string parseJSON(string jsonSTR)
        {
            string name = string.Empty;
            string company = string.Empty;
            try { 
            Console.WriteLine(string.Concat("parseJSON()"));
            /*   var jsonString = @"{""id"":""15"",""Name"":""West Wind"",
                           ""message"":""text""}";
               */
            dynamic json = JValue.Parse(jsonSTR);

            // values require casting
             name = json.id;
             company = json.Name;
            // label5.Text = name;
        }catch(Exception er) { name = "ERROR parseJSON: " + er.Message.ToString()+" ["+ jsonSTR + "]"; }

    Console.WriteLine(string.Concat("parseJSON: ", name));
            return name;
        }

        public string parseJSONArray(string jsonArray)
        {
            Console.WriteLine(string.Concat("parseJSONArray()"));
            string res = "";
            try
            {
                var jsonString = @"{""id"":""15"",""Name"":""West Wind"",
                        ""message"":""text""}";

                JArray jsonVal = JArray.Parse(jsonArray) as JArray;
                dynamic stringsJson = jsonVal;
                Console.WriteLine(string.Concat("parseJSONArray jsonArray: ", jsonArray));

                foreach (dynamic stringJson in stringsJson)
                {
                    Console.WriteLine(string.Concat("parseJSONArray stringJson.id: ", stringJson.id));

                    string name = stringJson.id;
                    string company = stringJson.Name;
                    //  label5.Text = name;
                    res = name;
                }
            }catch(Exception er) { res = "ERROR parseJSONArray: " + er.Message.ToString()+" ["+ jsonArray + "]"; }
            Console.WriteLine(string.Concat("parseJSONArray: ", res));
            return res;
        }

        public string createTicket(string theme, string message, bool resultint_Json)
        {
            Console.WriteLine(string.Concat("createTicket()"));
            loginGlpi();
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

                var clientses = new RestClient(_UriT);
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
                Console.WriteLine(string.Concat("CreateTicket_parseJSONArray: ", resultJsonstr));

                idjson = parseJSONArray(resultJsonstr);
                //   var idjson = parseJSON(label4.Text);
                //   label5.Text= idjson;
                // label6.Text = idjson.ToString();
                if (resultint_Json)
                    result = "{\"ticket_add\": \"" + idjson.ToString() + "\"}";
                else result = idjson;
            }
            catch (Exception er)
            {
                //  MessageBox.Show(er.Message.ToString(), "Ошибка заявки ");
                if (resultint_Json)
                    result = "{\"ticket_error\": \"" + er.Message.ToString() + "\"}";
                else result = "-1";// "ticket_error: " + er.Message.ToString();
            }
            finally { closeSession(); }

            return result;
        }

        public string addFileToTicket(string ticketID, string pathfile, string namefile)
        {
            Console.WriteLine(string.Concat("addFileToTicket()"));
            loginGlpi();
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
            catch (Exception er)
            {
                // MessageBox.Show(er.Message.ToString(), "Ошибка добавления файла");
                result = "{\"file_add_error\": \"" + er.Message.ToString() + "\"}";

            }
            finally
            {

                namefile = string.Empty;
                pathfile = string.Empty;
                 closeSession();
            }
            return result;

        }

        public string updateTicketId(string idTicket, string userid, string type)
        {
            Console.WriteLine(string.Concat("updateTicketId()"));

            string result = string.Empty;
            string fileaddres = string.Empty;
            var idjson = "";
            //создание заявки
            try
             {
            loginGlpiSA();
                string _UriT = this.GLPIurl + "/Ticket/" + idTicket + "/Ticket_User/";
                string JsonStringCreate = "{\"input\": {\"tickets_id\" : \"" + idTicket + "\",\"users_id\": \"" + userid + "\",\"type\": \"" + type + "\",\"use_notification\": \"1\"}}";
                //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                // ,\"users_id_recipient\":\"2\"
                var clientses = new RestClient(_UriT);
                clientses.AddDefaultHeader("Content-Type", "application/json");
                clientses.AddDefaultHeader("Session-Token", this.SaSession);

                clientses.AddDefaultHeader("App-Token", this.AppTocken);

                IRestRequest request = new RestRequest("", Method.POST, DataFormat.Json);
                request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");

                request.AddJsonBody(JsonStringCreate);
                Console.WriteLine(string.Concat("updateTicketJSON: ", JsonStringCreate));

                var response = clientses.Execute(request);
                Console.WriteLine("updateTicket:");
                Console.WriteLine(response.Content);

                string resultJson = response.Content.ToString();

                Console.WriteLine(string.Concat("updateTicket_parse: ", resultJson));

                // var details = Json.JsonParser.FromJson(label4.Text);
                //   idjson = parseJSONArray(resultJson);
              //  idjson = parseJSON(resultJson);

                //   var idjson = parseJSON(label4.Text);
                //   label5.Text= idjson;
                //  label6.Text = idjson.ToString();
             //   result = "{\"ticket_Update\": \"" + idjson + "\"}";
            result = "{\"ticket_Update\": \"ok\"}";

              }
              catch (Exception er)
              {
                  result = "{\"ticket_Update_Error\": \"" + er.Message.ToString() + "\"}";
                  //   MessageBox.Show(er.Message.ToString(), "Ошибка заявки "); }
                  // label6.Text = result;
              }
              finally 
            {
                 closeSessionSA(); 
            }
            return result;

            /* richTextBox1.Text = responsesesg.Content.ToString();

             label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
             return responsesesg.Content.ToString();*/
        }


        // getJsonString(string pathurl, int idItem, string itemPath)
        public string searchTicketId(string field, string name, string pathsearch)
    {
            Console.WriteLine(string.Concat("searchTicketId()"));

             loginGlpiSA();
            // string _UriT = this.GLPIurl;// + idDOc;
            string _UriT = this.GLPIurl;// + "/search/" + pathsearch + "?is_deleted=0&criteria[0]["+ field+"]=" + name;// +"/ " + idTicket + "/Ticket_User/";

        var clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

           // clientses.AddDefaultHeader("Session-Token", this.SaSession);
            clientses.AddDefaultHeader("Session-Token", this.SaSession);

            clientses.AddDefaultHeader("App-Token", this.AppTocken);
          //  clientses.AddDefaultHeader("Session-Token", this.SessionAdmin);

            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            //var requestses = new RestRequest("Ticket/" + idDOc + "/Document_Item/", Method.GET);
            var requestses = new RestRequest("" + pathsearch  + "?searchText[name]="+ name+"", Method.GET);

            // richTextBox1.Text = myGlpiLib.searchTicketId("name", "UsersLogin", "KnowbaseItemCategory");
            // "search/KnowbaseItemCategory?searchText[name]=UsersLogin"

            /*if (idItem != null || idItem != 0)
                requestses = new RestRequest(pathurl + "/" + idItem + "/" + itemPath, Method.GET);
            if (idItem == null || idItem == 0)
                requestses = new RestRequest(pathurl + "/", Method.GET);*/

            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);

            string resultJson = responsesesg.Content.ToString();

            //label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            // return resultJson;
            Console.WriteLine(string.Concat("searchTicketId_parse: ", resultJson));
            
           //  string idsearch=parseJSON(resultJson);
            string idsearch = parseJSONArray(resultJson);

            /* var details = Json.JsonParser.FromJson(resultJson);
             string idsearch = details["id"].ToString();*/
            Console.WriteLine(string.Concat("idsearch: ", idsearch));

            closeSessionSA(); 

            return idsearch;
        }


        public string addUserNaznachknowbase(string idTicket, string userId)
        {
            Console.WriteLine(string.Concat("addUserNaznachItemId()"));
           loginGlpiSA();
            string result = string.Empty;
            string fileaddres = string.Empty;
            string _UriT = string.Empty;
            var idjson = "";
            //создание заявки
            try
            {
              
                _UriT = this.GLPIurl + "/knowbaseitem/" + idTicket + "/knowbaseitem_User";

                string JsonStringCreate = "{\"input\":[{\"knowbaseitems_id\":\"" + idTicket + "\",\"users_id\":\"" + userId + "\"}]}";
             //   string JsonStringCreate = @"{input:[{knowbaseitem_id : " + idTicket + ",users_id: " + userId + "}]}";

                //  string JsonStringCreate = "{ \"input\": {\"item_id\" : \"" + idTicket + "\",\"" + fieldedit + "\": \"" + value + "\" }}";
                // string JsonStringCreate = "{\"input\": [{\"knowbaseitem_id\" : \"" + idTicket + "\",\""+fieldedit+"\": \"" + value + "\" }]}";

                ///     string JsonStringCreate = "{\"input\": [{\"knowbaseitem_id\" : \"" + idTicket + "\",\"users_id\": \""+ userId + "\"}]}";
                //string JsonStringCreate = "{ \"input\": {\"" + "knowbaseitem_id" + "\" : \"" + idTicket + "\",\"" + "users_id" + "\": \"" + userId + "\" }}";


                var clientses = new RestClient(_UriT);
                clientses.AddDefaultHeader("Content-Type", "application/json");

                //clientses.AddDefaultHeader("Session-Token", this.SaSession);
                  clientses.AddDefaultHeader("Session-Token", this.SaSession);

                clientses.AddDefaultHeader("App-Token", this.AppTocken);


                 IRestRequest request = new RestRequest("", Method.POST, DataFormat.Json);
              //  IRestRequest request = new RestRequest("knowbaseitem/" + idTicket + "/knowbaseitem_User", Method.POST, DataFormat.Json);

                request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");

                request.AddJsonBody(JsonStringCreate);
                  Console.WriteLine("AddJsonBody:");
                 Console.WriteLine(request.Resource);

                var response = clientses.Execute(request);
                Console.WriteLine("addUserNaznachItemId:");
                Console.WriteLine(response.Content.ToString());
                string resultJson = response.Content.ToString();
                // var details = Json.JsonParser.FromJson(label4.Text);
                Console.WriteLine(string.Concat("addUserNaznachItemId_parse: ", resultJson));
                idjson = parseJSONArray(resultJson);
                //  idjson = parseJSON(resultJson);           
                result = "{\"addUserNaznachItemId_Update\": \"" + idjson + "\"}" + _UriT;

            }
            catch (Exception er)
            {
                Console.WriteLine(string.Concat("addUserNaznachItemId_Error: ", er.Message.ToString()));

                result = "{\"addUserNaznachItemId_Error\": \"" + er.Message.ToString() + "\"}" + _UriT;
             
            }
            finally {
                closeSessionSA(); 
            }
            return result;

          
        }

        public string addUserNaznachItemId(string Path, string idTicket, string userId)
        {
            Console.WriteLine(string.Concat("addUserNaznachItemId()"));
             loginGlpiSA();
            string result = string.Empty;
            string fileaddres = string.Empty;
            string _UriT = string.Empty;
            var idjson = "";
            //создание заявки
            try
            {
                /*
                string _UriT0 = this.GLPIurl;
                string _UriT = string.Empty;
                if (itemPath == "" || itemPath == null || itemPath == string.Empty) _UriT = "/" + Path +"/" + idTicket + "/";
                
                if(itemPath!="" || itemPath!=null || itemPath!=string.Empty) _UriT += _UriT = "/" + Path + "/" + idTicket + "/" + itemPath + "/";
                
              //  string JsonStringCreate = "{\"input\": [{\"id\" : \"" + idTicket + "\",\"" + fieldedit+"\" : \"" + value + "\"}]}";
                string JsonStringCreate = "{\"input\": [{\"tickets_id\" : \"" + idTicket + "\",\"" + fieldedit + "\" : \"" + value + "\",\"type\": \"3\"}]}";
                */

                _UriT = this.GLPIurl;// + "/knowbaseitem/" + idTicket + "/knowbaseitem_User/";

              //  string JsonStringCreate = "{\"input\": {\"tickets_id\" : \"" + idTicket + "\",\"users_id\": \"" + userid + "\",\"type\": \"" + type + "\",\"use_notification\": \"1\"}}";

              //  _UriT = this.GLPIurl + "/" + Path + "/" + idTicket +"/"+""+ Path + "_User";            
                  string JsonStringCreate = "{\"input\":{\"id\" : \"\",\"knowbaseitems_id\" : \"" + idTicket + "\",\"users_id\": \"" + userId + "\"}}";
               //  string JsonStringCreate = "{ \"input\": [{ \"id\" : \""+ idTicket + "\" , \"\"" + fieldedit + "\" : \"" + value + "\"\" } ] }";
                 //
                //  string JsonStringCreate = "{ \"input\": {\""+ idname + "\" : \"" + idTicket + "\",\"name\" : \"" + "test" + "\",\"knowbaseitems_id\":\"" + idTicket + "\",\""+fieldedit+"\": \"" + value + "\",\"users_id\":2,\"type\": \"3\" }}";
              //  string JsonStringCreate = "{ \"input\": {\"item_id\" : \"" + idTicket + "\",\"" + fieldedit + "\": \"" + value + "\" }}";
              // string JsonStringCreate = "{\"input\": [{\"knowbaseitem_id\" : \"" + idTicket + "\",\""+fieldedit+"\": \"" + value + "\" }]}";
             
             ///     string JsonStringCreate = "{\"input\": [{\"knowbaseitem_id\" : \"" + idTicket + "\",\"users_id\": \""+ userId + "\"}]}";
                //string JsonStringCreate = "{ \"input\": {\"" + "knowbaseitem_id" + "\" : \"" + idTicket + "\",\"" + "users_id" + "\": \"" + userId + "\" }}";


                //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                // ,\"users_id_recipient\":\"2\"
                var clientses = new RestClient(_UriT);
                clientses.AddDefaultHeader("Content-Type", "application/json");
             
                 //clientses.AddDefaultHeader("Session-Token", this.SaSession);
                clientses.AddDefaultHeader("Session-Token", this.SaSession);

                clientses.AddDefaultHeader("App-Token", this.AppTocken);


                // IRestRequest request = new RestRequest("", Method.PUT, DataFormat.Json);
                IRestRequest request = new RestRequest("knowbaseitem/" + idTicket + "/knowbaseitem_User", Method.POST, DataFormat.Json);

                request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");

                request.AddJsonBody(JsonStringCreate);
              //  Console.WriteLine("AddJsonBody:");
               // Console.WriteLine(request.Resource);

                    var response = clientses.Execute(request);
                    Console.WriteLine("addUserNaznachItemId:");
                    Console.WriteLine(response.Content.ToString());
                    string resultJson = response.Content.ToString();
                // var details = Json.JsonParser.FromJson(label4.Text);
                Console.WriteLine(string.Concat("addUserNaznachItemId_parse: ", resultJson));
                idjson = parseJSONArray(resultJson);
              //  idjson = parseJSON(resultJson);
                    //   label5.Text= idjson;
                    //  label6.Text = idjson.ToString();
                    result = "{\"addUserNaznachItemId_Update\": \"" + idjson + "\"}"+ _UriT;
               
            }
            catch (Exception er)
            {
                Console.WriteLine(string.Concat("addUserNaznachItemId_Error: ", er.Message.ToString()));

                result = "{\"addUserNaznachItemId_Error\": \"" + er.Message.ToString() + "\"}" + _UriT;
                //   MessageBox.Show(er.Message.ToString(), "Ошибка заявки "); }
                // label6.Text = result;
            }
            finally {
                 closeSessionSA(); 
            }
            return result;

            /* richTextBox1.Text = responsesesg.Content.ToString();

             label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
             return responsesesg.Content.ToString();*/
        }

        public string updateItemId(string Path, string idname, string idTicket, string itemPath, string fieldedit, string value,bool addorupdate)
        {
            Console.WriteLine(string.Concat("updateItemId()"));
             loginGlpiSA();
            string result = string.Empty;
            string fileaddres = string.Empty;
            string _UriT = string.Empty;
            var idjson = "";
            string JsonStringCreate = string.Empty;
            //создание заявки
            try
            {
                /*
                string _UriT0 = this.GLPIurl;
                string _UriT = string.Empty;
                if (itemPath == "" || itemPath == null || itemPath == string.Empty) _UriT = "/" + Path +"/" + idTicket + "/";
                
                if(itemPath!="" || itemPath!=null || itemPath!=string.Empty) _UriT += _UriT = "/" + Path + "/" + idTicket + "/" + itemPath + "/";
                
              //  string JsonStringCreate = "{\"input\": [{\"id\" : \"" + idTicket + "\",\"" + fieldedit+"\" : \"" + value + "\"}]}";
                string JsonStringCreate = "{\"input\": [{\"tickets_id\" : \"" + idTicket + "\",\"" + fieldedit + "\" : \"" + value + "\",\"type\": \"3\"}]}";
                */
                _UriT  = this.GLPIurl + "/KnowbaseItem/" + idTicket + "/KnowbaseItem_User";

                //  string JsonStringCreate = "{\"input\": {\"tickets_id\" : \"" + idTicket + "\",\"users_id\": \"" + userid + "\",\"type\": \"" + type + "\",\"use_notification\": \"1\"}}";

                //  _UriT = this.GLPIurl + "/" + Path + "/" + idTicket +"/"+""+ Path + "_User";            
                //   string JsonStringCreate = "{\"input\": {\"knowbaseitem_id\" : \"" + idTicket + "\",\"users_id\": \"" + value + "\",\"type\": \"2\",\"use_notification\": \"1\"}}";

                //  _UriT = this.GLPIurl + "/" + Path + "/" + idTicket + "/" + itemPath;
                //  string JsonStringCreate = "{\"input\": [{\"tickets_id\" : \"" + idTicket + "\",\"\": \"" + value + "\",\"type\": \"2\",\"use_notification\": \"1\"}]}";
                //string JsonStringCreate = string.Empty;// "{ \"input\": [{ \"id\" : \""+ idTicket + "\" , \"\"" + fieldedit + "\" : \"" + value + "\"\" } ] }";

                //  IRestRequest request = new RestRequest("", Method.PUT, DataFormat.Json);
                if (addorupdate)
                {
                   //  JsonStringCreate = "{\"input\":{\"knowbaseitem_id\":\"" + idTicket + "\",\"\"" + fieldedit + "\":\"" + value + "\"\"}}";
                     JsonStringCreate = "{\"input\":[{\"knowbaseitems_id\":\"" + idTicket + "\",\"users_id\":\"" + value + "\"}]}";
                   // request = new RestRequest("", Method.POST, DataFormat.Json);

                }
                else
                {
                    // JsonStringCreate = "{\"input\":{\"id\":\"" + idTicket + "\",\"\"" + fieldedit + "\":\"" + value + "\"\"}}";
                     JsonStringCreate = "{\"input\":{\"knowbaseitems_id\":\"" + idTicket + "\",\"users_id\":\"" + value + "\"}}";
                 //   request = new RestRequest("", Method.PUT, DataFormat.Json);
                }
                //
                // string JsonStringCreate = "{ \"input\": {\""+ idname + "\" : \"" + idTicket + "\",\"name\" : \"" + "test" + "\",\"knowbaseitems_id\":\"" + idTicket + "\",\""+fieldedit+"\": \"" + value + "\",\"users_id\":2,\"type\": \"3\" }}";               
                //  string JsonStringCreate = "{ \"input\": {\"" + idname + "\" : \"" + idTicket + "\",\"" + fieldedit + "\": \"" + value + "\" }}";

                /*  if (addorupdate)
                  {
                      JsonStringCreate = "{ \"input\": {\"" + idname + "\" : \"" + idTicket + "\",\"" + fieldedit + "\": \"" + value + "\" }}";
                  }
                  else
                  {
                      JsonStringCreate = "{ \"input\": [{\"" + idname + "\" : \"" + idTicket + "\",\"" + fieldedit + "\": \"" + value + "\" }]}";

                  }*/
                // string JsonStringCreate = "{\"input\": [{\"knowbaseitem_id\" : \"" + idTicket + "\",\""+fieldedit+"\": \"" + value + "\" }]}";
                // string JsonStringCreate = "{\"input\": [{\"knowbaseitem_id\" : \"" + idTicket + "\",\"" + fieldedit + "\": \"" + value + "\",\"use_notification\": \"1\"}]}";

                //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                // ,\"users_id_recipient\":\"2\"
                var clientses = new RestClient(_UriT);
                clientses.AddDefaultHeader("Content-Type", "application/json");
              //  clientses.AddDefaultHeader("Session-Token", this.SaSession);
                clientses.AddDefaultHeader("Session-Token", this.SaSession);

                clientses.AddDefaultHeader("App-Token", this.AppTocken);


                IRestRequest request;
                request = new RestRequest("", Method.POST, DataFormat.Json);


                request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");

                request.AddJsonBody(JsonStringCreate);
                Console.WriteLine("AddJsonBody:");
                Console.WriteLine(request.Resource.ToString());

                var response = clientses.Execute(request);
                Console.WriteLine("updateItemId:");
                Console.WriteLine(response.Content);

                string resultJson = response.Content.ToString();


              
                var details = Json.JsonParser.FromJson(resultJson);
              
             //   idjson = parseJSON(resultJson);
                idjson = parseJSONArray(resultJson);

                //   label5.Text= idjson;
                //  label6.Text = idjson.ToString();
                result = idjson;// "{\"updateItemId_Update\": \"" +  "\"}" + _UriT + "  JSON{ " + JsonStringCreate + "} " + addorupdate.ToString();

            }
            catch (Exception er)
            {
                result = "{\"updateItemId_Error\": \"" + er.Message.ToString() + "\"} " + addorupdate.ToString() + "   " + _UriT;
                //   MessageBox.Show(er.Message.ToString(), "Ошибка заявки "); }
                // label6.Text = result;
            }
            finally {
                 closeSessionSA(); 
            }
            return result;

            /* richTextBox1.Text = responsesesg.Content.ToString();

             label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
             return responsesesg.Content.ToString();*/
        }

        public string addTicketPC( string idpc, string idTicket)
        {
            Console.WriteLine(string.Concat("addTicketPC()"));
             loginGlpiSA();
            string result = string.Empty;
            string fileaddres = string.Empty;
            string _UriT = string.Empty;
            var idjson = "";
            //создание заявки
            try
            {
                /*
                string _UriT0 = this.GLPIurl;
                string _UriT = string.Empty;
                if (itemPath == "" || itemPath == null || itemPath == string.Empty) _UriT = "/" + Path +"/" + idTicket + "/";
                
                if(itemPath!="" || itemPath!=null || itemPath!=string.Empty) _UriT += _UriT = "/" + Path + "/" + idTicket + "/" + itemPath + "/";
                
              //  string JsonStringCreate = "{\"input\": [{\"id\" : \"" + idTicket + "\",\"" + fieldedit+"\" : \"" + value + "\"}]}";
                string JsonStringCreate = "{\"input\": [{\"tickets_id\" : \"" + idTicket + "\",\"" + fieldedit + "\" : \"" + value + "\",\"type\": \"3\"}]}";
                */

                _UriT = this.GLPIurl + "/" + "Computer" + "/" + idpc + "/" + "item_ticket";
                //  string JsonStringCreate = "{\"input\": [{\"tickets_id\" : \"" + idTicket + "\",\"\": \"" + value + "\",\"type\": \"2\",\"use_notification\": \"1\"}]}";
                // string JsonStringCreate = "{ \"input\": [{ \"id\" : \""+ idTicket + "\" , \"\"" + fieldedit + "\" : \"" + value + "\"\" } ] }";
                //
                // string JsonStringCreate = "{ \"input\": {\""+ idname + "\" : \"" + idTicket + "\",\"name\" : \"" + "test" + "\",\"knowbaseitems_id\":\"" + idTicket + "\",\""+fieldedit+"\": \"" + value + "\",\"users_id\":2,\"type\": \"3\" }}";               
            
                //  string JsonStringCreate = "{ \"input\": {\"" + idname + "\" : \"" + idTicket + "\",\"" + fieldedit + "\": \"" + value + "\" }}";
                // string JsonStringCreate = "{\"input\": [{\"itemtype\":\"Computer\",\"items_id"\:\""+idpc+"\",\"tickets_id\":\""+ idTicket+"\","links":[{"rel":"Computer","href":"http://192.168.16.12:81/apirest.php/Computer/6"},{"rel":"Ticket","href":"http://192.168.16.12:81/apirest.php/Ticket/275"}]}]}"
                string JsonStringCreate = "{\"input\": [{\"itemtype\":\"Computer\",\"items_id\":\"" + idpc + "\",\"tickets_id\":\"" + idTicket + "\"}]}";

                // string JsonStringCreate = "{\"input\": [{\"knowbaseitem_id\" : \"" + idTicket + "\",\""+fieldedit+"\": \"" + value + "\" }]}";
                // string JsonStringCreate = "{\"input\": [{\"knowbaseitem_id\" : \"" + idTicket + "\",\"" + fieldedit + "\": \"" + value + "\",\"use_notification\": \"1\"}]}";

                //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                // ,\"users_id_recipient\":\"2\"
                var clientses = new RestClient(_UriT);
                clientses.AddDefaultHeader("Content-Type", "application/json");
               // clientses.AddDefaultHeader("Session-Token", this.SaSession);
                clientses.AddDefaultHeader("Session-Token", this.SaSession);
                clientses.AddDefaultHeader("App-Token", this.AppTocken);


                IRestRequest request;
                //  IRestRequest request = new RestRequest("", Method.PUT, DataFormat.Json);
               /* if (addorupdate)
                {*/
                    request = new RestRequest("", Method.POST, DataFormat.Json);
               /* }
                else
                {
                    request = new RestRequest("", Method.PUT, DataFormat.Json);
                }
                */
                request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");

                request.AddJsonBody(JsonStringCreate);
                Console.WriteLine("AddJsonBody:");
                Console.WriteLine(request.Resource);

                var response = clientses.Execute(request);
                Console.WriteLine("updateItemId:");
                Console.WriteLine(response.Content);

                string resultJson = response.Content.ToString();


                // var details = Json.JsonParser.FromJson(label4.Text);
               // idjson = parseJSON(resultJson);
                idjson = parseJSONArray(resultJson);

                //  var idjson = parseJSON(label4.Text);
                //   label5.Text= idjson;
                //  label6.Text = idjson.ToString();
                result = "{\"addTicketPC_Update\": \"" + idjson + "\"}" + _UriT + "  JSON{ " + JsonStringCreate + "} ";

            }
            catch (Exception er)
            {
                result = "{\"addTicketPC_Error\": \"" + er.Message.ToString() + "\"} " + "   " + _UriT;
                //   MessageBox.Show(er.Message.ToString(), "Ошибка заявки "); }
                // label6.Text = result;
            }
            finally {
                Console.WriteLine(string.Concat("Computer add ticket: ",  " Name: " + Environment.MachineName+" ticket add result: "+ result));
                closeSessionSA(); 
            }
            return result;

            /* richTextBox1.Text = responsesesg.Content.ToString();

             label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
             return responsesesg.Content.ToString();*/
        }

        public string createknowbaseitem(string theme, string user, string pass, string iduser, int viewid, bool resultint_Json)
        {
            Console.WriteLine(string.Concat("createknowbaseitem()"));
             loginGlpiSA();
            string result = string.Empty;
            string fileaddres = string.Empty;
            var idjson = "";
            //создание заявки
            try
            {
                string _UriT = this.GLPIurl + "/KnowbaseItem";
               // string JsonStringCreate = "{\"input\": [{\"name\" : \"" + user + "\",\"answer\": \"" + pass + "\",\"is_faq\": \"0\",\"view\": \"1\",\"urgency\": \"2\",\"_disablenotif\": \"true\",\"users_id_recipient\":\"101\"}]}";
                 string JsonStringCreate = "{\"input\": [{\"name\" : \"" + user + "\",\"answer\": \"" + pass + "\",\"knowbaseitemcategories_id\": \""+theme+ "\",\"users_id\": \"" + iduser + "\", \"is_faq\": \"0\",\"view\": \""+ viewid.ToString()+"\" }]}";
                //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                // ,\"users_id_recipient\":\"2\"

                var clientses = new RestClient(_UriT);
                
                clientses.AddDefaultHeader("Content-Type", "application/json");
               // clientses.AddDefaultHeader("Session-Token", this.SaSession);
                clientses.AddDefaultHeader("Session-Token", this.SaSession);
                clientses.AddDefaultHeader("App-Token", this.AppTocken);

                IRestRequest request = new RestRequest("", Method.POST, DataFormat.Json);
                request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");

                request.AddJsonBody(JsonStringCreate);

                var response = clientses.Execute(request);
                Console.WriteLine("KnowbaseItem:");
                Console.WriteLine(response.Content);

                string resultJsonstr = response.Content.ToString();

                // var details = Json.JsonParser.FromJson(label4.Text);
                Console.WriteLine(string.Concat("createknowbaseitem_parse: ", resultJsonstr));
                idjson = parseJSONArray(resultJsonstr);
                //   var idjson = parseJSON(label4.Text);
                //   label5.Text= idjson;
                // label6.Text = idjson.ToString();
                Console.WriteLine(string.Concat("KnowbaseItem_add: ", idjson.ToString()));

                if (resultint_Json)
                    result = "{\"KnowbaseItem_add\": \"" + idjson.ToString() + "\"}";
                else result = idjson;
            }
            catch (Exception er)
            {
                Console.WriteLine(string.Concat("KnowbaseItem_add: ", er.Message.ToString()));
                //  MessageBox.Show(er.Message.ToString(), "Ошибка заявки ");
                if (resultint_Json)
                    result = "{\"KnowbaseItem_error\": \"" + er.Message.ToString() + "\"}";
                else result = "KnowbaseItem_error: " + er.Message.ToString();
            }
            finally {
                 closeSessionSA();
            }

            return result;
        }




    }
}
