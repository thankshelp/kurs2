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
        public Ellipse[,] map2;

        public int[,] map = {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,2,3,2,1,2,2,2,1,1,2,2,2,2,3,1},
                {1,2,1,2,2,2,1,2,1,1,2,1,2,1,2,1},
                {1,2,1,2,1,1,1,2,1,1,2,1,2,1,2,1},
                {1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,1},
                {1,2,1,1,1,1,2,1,1,1,1,1,2,1,2,1},
                {1,2,1,2,2,2,3,2,2,1,2,2,2,1,2,1},
                {1,2,1,2,1,1,1,1,2,1,2,1,2,1,2,1},
                {1,2,2,2,1,0,0,1,2,2,2,1,2,2,2,1},
                {1,1,1,0,1,0,0,0,2,1,1,1,2,1,1,1},
                {1,1,1,2,1,0,0,0,2,1,1,1,2,1,1,1},
                {1,2,2,2,1,0,0,1,2,2,2,1,2,2,2,1},
                {1,2,1,2,1,1,1,1,2,1,2,1,2,1,2,1},
                {1,2,1,2,2,2,3,2,2,1,2,2,2,1,2,1},
                {1,2,1,1,1,1,2,1,1,1,1,1,2,1,2,1},
                {1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,1},
                {1,2,1,2,1,1,1,2,1,1,2,1,2,1,2,1},
                {1,2,1,2,2,2,1,2,1,1,2,1,2,1,2,1},
                {1,2,3,2,1,2,2,2,1,1,2,2,2,2,3,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        }; 

    

            public class CChar
             {
            int x, y;
            int px, py;
            Ellipse r;

            int h, w;
            Ellipse pm;
            ImageBrush ib = new ImageBrush();
            int currentFrame;
            int currentRow;



            public CChar(int x, int y)
            {
                this.x = x;
                this.y = y;

                w = h = 45;

                px = x * w;
                py = y * h;

                pm = new Ellipse();

                //pm.Stroke = Brushes.YellowGreen;
                //pm.Fill = Brushes.Yellow;
                //pm.StrokeThickness = 2;

                pm.Width = 45;
                pm.Height = 45;

                ib.AlignmentX = AlignmentX.Left;
                ib.AlignmentY = AlignmentY.Top;
                ib.Stretch = Stretch.None;

                ib.Viewbox = new Rect(10, 10, 0, 0);
                ib.ViewboxUnits = BrushMappingMode.Absolute;

                currentFrame = 0;


                ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/image/pm (1).png", UriKind.Absolute));

                pm.Fill = ib;

                pm.RenderTransform = new TranslateTransform(px, py);
                //pm.Margin = new Thickness(px, py, 0, 0);
            }

            public bool move(int x, int y, Ellipse[,] map2, ref Canvas scene)
            {
                //if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) return true;

                //if (map[x, y] == 1) return true;


                this.x = x;
                this.y = y;

                var frameCount = 8;
                var frameW = 65;
                var frameH = 65;



                if (px > x * w)
                {
                    px -= 1;
                    currentFrame = (currentFrame + 1 + frameCount) % frameCount;
                    currentRow = 1;

                    var frameLeft = currentFrame * frameW;
                    var frameTop = currentRow * frameH;
                    (pm.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop + 10, frameLeft + frameW, frameTop + frameH);
                }
                if (px < x * w)
                {
                    px += 1;
                    currentFrame = (currentFrame + 1 + frameCount) % frameCount;
                    currentRow = 0;

                    var frameLeft = currentFrame * frameW;
                    var frameTop = currentRow * frameH;
                    (pm.Fill as ImageBrush).Viewbox = new Rect(frameLeft + 10, frameTop + 10, frameLeft + frameW, frameTop + frameH);
                }

                //px = x * w;
                if (py > y * h)
                {
                    py -= 1;
                    currentFrame = (currentFrame + 1 + frameCount) % frameCount;
                    currentRow = 2;

                    var frameLeft = currentFrame * frameW;
                    var frameTop = currentRow * frameH;
                    (pm.Fill as ImageBrush).Viewbox = new Rect(frameLeft + 10, frameTop + 10, frameLeft + frameW, frameTop + frameH);
                }

                if (py < y * h)
                {
                    py += 1;
                    currentFrame = (currentFrame + 1 + frameCount) % frameCount;
                    currentRow = 3;

                    var frameLeft = currentFrame * frameW;
                    var frameTop = currentRow * frameH;
                    (pm.Fill as ImageBrush).Viewbox = new Rect(frameLeft + 10, frameTop + 20, frameLeft + frameW, frameTop + frameH);
                }

                //py = y * h;

                pm.RenderTransform = new TranslateTransform(px, py);

                if ((px == x * w) && (py == y * h)) {
                    
                    scene.Children.Remove(map2[x,y]);
                    return true; }
                else return false;

            }
          

                 public void addToScene(ref Canvas scene)
                 {
                      scene.Children.Add(pm);
                 }
            }

        public class Enemy
        {
            int x, y;
            int px, py;
            int dx, dy;

            int h, w;
            Rectangle ghost1, ghost2, ghost3;
            ImageBrush ib1 = new ImageBrush();
            ImageBrush ib2 = new ImageBrush();
            ImageBrush ib3 = new ImageBrush();
            int currentFrame;
            int currentRow;

            public Enemy(int x, int y)
            {
                this.x = x;
                this.y = y;

                w = h = 45;

                dx = x * w;
                dy = y * h;

                ghost1 = new Rectangle();
                ghost2 = new Rectangle();
                ghost3 = new Rectangle();

                //ghost1.Stroke = Brushes.Purple;
                //ghost1.Fill = Brushes.Red;
                //ghost2.Stroke = Brushes.Purple;
                //ghost2.Fill = Brushes.Blue;
                //ghost3.Stroke = Brushes.Purple;
                //ghost3.Fill = Brushes.Green;
                ////pm.StrokeThickness = 2;
                //ghost1.HorizontalAlignment = HorizontalAlignment.Left;
                //ghost1.VerticalAlignment = VerticalAlignment.Center;
                //ghost2.HorizontalAlignment = HorizontalAlignment.Left;
                //ghost2.VerticalAlignment = VerticalAlignment.Center;
                //ghost3.HorizontalAlignment = HorizontalAlignment.Left;
                //ghost3.VerticalAlignment = VerticalAlignment.Center;

                ghost1.Width = 45;
                ghost1.Height = 45;
                ghost2.Width = 45;
                ghost2.Height = 45;
                ghost3.Width = 45;
                ghost3.Height = 45;

                ib1.AlignmentX = AlignmentX.Left;
                ib1.AlignmentY = AlignmentY.Top;
                ib1.Stretch = Stretch.None;
                ib2.AlignmentX = AlignmentX.Left;
                ib2.AlignmentY = AlignmentY.Top;
                ib2.Stretch = Stretch.None;
                ib3.AlignmentX = AlignmentX.Left;
                ib3.AlignmentY = AlignmentY.Top;
                ib3.Stretch = Stretch.None;


                ib1.Viewbox = new Rect(-2, -1, 0,0 );
                ib1.ViewboxUnits = BrushMappingMode.Absolute;
                ib2.Viewbox = new Rect(0, 0, 0, 0);
                ib2.ViewboxUnits = BrushMappingMode.Absolute;
                ib3.Viewbox = new Rect(0, 0, 0, 0);
                ib3.ViewboxUnits = BrushMappingMode.Absolute;

                currentFrame = 0;


                ib2.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/image/gh_b копия.png", UriKind.Absolute));
                ib1.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/image/gh копия.png", UriKind.Absolute));
                ib3.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/image/gh_g.png", UriKind.Absolute));

                ghost1.Fill = ib1;
                ghost2.Fill = ib2;
                ghost3.Fill = ib3;

                ghost1.RenderTransform = new TranslateTransform(px, py);
                ghost2.RenderTransform = new TranslateTransform(px + 45, py);
                ghost3.RenderTransform = new TranslateTransform(px + 90, py);


            }

        
                public bool move(int x, int y, int px, int py)
                {
                    this.x = x;
                    this.y = y;
                if ((-5 <= (px-dx) && (px-dx) <= 0) || ((px-dx <=5) && (px-dx) >= 0))
                {
                    //приследование
                }
                else
                {
                    //рандом
                }

                if (dx > x * w)
                {
                    dx -= 1;
                   
                }
                if (dx < x * w)
                {
                    dx += 1;
                   
                }

                //px = x * w;
                if (dy > y * h)
                {
                    dy -= 1;
                   
                }

                if (dy < y * h)
                {
                    dy += 1;
                  
                }

                //py = y * h;

                ghost1.RenderTransform = new TranslateTransform(dx, dy);

                if ((dx == x * w) && (dy == y * h))
               
                    return true;
                
                else return false;

            }

            public void addToScene(ref Canvas scene)
            {
                scene.Children.Add(ghost1);
                scene.Children.Add(ghost2);
                scene.Children.Add(ghost3);
            }
        }

        public class GDir
        {
            public int x, y;
            int dx, dy;

            public GDir(int x,int y)
            {
                this.x = x;
                this.y = y;
                dx = 1;
                dy = 1;
            }

            public void updateg(int[,] map)
            {
                if ((x + dx < 0) || (x + dx >= map.GetLength(0)) || (y + dy < 0) || (y + dy > map.GetLength(1))) return;

                if ((map[x + dx, y + dy] == 2) || (map[x + dx, y + dy] == 0) || (map[x + dx, y + dy] == 3))
                {
                    x = x + dx;
                    y = y + dy;

                }
                else
                {
                    int s = new Random().Next(0, 3);
                    if (s == 0)
                    {
                        up();
                    }
                    if (s == 1)
                    {
                        down();
                    }
                    if(s == 2)
                    {
                        left();
                    }
                    if (s == 3)
                    {
                        right();
                    }
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

                if ((map[x + dx, y + dy] == 2) || (map[x + dx, y + dy] == 0) || (map[x + dx, y + dy] == 3))
                {
                    x = x + dx;
                    y = y + dy;
                    if ((map[x, y] == 2) || (map[x, y] == 3))
                    {
                        map[x, y] = 0;
                    }
                   
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
        Enemy gh;
        CDir dir;
        GDir gir;
        int score;
        System.Windows.Threading.DispatcherTimer Timer;

        //16x20

        //{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        //{ 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
        //{ 1, 2, 1, 1, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 2, 1, 1, 2, 1 },
        //{ 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
        //{ 1, 2, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 2, 1 },
        //{ 1, 2, 2, 2, 2, 1, 2, 2, 2, 1, 1, 2, 2, 2, 1, 2, 2, 2, 2, 1 },
        //{ 1, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 1 },
        //{ 1, 1, 1, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 1, 1, 1 },
        //{ 1, 2, 2, 2, 2, 1, 2, 1, 1, 0, 0, 1, 1, 2, 1, 2, 2, 2, 2, 1 },
        //{ 1, 2, 1, 1, 2, 2, 2, 1, 0, 0, 0, 0, 1, 2, 2, 2, 1, 1, 2, 1 },
        //{ 1, 2, 2, 1, 2, 1, 2, 1, 0, 0, 0, 0, 1, 2, 1, 2, 1, 2, 2, 1 },
        //{ 1, 1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1 },
        //{ 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1 },
        //{ 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1 },
        //{ 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
        //{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } }; 



        void fillmap2(int[,] map, ref Canvas scene)
        {
            map2 = new Ellipse[20, 16];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 2 || map[i, j] == 3)
                    {

                        if (map[i, j] == 2)
                        {
                            map2[i, j] = new Ellipse();
                            map2[i, j].Fill = Brushes.Azure;
                            map2[i, j].Width = 10;
                            map2[i, j].Height = 10;
                            map2[i, j].StrokeThickness = 2;
                            map2[i, j].RenderTransform = new TranslateTransform(i * 45 + 15, j * 45 + 15);
                            scene.Children.Add(map2[i, j]);
                        }
                        
                        if (map[i, j] == 3)
                        {
                            map2[i, j] = new Ellipse();
                            map2[i, j].Fill = Brushes.Azure;
                            map2[i, j].Width = 20;
                            map2[i, j].Height = 20;
                            map2[i, j].StrokeThickness = 2;
                            map2[i, j].RenderTransform = new TranslateTransform(i * 45 + 15, j * 45 + 15);
                            scene.Children.Add(map2[i, j]);
                        }
                       // else map2[i, j] = null;
                    }
                    else map2[i, j] = null;
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();

            pakman = new CChar(9, 3);

            gh = new Enemy(8, 5);

            gir = new GDir(8, 5);
            dir = new CDir(9, 3);

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);



            fillmap2(map, ref Game);
            pakman.addToScene(ref Game);
            gh.addToScene(ref Game);
         

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            
            if (pakman.move(dir.x, dir.y, map2, ref Game) == true)
            {
                dir.update(map);
            }
            if (gh.move(gir.x,gir.y) == true)
            {
                gir.updateg(map);
            }
           
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
