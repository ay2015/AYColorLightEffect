using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApplication9
{
    public class SolidColorBrushConverter
    {
        public static System.Windows.Media.Brush From16JinZhi(string color)
        {
            BrushConverter converter = new BrushConverter();
            return (System.Windows.Media.Brush)converter.ConvertFromString(color);
        }

        public static System.Windows.Media.Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new System.Windows.Media.Color()
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer updateTimer;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            updateTimer = new System.Windows.Threading.DispatcherTimer();
            updateTimer.Tick += new EventHandler(DrawingAY);
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            updateTimer.Start();
        }
        private void DrawingAY(object sender, EventArgs e)
        {
            AyKeyFrame();
        }

        List<GrainBase> grains = new List<GrainBase>();
        Random rand = new Random();

        public List<string> OtherTenColor = new List<string> {
           "54C7FE",
           "FFCD00",
           "FF9600",
           "FF2851",
           "0076FE",
           "45DB5E",
           "FF3823",
           "E559D0"
        };



        bool isEffect = true;
        int maxBrainsNum = 80;
        int sudu = 6;
        double maxJia = 12000;
        int minRadius = 200;
        int maxRadius = 900;
        bool isShowTile = false;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;
            if (!isShowTile)
            {
                bgTile.Visibility = Visibility.Collapsed;
            }
            for (int i = 0; i < maxBrainsNum; i++)
            {
                GrainBase gb = new GrainBase();
                gb.ColorBrushIndex = rand.Next(0, 8);
                gb.Radius = rand.Next(minRadius, maxRadius);
                gb.EffectRadius = rand.Next(160, 300);
                gb.x = rand.NextDouble() * Cav.ActualWidth;
                gb.y = rand.NextDouble() * Cav.ActualHeight;
                gb.xa = rand.NextDouble() * sudu - 1;
                gb.ya = rand.NextDouble() * sudu - 1;
                gb.max = maxJia;
                grains.Add(gb);
            }
        }

        public void AyKeyFrame()
        {
            Cav.Children.Clear();

            for (int i = 0; i < grains.Count; i++)
            {
                GrainBase dot = grains[i];
                if (dot.x == null || dot.y == null) continue;
                #region 创建碰撞粒子
                // 粒子位移
                dot.x += dot.xa;
                dot.y += dot.ya;
                // 遇到边界将加速度反向
                dot.xa *= (dot.x.Value > Cav.ActualWidth || dot.x.Value < 0) ? -1 : 1;
                dot.ya *= (dot.y.Value > Cav.ActualHeight || dot.y.Value < 0) ? -1 : 1;
                // 绘制点

                Ellipse elip = new Ellipse();
                //elip.Fill = new SolidColorBrush(OtherTenColor[dot.ColorBrushIndex]);
                elip.Stroke = new SolidColorBrush(Colors.Transparent);
                elip.StrokeThickness = 0;
                elip.Width = elip.Height = dot.Radius;
                if (isEffect)
                {
                    RadialGradientBrush rgb = new RadialGradientBrush();
                    GradientStop gs1 = new GradientStop();
                    var color1 = SolidColorBrushConverter.ToColor("#00" + OtherTenColor[dot.ColorBrushIndex]);
                    gs1.Offset = 1;
                    gs1.Color = color1;
                    rgb.GradientStops.Add(gs1);

                    GradientStop gs2 = new GradientStop();
                    var color2 = SolidColorBrushConverter.ToColor("#33" + OtherTenColor[dot.ColorBrushIndex]);
                    gs2.Offset = 0.7;
                    gs2.Color = color2;
                    rgb.GradientStops.Add(gs2);

                    GradientStop gs3 = new GradientStop();
                    var color3 = SolidColorBrushConverter.ToColor("#CC" + OtherTenColor[dot.ColorBrushIndex]);//99
                    gs3.Color = color3;
                    rgb.GradientStops.Add(gs3);


                    GradientStop gs4 = new GradientStop();
                    var color4 = SolidColorBrushConverter.ToColor("#99" + OtherTenColor[dot.ColorBrushIndex]);
                    gs4.Offset = 0.225;
                    gs4.Color = color4;
                    rgb.GradientStops.Add(gs4);

                    elip.Fill = rgb;
                }

                Canvas.SetLeft(elip, dot.x.Value - 0.5);
                Canvas.SetTop(elip, dot.y.Value - 0.5);

                Cav.Children.Add(elip);
                #endregion
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bgLay.Background = new SolidColorBrush(Colors.Black);
            bgTile.Visibility = Visibility.Visible;
            bgTile2.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bgLay.Background = new SolidColorBrush(Colors.White);
            bgTile.Visibility = Visibility.Collapsed;
            bgTile2.Visibility = Visibility.Visible;
        }

        private void startGrain_Click(object sender, RoutedEventArgs e)
        {
            int te;
            if (int.TryParse(grainsNumber.Text, out te))
            {
                updateTimer.Stop();
                grains.Clear();
                for (int i = 0; i < te; i++)
                {
                    GrainBase gb = new GrainBase();
                    gb.ColorBrushIndex = rand.Next(0, 8);
                    gb.Radius = rand.Next(minRadius, maxRadius);
                    gb.EffectRadius = rand.Next(160, 300);
                    gb.x = rand.NextDouble() * Cav.ActualWidth;
                    gb.y = rand.NextDouble() * Cav.ActualHeight;
                    gb.xa = rand.NextDouble() * sudu - 1;
                    gb.ya = rand.NextDouble() * sudu - 1;
                    gb.max = maxJia;
                    grains.Add(gb);
                }
                updateTimer.Start();
            }
            else
            {
                MessageBox.Show("例子数量必须是整数");
            }
        }
    }

    public class GrainBase
    {
        public double Radius { get; set; }

        //public double CurrentLife { get; set; }
        //public int MaxLife { get; set; }
        public int ColorBrushIndex { get; set; }
        public int EffectRadius { get; set; }
        public double? x { get; set; }
        public double? y { get; set; }

        public double xa { get; set; }

        public double ya { get; set; }

        public double max { get; set; }

    }


}
