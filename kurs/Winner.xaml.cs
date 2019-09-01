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
using System.Windows.Shapes;

namespace kurs
{
    /// <summary>
    /// Логика взаимодействия для Winner.xaml
    /// </summary>
    public partial class Winner : Window
    {
        public Winner()
        {
            InitializeComponent();
        }

        public Winner(int score)
        {
            int m;
            InitializeComponent();
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Mvideo\Desktop\kursPM\kursPM\score.txt");
            //while((line = file.ReadLine()) != null)
            //{
            //    int res = Int32.Parse(line);
            //    if(score>res)
            //    {
            //        m = score;//как отправить это значение на главный экран...
            //    }
            //}
            scr.Content = "Your score:" + score;
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
