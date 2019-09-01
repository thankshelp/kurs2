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

                //pm.Stroke = Brushes.YellowGreen;
                //pm.Fill = Brushes.Yellow;
                //pm.StrokeThickness = 2;

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
                //pm.Margin = new Thickness(px, py, 0, 0);
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

            void step(int[,] map, Ellipse[,] map2, ref Canvas scene)
            {
                if ((map[mpos.X + dir.X, mpos.Y + dir.Y] == 2) || (map[mpos.X + dir.X, mpos.Y + dir.Y] == 0) || (map[mpos.X + dir.X, mpos.Y + dir.Y] == 3))
                {
                    mpos = mpos + dir;
                    if ((map[mpos.X, mpos.Y] == 2))
                    {
                        s += 10;
                        map[mpos.X, mpos.Y] = 0;
                        scene.Children.Remove(map2[spos.X/45, spos.Y/45]);
                    }
                    if (map[mpos.X, mpos.Y] == 3)
                    {
                        s += 20;
                        map[mpos.X, mpos.Y] = 0;
                        scene.Children.Remove(map2[spos.X / 45, spos.Y / 45]);
                    }
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

            public void updatePos(int[,] map, Ellipse[,] map2, ref Canvas scene)
            {
                if ((mpos.X * w == spos.X) && (mpos.Y * h == spos.Y))
                {
                    step(map, map2, ref scene);
                    
                }

                if (spos.X < mpos.X * w) { spos.X += 1; animate(0, 10, 10); }
                if (spos.X > mpos.X * w) { spos.X -= 1; animate(1, 0, 10); }
                if (spos.Y < mpos.Y * w) { spos.Y += 1; animate(3, 10, 20); }
                if (spos.Y > mpos.Y * w) { spos.Y -= 1; animate(2, 10, 10); }


                pm.RenderTransform = new TranslateTransform(spos.X, spos.Y);

                
                //turn = false;
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

            //public void up()
            //{
            //    if (dir.X != 0 && dir.Y != -1) turn = true;

            //    dir.X = 0;
            //    dir.Y = -1;
            //}

            //public void down()
            //{
            //    if (dir.X != 0 && dir.Y != 1) turn = true;

            //    dir.X = 0;
            //    dir.Y = 1;
            //}

            //public void left()
            //{
            //    if (dir.X != -1 && dir.Y != 0) turn = true;

            //    dir.X = -1;
            //    dir.Y = 0;
            //}

            //public void right()
            //{
            //    if (dir.X != 1 && dir.Y != 0) turn = true;

            //    dir.X = 1;
            //    dir.Y = 0;
            //}

            public void randomdir()
            {
                int s = new Random().Next(0, 4);

                while (((d == 0) && (s == 1)) || ((d == 2) && (s == 3)) || ((d == 1) && (s == 0)) || ((d == 3) && (s == 2)))
                {
                    s = new Random().Next(100);
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

            public void animate(int n, int nx, int ny)
            {
                var frameCount = 8;
                var frameW = 45;
                var frameH = 50;

                currentFrame = (currentFrame + 1 + frameCount) % frameCount;

                currentRow = n;

                var frameLeft = currentFrame * frameW;
                var frameTop = currentRow * frameH;
                try
                {
                    (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft + nx, frameTop + ny, frameLeft + frameW, frameTop + frameH);
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

                if (spos.X < mpos.X * w) { spos.X += 1; animate(0, 10, 10); }
                if (spos.X > mpos.X * w) { spos.X -= 1; animate(1, 0, 10); }
                if (spos.Y < mpos.Y * w) { spos.Y += 1; animate(3, 10, 20); }
                if (spos.Y > mpos.Y * w) { spos.Y -= 1; animate(2, 10, 10); }


                ghost1.RenderTransform = new TranslateTransform(spos.X, spos.Y);


                //turn = false;
            }

            //public void bonusMode(int x, int y)
            //{
            //    this.x = x;
            //    this.y = y;

            //        ib1.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/image/gh.png", UriKind.Absolute));

            //        ghost1.Fill = ib1;

            //    var frameCount = 3;
            //    var frameW = 55;
            //    var frameH = 55;

            //    if (dx > x * w)
            //    {
            //        dx -= 1;
            //        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            //        currentRow = 2;

            //        var frameLeft = currentFrame * frameW;
            //        var frameTop = currentRow * frameH;
            //        try
            //        {
            //            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
            //        }
            //        catch (ArgumentException)
            //        {
            //            MessageBox.Show("Try again");
            //        }
            //    }
            //    if (dx < x * w)
            //    {
            //        dx += 1;
            //        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            //        currentRow = 1;

            //        var frameLeft = currentFrame * frameW;
            //        var frameTop = currentRow * frameH;
            //        (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
            //    }

            //    //px = x * w;
            //    if (dy > y * h)
            //    {
            //        dy -= 1;
            //        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            //        currentRow = 0;

            //        var frameLeft = currentFrame * frameW;
            //        var frameTop = currentRow * frameH;
            //        try
            //        {
            //            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
            //        }
            //        catch (ArgumentException)
            //        {
            //            MessageBox.Show("Try again");
            //        }
            //    }

            //    if (dy < y * h)
            //    {
            //        dy += 1;
            //        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            //        currentRow = 3;

            //        var frameLeft = currentFrame * frameW;
            //        var frameTop = currentRow * frameH;
            //        try
            //        {
            //            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
            //        }
            //        catch (ArgumentException)
            //        {
            //            MessageBox.Show("Try again");
            //        }                  
            //    }

            //    //py = y * h;

            //    ghost1.RenderTransform = new TranslateTransform(dx, dy);

            //    if ((dx == x * w) && (dy == y * h)) return;

            //}

            //public bool move(int x, int y)
            //{
            //    this.x = x;
            //    this.y = y;

            //        ib1.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/image/gh_r (1).png", UriKind.Absolute));

            //    ghost1.Fill = ib1;

            //    var frameCount = 3;
            //    var frameW = 55;
            //    var frameH = 55;

            //    if (dx > x * w)
            //    {
            //        dx -= 1;
            //        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            //        currentRow = 2;

            //        var frameLeft = currentFrame * frameW;
            //        var frameTop = currentRow * frameH;
            //        try
            //        {
            //            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop , frameLeft + frameW, frameTop + frameH);
            //        }
            //        catch (ArgumentException)
            //        {
            //            MessageBox.Show("Try again");
            //        }
            //    }
            //    if (dx < x * w)
            //    {
            //        dx += 1;
            //        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            //        currentRow = 1;

            //        var frameLeft = currentFrame * frameW;
            //        var frameTop = currentRow * frameH;
            //        try
            //        {
            //            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
            //        }
            //        catch (ArgumentException)
            //        {
            //            MessageBox.Show("Try again");
            //        }
            //    }

            //    //px = x * w;
            //    if (dy > y * h)
            //    {
            //        dy -= 1;
            //        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            //        currentRow = 0;

            //        var frameLeft = currentFrame * frameW;
            //        var frameTop = currentRow * frameH;
            //        (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
            //    }

            //    if (dy < y * h)
            //    {
            //        dy += 1;
            //        currentFrame = (currentFrame + 1 + frameCount) % frameCount;
            //        currentRow = 3;

            //        var frameLeft = currentFrame * frameW;
            //        var frameTop = currentRow * frameH;
            //        try
            //        {
            //            (ghost1.Fill as ImageBrush).Viewbox = new Rect(frameLeft, frameTop, frameLeft + frameW, frameTop + frameH);
            //        }
            //        catch (ArgumentException)
            //        {
            //            MessageBox.Show("Try again");
            //        }
            //    }

            //    //py = y * h;

            //    ghost1.RenderTransform = new TranslateTransform(dx, dy);

            //    if ((dx == x * w) && (dy == y * h))               
            //        return true;

            //    else return false;

            //}  //GHOST MOVE

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


        //public void randomdir()
        //{
        //    int s = new Random().Next(0, 4);

        //    while (((d == 0) && (s == 1)) || ((d == 2) && (s == 3)) || ((d == 1) && (s == 0)) || ((d == 3) && (s == 2)))
        //    {
        //        s = new Random().Next(100);
        //        s %= 4;
        //    }

        //    if (s == 0)
        //    {
        //        up();
        //        d = s;

        //    }
        //    if (s == 1)
        //    {
        //        down();
        //        d = s;

        //    }
        //    if (s == 2)
        //    {
        //        left();
        //        d = s;
        //    }
        //    if (s == 3)
        //    {
        //        right();
        //        d = s;
        //    }
        //}

        //public class CDir
        //{
        //    public int x, y, gx, gy;
        //    public int ox, oy;
        //    public int dx, dy;
        //    public int odx, ody;

        //    public CDir(int x, int y)
        //    {
        //        this.x = this.ox = x;
        //        this.y = this.oy = y;
        //        this.gx = gx;
        //        this.gy = gy;
        //        dx = odx = 1; 
        //        dy = ody = 1;
        //    }

        //    public int k(int[,] map, int k)
        //    {
        //            if ((map[x, y] == 2) || (map[x, y] == 3))
        //            {
        //                k--;
        //            }                
        //            return k;                
        //    }

        

        //    public void Update(int[,] map, DispatcherTimer Deathtimer, DispatcherTimer ghTimer)
        //    {
        //        if ((x + dx < 0) || (x + dx >= map.GetLength(0)) || (y + dy < 0) || (y + dy > map.GetLength(1))) return;
        //           // if ((ox != x)||(oy != y))
        //            {
        //                ox = x;
        //                oy = y;
        //            }

        //        //   if ((dx != 0) || (dy != -1))
        //        {

        //        }

        //        if ((map[x + dx, y + dy] == 2) || (map[x + dx, y + dy] == 0) || (map[x + dx, y + dy] == 3))
        //        {


        //            x = x + dx;
        //            y = y + dy;



        //            if ((map[x, y] == 2))
        //            {
        //                map[x, y] = 0;

        //            }
        //            if (map[x, y] == 3)
        //            {
        //                map[x, y] = 0;
        //                ghTimer.Stop();
        //                Deathtimer.Start();
        //            }
        //        }
        //    }

        //    public void up()
        //    {
        //        dy = -1;
        //        dx = 0;
        //    }

        //    public void down()
        //    {

        //        dy = 1;
        //        dx = 0;
        //    }

        //    public void left()
        //    {

        //        dx = -1;
        //        dy = 0;
        //    }

        //    public void right()
        //    {
        //        dx = 1;
        //        dy = 0;
        //    }
        //}

        CChar pakman;
        Enemy gh;
        Enemy gh2;

        int score;
        int i = 0;
        int k = 141;
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
                       // else map2[i, j] = null;
                    }
                    else map2[i, j] = null;
                }
                
            }
        }

        //public bool meet(int[,] map,ref Canvas scene)
        //{
        //    if ((pakman.x == gh.x) && (pakman.y == gh.y))
        //    {
        //            return true;               
        //    }
        //    else return false;
        //}


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

            //ghTimer = new System.Windows.Threading.DispatcherTimer();
            //ghTimer.Tick += new EventHandler(dispatcherGhostTimer_Tick);
            //ghTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            //DeathTimer = new System.Windows.Threading.DispatcherTimer();
            //DeathTimer.Tick += new EventHandler(dispatcherDeathTimer_Tick);
            //DeathTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            
            fillmap2(map, ref Game);
            pakman.addToScene(ref Game);
            gh.addToScene(ref Game);
            gh2.addToScene(ref Game);
            

            //meet(map, ref Game);
        }

        //private void dispatcherDeathTimer_Tick(object sender, EventArgs e)
        //{
        //    if (i != 600)
        //    {

        //        gh.bonusMode(1, 1);
        //        // gir.randomdir();
        //        i++;

        //        gh.isChasing = false;
        //        gh.waypoints.Clear();
        //        if (meet(map, ref Game) == true)
        //        {
        //            //scene.Children.Remove(ghost1);
        //            gh.move(55, 55);
        //            score += 50;

        //            DeathTimer.Stop();
        //            ghTimer.Start();
        //            i = 0;
        //        }
        //    }
        //    else
        //    {
        //        DeathTimer.Stop();
        //        ghTimer.Start();
        //        i = 0;
        //    }

        //}

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            pakman.updatePos(map, map2, ref Game);    
            score = pakman.s;
          
            gh.updatePos(map, map2, ref Game);
            gh2.updatePos(map, map2, ref Game);

            //l1.Content = pakman.turn;
            if (pakman.turn == true)
            {
                //MessageBox.Show("turn");
                if (gh.isChasing == true)
                    gh.waypoints.Add(pakman.mpos);
                if (gh2.isChasing == true)
                    gh2.waypoints.Add(pakman.mpos);
                pakman.turn = false;
            }

            l1.Content = gh.iSeeYou(pakman.mpos, map);

            //if (gh.waypoints.Count > 0)
            //{
            //    l1.Content += "\n" + gh.waypoints[0].X + " " + gh.waypoints[0].Y;
            //}
            //l1.Content += "\n" + pakman.mpos.X + " " + pakman.mpos.Y;
            //l1.Content += "\n" + gh.mpos.X + " " + gh.mpos.Y;

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

            l1.Content = gh.waypoints.Count;

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
                if (go.ShowDialog() == true) return;
            }
                
            sc.Content = score;

            
            //    if ( ((dir.x != dir.ox) || (dir.y != dir.oy)) && ((dir.dx != dir.odx) || (dir.dy != dir.ody)) )
            //    {
            //        //l1.Content = dir.x.ToString() + " " + dir.y.ToString() + " " + dir.ox.ToString() + " " + dir.oy.ToString() + "\n" + dir.dx.ToString() + " " + dir.dy.ToString() + " " + dir.odx.ToString() + " " + dir.ody.ToString();
            //        // MessageBox.Show(dir.x.ToString() + " " + dir.y.ToString() + " " + dir.ox.ToString() + " " + dir.oy.ToString() + "\n" + dir.dx.ToString() + " " + dir.dy.ToString() + " " + dir.odx.ToString() + " " + dir.ody.ToString());
            //        if (gh.isChasing == true)
            //            gh.waypoints.Add(new Point(dir.ox, dir.oy));
            //    }

            //if (pakman.move(dir.x, dir.y, map2, ref Game, player) == true)
            //{
            //   // l1.Content = dir.ox + " "  + dir.oy + " " + gir.x + " " + gir.y;
            //    //l1.Content += "\n" + gh.waypoints.Count;

            //    if (gh.iSeeYou(dir.ox, gh.x, dir.oy, gh.y, map) == true)
            //    {
            //        gh.isChasing = true;
            //        gh.waypoints.Clear();
            //        gh.waypoints.Add(new Point(dir.ox, dir.oy));
            //    }

            //    if ((pakman.px == dir.x * pakman.w +1) || (pakman.py == dir.y * pakman.h +1)|| (pakman.px == dir.x * pakman.w - 1) || (pakman.py == dir.y * pakman.h - 1))//без этого k начинает обнуляться даже если пакман стоит на месте
            //        k = dir.k(map, k);

            //    score = dir.score(map, score);

            //    dir.Update(map, DeathTimer, ghTimer);

            //    if ( ((dir.x != dir.ox) || (dir.y != dir.oy)) && ((dir.dx != dir.odx) || (dir.dy != dir.ody)) )
            //    {
            //        //l1.Content = dir.x.ToString() + " " + dir.y.ToString() + " " + dir.ox.ToString() + " " + dir.oy.ToString() + "\n" + dir.dx.ToString() + " " + dir.dy.ToString() + " " + dir.odx.ToString() + " " + dir.ody.ToString();
            //        // MessageBox.Show(dir.x.ToString() + " " + dir.y.ToString() + " " + dir.ox.ToString() + " " + dir.oy.ToString() + "\n" + dir.dx.ToString() + " " + dir.dy.ToString() + " " + dir.odx.ToString() + " " + dir.ody.ToString());
            //        if (gh.isChasing == true)
            //            gh.waypoints.Add(new Point(dir.ox, dir.oy));
            //    }

            //    dir.odx = dir.dx;
            //    dir.ody = dir.dy;

            //    if (k == 0)
            //    {
            //        pmTimer.Stop();
            //        ghTimer.Stop();
            //        DeathTimer.Stop();

            //        Winner win = new Winner(score);

            //        if (win.ShowDialog() == true)
            //        {
            //            string[] array = new string[10];
            //            string name = win.name.Text;
            //            using (StreamWriter outputFile = new StreamWriter(@"C:\ri-89\kurs\score.txt"))
            //            {
            //                foreach (string score in array)
            //                    outputFile.WriteLine(score);
            //            }
            //        }
            //    }
            //}

            //sc.Content = score;

        }

        //private void dispatcherGhostTimer_Tick(object sender, EventArgs e)
        //{
        //    //l1.Content = gir.x + " " + gir.y;
        //    // l1.Content += "\n" + gh.waypoints.Count;
        //    if (gh.iSeeYou(dir.ox, gh.x, dir.oy, gh.y, map) == false)
        //    {
        //        //if (gh.move(gir.x, gir.y) == true)
        //        //{
        //        //    gir.randomdir();
        //        //    gir.Update(map);
        //        //}
        //    }

        //    if (gh.waypoints.Count > 4)
        //    {
        //        gh.isChasing = false;
        //        gh.waypoints.Clear();
        //    }

        //    if (gh.isChasing == true)
        //    {
        //        gh.chase();
        //    }

        //    //if (gh.mode(gir.x, gir.y, dir.x, dir.y, map) == false)
        //    //{
        //    //    if (gh.move(gir.x, gir.y) == true)
        //    //    {
        //    //        gir.randomdir();
        //    //        gir.Update(map);
        //    //    }

        //    //}
        //    //else
        //    //{
        //    //    if (gh.move(gir.x, gir.y) == true)
        //    //    {
        //    //        gir.hunt(dir.x, dir.y, gir.x, gir.y, map);
        //    //        gir.Update(map);
        //    //    }
        //    //}
        //}       

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            menu.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Visible;
            player.Open(new Uri("C:\\Users\\Mvideo\\Desktop\\kurs-master\\kurs-master\\kurs\\kurs\\sounds\\Body.mp3", UriKind.Relative));

            pmTimer.Start();
            //ghTimer.Start();
        }

        private void Start_MouseEnter(object sender, MouseEventArgs e)
        {
            

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left) pakman.left();
            //else
            if (e.Key == Key.Right) pakman.right();
            //else          
            if (e.Key == Key.Down) pakman.down();
            //else
            if (e.Key == Key.Up) pakman.up();

            //try
            //{

            //if (e.Key == Key.Left) dir.left();
            ////else
            //if (e.Key == Key.Right) dir.right();
            ////else          
            //if (e.Key == Key.Down) dir.down();
            ////else
            //if (e.Key == Key.Up) dir.up();

            //if (e.Key == Key.Escape) ;
            //}
            //catch(else)
            //{
            //    MessageBox.Show("Click the arrow button")
            //}
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
