using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Drawing.Imaging;
using GlPiLibNet;
using System.Security.Principal;
using System.DirectoryServices;
using INISettings;
using CoderGLPI;

namespace RestSharpGLPI
{
    public partial class Form1 : Form
    {
        public string pathfile = string.Empty;
        public string namefile = string.Empty;
        public string fpath = Application.StartupPath.ToString();

        public string session = "";
        public string sessionAdmin = "";

        public RestClient clientses ;

        public GlPiLibNet.GlPiLibNet myGlpiLib = new GlPiLibNet.GlPiLibNet(Properties.Settings.Default.GLPI_APP_TOKEN0, Properties.Settings.Default.GLPI_URL0);
        IniFile settingFile = new IniFile("Settings.ini");
        CoderDecoder coderDecoder = new CoderDecoder();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox3.Text;
            string pass = textBox4.Text;
            string auth = textBox13.Text;


            
            string userini= settingFile.ReadINI("Auth", "User");
            string passini = settingFile.ReadINI("Auth", "Pass");
            string authini = settingFile.ReadINI("Auth", "Type");

            string decodeuser = coderDecoder.DeShifrovka(userini, "SeGlPi@RnGiLoCaL");
            string decodepass= coderDecoder.DeShifrovka(passini, "SeGlPi@RnGiLoCaL");
            string decodeauth = coderDecoder.DeShifrovka(authini, "SeGlPi@RnGiLoCaL");
            

            string useraini = settingFile.ReadINI("AuthA", "User");
            string passaini = settingFile.ReadINI("AuthA", "Pass");
            string authaini = settingFile.ReadINI("AuthA", "Type");

            string decodeusera = coderDecoder.DeShifrovka(useraini, "SeGlPi@RnGiLoCaL");
            string decodepassa = coderDecoder.DeShifrovka(passaini, "SeGlPi@RnGiLoCaL");
            string decodeautha = coderDecoder.DeShifrovka(authaini, "SeGlPi@RnGiLoCaL");

            

            /* string decodepass = coderDecoder.DeShifrovka(codepass, "SeGlPi@RnGiLoCaL");
              this.Text = "Pass: " + pass + " code: " + codepass + " decode: " + decodepass;
              */



            myGlpiLib.UserName =  user;
            myGlpiLib.UserPass =  pass;
            myGlpiLib.AuthType =  auth;
            
            myGlpiLib.SaUserName = decodeusera;
            myGlpiLib.SaUserPass = decodepassa;
            myGlpiLib.SaAuthType = decodeautha;
            
            /* if (myGlpiLib.SessionAdmin == string.Empty)
             {

                 myGlpiLib.loginGlpi();
             }
             else { 
                 myGlpiLib.closeSession(); 

             }*/

            if (myGlpiLib.SessionAdmin != string.Empty)
            {
                myGlpiLib.closeSession();
                button1.Text = "LogOut";
              //  getListUsers();
            }
            else {
                myGlpiLib.loginGlpi();
              

                getListUsers();
                button1.Text = "LogIn";
                string users_idNumber = myGlpiLib.searchTicketId("name", myGlpiLib.SaUserName, "User");
                textBox12.Text = users_idNumber;
                Console.WriteLine(string.Concat("users_idNumber: ", users_idNumber));
            }

            string codeuser = coderDecoder.Shifrovka(user, "SeGlPi@RnGiLoCaL");
            string codepass = coderDecoder.Shifrovka(pass, "SeGlPi@RnGiLoCaL");
            string codeauth = coderDecoder.Shifrovka(auth, "SeGlPi@RnGiLoCaL");
            /* string decodepass = coderDecoder.DeShifrovka(codepass, "SeGlPi@RnGiLoCaL");
              this.Text = "Pass: " + pass + " code: " + codepass + " decode: " + decodepass;
              */
            settingFile.Write("Auth", "User", codeuser);
            settingFile.Write("Auth", "Pass", codepass);
            settingFile.Write("Auth", "Type", codeauth);

            string codeusera = coderDecoder.Shifrovka("glpi", "SeGlPi@RnGiLoCaL");
            string codepassa = coderDecoder.Shifrovka("glpi", "SeGlPi@RnGiLoCaL");
            string codeautha = coderDecoder.Shifrovka("local", "SeGlPi@RnGiLoCaL");
            /* string decodepass = coderDecoder.DeShifrovka(codepass, "SeGlPi@RnGiLoCaL");
              this.Text = "Pass: " + pass + " code: " + codepass + " decode: " + decodepass;
              */
            settingFile.Write("AuthA", "User", codeusera);
            settingFile.Write("AuthA", "Pass", codepassa);
            settingFile.Write("AuthA", "Type", codeautha);
        }


        
        /*
        public void adminlogin()
        {

            string _Uri = Properties.Settings.Default.GLPI_URL0 + "/initSession/";

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
        }
        */
        /*
        void sessionConnect(string getsession)
        {
            string _UriT = Properties.Settings.Default.GLPI_URL0 + "/Ticket/";

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);

            clientses.AddDefaultHeader("Content-Type", "application/json");
            clientses.AddDefaultHeader("Session-Token", getsession);
            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);

            var requestses = new RestRequest("resource", Method.GET);
            //  client.Execute(request);
            IRestResponse responseses = clientses.Execute(requestses);

            

            richTextBox1.Text = responseses.Content.ToString();

          
        }
        */
        private void search()
        {

         //   richTextBox1.Text = searchTicketId(string idTicket, "UsersLogin", "KnowbaseItemCategory");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreateTicket();
        }



        private void CreateTicket()
        {
            
            string idNewticket =  myGlpiLib.createTicket(textBox2.Text, richTextBox2.Text, false);
            label4.Text = idNewticket;
            if (pathfile != string.Empty)
              label5.Text=  myGlpiLib.addFileToTicket(idNewticket, pathfile, namefile);            
           
            label6.Text =  myGlpiLib.updateTicketId(idNewticket,"101","2");                     

            try
            {
                string idpcname = string.Empty;
                idpcname = myGlpiLib.searchTicketId("name", Environment.MachineName, "Computer");
                Console.WriteLine(string.Concat("Computer: ", idpcname + " Name: " + Environment.MachineName));

                if (idpcname != string.Empty) myGlpiLib.addTicketPC(idpcname, idNewticket);
                else
                    Console.WriteLine(string.Concat("Computer: ", " Name: " + Environment.MachineName + " не найден в GLPI"));
            }catch(Exception er) { Console.WriteLine(string.Concat("Computer: ", " Name: " + Environment.MachineName + " Error: "+er.Message.ToString())); }

            addUserLogin(textBox3.Text, textBox4.Text);


            // string idNewticket3 = myGlpiLib.updateItemId("KnowbaseItem", idNewticket2, "KnowbaseItemCategory", idcategory);
            // richTextBox1.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItem + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";
            // label4.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItem + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";



        }
/*
        public void closeSession(string getsession) {
            string _UriT = Properties.Settings.Default.GLPI_URL0 + "/killSession/";

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", getsession);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);

            var requestses = new RestRequest("resource", Method.GET);
            //  client.Execute(request);
            IRestResponse responseses = clientses.Execute(requestses);



            richTextBox1.Text ="";
                //responseses.Content.ToString();

        }
        */
        /*
        public void createTicket(string theme, string message, string screenshot, string getsession)
        {
            string result = string.Empty;
            string fileaddres = string.Empty;
            var idjson="";
            //создание заявки
            try { 
           string _UriT = Properties.Settings.Default.GLPI_URL0+ "/Ticket";
            string JsonStringCreate = "{\"input\": [{\"name\" : \""+ theme + "\",\"content\": \"" + message + "\",\"status\": \"1\",\"type\": \"1\",\"urgency\": \"2\",\"_disablenotif\": \"true\",\"users_id_recipient\":\"101\"}]}";
                //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                // ,\"users_id_recipient\":\"2\"
                clientses = new RestClient(_UriT);
            clientses.AddDefaultHeader("Content-Type", "application/json");
            clientses.AddDefaultHeader("Session-Token", getsession);
            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);

            IRestRequest request = new RestRequest("", Method.POST, DataFormat.Json);
            request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");           

            request.AddJsonBody(JsonStringCreate);

            var response = clientses.Execute(request);
            Console.WriteLine("CreateTicket:");
            Console.WriteLine(response.Content);  

            label4.Text = response.Content.ToString();


            // var details = Json.JsonParser.FromJson(label4.Text);
            idjson = parseJSONArray(label4.Text);
            //   var idjson = parseJSON(label4.Text);
            //   label5.Text= idjson;
            label6.Text = idjson.ToString();
                result = "{\"ticket_add\": \"OK\"";
            }
            catch(Exception er) { MessageBox.Show(er.Message.ToString(),"Ошибка заявки "); }


            //добавление файла в заявку

            try
            {
                if(namefile!=string.Empty)
                { 
                string _UriTf = Properties.Settings.Default.GLPI_URL0 + "/Document";

                var RSClient = new RestClient(_UriTf);

                var requestf = new RestRequest("", Method.POST);
                requestf.AddHeader("Session-Token", getsession);
                requestf.AddHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);
                requestf.AddHeader("Accept", "application/json");
                requestf.AddHeader("Content-Type", "multipart/form-data");

                requestf.AddQueryParameter("uploadManifest", "{\"input\": {\"name\": \"UploadFileTest_" + idjson.ToString() + "\",\"items_id\": \"" + idjson.ToString() + "\",\"itemtype\": \"Ticket\", \"_filename\": \"" + namefile + "\"}}");

                // requestf.AddQueryParameter("uploadManifest", "{\"input\": {\"name\": \"UploadFileTest_"+ idjson.ToString() + "\",\"items_id\": \"" + idjson.ToString() + "\",\"itemtype\": \"Ticket\", \"_filename\": \"TicketScrshot_102320_0823_0.png\"}}");
                // requestf.AddFile("test_"+ idjson, @"D:\TicketScrshot_102320_0823_0.png");   //            ,"items_id":76,"itemtype":"Ticket","      \"itemtype\": \"Ticket\",
                requestf.AddFile("Screen_" + idjson.ToString(), @pathfile);

                // MessageBox.Show(pathfile, "добавлениt файла");


                IRestResponse responsef = RSClient.Execute(requestf);

                var contentf = responsef.Content;

                label5.Text = contentf;

                //  label4.Text = responsef.Content.ToString();


                // var details = Json.JsonParser.FromJson(label4.Text);
                var idDOC = parseJSON(label5.Text);
                Console.WriteLine("ID Document:");
                Console.WriteLine(idDOC);
                result += ",\"file_add\": \"" + namefile + "\"";
            }
            }
            catch(Exception er) { MessageBox.Show(er.Message.ToString(),"Ошибка добавления файла"); }
            finally { result += "}";
                namefile = string.Empty;
                pathfile = string.Empty;
                
            }
            label6.Text = result;         

        }
        */
/*
        public string getDocumentId(string idDOc)
        {
            string _UriT = Properties.Settings.Default.GLPI_URL0 ;// + idDOc;

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", sessionAdmin);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            var requestses = new RestRequest("Document/"+idDOc+ "/Document_Item/", Method.GET);
            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);



            richTextBox1.Text = responsesesg.Content.ToString();

            label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            return responsesesg.Content.ToString();
        }
        */
/*
        public string getTicketId(string idDOc)
        {
            string _UriT = Properties.Settings.Default.GLPI_URL0;// + idDOc;

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", session);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            //var requestses = new RestRequest("Ticket/" + idDOc + "/Document_Item/", Method.GET);
            var requestses = new RestRequest("Ticket/" + idDOc + "/Ticket_User", Method.GET);

            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);



            richTextBox1.Text = responsesesg.Content.ToString();

            label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            return responsesesg.Content.ToString();
        }
        */
        public void getListUsers()
        {
            listBox1.DisplayMember = "UserName";
            listBox1.ValueMember = "Id"; // optional depending on your needs

            listBox1.DataSource = myGlpiLib.getListUsers();

            /*
            List<string> usersGlpi = new List<string>();

            string _UriT = Properties.Settings.Default.GLPI_URL0;// + idDOc;

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", session);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            //var requestses = new RestRequest("Ticket/" + idDOc + "/Document_Item/", Method.GET);
            var requestses = new RestRequest("User/?is_deleted=0&range=0-500&get_hateoas=0", Method.GET);

            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);



            richTextBox1.Text = responsesesg.Content;

            label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
                                                              // idjson = parseJSONArray(label6.Text);


            listBox1.DisplayMember =  "UserName";
            listBox1.ValueMember = "Id"; // optional depending on your needs
           
            listBox1.DataSource = parseJSONUserArray(responsesesg.Content.ToString());
            */
            //  return usersGlpi;
        }
        /*
        public List<ClassUsers> parseJSONUserArray(string jsonArray)
        {
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
                    int name = stringJson.id;
                    string company = stringJson.name;
                    string phone = stringJson.phone;
                  //  string email = stringJson.email;
                    string realname = stringJson.realname;
                    string firstname = stringJson.firstname;

                    label5.Text = name.ToString();
                   // res = name;
                  //  usersGlpi.Add(name + " : " + company);

                    //this.listBox1.DataSource = usersGlpi;
                    listToBind.Add(new ClassUsers(name, company, phone, realname, firstname));
                }
            }
            catch(Exception er) { MessageBox.Show(er.Message.ToString(), "parseJSONUserArray"); }
            // listBox1.DataSource = usersGlpi;


            return listToBind;
        }
        */
        /*
        public void updateTicketId(string idTicket,string userid,string type)
        {
           
                string result = string.Empty;
                string fileaddres = string.Empty;
                var idjson = "";
                //создание заявки
                try
                {
                    string _UriT = Properties.Settings.Default.GLPI_URL0 + "/Ticket/"+ idTicket+ "/Ticket_User/";
                    string JsonStringCreate = "{\"input\": [{\"tickets_id\" : \"" + idTicket + "\",\"users_id\": \"" + userid + "\",\"type\": \""+type+ "\",\"use_notification\": \"1\"}]}";
                    //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                    // ,\"users_id_recipient\":\"2\"
                    clientses = new RestClient(_UriT);
                    clientses.AddDefaultHeader("Content-Type", "application/json");
                    clientses.AddDefaultHeader("Session-Token", sessionAdmin);
                    clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN0);

                    IRestRequest request = new RestRequest("", Method.POST, DataFormat.Json);
                    request.AddHeader("Content-Type", "application/json; CHARSET=UTF-8");

                    request.AddJsonBody(JsonStringCreate);

                    var response = clientses.Execute(request);
                    Console.WriteLine("CreateTicket:");
                    Console.WriteLine(response.Content);

                    label4.Text = response.Content.ToString();


                    // var details = Json.JsonParser.FromJson(label4.Text);
                    idjson = parseJSONArray(label4.Text);
                    //   var idjson = parseJSON(label4.Text);
                    //   label5.Text= idjson;
                    label6.Text = idjson.ToString();
                    result = "{\"ticket_Update\": \"OK\"}";
                }
                catch (Exception er) { MessageBox.Show(er.Message.ToString(), "Ошибка заявки "); }
            label6.Text = result;

        }
        */
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


        static string GenerateRandomString()
        {
            bool UseSigns = true;
            bool UseUpperLetters = true;
            bool UseLetters = true;
            int Length;
            NewLabel:
            try
            {
                Length = new Random(DateTime.Now.Millisecond - DateTime.Now.Second + new Random(DateTime.Now.Millisecond).Next(0, 100) / new Random(DateTime.Now.Millisecond - DateTime.Now.Second).Next(0, 10)).Next(0, 100);
            }
            catch { goto NewLabel; }
            string result = "";
            try
            {
                int Seed = 0;
                char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
                char[] signs = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                List<char> keyWords = new List<char>();
                List<char> upperLetters = new List<char>();
                foreach (char c in letters)
                    upperLetters.Add(Convert.ToChar(c.ToString().ToUpper()));
                if (UseLetters)
                    foreach (char c in letters)
                        keyWords.Add(c);
                if (UseSigns)
                    foreach (char c in signs)
                        keyWords.Add(c);
                if (UseUpperLetters)
                    foreach (char c in upperLetters)
                        keyWords.Add(c);
                int MaxValue = keyWords.Count;
                for (int i = 0; i <= Length; i++)
                {
                    try
                    {
                        Random mainrand = new Random(Seed);
                        char RandChar = keyWords[mainrand.Next(0, MaxValue)];
                        result += RandChar;
                        Seed += DateTime.Now.Millisecond + Seed - new Random().Next(10) + new Random(DateTime.Now.Millisecond + 800 * 989 / 3).Next(10);
                    }
                    catch { continue; }
                }
            }
            catch { }
            return result;
        }
        public string GenerateRandomJpegName()
        {
            DateTime date1 = new DateTime();
            date1 = DateTime.Now;
          string dateName= date1.ToString("dd_MM_yyyy_HH_mm_ss");
            return "Screen_"+dateName + ".jpg";
          //  return "Screen_" + ".jpg";

            //  return GenerateRandomString() + ".jpg";
        }
        public void DoScreenShot()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            namefile = GenerateRandomJpegName();

            
               
            string filePath = @"" + fpath + "\\" + namefile;
           
          /*  var fileContent = string.Empty;
            var filePath1 = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Application.StartupPath.ToString();
                openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FileName = filePath;
              //  if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath1 = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            pathfile = filePath1;
            */


           
            //openFileDialog1.FileName = filePath;

            pathfile =  fpath + @"\" + namefile; // filePath;// openFileDialog1.FileName;
            label7.Text = pathfile;
            bmp.Save(pathfile, ImageFormat.Jpeg);
           // openFile(fpath, namefile);
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
                label5.Text = name;
                res= name;
            }
            return res;
        }

        public  void DisplayUser(IIdentity id)
        {
            WindowsIdentity winId = id as WindowsIdentity;
            if (id == null)
            {
                Console.WriteLine("Identity is not a windows identity");
                return;
            }

            string userInQuestion = winId.Name.Split('\\')[1];
            string myDomain = winId.Name.Split('\\')[0]; // this is the domain that the user is in
                                                         // the account that this program runs in should be authenticated in there                    
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + myDomain);
            DirectorySearcher adSearcher = new DirectorySearcher(entry);

            adSearcher.SearchScope = SearchScope.Subtree;
            adSearcher.Filter = "(&(objectClass=user)(samaccountname=" + userInQuestion + "))";
            SearchResult userObject = adSearcher.FindOne();
            if (userObject != null)
            {
                string[] props = new string[] { "title", "mail","pwdlastset","name" };
                foreach (string prop in props)
                {
                    Console.WriteLine("{0} : {1}", prop, userObject.Properties[prop][0]);
                  //  if (prop == "pwdlastset") textBox4.Text = userObject.Properties[prop][0].ToString();
                        if (prop== "name") textBox3.Text = userObject.Properties[prop][0].ToString();
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string idpcname = string.Empty;
                idpcname = myGlpiLib.searchTicketId("name", Environment.MachineName, "Computer");
            Console.WriteLine(string.Concat("Computer: ", idpcname + " Name: " + Environment.MachineName));

          if(idpcname != string.Empty) myGlpiLib.addTicketPC(idpcname, textBox12.Text);
          else
                Console.WriteLine(string.Concat("Computer: ",  " Name: " + Environment.MachineName+" не найден в GLPI"));


            /*
            var jsonString = @"[{""id"":""15"",""Name"":""West Wind"",
                        ""message"":""text""}]";

            // dynamic json = JValue.Parse(jsonString);

            JArray jsonVal = JArray.Parse(jsonString) as JArray;
            dynamic stringsJson = jsonVal;

            foreach (dynamic stringJson in stringsJson)
            {
                string name = stringJson.id;
                string company = stringJson.Name;
                label5.Text = name;
            }
            */
            // values require casting
            /*  string name = json.id;
          string company = json.Name;*/

        }

        private void button5_Click(object sender, EventArgs e)
        {
           // getDocumentId("24");
          //  getTicketId(textBox5.Text);
            richTextBox1.Text = myGlpiLib.getJsonString(textBox5.Text, Convert.ToInt32(numericUpDown1.Value), textBox9.Text);
          //  richTextBox1.Text = myGlpiLib.getJsonString("Ticket", 0, null);

        }

        void openFile(string dir, string file)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = dir;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    openFileDialog.FileName = file;
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            pathfile = filePath;
            label7.Text = pathfile;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "D:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            pathfile = filePath;
            label7.Text = pathfile;// (fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DoScreenShot();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayUser(WindowsIdentity.GetCurrent());


        



            string userini = settingFile.ReadINI("Auth", "User");
            string passini = settingFile.ReadINI("Auth", "Pass");
            string authini = settingFile.ReadINI("Auth", "Type");
            
            string decodeuser = coderDecoder.DeShifrovka(userini, "SeGlPi@RnGiLoCaL");
            string decodepass = coderDecoder.DeShifrovka(passini, "SeGlPi@RnGiLoCaL");
            string decodeauth = coderDecoder.DeShifrovka(authini, "SeGlPi@RnGiLoCaL");

            textBox3.Text = decodeuser;
            textBox4.Text = decodepass;
            textBox13.Text = decodeauth;
            /*
            textBox3.Text = userini;
            textBox4.Text = passini;
            textBox13.Text = authini;
            */
            
          string useraini = settingFile.ReadINI("AuthA", "User");
          string passaini = settingFile.ReadINI("AuthA", "Pass");
          string authaini = settingFile.ReadINI("AuthA", "Type");

          string decodeusera = coderDecoder.DeShifrovka(useraini, "SeGlPi@RnGiLoCaL");
          string decodepassa = coderDecoder.DeShifrovka(passaini, "SeGlPi@RnGiLoCaL");
          string decodeautha = coderDecoder.DeShifrovka(authaini, "SeGlPi@RnGiLoCaL");


            myGlpiLib.SaUserName = decodeusera;
            myGlpiLib.SaUserPass = decodepassa;
            myGlpiLib.SaAuthType = decodeautha;
            //   button1.PerformClick();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
          //  richTextBox1.Text = myGlpiLib.updateTicketId(textBox6.Text, textBox7.Text, textBox8.Text);
           // string idNewticket3 = myGlpiLib.updateItemId("KnowbaseItem", idNewticket2, "KnowbaseItemCategory", idcategory);
            string idNewticket3 = myGlpiLib.updateItemId(textBox11.Text, textBox15.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox12.Text,true);
            richTextBox1.Text = idNewticket3;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label13.Text = listBox1.SelectedValue.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.Text =myGlpiLib.searchTicketId( "name", textBox10.Text, textBox14.Text);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
               // break;
            }
            else
            {
                CreateTicket();
                // Perform a time consuming operation and report progress.
                System.Threading.Thread.Sleep(500);
              //  worker.ReportProgress(i * 10);
            }
           
          
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // string idNewticket3 = myGlpiLib.addUserNaznachItemId(textBox11.Text, textBox6.Text, textBox12.Text);
            string idNewticket3 = myGlpiLib.addUserNaznachknowbase(textBox6.Text, textBox12.Text);
             richTextBox1.Text = idNewticket3;

          /*  string idNewticket3 = myGlpiLib.updateItemId(textBox11.Text, textBox15.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox12.Text, true);
            richTextBox1.Text = idNewticket3;
            */
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (myGlpiLib.SessionAdmin != string.Empty)
            {

                button1.Text = "LogOut";
               // getListUsers();
            }
            else { button1.Text = "LogIn"; }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string user = textBox3.Text;         

            myGlpiLib.UserName = user;
          
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string pass = textBox4.Text;

            myGlpiLib.UserPass = pass;
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
          
            string auth = textBox13.Text;

            myGlpiLib.AuthType = auth;
        }



        private void button10_Click(object sender, EventArgs e)
        {
            addUserLogin(textBox3.Text, textBox4.Text);
        }

        private void addUserLogin(string user,string pass)
        {
            string idcategory = string.Empty;
            try
            {
                idcategory = myGlpiLib.searchTicketId("name", "UsersLogin", "KnowbaseItemCategory");//1
                Console.WriteLine(string.Concat("idcategory: ", idcategory));
                int idItem = Convert.ToInt32(idcategory);

            }
            catch (Exception er)
            {
                idcategory = string.Empty;
                Console.WriteLine(string.Concat("idcategory: ", er.Message.ToString())); ;
            }

            string idKnowbaseItemBD = string.Empty;
            try
            {

                idKnowbaseItemBD = myGlpiLib.searchTicketId("name", user, "KnowbaseItem"); //поиск имеющейся записи в базе знаний
                Console.WriteLine(string.Concat("idKnowbaseItemBD: ", idKnowbaseItemBD));
               
                int idItem = Convert.ToInt32(idKnowbaseItemBD);
                Console.WriteLine(string.Concat("idKnowbaseItemBD idItem: ", idItem));

            }
            catch (Exception er)
            {
                idKnowbaseItemBD = string.Empty;
                Console.WriteLine(string.Concat("idKnowbaseItemBDError: ", er.Message.ToString())); ;
            }//1

            string idKnowbaseItemNew = string.Empty;
            if (idKnowbaseItemBD == string.Empty)
                //если записи в базе знаний нет, создаём новую
                idKnowbaseItemNew = myGlpiLib.createknowbaseitem(idcategory, user, pass, idKnowbaseItemBD, 5, false);
            else idKnowbaseItemNew = string.Empty;//если запись в базе знаний есть, не создаём новую
            Console.WriteLine(string.Concat("idKnowbaseItemNew: ", idKnowbaseItemNew));


            string idKnowbaseItemUser = string.Empty;
            if (idKnowbaseItemNew != string.Empty)//если запись в базе знаний создана новая, назначаем ей пользователя
            {

                //string users_idNumber = myGlpiLib.searchTicketId("name", "user", "Path");
                Console.WriteLine(string.Concat("idKnowbaseItemNew != string.Empty: ", idKnowbaseItemNew));

                string users_idNumber = myGlpiLib.searchTicketId("name", myGlpiLib.SaUserName, "User");
                Console.WriteLine(string.Concat("users_idNumber: ", users_idNumber));
                //string idNewticket3 = myGlpiLib.updateItemId("PathName", "PathName_id", PathName_idNumber, "PathName_User", "users_id", users_idNumber, true);
                idKnowbaseItemUser = myGlpiLib.updateItemId("knowbaseitem", "knowbaseitems_id", idKnowbaseItemNew, "knowbaseitem_User", "users_id", users_idNumber, false);
                Console.WriteLine(string.Concat("idKnowbaseItemUser: ", idKnowbaseItemUser));
                int idItem = Convert.ToInt32(idKnowbaseItemUser);
                //  string profile_idNumber = myGlpiLib.searchTicketId("name", "admin", "profile");
                //  profile_idNumber = myGlpiLib.updateItemId("knowbaseitem", "knowbaseitems_id", idKnowbaseItemNew, "knowbaseitem_profile", "profiles_id", profile_idNumber, true);


            }
            else idKnowbaseItemUser = string.Empty;
            Console.WriteLine(string.Concat("idKnowbaseItem: ", idKnowbaseItemBD));

           


            // string idNewticket3 = myGlpiLib.updateItemId("KnowbaseItem", idNewticket2, "KnowbaseItemCategory", idcategory);
            richTextBox1.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItemBD + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";
            label4.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItemBD + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";

        }

        private void button11_Click(object sender, EventArgs e)
        {
            string users_idNumber = myGlpiLib.searchTicketId("name", myGlpiLib.SaUserName, "User");
            Console.WriteLine(string.Concat("users_idNumber: ", users_idNumber));
            string idNewticket3 = myGlpiLib.updateItemId(textBox11.Text, textBox15.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox12.Text, false);
            richTextBox1.Text = idNewticket3;
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }
    }
}
