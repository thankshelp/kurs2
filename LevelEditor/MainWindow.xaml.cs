using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LevelEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int W = 60;
        int H = 60;
        int C = 20;
        int R = 16;

        BitmapImage wall = new BitmapImage(new Uri(@"pack://application:,,,/Resources/2pt3sid.png", UriKind.Absolute));
        BitmapImage floor = new BitmapImage(new Uri(@"pack://application:,,,/Resources/ft.jpg", UriKind.Absolute));


        public MainWindow()
        {
            InitializeComponent();

            //указыается количество строк и столбцов в сетке
            level.Rows = R;
            level.Columns = C;
            //указываются размеры сетки (число ячеек * (размер кнопки в ячейки + толщина её границ))
            level.Width = C * (W + 1);
            level.Height = R * (H + 1);
            //толщина границ сетки
            level.Margin = new Thickness(0, 0, 0, 0);
            for (int i = 0; i < C * R; i++)
            {
                //создание кнопки
                Button btn = new Button();
                //запись номера кнопки
                btn.Tag = 0;
                //установка размеров кнопки
                btn.Width = W;
                btn.Height = H;
                //текст на кнопке
                btn.Content = "0";
                //толщина границ кнопки
                btn.Margin = new Thickness(0);
                //при нажатии кнопки, будет вызываться метод Btn_Click
                btn.Click += Btn_Click; ;
                //добавление кнопки в сетку
                level.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //получение значения лежащего в Tag
            int n = (int)((Button)sender).Tag;

            //создание контейнера для хранения изображения
            Image img = new Image();

            if (n == 0)
            {
                ((Button)sender).Tag = 1;
                //запись картинки с миной в контейнер
                img.Source = wall;
            }

            if (n == 1)
            {
                ((Button)sender).Tag = 2;
                //запись картинки с миной в контейнер
                img.Source = floor;
            }

            if (n == 2)
            {
                ((Button)sender).Tag = 0;
                //запись картинки с миной в контейнер
                img.Source = wall;
            }


            ////установка фона нажатой кнопки, цвета и размера шрифта
            //((Button)sender).Background = Brushes.White;
            //((Button)sender).Foreground = Brushes.Red;
            //((Button)sender).FontSize = 23;

            //создание компонента для отображения изображения
            StackPanel stackPnl = new StackPanel();
            //установка толщины границ компонента
            stackPnl.Margin = new Thickness(1);
            //добавление контейнера с картинкой в компонент
            stackPnl.Children.Add(img);

            //запись в нажатую кнопку её номера
            ((Button)sender).Content = stackPnl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.ShowDialog();

            //File.OpenWrite();
            //int i = 0;
            foreach (Button b in level.Children)
            {
                
                File.AppendAllText(dlg.FileName, (b.Tag.ToString()));
                //i++;
                //if (i % C == 0) File.AppendAllText(dlg.FileName, "\r\n");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();

            //File.OpenWrite();
            int i = 0;
            string s = File.ReadAllText(dlg.FileName);

            foreach (Button b in level.Children)
            {
                if (s[i] == '1')
                    Btn_Click(b, e);

                if (s[i] == '2')
                {
                    Btn_Click(b, e);
                    Btn_Click(b, e);
                }

                i += 1;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (Button b in level.Children)
            {
                b.Tag = 0;

                b.Content = "0";
            }
        }
    }
}
