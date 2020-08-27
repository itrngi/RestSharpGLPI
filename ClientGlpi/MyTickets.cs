using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using RestSharp;

namespace ClientGlpi
{
    public partial class MyTickets : Form
    {
        public string userMain = string.Empty;
        public string passMain = string.Empty;
        public string authMain = string.Empty;


        public bool closeForm = false;
        public string pathfile = string.Empty;
        public string pathfile2 = string.Empty;

        public string namefiles1 = string.Empty;
        public string namefiles2 = string.Empty;
        public string namefilesother = string.Empty;

        public string fpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString();

        // public string fpath = Application.StartupPath.ToString();

        public string session = "";
        public string sessionAdmin = "";

        public RestClient clientses;

        public GlPiLibNet.GlPiLibNet myGlpiLib = new GlPiLibNet.GlPiLibNet(Properties.Settings.Default.GLPI_APP_TOKEN0, Properties.Settings.Default.GLPI_URL0);
        IniFile settingFile = new IniFile("Settings.ini");
        CoderDecoder coderDecoder = new CoderDecoder();


        public int selectIdTicket = 0;
        //public string session = "";

        public MyTickets()
        {
            InitializeComponent();
        }

        private void MyTickets_Load(object sender, EventArgs e)
        {
            string user = userMain;
            string pass = passMain;
            string auth = authMain;

            /*

        string userini = settingFile.ReadINI("Auth", "User");
        string passini = settingFile.ReadINI("Auth", "Pass");
        string authini = settingFile.ReadINI("Auth", "Type");

        string decodeuser = coderDecoder.DeShifrovka(userini, "SeGlPi@RnGiLoCaL");
        string decodepass = coderDecoder.DeShifrovka(passini, "SeGlPi@RnGiLoCaL");
        string decodeauth = coderDecoder.DeShifrovka(authini, "SeGlPi@RnGiLoCaL");

            */
            myGlpiLib.UserName = user;
            myGlpiLib.UserPass = pass;
            myGlpiLib.AuthType = auth.ToLower();

            // richTextBox2.Text = "user: " + myGlpiLib.UserName + ";Pass:" + myGlpiLib.UserPass + ";Auth:" + myGlpiLib.AuthType + ";";


            string useraini = settingFile.ReadINI("AuthA", "User");
            string passaini = settingFile.ReadINI("AuthA", "Pass");
            string authaini = settingFile.ReadINI("AuthA", "Type");

            string decodeusera = coderDecoder.DeShifrovka(useraini, "SeGlPi@RnGiLoCaL");
            string decodepassa = coderDecoder.DeShifrovka(passaini, "SeGlPi@RnGiLoCaL");
            string decodeautha = coderDecoder.DeShifrovka(authaini, "SeGlPi@RnGiLoCaL");

            myGlpiLib.SaUserName = decodeusera;
            myGlpiLib.SaUserPass = decodepassa;
            myGlpiLib.SaAuthType = decodeautha;


            string codeuser = coderDecoder.Shifrovka(user, "SeGlPi@RnGiLoCaL");
            string codepass = coderDecoder.Shifrovka(pass, "SeGlPi@RnGiLoCaL");
            string codeauth = coderDecoder.Shifrovka(auth, "SeGlPi@RnGiLoCaL");

            settingFile.Write("Auth", "User", codeuser);
            settingFile.Write("Auth", "Pass", codepass);
            settingFile.Write("Auth", "Type", codeauth);



            string codeusera = coderDecoder.Shifrovka("glpi", "SeGlPi@RnGiLoCaL");
            string codepassa = coderDecoder.Shifrovka("glpi", "SeGlPi@RnGiLoCaL");
            string codeautha = coderDecoder.Shifrovka("local", "SeGlPi@RnGiLoCaL");

            settingFile.Write("AuthA", "User", codeusera);
            settingFile.Write("AuthA", "Pass", codepassa);
            settingFile.Write("AuthA", "Type", codeautha);


            if (myGlpiLib.SessionAdmin == string.Empty)
            // if(myGlpiLib.SessionConnectResult=="Отключен")
            {

                myGlpiLib.authGlpi(user, pass, auth);
                myGlpiLib.loginGlpi();

                //  addUserLogin(user, pass);
                //  richTextBox1.Text = myGlpiLib.getJsonString(textBox5.Text, Convert.ToInt32(numericUpDown1.Value), textBox9.Text);
                string jsonResult = myGlpiLib.getTicketsJsonString("Ticket", 0, null);
               // richTextBox1.Text = jsonResult;
                // myGlpiLib.getListTickets(jsonResult);

                listBox1.DisplayMember = "ThemeTicket";
                listBox1.ValueMember = "Id"; // optional depending on your needs
                

                listBox1.DataSource = myGlpiLib.getListTickets(jsonResult);


                // myGlpiLib.
            }
            else
           if (myGlpiLib.SessionAdmin != string.Empty)
            {

                myGlpiLib.closeSession();
                //  myGlpiLib.SessionConnectResult
            }
        }

        private void listBox1_Enter(object sender, EventArgs e)
        {
           // label7.Text = listBox1.SelectedItem.ToString();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label7.Text = listBox1.SelectedValue.ToString();
            selectIdTicket = Convert.ToInt32(listBox1.SelectedValue);

            string jst = myGlpiLib.getJsonString("Ticket", Convert.ToInt32(listBox1.SelectedValue), null);
            richTextBox3.Text = myGlpiLib.getJsonString("Ticket", Convert.ToInt32(listBox1.SelectedValue), "ITILFollowup");
            label2.Text = myGlpiLib.parseJSONParam(jst, "name");
            richTextBox1.Text = myGlpiLib.parseJSONParam(jst, "content");
            switch (Convert.ToInt32(myGlpiLib.parseJSONParam(jst, "status"))) {
                case 1:
                   label5.Text ="Новая";
                break;
                case 2:
                    label5.Text = "В работе (Назначена)";
                    break;
                case 3:
                    label5.Text = "В работе (Запланирована)";
                    break;
                case 4:
                    label5.Text = "Ожидающая решения";
                    break;
                case 5:
                    label5.Text = "Решена";
                    break;
                case 6:
                    label5.Text = "Закрыта";
                    break;

                default:
                    label5.Text = myGlpiLib.parseJSONParam(jst, "status");
                    break;

            }
            string jstComent = myGlpiLib.getJsonString("Ticket", Convert.ToInt32(listBox1.SelectedValue), "ITILFollowup");
            string jstComentr = myGlpiLib.parseJSONArrayParam(jstComent, "content");
           // richTextBox1.Text = jstComent;

            listBox1.DisplayMember = "content";
            listBox1.ValueMember = "Id"; // optional depending on your needs
            listBox2.DataSource = myGlpiLib.parseJSONArrayParamList(jstComent, "content");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userid = myGlpiLib.searchTicketId("name", userMain, "User");
            // string userid = myGlpiLib.searchTicketId("name", "glpi", "User");

            // richTextBox2.Text = userid;
            //  richTextBox2.Text += selectIdTicket.ToString();
            myGlpiLib.addCommentTicketId(selectIdTicket.ToString(), userid, richTextBox2.Text);
            //  MessageBox.Show("ticket id:"+ selectIdTicket.ToString()+"; UserID: "+userid+"; Coment: "+richTextBox2.Text, "Comment add");


            string jstComent = myGlpiLib.getJsonString("Ticket", Convert.ToInt32(listBox1.SelectedValue), "ITILFollowup");
            string jstComentr = myGlpiLib.parseJSONArrayParam(jstComent, "content");
            // richTextBox1.Text = jstComent;

            listBox1.DisplayMember = "content";
            listBox1.ValueMember = "Id"; // optional depending on your needs
            listBox2.DataSource = myGlpiLib.parseJSONArrayParamList(jstComent, "content");

            try
            {
                if (pathfile2 != string.Empty)
                {

                    // label5.Text =
                    myGlpiLib.addFileToTicket(selectIdTicket.ToString(), pathfile2, namefiles2);

                    // myGlpiLib.addFileToTicket(idNewticket, pathfile2, namefiles2);
                }
            }
            catch (Exception er) { MessageBox.Show(er.Message.ToString(), "pathfile2"); }

            try
            {
                if (pathfile != string.Empty)
                {

                    // label5.Text =
                    myGlpiLib.addFileToTicket(selectIdTicket.ToString(), pathfile, namefiles1);

                    // myGlpiLib.addFileToTicket(idNewticket, pathfile2, namefiles2);
                }
            }
            catch (Exception er) { MessageBox.Show(er.Message.ToString(), "pathfile"); }


            if (pathfile2 != string.Empty)
            {
                File.Delete(pathfile2);
                pictureBox1.Image = null;

            }

            if (pathfile != string.Empty) 
            { 
            pathfile = string.Empty;
            label9.Text = "нет файлов";
            }

            richTextBox2.Clear();

        }


        private Bitmap CaptureScreenShot() // делаем скриншот экрана 
        {
            // MessageBox.Show(Screen.AllScreens[screenNum].WorkingArea.ToString(), Screen.AllScreens[screenNum].DeviceName+" [ "+ screenNum + "]"+ Screen.AllScreens.Count()+" | "+ Screen.AllScreens[screenNum].Bounds.Location);

            //  Bitmap bitmap1=new Bitmap(10,10);
            //  try
            //   {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            // if (screenNum >= 0)

            // Rectangle bounds = Screen.AllScreens[screenNum].Bounds;
            // if(screenNum==0)  
            // Rectangle bounds = Screen.GetBounds(Screen.AllScreens[screenNum].Bounds.Location);
            // if (screenNum > 0) 
            //  Rectangle bounds = Screen.GetBounds(new Point(Screen.AllScreens[screenNum].Bounds.X, Screen.AllScreens[screenNum].Bounds.Y));

            //  bitmap = new Bitmap(Screen.AllScreens[screenNum].Bounds.Width, Screen.AllScreens[screenNum].Bounds.Height);

            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            // Create a bitmap of the appropriate size to receive the screenshot.
            Bitmap bitmap = new Bitmap(screenWidth, screenHeight);
            try
            {

                // Draw the screenshot into our bitmap.
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap.Size);

                }

            }
            catch (Exception er) { MessageBox.Show(er.Message.ToString(), "CaptureScreenShot"); }
            // Do something with the Bitmap here, like save it to a file:
            //  bitmap.Save(savePath, ImageFormat.Jpeg);


            //    Bitmap bitmap = new Bitmap(Screen.AllScreens[screenNum].Bounds.Width, Screen.AllScreens[screenNum].Bounds.Height);
            //   bitmap = new Bitmap(bounds.Width, bounds.Height);

            /* using (Graphics gr = Graphics.FromImage(bitmap1)) 
             { gr.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size); }*/

            /*   }catch(Exception er)
               {
                   MessageBox.Show(er.Message.ToString(), "Error Screen Shot");
               }*/

            return bitmap;
        }

        public string GenerateRandomJpegName(string pref)
        {
            DateTime date1 = new DateTime();
            date1 = DateTime.Now;
            string dateName = date1.ToString("dd_MM_yyyy_HH_mm_ss");
            return "Screen_" + pref + "" + dateName + ".jpg";
        }
        public void DoScreenShotbmpMain()
        {
            //Bitmap bmpPrimary = new Bitmap(10, 10); //new Bitmap(Screen.AllScreens[1].Bounds.Width, Screen.AllScreens[1].Bounds.Height);


            try
            {
                int screenWidth = SystemInformation.VirtualScreen.Width;
                int screenHeight = SystemInformation.VirtualScreen.Height;

                // Create a bitmap of the appropriate size to receive the screenshot.
                Bitmap bmpPrimary = new Bitmap(screenWidth, screenHeight);
                bmpPrimary = CaptureScreenShot();
                pictureBox1.Image = bmpPrimary;
                //  MessageBox.Show(this.Controls.Find("pictureBoxScreen", true).ToString());
                // if () MessageBox.Show("picture");
                /*  PictureBox pictureScreen = new PictureBox
                  {
                      Name = "pictureBox1",
                      Size = new Size(60, 30),
                      Location = new Point(200, 290),
                      Image = bmpPrimary,//Image.FromFile("hello.jpg"),
                      SizeMode= PictureBoxSizeMode.StretchImage,

                  };
                  this.Controls.Add(pictureScreen);*/
                /*
                Graphics g = Graphics.FromImage(bmpPrimary);
                g.CopyFromScreen(0,0, Screen.AllScreens[1].Bounds.X, Screen.AllScreens[1].Bounds.Y, Screen.AllScreens[1].Bounds.Size, CopyPixelOperation.SourceCopy);
              */


                namefiles2 = GenerateRandomJpegName("Main_");
                string filePath = @"" + fpath + "\\" + namefiles2;
                pathfile2 = @"" + fpath + "\\" + namefiles2; // filePath;// openFileDialog1.FileName;
               // label7.Text = pathfile2;
                //   MessageBox.Show(pathfile2,"ScreenPath");
                bmpPrimary.Save(pathfile2, ImageFormat.Jpeg);
            }
            catch (Exception er) { MessageBox.Show(er.Message.ToString(), "DoScreenShotbmpMain"); }


            // openFile(fpath, namefile);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DoScreenShotbmpMain();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "С:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    namefiles1 = openFileDialog.SafeFileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            pathfile = filePath;
            label9.Text = pathfile;

        }
    }
}
