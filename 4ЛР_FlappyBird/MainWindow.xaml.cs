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
using System.IO; //+ 

namespace _4ЛР_FlappyBird
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        int gravity = 8;
        int MyScore;

        Rect FlappyBirdRect;

        Random random = new Random();

        string BestScoreString;
        int BestScoreInt = 0;

        int PipesPosition = 0;
      
        int counter;

        bool check;
        int scorecheck = 0;

        const int WidthOfPipe = 66;
        const int HeightOfPipe = 390;

        const int LeftDistancePipes = 250;
        const int TopDistancePipes = 550;

        const int upper_bound_Bottom = 250;
        const int lower_bound_Bottom = 470;

        List<Rectangle> ItemRemover = new List<Rectangle>();

        public MainWindow()
        {
            InitializeComponent();
            
            // Font - backdround
            ImageBrush background = new ImageBrush();
            
            background.ImageSource = new BitmapImage(new Uri("D:/Visual Studio (project)/Визуальное программирование/Введение/4ЛР_FlappyBird/4ЛР_FlappyBird/4ЛР_FlappyBird/images/bg.png"));
            My.Background = background;

            // timer
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += DispatcherTimer_Tick;
            timer.Start();

            // Title
            Title = "Flappy Bird";

            StartGame();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(MyScore % 10 == 0 && MyScore != 0 && check == false)
            {
                CreatePipes(10, 2);
                scorecheck = MyScore;
                check = true;
            }

            if (MyScore - scorecheck >= 1)
            {
                check = false;
            }

            // Score
            ScoreRes.Content = "Score: " + MyScore;
            
            FlappyBirdRect = new Rect(Canvas.GetLeft(FlappyBird), Canvas.GetTop(FlappyBird),
                                        FlappyBird.Width, FlappyBird.Height);
            // Ход
            Canvas.SetTop(FlappyBird, Canvas.GetTop(FlappyBird) + gravity);

            // Касание краев: верхний и нижний - проверка:
            if (Canvas.GetTop(FlappyBird) < 0 || Canvas.GetTop(FlappyBird) + FlappyBird.Height > 560)
            {
                EndGame();
            }
 
            foreach (var x in My.Children.OfType<Rectangle>())
            {
                // Движение труб
                Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);
                // Обновление
                if (Canvas.GetLeft(x) < -50)
                {
                    ItemRemover.Add(x);
                }
                // Счетчик
                if (Canvas.GetLeft(x) == Canvas.GetLeft(FlappyBird) && Canvas.GetTop(FlappyBird) > Canvas.GetTop(x))
                {
                    MyScore += 1;
                }
                // Столкновение с трубой
                Rect PipeRect = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); // ?
                if (FlappyBirdRect.IntersectsWith(PipeRect))
                {
                    EndGame();
                }
            }
            foreach (Rectangle i in ItemRemover)
            {
                My.Children.Remove(i);
            }
        }

        private void My_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                FlappyBird.RenderTransform = new RotateTransform(-20, FlappyBird.Width / 2, FlappyBird.Height / 2);
                gravity = -8;
            }
            if (e.Key == Key.R)
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        private void My_KeyUp(object sender, KeyEventArgs e)
        { 
            FlappyBird.RenderTransform = new RotateTransform(10, FlappyBird.Width / 2, FlappyBird.Height / 2);
            gravity = 8;
        }
        private void StartGame()
        {
            My.Focus();

            MyScore = 0;

            // The Best of Score
            BestScoreString = File.ReadAllText("BestScore.txt");
            ScoreBest.Content = "Best Score: " + BestScoreString;

            // Location of flappyBird
            Canvas.SetTop(FlappyBird, 260);

            CreatePipes(10,0);

            timer.Start();
        }

        private void EndGame()
        {
            timer.Stop();

            // Visible
            FlappyBird.Visibility = Visibility.Collapsed;
            ScoreBest.Visibility = Visibility.Collapsed;
            ScoreRes.Visibility = Visibility.Collapsed;
            foreach (var x in My.Children.OfType<Rectangle>())
            {
                x.Visibility = Visibility.Collapsed;
            }

            if (MyScore > Convert.ToInt32(BestScoreString))
            {
                BestScoreInt = MyScore;
                File.WriteAllText("BestScore.txt", BestScoreInt.ToString());
            }
            else BestScoreInt = Convert.ToInt32(BestScoreString);

            // Text  
            label.Content = "Game Over!\n" + "Your Score: " + MyScore + "\n" + "Best Score: " + BestScoreInt.ToString() + "\n" +
                "Press 'R' to Play Again\n" + "Press 'Esc' to Exit the Game";
            label.Foreground = Brushes.White;
            label.FontSize = 30;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            Canvas.SetLeft(label, 100);
            Canvas.SetTop(label, 70);

        }

        private void CreatePipes(int num, int i)
        {
            PipesPosition = i;
            while (num-- > 0)
            {
                ImageBrush imagepipebottom = new ImageBrush();
                ImageBrush imagepipetop = new ImageBrush();

                imagepipebottom.ImageSource = new BitmapImage(new Uri("D:/Visual Studio (project)/Визуальное программирование/Введение/4ЛР_FlappyBird/4ЛР_FlappyBird/4ЛР_FlappyBird/images/pipeBottom.png"));
                imagepipetop.ImageSource = new BitmapImage(new Uri("D:/Visual Studio (project)/Визуальное программирование/Введение/4ЛР_FlappyBird/4ЛР_FlappyBird/4ЛР_FlappyBird/images/pipeTop.png"));

                Rectangle pipebottom = new Rectangle
                { 
                    Width = WidthOfPipe,
                    Height = HeightOfPipe,
                    Fill = imagepipebottom,
                    //Tag = "obj"
                };
                Rectangle pipetop = new Rectangle
                {
                    Width = WidthOfPipe,
                    Height = HeightOfPipe,
                    Fill = imagepipetop,
                    //Tag = "obj"
                };

                counter = random.Next(upper_bound_Bottom, lower_bound_Bottom); // 250 - 470

                Canvas.SetTop(pipebottom, counter);
                Canvas.SetLeft(pipebottom, 350 + (PipesPosition * LeftDistancePipes));

                Canvas.SetTop(pipetop, counter - TopDistancePipes);
                Canvas.SetLeft(pipetop, 350 + (PipesPosition * LeftDistancePipes));

                My.Children.Add(pipebottom);
                My.Children.Add(pipetop);

                PipesPosition++;
            }
        }
    }
}
