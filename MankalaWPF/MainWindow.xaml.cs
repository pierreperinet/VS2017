using System.Windows;
using System.Windows.Media;
using MankalaLib;

namespace MankalaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Machine Machine { get; }

        private Game Game { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Game = new Game();
            Machine = new Machine();
        }

        private void A1_Click(object sender, RoutedEventArgs e)
        {
            AX_Click(Game.A1);
        }

        private void A2_Click(object sender, RoutedEventArgs e)
        {
            AX_Click(Game.A2);
        }

        private void A3_Click(object sender, RoutedEventArgs e)
        {
            AX_Click(Game.A3);
        }

        private void A4_Click(object sender, RoutedEventArgs e)
        {
            AX_Click(Game.A4);
        }

        private void A5_Click(object sender, RoutedEventArgs e)
        {
            AX_Click(Game.A5);
        }

        private void A6_Click(object sender, RoutedEventArgs e)
        {
            AX_Click(Game.A6);
        }

        private async void AX_Click(Box box)
        {
            var next = box.Click(box.Name);
            RedrawBoxes();
            ActivateA();

            if (next == BoxName.AG)
            {
                var winner1 = Game.GetWinner(next);
                if (winner1 != null)
                {
                    Message.Text = winner1 == BoxName.AG ? "You win!" : "You loose!";
                }
                return;
            }
            DeactivateA();
            var winner = Game.GetWinner(next);
            if (winner != null)
            {
                Message.Text = winner == BoxName.AG ? "You win!" : "You loose!";
                return;
            }
            Message.Text = "Machine's turn!";
            do
            {
                var mboxName = await Machine.Play(Game);
                var mbox = Game.GetBox(mboxName);
                next = mbox.Click(BoxName.BG);
                RedrawBoxes();
                winner = Game.GetWinner(next);
                if (winner == null) continue;
                Message.Text = winner == BoxName.AG ? "You win!" : "You loose!";
                return;
            }
            while (next == BoxName.BG);
            ActivateA();
            Message.Text = "Your turn!";
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Game = new Game();
            RedrawBoxes();
            ActivateA();
            Message.Text = "Your turn!";
        }

        private void RedrawBoxes()
        {
            A1.Content = Game.A1.Value;
            A2.Content = Game.A2.Value;
            A3.Content = Game.A3.Value;
            A4.Content = Game.A4.Value;
            A5.Content = Game.A5.Value;
            A6.Content = Game.A6.Value;
            A7.Text = Game.AG.Value.ToString();

            B1.Text = Game.B1.Value.ToString();
            B2.Text = Game.B2.Value.ToString();
            B3.Text = Game.B3.Value.ToString();
            B4.Text = Game.B4.Value.ToString();
            B5.Text = Game.B5.Value.ToString();
            B6.Text = Game.B6.Value.ToString();
            B7.Text = Game.BG.Value.ToString();

            var color = new Color { A = 102, R = 203, G = 172, B = 172 };
            var bgColorBrush = new SolidColorBrush(color);
            A1.Background = bgColorBrush;
            A2.Background = bgColorBrush;
            A3.Background = bgColorBrush;
            A4.Background = bgColorBrush;
            A5.Background = bgColorBrush;
            A6.Background = bgColorBrush;
            A7.Background = bgColorBrush;
        }

        private void DeactivateA()
        {
            A1.IsEnabled = false;
            A2.IsEnabled = false;
            A3.IsEnabled = false;
            A4.IsEnabled = false;
            A5.IsEnabled = false;
            A6.IsEnabled = false;
        }

        private void ActivateA()
        {
            A1.IsEnabled = (int)A1.Content != 0;

            A2.IsEnabled = (int)A2.Content != 0;

            A3.IsEnabled = (int)A3.Content != 0;

            A4.IsEnabled = (int)A4.Content != 0;

            A5.IsEnabled = (int)A5.Content != 0;

            A6.IsEnabled = (int)A6.Content != 0;
        }
    }
}
