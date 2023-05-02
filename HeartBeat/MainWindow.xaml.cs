using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace HeartBeat
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        public AllInputSources lastInput;
        private KeyboardInput keyboard;
        private MouseInput mouse;

        private DateTime mouseTimer;
        private DateTime keyboardTimer;

        public static string resourceId = "edcdb4c9-6b44-438e-b983-858850a00e87";
        public string tenantKey;
        public string subscriptionKey;
        public string labsAPIURL;
        public string computerName;

        public MainWindow()
        {
            InitializeComponent();

           // Application.Current.MainWindow.Visibility = Visibility.Hidden;
            keyboard = new KeyboardInput();
            keyboard.KeyBoardKeyPressed += keyboard_KeyBoardKeyPressed;

            mouse = new MouseInput();
            mouse.MouseMoved += mouse_MouseMoved;
            lastInput = new AllInputSources();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan();
            timer.Interval = new TimeSpan(0, 0, 0, 15); //check every 15 seconds
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void keyboard_KeyBoardKeyPressed(object sender, EventArgs e)
        {
            keyboardTime.Content = DateTime.Now;
            keyboardTimer = DateTime.Now;
            //keyboardTime.Content = FormatDateTime(DateTime.Now);
        }

        private string FormatDateTime(DateTime dateTime)
        {
            //return dateTime.ToString("HH:mm", CultureInfo.CurrentUICulture);
            return dateTime.ToString("HH:mm:ss fff", CultureInfo.CurrentUICulture);
        }
        void mouse_MouseMoved(object sender, EventArgs e)
        {
            mouseTime.Content = DateTime.Now;
            mouseTimer = DateTime.Now;
            //mouseTime.Content = FormatDateTime(DateTime.Now);
        }
        
        async void timer_Tick(object sender, EventArgs e)
        {
            ApiConfigurations apiCall = new ApiConfigurations();

            Console.WriteLine(DateTime.Now);
            TimeSpan idleMouse = DateTime.Now - mouseTimer; //Difference between active MOUSE vs. not active MOUSE
            TimeSpan idleKeyboard = DateTime.Now - keyboardTimer; //Difference between active KEYBOARD vs. not active KEYBOARD

            lastInputTime.Content = FormatDateTime(lastInput.GetLastInputTime());
            Console.WriteLine("Idle Minutes --" + idleMouse.Minutes);

            //check if idle in 10 minutes then show dialog box
            //if(idleMouse.Minutes >= 1 && idleKeyboard.Minutes >= 1)
            //{
            //    Application.Current.MainWindow.Visibility = Visibility.Visible;
            //}

            if (mouseTimer != null || keyboardTimer != null) // user is active
            {
                //check if idle in 15 minutes then autoshutdown trigger
                if (idleMouse.Minutes >= 1 && idleKeyboard.Minutes >= 1)
                {
                    var jsonVMData = new { computerName =  computerName};
                    var data = JsonConvert.SerializeObject(jsonVMData);

                    await apiCall.UpdateConsumedHours(labsAPIURL + "/MachineLabs/UpdateConsumedHours" , data);
                    await apiCall.ShutdownVM(resourceId);

                    Console.WriteLine("Mouse : " + mouseTime.Content + " AND " + "Keyboard : " + keyboardTime.Content);
                }
            }
            else
            {
                mouseTime.Content = DateTime.Now;
                keyboardTime.Content = DateTime.Now;
            }
        }

    }
}
