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
using System.Windows.Threading;
using System.IO;

namespace kurs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Winner win = new Winner();
        GO go = new GO();

        MediaPlayer player = new MediaPlayer();
        public Ellipse[,] map2;

        //Исправить анимацию при преследовании (уменьшить скорость привидения). Подключить рандомное движение. Починить появление окон. Размножить привидений. 

        public int[,] map = {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,2,3,2,1,2,2,2,1,1,2,2,2,2,3,1},
                {1,2,1,2,2,2,1,2,1,1,2,1,2,1,2,1},
                {1,2,1,2,1,1,1,2,1,1,2,1,2,1,2,1},
                {1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,1},
                {1,2,1,1,1,1,2,1,1,1,1,1,2,1,2,1},
                {1,2,1,2,2,2,2,2,2,1,2,2,2,1,2,1},
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

        public class SPoint
        {
            public int X;
            public int Y;

            public SPoint()
            {
                X = 0;
                Y = 0;
            }

            public static SPoint operator+ (SPoint a, SPoint b)
            {
                SPoint r = new SPoint();
                r.X = a.X + b.X;
                r.Y = a.Y + b.Y;
                return r;
            }
        }

        public class CChar
        {
            public SPoint mpos = new SPoint(); //position in map
            public SPoint spos = new SPoint(); //position on screen
            public SPoint dir = new SPoint();  //movement direction

            public int h, w;
            public int s;
            public bool turn = false;

            Ellipse pm;
            ImageBrush ib = new ImageBrush();
            int currentFrame;
            int currentRow;
                       
            public CChar(int x, int y)
            {
                mpos.X = x;
                mpos.Y = y;

                w = h = 45;
                

                spos.X = x * w;
                spos.Y = y * h;

                pm = new Ellipse();
                pm.Width = 45;
                pm.Height = 45;

                ib.AlignmentX = AlignmentX.Left;
                ib.AlignmentY = AlignmentY.Top;
                ib.Stretch = Stretch.None;

                try
                {
                    ib.Viewbox = new Rect(10, 10, 0, 0);
                ib.ViewboxUnits = BrushMappingMode.Absolute;
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Try again");
                }
                currentFrame = 0;

                    ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/image/pm (1).png", UriKind.Absolute));

                pm.Fill = ib;

                pm.RenderTransform = new TranslateTransform(spos.X, spos.Y);
               
            }

            public void up()
            {
                if (dir.X != 0 && dir.Y != -1) turn = true;

                dir.X = 0;
                dir.Y = -1;
            }

            public void down()
            {
                if (dir.X != 0 && dir.Y != 1) turn = true;

                dir.X = 0;
                dir.Y = 1;
            }

            public void left()
            {
                if (dir.X != -1 && dir.Y != 0) turn = true;

                dir.X = -1;
                dir.Y = 0;
            }

            public void right()
            {
                if (dir.X != 1 && dir.Y != 0) turn = true;

                dir.X = 1;
                dir.Y = 0;
            }

            void step(int[,] map, Ellipse[,] map2, ref Canvas scene, bool bonus)
            {
                if ((map[mpos.X + dir.X, mpos.Y + dir.Y] == 2) || (map[mpos.X + dir.X, mpos.Y + dir.Y] == 0) || (map[mpos.X + dir.X, mpos.Y + dir.Y] == 3))
                {
                    if (map[mpos.X, mpos.Y] == 0) bonus = false;

                        if ((map[mpos.X, mpos.Y] == 2))
                    {
                        s += 10;
                        map[mpos.X, mpos.Y] = 0;
                        scene.Children.Remove(map2[spos.X/45, spos.Y/45]);
                        bonus = false;
                    }
                    if (map[mpos.X, mpos.Y] == 3)
                    {
                        s += 20;
                        bonus = true;
                        map[mpos.X, mpos.Y] = 0;
                        scene.Children.Remove(map2[spos.X / 45, spos.Y / 45]);
                    }
                    mpos = mpos + dir;
                }
                else turn = false;
            }

            public void animate(int n, int nx, int ny)
            {
                var frameCount = 8;
                var frameW = 65;
                var frameH = 65;

                    currentFrame = (currentFrame + 1 + frameCount) % frameCount;

                currentRow = n;

                var frameLeft = currentFrame * frameW;
                    var frameTop = currentRow * frameH;
                    try
                    {
                        (pm.Fill as ImageBrush).Viewbox = new Rect(frameLeft+nx, frameTop + ny, frameLeft + frameW, frameTop + frameH);
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("Try again");
                    }

            }

            public void updatePos(int[,] map, Ellipse[,] map2, ref Canvas scene, bool bonus)
            {
                if ((mpos.X * w == spos.X) && (mpos.Y * h == spos.Y))
                {
                    step(map, map2, ref scene, bonus);
                    
                }

                if (spos.X < mpos.X * w) { spos.X += 1; animate(0, 10, 10); }
                if (spos.X > mpos.X * w) { spos.X -= 1; animate(1, 0, 10); }
                if (spos.Y < mpos.Y * w) { spos.Y += 1; animate(3, 10, 20); }
                if (spos.Y > mpos.Y * w) { spos.Y -= 1; animate(2, 10, 10); }


                pm.RenderTransform = new TranslateTransform(spos.X, spos.Y);
            }

            public void addToScene(ref Canvas scene)
            {
                scene.Children.Add(pm);
            }
        }

        public class Enemy
        {
            public SPoint mpos = new SPoint();
            public SPoint spos = new SPoint();
            public SPoint dir = new SPoint();

            int h, w;
            int d = 1;

            public bool turn = false;

            public Rectangle ghost1;
            ImageBrush ib1 = new ImageBrush();

            int currentFrame;
            int currentRow;

            public bool isChasing = false;

            public List<SPoint> waypoints = new List<SPoint>();
                       
            public Enemy(int x, int y, string iname)
            {
                mpos.X = x;
                mpos.Y = y;

                w = h = 45;

                spos.X = x * w;
                spos.Y = y * h;

                ghost1 = new Rectangle();

                ghost1.Width = 45;
                ghost1.Height = 50;

                ib1.AlignmentX = AlignmentX.Left;
                ib1.AlignmentY = AlignmentY.Top;
                ib1.Stretch = Stretch.None;


                ib1.Viewbox = new Rect(0, 0, 0, 0);
                ib1.ViewboxUnits = BrushMappingMode.Absolute;

                currentFrame = 0;

                    ib1.ImageSource = new BitmapImage(new Uri(iname, UriKind.Absolute));
              
                    ghost1.Fill = ib1;


                ghost1.RenderTransform = new TranslateTransform(spos.X, spos.Y);

            }

            public bool iSeeYou(SPoint ppos, int[,] map)
            {
                if ((ppos.X == mpos.X))// || (pakman.x == gh.x))
                {
                    for (int i = Math.Min(ppos.Y, mpos.Y); i < Math.Max(ppos.Y, mpos.Y); i++)
                    {
                        if (map[ppos.X, i] == 1) return false;
                    }
                    return true;
                }
                if ((ppos.Y == mpos.Y))// || (pakman.x == gh.x))
                {
                    for (int i = Math.Min(ppos.X, mpos.X); i < Math.Max(ppos.X, mpos.X); i++)
                    {
                        if (map[i, ppos.Y] == 1) return false;
                    }
                    return true;
                }
                return false;
            }

            public void bonusmode(int x, int y)
            {
                mpos.X = x;
                mpos.Y = y;

                if (spos.X > mpos.X * w)
                {
                    spos.X -= 1;
                }
                if (spos.X < mpos.X * w)
                {
                    spos.X += 1;
                }
                if (spos.Y > mpos.Y * h)
                {
                    spos.Y -= 1;
                }
                if (spos.Y < mpos.Y * h)
                {
                    spos.Y += 1;
                }

                ghost1.RenderTransform = new TranslateTransform(spos.X, spos.Y);

                if ((spos.X == mpos.X * w) && (spos.Y == mpos.Y * h)) return;
            }
            

            public void randomdir()
            {
                int s = new Random().Next(0, 4);

                while (((d == 0) && (s == 1)) || ((d == 2) && (s == 3)) || ((d == 1) && (s == 0)) || ((d == 3) && (s == 2)))
                {
                    s = new Random().Next(1000);
                    s %= 4;
                }

                if (s == 0)
                {
                    if (dir.X != 0 && dir.Y != -1) turn = true;

                    dir.X = 0;
                    dir.Y = -1;

                    d = s;

                }
                if (s == 1)
                {
                    if (dir.X != 0 && dir.Y != 1) turn = true;

                    dir.X = 0;
                    dir.Y = 1;

                    d = s;

                }
                if (s == 2)
                {
                    if (dir.X != -1 && dir.Y != 0) turn = true;

                    dir.X = -1;
                    dir.Y = 0;

                    d = s;
                }
                if (s == 3)
                {
                    if (dir.X != 1 && dir.Y != 0) turn = true;

                    dir.X = 1;
                    dir.Y = 0;

                    d = s;
                }
            }        

            void step(int[,] map)
            {
                if ((map[mpos.X + dir.X, mpos.Y + dir.Y] == 2) || (map[mpos.X + dir.X, mpos.Y + dir.Y] == 0) || (map[mpos.X + dir.X, mpos.Y + dir.Y] == 3))
                {
                    mpos += dir;                   
                }
                else turn = false;
            }

            public void animate(int n)
            {
                var frameCount = 3;
                var frameW = 55;
                var frameH = 55;

                currentFrame = (currentFrame + 1 + frameCount) % frameCount;

                currentRow = n;

                var frameLeft = currentFrame * frameW;
                var frameTop = currentRow * frameH;
                try
                {
                    (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Try again");
                }

            }

            public void updatePos(int[,] map, Ellipse[,] map2, ref Canvas scene)
            {
                if ((mpos.X * w == spos.X) && (mpos.Y * h == spos.Y))
                {
                    step(map);
                }

                if (spos.X < mpos.X * w) { spos.X += 1; animate(0); }
                if (spos.X > mpos.X * w) { spos.X -= 1; animate(1); }
                if (spos.Y < mpos.Y * w) { spos.Y += 1; animate(3); }
                if (spos.Y > mpos.Y * w) { spos.Y -= 1; animate(2); }


                ghost1.RenderTransform = new TranslateTransform(spos.X, spos.Y);
            }


            public void addToScene(ref Canvas scene)
            {
                scene.Children.Add(ghost1);
            }

            public void chase()
            {
                //dx -> spos.X
                //dy -> spos.Y
                //x -> mpos.X
                //y -> mpos.Y

                if (waypoints.Count > 0)
                {
                    w = h = 45;

                    ib1.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/image/gh_r (1).png", UriKind.Absolute));

                    ghost1.Fill = ib1;

                    if ((mpos.X * w == spos.X) && (mpos.Y * h == spos.Y))
                    {
                        if (mpos.X == waypoints[0].X && mpos.Y == waypoints[0].Y)
                            waypoints.RemoveAt(0);

                        if (waypoints.Count == 0) return;

                        int dX = (waypoints[0].X - mpos.X);

                        if (dX != 0)
                            dX /= Math.Abs(dX);

                        int dY = (waypoints[0].Y - mpos.Y);

                        if (dY != 0)
                            dY /= Math.Abs(dY);

                        mpos.X += dX;
                        mpos.Y += dY;
                    }


                    var frameCount = 3;
                    var frameW = 55;
                    var frameH = 55;

                    if (spos.X > mpos.X * w)
                    {
                        spos.X -= 1;
                        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
                        currentRow = 2;

                        var frameLeft = currentFrame * frameW;
                        var frameTop = currentRow * frameH;
                        try
                        {
                            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
                        }
                        catch (ArgumentException)
                        {
                            MessageBox.Show("Try again");
                        }
                    }
                    if (spos.X < mpos.X * w)
                    {
                        spos.X += 1;
                        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
                        currentRow = 1;

                        var frameLeft = currentFrame * frameW;
                        var frameTop = currentRow * frameH;
                        try
                        {
                            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
                        }
                        catch (ArgumentException)
                        {
                            MessageBox.Show("Try again");
                        }
                    }

                    //px = x * w;
                    if (spos.Y > mpos.Y * h)
                    {
                        spos.Y -= 1;
                        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
                        currentRow = 0;

                        var frameLeft = currentFrame * frameW;
                        var frameTop = currentRow * frameH;
                        (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
                    }

                    if (spos.Y < mpos.Y * h)
                    {
                        spos.Y += 1;
                        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
                        currentRow = 3;

                        var frameLeft = currentFrame * frameW;
                        var frameTop = currentRow * frameH;
                        try
                        {
                            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
                        }
                        catch (ArgumentException)
                        {
                            MessageBox.Show("Try again");
                        }
                    }

                    ghost1.RenderTransform = new TranslateTransform(spos.X, spos.Y);



                } else
                {
                    isChasing = false;
                }
            }            
        }


        CChar pakman;
        Enemy gh;
        Enemy gh2;

        int score;
        int i = 0;
        int k = 141;
        bool bonus;
        System.Windows.Threading.DispatcherTimer pmTimer;
       

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

        //    void delpos(SPoint x, SPoint y, ref Canvas scene)
        //{
        //    int x1 = int.Parse(SPoint x);
        //    scene.Children.Remove();
        //}

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
                            map2[i, j].RenderTransform = new TranslateTransform(i * 45 + 11, j * 45 + 10);
                            scene.Children.Add(map2[i, j]);
                        }
                    
                    }
                    else map2[i, j] = null;
                }
                
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            player.Open(new Uri("C:\\Users\\Mvideo\\Desktop\\kurs-master\\kurs-master\\kurs\\kurs\\sounds\\8-Bit Universe - Our House (8-Bit Version) (8-Bit Version).mp3", UriKind.Relative));
            player.Play();
            player.Volume = 50.0 / 100.0;


            using (StreamReader outputFile = new StreamReader(@"C:\Users\Mvideo\Desktop\kursPM\kursPM\score.txt"))
            {
                string line;
                while ((line = outputFile.ReadLine()) != null)
                {
                    best.Content = line;
                }
            }

            pakman = new CChar(1, 1);
            pakman.up();
            gh = new Enemy(12, 8, @"pack://application:,,,/image/gh_r (1).png");
            gh2 = new Enemy(13, 7, @"pack://application:,,,/image/gh_b (1).png");

            pmTimer = new System.Windows.Threading.DispatcherTimer();
            pmTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            pmTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);



            fillmap2(map, ref Game);
            pakman.addToScene(ref Game);
            gh.addToScene(ref Game);
            gh2.addToScene(ref Game);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            pakman.updatePos(map, map2, ref Game, bonus);    
            score = pakman.s;
            l1.Content = bonus;
            
            if (bonus == true)
            {
                while (i < 600)
                {
                    gh.bonusmode(1,1);
                    gh2.bonusmode(15,19);
                    i += 1;
                }
                i = 0;
            }
            else
            {
                gh.updatePos(map, map2, ref Game);
                gh2.updatePos(map, map2, ref Game);
            }

           
            if (pakman.turn == true)
            {
               
                if (gh.isChasing == true)
                    gh.waypoints.Add(pakman.mpos);
                if (gh2.isChasing == true)
                    gh2.waypoints.Add(pakman.mpos);
                pakman.turn = false;
            }

        //    l1.Content = gh.iSeeYou(pakman.mpos, map);

           

            if (gh.iSeeYou(pakman.mpos, map) == true)
            {
                gh.isChasing = true;
                gh.waypoints.Clear();
                gh.waypoints.Add(pakman.mpos);
            }
            else
            {
                gh.randomdir();                              
            }

            if (gh.waypoints.Count > 2)
            {
                gh.isChasing = false;
                gh.waypoints.Clear();
            }

            if (gh2.iSeeYou(pakman.mpos, map) == true)
            {
                gh2.isChasing = true;
                gh2.waypoints.Clear();
                gh2.waypoints.Add(pakman.mpos);
            }
            else
            {
                gh2.randomdir();                              
            }

            if (gh2.waypoints.Count > 2)
            {
                gh2.isChasing = false;
                gh2.waypoints.Clear();
            }

         //   l1.Content = gh.waypoints.Count;

            if (gh2.isChasing == true)
            {
                gh2.chase();

            }

            if (gh.isChasing == true)
            {
               gh.chase();
            }

            if (gh.mpos==pakman.mpos)
            {
                if (go.ShowDialog() == true)
                {
                    pmTimer.Stop();
                    player.Stop();
                }
            }
            if (gh2.mpos == pakman.mpos)
            {
                if (go.ShowDialog() == true)
                {
                    pmTimer.Stop();
                    player.Stop();
                }
            }

            sc.Content = score;
            
        }

     

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            menu.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Visible;
            player.Open(new Uri("C:\\Users\\Mvideo\\Desktop\\kurs-master\\kurs-master\\kurs\\kurs\\sounds\\Body.mp3", UriKind.Relative));

            pmTimer.Start();
           
        }

        private void Start_MouseEnter(object sender, MouseEventArgs e)
        {
            

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left) pakman.left();
            if (e.Key == Key.Right) pakman.right();
            if (e.Key == Key.Down) pakman.down();
            if (e.Key == Key.Up) pakman.up();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ex1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void menu1_Click(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Visible;
            creat.Visibility = Visibility.Hidden;
        }

        private void cr_Click(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Hidden;
            creat.Visibility = Visibility.Visible;
        }
    }

   
}
