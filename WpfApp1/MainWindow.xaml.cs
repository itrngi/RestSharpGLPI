using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using System.Data;
//using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Drawing.Imaging;
using GlPiLibNet;
using System.Security.Principal;
using System.DirectoryServices;
using INISettings;
using CoderGLPI;
using RestSharp;
using System.Drawing;
using Path = System.IO.Path;
using System.Windows.Forms;
using System.IO;
using Hardcodet.Wpf.TaskbarNotification;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        TaskbarIcon notifyIcon1 = new TaskbarIcon();
       
        public MainWindow()
        {
            InitializeComponent();
            notifyIcon1.Icon = Properties.Resources.GLPI_256;// "/GLPI_256.ico";
            notifyIcon1.Visibility = Visibility.Visible;
            //  notifyIcon1.Icon = Resources.GLPI_256;

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
                  //  if (prop == "name") labelUser.Text = userObject.Properties[prop][0].ToString();
                }

            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationUser();
        }

            private void AuthenticationUser()
        {

            string user = userName.Text;
            string pass = userPass.Password;
            string auth = userDomain.Content.ToString();

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

                addUserLogin(user, pass);

            }
            else
           if (myGlpiLib.SessionAdmin != string.Empty)
            {

                myGlpiLib.closeSession();
                //  myGlpiLib.SessionConnectResult
            }
            //   richTextBox2.Text = myGlpiLib.SessionAdmin;


            themeText.Focus();
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

                 System.Windows.MessageBox.Show(er.Message.ToString(), "idcategory_Error");
            }


            string idKnowbaseItemBD = string.Empty;
            try
            {

                idKnowbaseItemBD = myGlpiLib.searchTicketId("name", user, "KnowbaseItem"); //поиск имеющейся записи в базе знаний
                Console.WriteLine(string.Concat("idKnowbaseItemBD: ", idKnowbaseItemBD));

                Console.WriteLine(string.Concat("idKnowbaseItemBD idItem: ", idKnowbaseItemBD));

            }
            catch (Exception er)
            {
                idKnowbaseItemBD = string.Empty;
                Console.WriteLine(string.Concat("idKnowbaseItemBDError: ", er.Message.ToString()));
                 System.Windows.MessageBox.Show(er.Message.ToString(), "idKnowbaseItemBDError");
            }//1

            string idKnowbaseItemNew = string.Empty;
            try
            {
                if (idKnowbaseItemBD == string.Empty)
                    //если записи в базе знаний нет, создаём новую
                    idKnowbaseItemNew = myGlpiLib.createknowbaseitem(idcategory, user, pass, idKnowbaseItemBD, 5, false);
                else idKnowbaseItemNew = string.Empty;//если запись в базе знаний есть, не создаём новую
                Console.WriteLine(string.Concat("idKnowbaseItemNew: ", idKnowbaseItemNew));
            }
            catch (Exception er)
            {
                 System.Windows.MessageBox.Show(er.Message.ToString(), "idKnowbaseItemNewError");
            }

            string idKnowbaseItemUser = string.Empty;
            try
            {
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
                 System.Windows.MessageBox.Show(er.Message.ToString(), "idKnowbaseItemError");
            }
            // string idNewticket3 = myGlpiLib.updateItemId("KnowbaseItem", idNewticket2, "KnowbaseItemCategory", idcategory);
            //  richTextBox1.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItemBD + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";
            //  label4.Text = "idcategory" + idcategory + "; idKnowbaseItem:" + idKnowbaseItemBD + "; idKnowbaseItemNew: " + idKnowbaseItemNew + "; idKnowbaseItemUser: " + idKnowbaseItemUser + ";";

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = this.Title;// +" Screen " +screenCount().ToString(); //Environment.UserName;
          //  label7.Visible = false;

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

                userPass.Password = decodepass;
                userName.Text = Environment.UserName.ToLower();
                userDomain.Content = decodeauth;

                myGlpiLib.UserName = decodeuser;
                myGlpiLib.UserPass = decodepass;
                myGlpiLib.AuthType = decodeauth;
            }
            else
            {
               // richTextBox1.Text = Environment.UserName;
                //  textBoxUser.Text = string.Empty;
                userPass.Password = string.Empty;

                userName.Text = Environment.UserName.ToLower();
                //  textBox1.Text = decodepass;

                userDomain.Content = Environment.UserDomainName;
                 System.Windows.MessageBox.Show("Введите пароль", "Авторизация");
                userPass.Focus();
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

           // timer1.Enabled = true;
           // timer1.Start();
        }

        public string GenerateRandomJpegName(string pref)
        {
            DateTime date1 = new DateTime();
            date1 = DateTime.Now;
            string dateName = date1.ToString("dd_MM_yyyy_HH_mm_ss");
            return "Screen_" + pref + "" + dateName + ".jpg";
        }

        private Bitmap CaptureScreenShot() // делаем скриншот экрана 
        {
            //  System.Windows.MessageBox.Show(Screen.AllScreens[screenNum].WorkingArea.ToString(), Screen.AllScreens[screenNum].DeviceName+" [ "+ screenNum + "]"+ Screen.AllScreens.Count()+" | "+ Screen.AllScreens[screenNum].Bounds.Location);

            //  Bitmap bitmap1=new Bitmap(10,10);
            //  try
            //   {
            System.Drawing.Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
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
            catch (Exception er) {  System.Windows.MessageBox.Show(er.Message.ToString(), "CaptureScreenShot"); }
            // Do something with the Bitmap here, like save it to a file:
            //  bitmap.Save(savePath, ImageFormat.Jpeg);


            //    Bitmap bitmap = new Bitmap(Screen.AllScreens[screenNum].Bounds.Width, Screen.AllScreens[screenNum].Bounds.Height);
            //   bitmap = new Bitmap(bounds.Width, bounds.Height);

            /* using (Graphics gr = Graphics.FromImage(bitmap1)) 
             { gr.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size); }*/

            /*   }catch(Exception er)
               {
                    System.Windows.MessageBox.Show(er.Message.ToString(), "Error Screen Shot");
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
                //  imgScreen.Image = bmpPrimary;
                //   System.Windows.MessageBox.Show(this.Controls.Find("pictureBoxScreen", true).ToString());
                // if ()  System.Windows.MessageBox.Show("picture");
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
                pathfile2 = @"" + fpath + "\\" + namefiles1; // filePath;// openFileDialog1.FileName;
             //   label7.Text = pathfile2;
                //    System.Windows.MessageBox.Show(pathfile2,"ScreenPath");
                bmpPrimary.Save(pathfile2, ImageFormat.Jpeg);


                fileNameText.Content = pathfile2;

                 BitmapImage bitmapImage = new BitmapImage();
                //  imgScreen.Width = bitmapImage.DecodePixelWidth = 1980;
                //  imgScreen.Height = bitmapImage.DecodePixelHeight = 1080;

                bitmapImage.UriSource = new Uri("file:///"+pathfile2);
                imgScreen.Source = bitmapImage;
                // imgScreen.UpdateLayout();

            }
            catch (Exception er) {  System.Windows.MessageBox.Show(er.Message.ToString(), "DoScreenShotbmpMain"); }

            // Uri url = new Uri(pathfile2);
        

            FlowDocument myFlowDoc = new FlowDocument();
            // Add paragraphs to the FlowDocument.
            myFlowDoc.Blocks.Add(new Paragraph(new Run("file:///" + pathfile2)));
        //   myFlowDoc.Blocks.Add(new Paragraph(new Run("Paragraph 2")));
         //   myFlowDoc.Blocks.Add(new Paragraph(new Run("Paragraph 3")));
           // RichTextBox myRichTextBox = new RichTextBox();
            contentTicket.Document = myFlowDoc;
        }

       

        public void DoScreenShot()
        {
            DoScreenShotbmpMain();

        }

        private void screenShot_Click(object sender, RoutedEventArgs e)
        {
            DoScreenShot();
        }

        private void otherFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = ".png";
            openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = openFileDialog.ShowDialog();

            var fileContent = string.Empty;
            var filePath = string.Empty;

           // using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "С:\\";
             //   openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
               // openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                // if (openFileDialog.ShowDialog() == DialogResult.OK)

                if (result == true)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();


                    /* using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                     {
                       //  phones = jsonFormatter.ReadObject(fs) as List<Phone>;
                         fileContent = reader.ReadObject(fs);
                     }*/

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
                else return;
            }
            pathfile = filePath;
            fileNameText.Content = pathfile;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("file:///" + pathfile);
            imgScreen.Source = bitmapImage;
        }

        private void CreateTicket()
        {

            addUserLogin(userName.Text, userPass.Password);

            string idNewticket = string.Empty;
            try
            {
                idNewticket = myGlpiLib.createTicket(themeText.Text, contentTicket.Document.Blocks.FirstBlock.ToString(), false);

            }
            catch (Exception er) { System.Windows.MessageBox.Show(er.Message.ToString(), "idNewticket"); }
            //  label4.Text = idNewticket;
            try
            {
                if (pathfile2 != string.Empty)
                {

                    // label5.Text =
                    myGlpiLib.addFileToTicket(idNewticket, pathfile2, namefiles1);

                    // myGlpiLib.addFileToTicket(idNewticket, pathfile2, namefiles2);
                }
            }
            catch (Exception er) { System.Windows.MessageBox.Show(er.Message.ToString(), "pathfile2"); }

            try
            {
                if (pathfile != string.Empty)
                {

                    // label5.Text =
                    myGlpiLib.addFileToTicket(idNewticket, pathfile, namefiles1);

                    // myGlpiLib.addFileToTicket(idNewticket, pathfile2, namefiles2);
                }
            }
            catch (Exception er) { System.Windows.MessageBox.Show(er.Message.ToString(), "pathfile"); }

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
                  //  TaskbarIcon notifyIcon1 = new TaskbarIcon();

                    //   MessageBox.Show("Заявка создана c № " + idNewticket + " .", "Сообщение");
                    notifyIcon1.ToolTip = "Заявка  № " + idNewticket;
                    notifyIcon1.ToolTipText = "Заявка создана успешно.";
                    notifyIcon1.ShowBalloonTip("Заявка  № " + idNewticket, "Заявка создана успешно.", BalloonIcon.Info);
                    this.Hide();
                   // this.WindowState = FormWindowState.Minimized;
                    themeText.Text = string.Empty;
                    contentTicket.Document.Blocks.Clear();
                }
            }
            catch (Exception er) {
                
                System.Windows.MessageBox.Show(er.Message.ToString(), "ShowBalloonTipError"); }

            if (pathfile2 != string.Empty)
            {
                File.Delete(pathfile2);
                imgScreen.Source = null;
            }

        }
        private void createTicket_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationUser();
            CreateTicket();
        }
    }
}
