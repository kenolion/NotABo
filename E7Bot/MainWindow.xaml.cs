using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using OpenCvSharp;
using Rect = OpenCvSharp.Rect;
using Window = System.Windows.Window;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;
using Timer = System.Timers.Timer;

namespace E7Bot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    enum state
    {
        finishRun,
        stageclear,
        SelectTeam,
        start,
        BuyEnergy,
        tryagain,
        confirm
    }


    public class Stats : INotifyPropertyChanged
    {
        public int _total;
        public int _failed;


        public Stats(int total, int failed)
        {
            _total = total;
            _failed = failed;
        }

        public int total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Subtotal");
            }
        }

        public int failed
        {
            get { return _failed; }
            set
            {
                _failed = value;
                OnPropertyChanged("failed");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public partial class MainWindow : Window
    {
        public E7Timer timerTest;
        private E7Timer updateUi;
        private List<Action> temp;
        public string selectedName { get; set; }
        public SimpleCommand simp;

        Stats test = new Stats(0, 0);
        private int i = 0;

        public bool findSelectTeam = false;
        public bool noRefill = false;


        public ObservableCollection<Action> actions { get; set; }

        public static ObservableCollection<Node> dcsTree { get; set; }

        //timer
        private Timer timer;

        private state gameState;

        //packet sniffer
        private PacketSniffer p;

        //KeyValueControl
        private KeyValueControl imgName;

        public Profile cfg;


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();


            //actionBT = new Tree();
            timerTest = new E7Timer(1500);
            actions = new ObservableCollection<Action>();
            dcsTree = new ObservableCollection<Node>();
            //designerListBox.ItemsSource = actions;
            nodeListView.ItemsSource = dcsTree;
            this.DataContext = test;
            //hotkeys
            HotkeyManager.Current.AddOrReplace("StartOrStop", Key.F1, ModifierKeys.Shift, onStartHotkey);
            //
            cfg = new Profile();
            imgName = new KeyValueControl();
            imgName.Key = "firstName";
            timerTest.SetFunction(iterateThroughList);
            LoadProfile_OnClick(null, null);
        }


        private void onStartHotkey(object sender, HotkeyEventArgs e)
        {
            Btn_OnClick(sender, null);
        }


        /*private void capture()
        {
            var devices = CaptureDeviceList.Instance;

            if (devices.Count == 0)
            {
                Console.WriteLine(
                    "No devices found, are you running as admin(if in Windows), or root(if in Linux/Mac)?");

                return;
            }

            Console.WriteLine("Available AirPcap devices:");
            for (var i = 0; i < devices.Count; i++)
            {
                Console.WriteLine("[{0}] - {1}", i, devices[i].Name);
            }

            Console.WriteLine();
            Console.Write("-- Please choose a device to capture: ");
            var devIndex = int.Parse(Console.ReadLine());

            var device = devices[devIndex];

            device.Open();

            device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

            device.StartCapture();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();

            device.StopCapture();

            Console.WriteLine("-- Capture stopped.");

            // Print out the device statistics
            Console.WriteLine(device.Statistics.ToString());
            Console.ReadKey();
            // Close the pcap device
            device.Close();
        }

        private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var time = e.Packet.Timeval.Date;
            var len = e.Packet.Data.Length;
            Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
                time.Hour, time.Minute, time.Second, time.Millisecond, len);
            Console.WriteLine(e.Packet.ToString());

            var p = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            Console.WriteLine(p.ToString(PacketDotNet.StringOutputType.VerboseColored));
        }*/

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();


            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter =
                "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Mat test = new Mat(filename);
                Console.WriteLine(filename);
            }
        }

        public void startBot()
        {
            timerTest.Start();
        }

        public void iterateThroughList(Object source, ElapsedEventArgs e)
        {
            Config.actionBT.run();
//            bool tempCond = true;
//            List<Rect> r;
//            for (int j = 0; j < actions[i].link.Count; j++)
//            {
//                var temp = actions[actions[i].link[j]];
//                tempCond = temp.run();
//            }
//
//            if (actions[i].isAnd)
//            {
//                if (actions[i].run())
//                {
//                    i++;
//                    if (i >= actions.Count)
//                    {
//                        i = 0;
//                    }
//                }
//            }
        }

        public bool findImage(ImageRecognition find)
        {
            bool found = false;
            // find.RunTemplateMatch(out found, Dispatcher);
            Dispatcher.Invoke(() =>
            {
                if (found)
                    Console.WriteLine("Found" + find.name);
                else
                {
                    Console.WriteLine("Cannot Find" + find.name);
                }
            });
            return found;
        }


        private void Btn_OnClick(object sender, RoutedEventArgs e)
        {
            gameState = state.finishRun;
            timerTest.Start();
            Config.actionBT.resetToRoot();
            i = 0;
            if (timerTest.isStart)
                btn.Content = "Stop Bot";
            else
                btn.Content = "Start Bot";
        }

        private void Btn_Stop(object sender, RoutedEventArgs e)
        {
            timerTest.Stop();
            gameState = state.stageclear;
        }

        private void cbChanged(object sender, RoutedEventArgs e)
        {
            findSelectTeam = !findSelectTeam;
            Console.WriteLine(findSelectTeam);
        }

        private void No_Refill_OnChecked(object sender, RoutedEventArgs e)
        {
            noRefill = !noRefill;
        }

        private void NewImageBtn_OnClick(object sender, RoutedEventArgs e)
        {
            //actions.Add(new Action(ComboBox1.SelectionBoxItem.ToString(), true));
            if (!String.IsNullOrEmpty(nodeName.Text))
            {
                Config.actionBT.Insert(nodeName.Text, insertRight.IsChecked.Value);
                dcsTree.Add(Config.actionBT.getNodeByName(nodeName.Text));
                ActionsList al = new ActionsList(dcsTree.Last());
                al.Show();
                ((MainWindow) System.Windows.Application.Current.MainWindow).UpdateLayout();
                nodeName.Text = "";
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Action a = (Action) ((ListViewItem) sender).Content;
            Console.WriteLine(a.name);
        }

        private void ListViewItem_PreviewNode(object sender, MouseButtonEventArgs e)
        {
            Node a = (Node) ((ListViewItem) sender).Content;
            ActionsList al = new ActionsList(a);
            // a.name = "test";

            al.Show();
            Console.WriteLine(a.name);

            Console.WriteLine(dcsTree.Last().name);
        }

        private void deleteNode(object sender, RoutedEventArgs e)
        {
            Button btn = (Button) sender;
            int index = int.Parse(btn.Tag.ToString());
            bool removeAll = Config.actionBT.deleteNode(index);
            if (removeAll)
            {
                dcsTree.Clear();
                NewImageBtn.Visibility = Visibility.Visible;
                return;
            }

            dcsTree.RemoveAt(index);
        }

        private void LoadProfile_OnClick(object sender, RoutedEventArgs e)
        {
            String selected = "";
            selected = Config.CFG_PATH + "default";
            profNameTextBox.Text = "default";
            if (!String.IsNullOrEmpty(ConfigFilesCB.SelectionBoxItem.ToString()))
            {
                selected = Config.CFG_PATH + ConfigFilesCB.SelectionBoxItem;
                profNameTextBox.Text = ConfigFilesCB.SelectionBoxItem.ToString();
            }

            if (!String.IsNullOrEmpty(selected))
            {
                cfg = SaveNLoad.deSerialize(selected);

                Config.actionBT = cfg.actionBT;

                dcsTree.Clear();

                for (int j = 0; j < cfg.dcsTree.Count; j++)
                {
                    dcsTree.Add(cfg.dcsTree[j]);
                }
            }

            NewImageBtn.Visibility = Visibility.Hidden;
            Config.getCfg(ConfigFilesCB);
        }

        private void SaveProfile_OnClick(object sender, RoutedEventArgs e)
        {
            String filePath = Config.CFG_PATH + "default";
            if (!String.IsNullOrEmpty(profNameTextBox.Text))
            {
                filePath = Config.CFG_PATH + profNameTextBox.Text;
            }

            cfg.actionBT = Config.actionBT;
            cfg.dcsTree = dcsTree;
            SaveNLoad.serializeFile(filePath, cfg);
            Config.getCfg(ConfigFilesCB);
        }

        private void insertRightOnClick(object sender, RoutedEventArgs e)
        {
            insertLeftOrRight(sender, true);
        }

        private void insertLeftOnClick(object sender, RoutedEventArgs e)
        {
            insertLeftOrRight(sender);
        }

        public void insertLeftOrRight(object sender, bool right = false)
        {
            if (!String.IsNullOrEmpty(nodeName.Text))
            {
                Button btn = (Button) sender;
                int index = int.Parse(btn.Tag.ToString());
                Config.actionBT.Insert(index, nodeName.Text, right);
                dcsTree.Add(Config.actionBT.getNodeById(Config.actionBT.lastAsgId));
                ActionsList al = new ActionsList(dcsTree.Last());
                al.Show();
                ((MainWindow) System.Windows.Application.Current.MainWindow).UpdateLayout();
                nodeName.Text = "";
            }
        }
    }

    [Serializable]
    public class Profile
    {
        public Tree actionBT;
        public ObservableCollection<Node> dcsTree { get; set; }
    }


    public static class VirtualMouse
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        static Random _random = new Random((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        
        public static void Move(int xDelta, int yDelta)
        {
            mouse_event(MOUSEEVENTF_MOVE, xDelta, yDelta, 0, 0);
        }

        public static void ClickImg(Rect r, bool isClick = true)
        {
            MoveTo(r, false);
            if (isClick)
                LeftClick();
        }

        public static void MoveTo(Rect r, bool midpoint = true)
        {
            var tempX = r.TopLeft.X;
            var tempY = r.TopLeft.Y;

            if (midpoint)
            {
                tempX += r.Width / 2;
                tempY += r.Height / 2;
            }
            else
            {
                tempX += _random.Next(r.Width);
                tempY += _random.Next(r.Height);
            }
            
            

            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(tempX,
                tempY);
        }

        public static void LeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, System.Windows.Forms.Control.MousePosition.X,
                System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, System.Windows.Forms.Control.MousePosition.X,
                System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void LeftDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, System.Windows.Forms.Control.MousePosition.X,
                System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void LeftUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, System.Windows.Forms.Control.MousePosition.X,
                System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void RightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, System.Windows.Forms.Control.MousePosition.X,
                System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, System.Windows.Forms.Control.MousePosition.X,
                System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void RightDown()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, System.Windows.Forms.Control.MousePosition.X,
                System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void RightUp()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, System.Windows.Forms.Control.MousePosition.X,
                System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }
    }

    public class SimpleCommand : ICommand
    {
        public event EventHandler<object> Executed;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (Executed != null)
                Executed(this, parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}