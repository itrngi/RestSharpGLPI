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
    public partial class Form1 : Form
    {
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

        public Form1()
        {
            InitializeComponent();
              this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.FormClosing += Form1_FormClosing;
            this.ShowInTaskbar = false;
            notifyIcon1.Click += notifyIcon1_Click;
        }

        void notifyIcon1_Click(object sender, EventArgs e)
        {
          //  this.WindowState = FormWindowState.Normal;
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
           // label7.Text = pathfile;
        }

        private void button2_Click(object sender, EventArgs e)
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

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            pathfile = filePath;
        }

        public string GenerateRandomJpegName(string pref)
        {
            DateTime date1 = new DateTime();
            date1 = DateTime.Now;
            string dateName = date1.ToString("dd_MM_yyyy_HH_mm_ss");
            return "Screen_"+ pref+"" + dateName + ".jpg";            
        }

        public void DoScreenShot()
        {
            DoScreenShotbmpMain();
           
        }

        private void setFormLocation(Form form, Screen screen)
        {
            // first method
            Rectangle bounds = screen.Bounds;
            form.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            // second method
            //Point location = screen.Bounds.Location;
            //Size size = screen.Bounds.Size;

            //form.Left = location.X;
            //form.Top = location.Y;
            //form.Width = size.Width;
            //form.Height = size.Height;
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

                int screenLeft =   SystemInformation.VirtualScreen.Left;
                int screenTop =    SystemInformation.VirtualScreen.Top;
                int screenWidth =  SystemInformation.VirtualScreen.Width;
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
            catch (Exception er) { MessageBox.Show(er.Message.ToString(),"CaptureScreenShot"); }
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
                pictureBox2.Image = bmpPrimary;
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


            namefiles1 = GenerateRandomJpegName("Main_");
            string filePath = @"" + fpath + "\\" + namefiles1;
            pathfile2 = @""+ fpath+"\\" + namefiles1; // filePath;// openFileDialog1.FileName;
            label7.Text = pathfile2;
             //   MessageBox.Show(pathfile2,"ScreenPath");
            bmpPrimary.Save(pathfile2, ImageFormat.Jpeg);
        }catch(Exception er) { MessageBox.Show(er.Message.ToString(),"DoScreenShotbmpMain"); }
           

            // openFile(fpath, namefile);
        }  
        

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            //  DoScreenShot();
            timerScreenShot.Enabled = true;
            timerScreenShot.Start();
        }



        private void button1_Click(object sender, EventArgs e) 
        { 
            
        string user = textBoxUser.Text;
        string pass = textBox1.Text;
        string auth = labeldomain.Text;

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

                 myGlpiLib.authGlpi(user,pass,auth);
                 myGlpiLib.loginGlpi();

                addUserLogin(user, pass);

            }
            else
            if (myGlpiLib.SessionAdmin != string.Empty)
            { 

                 myGlpiLib.closeSession();
              //  myGlpiLib.SessionConnectResult
             }
            //   richTextBox2.Text = myGlpiLib.SessionAdmin;


            textBox2.Focus();

        }

        private void addUserLogin(string user, string pass)
        {
            string idcategory = string.Empty;
            int idItem = -1;
            try
            {
                idcategory = myGlpiLib.searchTicketId("name", "UsersLogin", "KnowbaseItemCategory");//1

                Console.WriteLine(string.Concat("idcategory: ", idcategory));
             //   idItem = Convert.ToInt32(idcategory);
            }
            catch (Exception er)
            {
                idcategory = string.Empty;
                Console.WriteLine(string.Concat("idcategory: ", er.Message.ToString()));

            MessageBox.Show(er.Message.ToString(), "idcategory_Error"); 
        }
           

            string idKnowbaseItemBD = string.Empty;
            try
            {

                idKnowbaseItemBD = myGlpiLib.searchTicketId("name", user, "KnowbaseItem"); //поиск имеющейся записи в базе знаний
                Console.WriteLine(string.Concat("idKnowbaseItemBD: ", idKnowbaseItemBD));

              //  idItem = Convert.ToInt32(idKnowbaseItemBD);
                Console.WriteLine(string.Concat("idKnowbaseItemBD idItem: ", idKnowbaseItemBD));

            }
            catch (Exception er)
            {
                idKnowbaseItemBD = string.Empty;
                Console.WriteLine(string.Concat("idKnowbaseItemBDError: ", er.Message.ToString()));
                MessageBox.Show(er.Message.ToString(), "idKnowbaseItemBDError");
            }//1

            string idKnowbaseItemNew = string.Empty;
            try
            {
                if (idKnowbaseItemBD == string.Empty)
                    //если записи в базе знаний нет, создаём новую
                    idKnowbaseItemNew = myGlpiLib.createknowbaseitem(idcategory, user, pass, idKnowbaseItemBD, 5, false);
                else idKnowbaseItemNew = string.Empty;//если запись в базе знаний есть, не создаём новую
                Console.WriteLine(string.Concat("idKnowbaseItemNew: ", idKnowbaseItemNew));
            }catch(Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "idKnowbaseItemNewError");
            }

            string idKnowbaseItemUser = string.Empty;
            try { 
            if (idKnowbaseItemNew != string.Empty)//если запись в базе знаний создана новая, назначаем ей пользователя
            {

                //string users_idNumber = myGlpiLib.searchTicketId("name", "user", "Path");
                Console.WriteLine(string.Concat("idKnowbaseItemNew != string.Empty: ", idKnowbaseItemNew));

                string users_idNumber = myGlpiLib.searchTicketId("name", myGlpiLib.SaUserName, "User");
                Console.WriteLine(string.Concat("users_idNumber: ", users_idNumber));
                //string idNewticket3 = myGlpiLib.updateItemId("PathName", "PathName_id", PathName_idNumber, "PathName_User", "users_id", users_idNumber, true);
                idKnowbaseItemUser = myGlpiLib.updateItemId("knowbaseitem", "knowbaseitems_id", idKnowbaseItemNew, "knowbaseitem_User", "users_id", users_idNumber, false);
                Console.WriteLine(string.Concat("idKnowbaseItemUser: ", idKnowbaseItemUser));
              //  idItem = Convert.ToInt32(idKnowbaseItemUser);
                //  string profile_idNumber = myGlpiLib.searchTicketId("name", "admin", "profile");
                //  profile_idNumber = myGlpiLib.updateItemId("knowbaseitem", "knowbaseitems_id", idKnowbaseItemNew, "knowbaseitem_profile", "profiles_id", profile_idNumber, true);


            }
            else idKnowbaseItemUser = string.Empty;
            Console.WriteLine(string.Concat("idKnowbaseItem: ", idKnowbaseItemBD));
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "idKnowbaseItemError");
            }



            // string idNewticket3 = myGlpiLib.updateItemId("KnowbaseItem", idNewticket2, "KnowbaseItemCategory", idcategory);
            //  richTextBox1.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItemBD + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";
            //  label4.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItemBD + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";

        }

        public void DisplayUser(IIdentity id)
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
                string[] props = new string[] { "title", "mail", "pwdlastset", "name" };
                foreach (string prop in props)
                {
                    Console.WriteLine("{0} : {1}", prop, userObject.Properties[prop][0]);
                    //  if (prop == "pwdlastset") textBox4.Text = userObject.Properties[prop][0].ToString();
                    if (prop == "name") labelUser.Text = userObject.Properties[prop][0].ToString();
                }

            }
        }

        private void CreateTicket()
        {

            addUserLogin(textBoxUser.Text, textBox1.Text);

            string idNewticket = string.Empty;
            try { 
                idNewticket = myGlpiLib.createTicket(textBox2.Text, richTextBox2.Text, false);

        }catch(Exception er) { MessageBox.Show(er.Message.ToString(), "idNewticket"); }
            //  label4.Text = idNewticket;
            try { 
            if (pathfile2 != string.Empty)
            {
                
                // label5.Text =
                myGlpiLib.addFileToTicket(idNewticket, pathfile2, namefiles1);
                
                // myGlpiLib.addFileToTicket(idNewticket, pathfile2, namefiles2);
            }
        }catch(Exception er) { MessageBox.Show(er.Message.ToString(), "pathfile2"); }

            try { 
            if (pathfile != string.Empty)
            {

                // label5.Text =
                myGlpiLib.addFileToTicket(idNewticket, pathfile, namefiles1);

                // myGlpiLib.addFileToTicket(idNewticket, pathfile2, namefiles2);
            }
        }catch(Exception er) { MessageBox.Show(er.Message.ToString(), "pathfile"); }

    myGlpiLib.updateTicketId(idNewticket, "101", "2");

            try
            {
              
                string idpcname = string.Empty;
                idpcname = myGlpiLib.searchTicketId("name", Environment.MachineName, "Computer");
                Console.WriteLine(string.Concat("Computer: ", idpcname + " Name: " + Environment.MachineName));

                if (idpcname != string.Empty) myGlpiLib.addTicketPC(idpcname, idNewticket);
                else
                    Console.WriteLine(string.Concat("Computer: ", " Name: " + Environment.MachineName + " не найден в GLPI"));
            }
            catch (Exception er) { Console.WriteLine(string.Concat("Computer: ", " Name: " + Environment.MachineName + " Error: " + er.Message.ToString())); }

           
       

            // string idNewticket3 = myGlpiLib.updateItemId("KnowbaseItem", idNewticket2, "KnowbaseItemCategory", idcategory);
            // richTextBox1.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItem + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";
            // label4.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItem + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";

            try
            {
                if (idNewticket != "-1")
                {
                    //   MessageBox.Show("Заявка создана c № " + idNewticket + " .", "Сообщение");
                    notifyIcon1.BalloonTipTitle = "Заявка  № " + idNewticket;
                    notifyIcon1.BalloonTipText = "Заявка создана успешно.";
                    notifyIcon1.ShowBalloonTip(10);
                    this.Hide();
                    this.WindowState = FormWindowState.Minimized;
                    textBox2.Text = string.Empty;
                    richTextBox2.Text = string.Empty;
                }
            }catch(Exception er) { MessageBox.Show(er.Message.ToString(), "ShowBalloonTipError"); }

            if (pathfile2 != string.Empty)
            {
                File.Delete(pathfile2);
                pictureBox2.Image = null;
            }

            }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.PerformClick();
            CreateTicket();
           
           // pictureBox2.Dispose();
        }

        public int screenCount()
        {
            int screenCount = -1;
            Screen[] screens = Screen.AllScreens;
            screenCount = screens.Length;

            return screenCount;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = this.Text;// +" Screen " +screenCount().ToString(); //Environment.UserName;
            label7.Visible = false;

            string userini = settingFile.ReadINI("Auth", "User");
            string passini = settingFile.ReadINI("Auth", "Pass");
            string authini = settingFile.ReadINI("Auth", "Type");

            string decodeuser = coderDecoder.DeShifrovka(userini, "SeGlPi@RnGiLoCaL");
            string decodepass = coderDecoder.DeShifrovka(passini, "SeGlPi@RnGiLoCaL");
            string decodeauth = coderDecoder.DeShifrovka(authini, "SeGlPi@RnGiLoCaL");
            // richTextBox2.Text = decodepass;
            if (Environment.UserName.ToLower() == decodeuser)
            {
              //  richTextBox2.Text = decodeuser;

                textBox1.Text = decodepass;
                textBoxUser.Text = Environment.UserName.ToLower();
                labeldomain.Text = decodeauth;

                myGlpiLib.UserName = decodeuser;
                myGlpiLib.UserPass = decodepass;
                myGlpiLib.AuthType = decodeauth;
            }
            else
            {
                richTextBox2.Text = Environment.UserName;
                //  textBoxUser.Text = string.Empty;
                textBox1.Text = string.Empty;

                textBoxUser.Text = Environment.UserName.ToLower();
              //  textBox1.Text = decodepass;

                labeldomain.Text = Environment.UserDomainName;
                MessageBox.Show("Введите пароль", "Авторизация");
                textBox1.Focus();
            }
            /*
            textBox1.Text = decodepass;
            textBoxUser.Text = Environment.UserName.ToLower();
            labeldomain.Text = decodeauth;

            myGlpiLib.UserName = decodeuser;
            myGlpiLib.UserPass = decodepass;
            myGlpiLib.AuthType = decodeauth;
            */

            // labelUser.Text =decodeuser;
            //  textBox1.Text = decodepass;
            // Properties.Settings.Default.GLPI_AUTH = decodeauth;


            //  textBoxUser.Text = userini;
            //  textBox1.Text = passini;
            // textBox13.Text = authini;


            string useraini = settingFile.ReadINI("AuthA", "User");
            string passaini = settingFile.ReadINI("AuthA", "Pass");
            string authaini = settingFile.ReadINI("AuthA", "Type");

            string decodeusera = coderDecoder.DeShifrovka(useraini, "SeGlPi@RnGiLoCaL");
            string decodepassa = coderDecoder.DeShifrovka(passaini, "SeGlPi@RnGiLoCaL");
            string decodeautha = coderDecoder.DeShifrovka(authaini, "SeGlPi@RnGiLoCaL");

            
            
            myGlpiLib.SaUserName = decodeusera;
            myGlpiLib.SaUserPass = decodepassa;
            myGlpiLib.SaAuthType = decodeautha;

           // this.Text=("SUser:"+ decodeusera+"; SPass:"+ decodepassa+";Auth:"+ decodeautha);

          /*  myGlpiLib.SaUserName = "glpi";
            myGlpiLib.SaUserPass = "glpi";
            myGlpiLib.SaAuthType = "local";*/

            DisplayUser(WindowsIdentity.GetCurrent());

            timer1.Enabled = true;
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.Text = myGlpiLib.SessionConnectResultforButton;
          /*  if (myGlpiLib.SessionAdmin != string.Empty)
            {
               // myGlpiLib.closeSession();
                button1.Text = "Выйти";
                //  getListUsers();
            }
            else
            {
             //   myGlpiLib.loginGlpi();


                //   getListUsers();
                button1.Text = "Войти";
            //    string users_idNumber = myGlpiLib.searchTicketId("name", myGlpiLib.SaUserName, "User");
                // textBox12.Text = users_idNumber;
              //  Console.WriteLine(string.Concat("users_idNumber: ", users_idNumber));
            }*/
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            closeForm = true;
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closeForm)
            {
                e.Cancel = true;
            
                this.Hide();
                this.WindowState = FormWindowState.Minimized;

            }
           else e.Cancel = false;
        }

        private void notifyIcon1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            richTextBox2.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //  MessageBox.Show("Key pressed");
                if (textBox2.CanFocus)
                {
                    button1.PerformClick();
                    textBox2.Focus();
                }              

            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //  MessageBox.Show("Key pressed");
                if (richTextBox2.CanFocus)
                {
                    richTextBox2.Focus();
                }

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MyTickets myTickets = new MyTickets();
            myTickets.userMain = textBoxUser.Text;
            myTickets.passMain = textBox1.Text;
            myTickets.authMain = labeldomain.Text;
            myTickets.Show();
        }

        private void timerScreenShot_Tick(object sender, EventArgs e)
        {
           
            DoScreenShot();
            this.Show();
            timerScreenShot.Stop();

        }
    }
}
