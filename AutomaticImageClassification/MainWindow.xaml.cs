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


namespace AutomaticImageClassification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void parentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tabControl.Height = MainGrid.ActualHeight;
            tabControl.Width = MainGrid.ActualWidth;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // ... Cast sender object.
            MenuItem item = sender as MenuItem;
            // ... Change Title of this window.
            this.Title = "Info: " + item.Header;
        }

        private void GetTrainSet(object sender, RoutedEventArgs e)
        {
            string path = GetFolderBrowserPath();
            this.TrainSetTextBox.Text = string.Join("\\", path.Split('\\').Reverse().Take(2).Reverse());
        }

        private string GetFolderBrowserPath()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                MessageBox.Show(dialog.SelectedPath + " res : " + result);
                return dialog.SelectedPath;
            }
            return "";
        }

        private void GetTestSet(object sender, RoutedEventArgs e)
        {
            string path = GetFolderBrowserPath();
            this.testSetTextBox.Text = string.Join("\\",path.Split('\\').Reverse().Take(2).Reverse());
        }


    }
}
