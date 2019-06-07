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

            public bool move(int x, int y, int[,] map)
            {
                if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) return true;

                if (map[x, y] == 1) return true;

                this.x = x;
                this.y = y;

                if (px > x * w)
                    px-=1;
                if (px < x * w)
                    px+=1;
                //px = x * w;
                if (py > y * h)
                    py-=1;
                if (py < y * h)
                    py+=1;


                //py = y * h;

                pm.RenderTransform = new TranslateTransform(px, py);

                if ((px == x * w) && (py == y * h)) return true;
                else return false;

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
                dy = -1;
                dx = 0;
            }

            public void down()
            {
                dy = 1;
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

            pakman = new CChar(1, 1);

            dir = new CDir(1, 1);

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
          
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Rectangle r = new Rectangle();

                    r.Stroke = Brushes.YellowGreen;

                    if (map[i,j] == 2 || map[i, j] == 0)
                        r.Fill = Brushes.Azure;
                    if (map[i, j] == 1)
                        r.Fill = Brushes.Red;


                    r.StrokeThickness = 2;

                    r.Width = 45;
                    r.Height = 45;

                    r.RenderTransform = new TranslateTransform(i * 45, j * 45);
                    Game.Children.Add(r);
                }

            pakman.addToScene(ref Game);

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            
            if (pakman.move(dir.x, dir.y, map) == true)
                dir.update(map);
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
            //else
            if (e.Key == Key.Right) dir.right();
            //else
            if (e.Key == Key.Down) dir.down();
            //else
            if (e.Key == Key.Up) dir.up();
            
        }

    }

   
}
