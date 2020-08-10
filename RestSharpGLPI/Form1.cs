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

namespace RestSharpGLPI
{
    public partial class Form1 : Form
    {
       public string session = "";
        public string sessionAdmin = "";

        public RestClient clientses ;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _Uri = Properties.Settings.Default.GLPI_URL + "/initSession/";
         
            var client = new RestClient(_Uri);
            client.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
          //  client.Authenticator = new HttpBasicAuthenticator("glpi", "glpi");
           // client.AddDefaultHeader("Auth", "local");

            client.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);

            var request = new RestRequest("resource", Method.GET);
          //  client.Execute(request);
            IRestResponse response = client.Execute(request);

            //var httpResponseMessage = request.ToString();
           // var response = new RestResponse();

            textBox1.Text = response.Content.ToString() ;

            var details=Json.JsonParser.FromJson(textBox1.Text);
            sessionAdmin = details["session_token"].ToString();
              Console.WriteLine(string.Concat("session_token: ", details["session_token"]));
            textBox1.Text = sessionAdmin;// response.Content.ToString();
           // sessionConnect(sessionAdmin);
        }

        public void adminlogin()
        {

            string _Uri = Properties.Settings.Default.GLPI_URL + "/initSession/";

            var client = new RestClient(_Uri);
           // client.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.GLPI_USER, Properties.Settings.Default.GLPI_PASS);
             client.Authenticator = new HttpBasicAuthenticator("glpi", "glpi");
             client.AddDefaultHeader("Auth", "local");

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
            closeSession(session);
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



            richTextBox1.Text = responseses.Content.ToString();
        }
        public void createTicket(string theme, string message, string screenshot, string getsession)
        {           

           string _UriT = Properties.Settings.Default.GLPI_URL+ "/Ticket";
            string JsonStringCreate = "{\"input\": [{\"name\" : \""+ theme + "\",\"content\": \"" + message + "\",\"status\": \"1\",\"type\": \"1\",\"urgency\": \"2\",\"_disablenotif\": \"true\"}]}";
          
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
            var idjson = parseJSONArray(label4.Text);

         //   label5.Text= idjson;



            //  string idticket = details["id"].ToString();
            /* string idticket1 = idticket[0].ToString();
             string idticket2 = idticket1[0].ToString();*/
            //Console.WriteLine(string.Concat("idticket: ", res.id));
          //  label5.Text = res.id;// response.Content.ToString();

            ///upload file
            ///

            // textBox1.Text = response.Content.ToString();
            /*
             var details = Json.JsonParser.FromJson(label4.Text);
             string idticket = details["id"].ToString();
             Console.WriteLine(string.Concat("idticket: ", details["id"]));
             label4.Text = idticket;// response.Content.ToString();
             */


            
            string _UriTf = Properties.Settings.Default.GLPI_URL + "/Document";

            var RSClient = new RestClient(_UriTf);

            var requestf = new RestRequest("", Method.POST);
            requestf.AddHeader("Session-Token", getsession);
            requestf.AddHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);
            requestf.AddHeader("Accept", "application/json");
            requestf.AddHeader("Content-Type", "multipart/form-data");
            requestf.AddQueryParameter("uploadManifest", "{\"input\": {\"name\": \"UploadFileTest_"+ idjson.ToString() + "\",\"items_id\": \"" + idjson.ToString() + "\",\"itemtype\": \"Ticket\", \"_filename\": \"TicketScrshot_102320_0823_0.png\"}}");
            requestf.AddFile("test_"+ idjson, @"D:\TicketScrshot_102320_0823_0.png");   //            ,"items_id":76,"itemtype":"Ticket","      \"itemtype\": \"Ticket\",

            IRestResponse responsef = RSClient.Execute(requestf);

            var contentf = responsef.Content;
          
            label5.Text = contentf;

          //  label4.Text = responsef.Content.ToString();


            // var details = Json.JsonParser.FromJson(label4.Text);
            var idDOC = parseJSON(label5.Text);
            Console.WriteLine("ID Document:");
            Console.WriteLine(idDOC);
            //label6.Text = idDOC;
           
            
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

            clientses.AddDefaultHeader("Session-Token", session);

            clientses.AddDefaultHeader("App-Token", Properties.Settings.Default.GLPI_APP_TOKEN);
            //http://192.168.16.12:81/apirest.php/Document/24/Document_Item/"}]}
            var requestses = new RestRequest("Document/"+idDOc+ "/Document_Item/", Method.GET);
            //  client.Execute(request);
            IRestResponse responsesesg = clientses.Execute(requestses);



            richTextBox1.Text = responsesesg.Content.ToString();

            label6.Text = responsesesg.ResponseUri.ToString();// Content.ToString();
            return responsesesg.Content.ToString();
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
            getDocumentId("24");
        }
    }
}
