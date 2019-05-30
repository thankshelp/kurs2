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

namespace kurs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public class CChar
        {
            int x, y;
            int px, py;

            int h, w;
            Ellipse pm;

            public CChar(int x, int y)
            {
                this.x = x;
                this.y = y;

                w = h = 45;

                px = x * w; 
                py = y * h;

                pm = new Ellipse();

                pm.Stroke = Brushes.YellowGreen;
                pm.Fill = Brushes.Yellow;
                pm.StrokeThickness = 2;

                pm.Width = 45;
                pm.Height = 45;

                pm.RenderTransform = new TranslateTransform(px, py);
                //pm.Margin = new Thickness(px, py, 0, 0);
            }

            public void move(int x, int y, int[,] map)
            {
                if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) return;

                if (map[x, y] == 1) return;

                this.x = x;
                this.y = y;

                if (px > x * w)
                    px-=5;
                if (px < x * w)
                    px+=5;
                //px = x * w;
                if (py > y * h)
                    py-=5;
                if (py < y * h)
                    py+=5;
                //py = y * h;

                pm.RenderTransform = new TranslateTransform(px, py);
            }

            public void addToScene(ref Canvas scene)
            {
                scene.Children.Add(pm);
            }
        }

        public class CDir
        {
            public int x, y;
            int dx, dy;

            public CDir(int x, int y)
            {
                this.x = x;
                this.y = y;
                dx = 1; 
                dy = 1;
            }

            public void update(int [,] map)
            {
                if ((x + dx < 0) || (x + dx >= map.GetLength(0)) || (y + dy < 0) || (y + dy > map.GetLength(1))) return;

                if ((map[x + dx, y + dy] == 2) || (map[x + dx, y + dy] == 0))
                {
                    x = x + dx;
                    y = y + dy;
                }
            }

            public void up()
            {
                dy = 1;
                dx = 0;
            }

            public void down()
            {
                dy = -1;
                dx = 0;
            }

            public void left()
            {
                dx = -1;
                dy = 0;
            }

            public void right()
            {
                dx = 1;
                dy = 0;
            }

        }

        CChar pakman;
        CDir dir;
        System.Windows.Threading.DispatcherTimer Timer;

        //20x16
        public int[,] map = { 
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
            { 1, 2, 1, 1, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 2, 1, 1, 2, 1 },
            { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
            { 1, 2, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 2, 1 },
            { 1, 2, 2, 2, 2, 1, 2, 2, 2, 1, 1, 2, 2, 2, 1, 2, 2, 2, 2, 1 },
            { 1, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 1, 1, 1 },
            { 1, 2, 2, 2, 2, 1, 2, 1, 1, 0, 0, 1, 1, 2, 1, 2, 2, 2, 2, 1 },
            { 1, 2, 1, 1, 2, 2, 2, 1, 0, 0, 0, 0, 1, 2, 2, 2, 1, 1, 2, 1 },
            { 1, 2, 2, 1, 2, 1, 2, 1, 0, 0, 0, 0, 1, 2, 1, 2, 1, 2, 2, 1 },
            { 1, 1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1 },
            { 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1 },
            { 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1 },
            { 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } }; 

        public MainWindow()
        {
            InitializeComponent();

            pakman = new CChar(5, 1);
            pakman.addToScene(ref Game);
            dir = new CDir(5, 1);

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 60);
          
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dir.update(map);
            pakman.move(dir.x, dir.y, map);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Visible;
            
            Timer.Start();
        }

        private void Start_MouseEnter(object sender, MouseEventArgs e)
        {
            

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) dir.left();
        }

    }

   
}
