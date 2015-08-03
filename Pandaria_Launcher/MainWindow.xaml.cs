using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.IO;

namespace Pandaria_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        public object currObject;

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Version AppVersion = Assembly.GetExecutingAssembly().GetName().Version;
            this.version.Content = String.Format("{0}.{1}.{2}.{3}", AppVersion.Major, AppVersion.Minor, AppVersion.Build, AppVersion.Revision);

            bool hasRealm = false;
            try
            {
                using (StreamReader configRead = new StreamReader(@"WTF/Config.wtf"))
                {
                    string line;
                    int lineID = 0;
                    while ((line = configRead.ReadLine()) != null)
                    {
                        if (line.Substring(0, 13) == "SET realmlist")
                        {
                            if (line.Substring(13) == "\"pandaria.es\"")
                                hasRealm = true;
                            else
                            {
                                var file = new List<string>(File.ReadAllLines(@"WTF/Config.wtf"));
                                file.RemoveAt(lineID);
                                File.WriteAllLines("WTF/temp.wtf", file.ToArray());
                            }
                        }
                        lineID++;
                    }
                }
                if (hasRealm == false)
                {
                    using (var writer = File.AppendText(@"WTF/temp.wtf"))
                    {
                        writer.WriteLine("SET realmlist \"pandaria.es\"");
                    }
                    File.Replace(@"WTF/temp.wtf", @"WTF/Config.wtf", @"WTF/backup.wtf");
                    File.Delete(@"WTF/backup.wtf");
                }
            }
            catch(Exception ex)
            {
                if (ex is DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(@"WTF");
                    using (StreamWriter newConfig = new StreamWriter(@"WTF/Config.wtf"))
                        newConfig.WriteLine("SET realmlist \"pandaria.es\"");
                }
                else if (ex is IOException)
                    using (StreamWriter newConfig = new StreamWriter(@"WTF/Config.wtf"))
                        newConfig.WriteLine("SET realmlist \"pandaria.es\"");
                else
                    MessageBox.Show("Something went wrong! If the problem continues to persist please contact us!");
            }
            
            try
            {
                Ping ping = new Ping();
                PingReply r = ping.Send("216.58.209.14");
                if (r.Status.ToString() == "Success")
                {
                    status.Content = "Online";
                    status.Foreground = new SolidColorBrush(Color.FromArgb(255, 88, 122, 141));
                }
                else
                {
                    status.Content = "Offline";
                    status.Foreground = new SolidColorBrush(Color.FromArgb(255, 145, 104, 63));
                }
                this.Browser.Navigate("http://demo2.spliddo.net/news.php");
                currObject = News;
                News.Background = new SolidColorBrush(Colors.Goldenrod);
                this.Browser.Visibility = Visibility.Visible;
                
            }
            catch(Exception ex)
            {
                status.Content = "Disconnected";
                status.Foreground = new SolidColorBrush(Colors.Red);
            }
            
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            try
            {
                base.OnMouseLeftButtonDown(e);
                Launcher.DragMove();
            }
            catch(Exception ex)
            {
                // e da ama ne :)
            }
        }

        private void OnClickMinimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnClickClose(object sender, MouseButtonEventArgs e)
        {
            
            this.Close();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            
            if (sender == minimize)
            {
                minimize.Foreground = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                minimize.Opacity = 1;
            }
            if (sender == exit)
            {
                exit.Foreground = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                exit.Opacity = 1;
            }

            if (sender == News && currObject != News)
                News.Background = new SolidColorBrush(Colors.DimGray);
            if (sender == Ranking && currObject != Ranking)
                Ranking.Background = new SolidColorBrush(Colors.DimGray);
            if (sender == Changelog && currObject != Changelog)
                Changelog.Background = new SolidColorBrush(Colors.DimGray);
            if (sender == play)
                play.Background = new SolidColorBrush(Color.FromArgb(255,46,134,224));

        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            
            if (sender == minimize)
                minimize.Foreground = new SolidColorBrush(Color.FromArgb(255, 222, 222, 222));
            minimize.Opacity = 0.60;
            if (sender == exit)
                exit.Foreground = new SolidColorBrush(Color.FromArgb(255, 222, 222, 222));
           exit.Opacity = 0.60;
            if (sender == News && currObject != News)
                News.Background = null;
            if (sender == Ranking && currObject != Ranking)
                Ranking.Background = null;
            if (sender == Changelog && currObject != Changelog)
                Changelog.Background = null;
            if (sender == play)
                play.Background = new SolidColorBrush(Color.FromArgb(255,62,102,143));
        }

        private void OnClickPlay(object sender, MouseButtonEventArgs e)
        {
            string GamePath1 = "Pandaria.exe";
            string GamePath2 = "Wow.exe";
            try
            {
                System.Diagnostics.Process.Start(GamePath1);
            }
            catch (Exception ex1)
            {
                try
                {
                    System.Diagnostics.Process.Start(GamePath2);
                    Environment.Exit(0);
                }
                catch(Exception ex2)
                {
                    MessageBox.Show("Could not find the game executable file!");
                }
            }

        }

        private void wb_OnClick(object sender, MouseButtonEventArgs e)
        {
                  
            try
            {
                if ((sender == News) && (currObject != News))
                {
                    Browser.Navigate("http://demo2.spliddo.net/news.php");
                    News.Background = new SolidColorBrush(Colors.Goldenrod);
                }
                if ((sender == Ranking) && (currObject != Ranking))
                {
                    Browser.Navigate("http://demo2.spliddo.net/index.php");
                    Ranking.Background = new SolidColorBrush(Colors.Goldenrod);
                }
                if ((sender == Changelog) && (currObject != Changelog))
                {
                    Browser.Navigate("http://demo2.spliddo.net/changelog.php");
                    Changelog.Background = new SolidColorBrush(Colors.Goldenrod);
                }
                currObject = sender;
            }
            catch(Exception ex)
            {
                Browser.Visibility = Visibility.Hidden;
                News.Background = null;
                Ranking.Background = null;
                Changelog.Background = null;
                MessageBox.Show("Something went wrong!");
            }
            if (currObject == News)
            {
                Ranking.Background = null;
                Changelog.Background = null;
            }
            if (currObject == Ranking)
            {
                News.Background = null;
                Changelog.Background = null;
            }
            if (currObject == Changelog)
            {
                News.Background = null;
                Ranking.Background = null;
            }
        }

    }
}
