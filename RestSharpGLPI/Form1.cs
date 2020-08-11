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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string user = textBox3.Text;
                string pass = textBox4.Text;
                if (user != string.Empty && pass != string.Empty)
                {
                    string _Uri = Properties.Settings.Default.GLPI_URL + "/initSession/";



                    // Properties.Settings.Default.GLPI_USER = user;
                    //  Properties.Settings.Default.GLPI_PASS = pass;

                    var client = new RestClient(_Uri);
                    // client.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
                    client.Authenticator = new HttpBasicAuthenticator(user, pass);
                    // client.AddDefaultHeader("Auth", "local");
                    client.AddDefaultHeader("Auth", "RNGI");

                    client.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);

                    var request = new RestRequest("resource", Method.GET);
                    //  client.Execute(request);
                    IRestResponse response = client.Execute(request);

                    //var httpResponseMessage = request.ToString();
                    // var response = new RestResponse();

                    /* textBox1.Text = response.Content.ToString();

                     var details = Json.JsonParser.FromJson(textBox1.Text);
                     session = details["session_token"].ToString();
                     Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
                     textBox1.Text = session;// response.Content.ToString();
                     sessionConnect(session);*/

                    textBox1.Text = response.Content.ToString();

                    var details = Json.JsonParser.FromJson(textBox1.Text);
                    sessionAdmin = details["session_token"].ToString();
                    Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
                    textBox1.Text = sessionAdmin;// response.Content.ToString();
                    sessionConnect(sessionAdmin);
                    adminlogin();
                    getListUsers();
                    // listBox1.Items.Add("Start User");
                }
            }catch(Exception er) { MessageBox.Show(er.Message.ToString(),"Error Login"); }
        }

        public void adminlogin()
        {

            string _Uri = Properties.Settings.Default.GLPI_URL + "/initSession/";

            var client = new RestClient(_Uri);
            //  client.Authenticator = new HttpBasicAuthenticator("glpi", "glpi");
            // client.AddDefaultHeader("Auth", "local");
             client.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            client.AddDefaultHeader("Auth", "RNGI");
            client.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);


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

        void sessionConnect(string getsession)
        {
            string _UriT = Properties.Settings.Default.GLPI_URL + "/Ticket/";

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);

            clientses.AddDefaultHeader("Content-Type", "application/json");
            clientses.AddDefaultHeader("Session-Token", getsession);
            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);

            var requestses = new RestRequest("resource", Method.GET);
            //  client.Execute(request);
            IRestResponse responseses = clientses.Execute(requestses);

            

            richTextBox1.Text = responseses.Content.ToString();

           /* var details = Json.JsonParser.FromJson(textBox1.Text);
            session = details["session_token"].ToString();
            Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
            textBox1.Text = session;// response.Content.ToString();
            sessionConnect(session);*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            createTicket(textBox2.Text, richTextBox2.Text, "", sessionAdmin);
            closeSession(sessionAdmin);

        }

        public void closeSession(string getsession) {
            string _UriT = Properties.Settings.Default.GLPI_URL + "/killSession/";

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", getsession);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);

            var requestses = new RestRequest("resource", Method.GET);
            //  client.Execute(request);
            IRestResponse responseses = clientses.Execute(requestses);



            richTextBox1.Text ="";
                //responseses.Content.ToString();

        }
        public void createTicket(string theme, string message, string screenshot, string getsession)
        {
            string result = string.Empty;
            string fileaddres = string.Empty;
            var idjson="";
            //создание заявки
            try { 
           string _UriT = Properties.Settings.Default.GLPI_URL+ "/Ticket";
            string JsonStringCreate = "{\"input\": [{\"name\" : \""+ theme + "\",\"content\": \"" + message + "\",\"status\": \"1\",\"type\": \"1\",\"urgency\": \"2\",\"_disablenotif\": \"true\",\"users_id_recipient\":\"101\"}]}";
                //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                // ,\"users_id_recipient\":\"2\"
                clientses = new RestClient(_UriT);
            clientses.AddDefaultHeader("Content-Type", "application/json");
            clientses.AddDefaultHeader("Session-Token", getsession);
            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);

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
                string _UriTf = Properties.Settings.Default.GLPI_URL + "/Document";

                var RSClient = new RestClient(_UriTf);

                var requestf = new RestRequest("", Method.POST);
                requestf.AddHeader("Session-Token", getsession);
                requestf.AddHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);
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


            /*
              string _UriTd = Properties.Settings.Default.GLPI_URL + "/Document/"+ idDOC.ToString()+ "?expand_drodpowns=true";
            //   string JsonStringCreated = "{\"input\": [{\"name\" : \"" + theme + "\",\"content\": \"" + message + "\",\"status\": \"1\",\"type\": \"1\",\"urgency\": \"2\",\"_disablenotif\": \"true\"}]}";


            clientses = new RestClient(_UriTd);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", getsession);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);

            var requestses = new RestRequest(Method.GET);
            //  client.Execute(request);
            IRestResponse responseses = clientses.Execute(requestses);

              Console.WriteLine("READ Document:");
              Console.WriteLine(responseses.Content);
            richTextBox1.Text = responseses.Content.ToString();

            label6.Text = responseses.Content.ToString();
              */

            // var details = Json.JsonParser.FromJson(label4.Text);
            // var idjsond = parseJSONArray(label4.Text);

            //    label4.Text = response.ErrorMessage.ToString();

        }

        public string getDocumentId(string idDOc)
        {
            string _UriT = Properties.Settings.Default.GLPI_URL ;// + idDOc;

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", sessionAdmin);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            var requestses = new RestRequest("Document/"+idDOc+ "/Document_Item/", Method.GET);
            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);



            richTextBox1.Text = responsesesg.Content.ToString();

            label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            return responsesesg.Content.ToString();
        }

        public string getTicketId(string idDOc)
        {
            string _UriT = Properties.Settings.Default.GLPI_URL;// + idDOc;

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", session);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            //var requestses = new RestRequest("Ticket/" + idDOc + "/Document_Item/", Method.GET);
            var requestses = new RestRequest("Ticket/" + idDOc + "/Ticket_User", Method.GET);

            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);



            richTextBox1.Text = responsesesg.Content.ToString();

            label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            return responsesesg.Content.ToString();
        }

        public void getListUsers()
        {
            List<string> usersGlpi = new List<string>();

            string _UriT = Properties.Settings.Default.GLPI_URL;// + idDOc;

            clientses = new RestClient(_UriT);

            //  clientses.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
            clientses.AddDefaultHeader("Content-Type", "application/json");

            clientses.AddDefaultHeader("Session-Token", session);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);
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
          //  return usersGlpi;
        }

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

        public void updateTicketId(string idTicket,string userid,string type)
        {
           
                string result = string.Empty;
                string fileaddres = string.Empty;
                var idjson = "";
                //создание заявки
                try
                {
                    string _UriT = Properties.Settings.Default.GLPI_URL + "/Ticket/"+ idTicket+ "/Ticket_User/";
                    string JsonStringCreate = "{\"input\": [{\"tickets_id\" : \"" + idTicket + "\",\"users_id\": \"" + userid + "\",\"type\": \""+type+ "\",\"use_notification\": \"1\"}]}";
                    //   ,\"links\":\"[{rel\":\"User\",\"href\":\"http://192.168.16.12:81/apirest.php/User/2 \"  }]
                    // ,\"users_id_recipient\":\"2\"
                    clientses = new RestClient(_UriT);
                    clientses.AddDefaultHeader("Content-Type", "application/json");
                    clientses.AddDefaultHeader("Session-Token", sessionAdmin);
                    clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);

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



           /* richTextBox1.Text = responsesesg.Content.ToString();

            label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            return responsesesg.Content.ToString();*/
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
        private void button4_Click(object sender, EventArgs e)
        {
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
                // values require casting
              /*  string name = json.id;
            string company = json.Name;*/
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
           // getDocumentId("24");
            getTicketId(textBox5.Text);
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
         //   button1.PerformClick();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            updateTicketId(textBox6.Text, textBox7.Text, textBox8.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label13.Text = listBox1.SelectedValue.ToString();
        }
    }
}
