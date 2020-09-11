using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
using TheGameOfLife.Classes;

namespace TheGameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameHandler game;
        Boolean Initialized;
        public MainWindow()
        {
            InitializeComponent();
            Initialized = false;
            ClearButton.IsEnabled = false;
            RepeatButton.IsEnabled = false;
            NextButton.IsEnabled = false;
            SetRandomButton.IsEnabled = false;

        }

        private void InitializeUIComponents()
        {
            SpeedComboBox.ItemsSource = new List<int>{1, 5, 10, 15, 30, 60, 1000};
            SpeedComboBox.SelectedIndex = 2;
            SizeSlider.Value = 5;
            SizeSlider.ValueChanged += (obj, e) => { game.setNewSizeAndRestart((int)(SizeSlider.Value * 2.5 + 5)); };
            game.setNewSizeAndRestart((int)(SizeSlider.Value * 2.5 + 5));
            ClearButton.IsEnabled = true;
            RepeatButton.IsEnabled = true;
            NextButton.IsEnabled = true;
            SetRandomButton.IsEnabled = true;
        }

        private void InitializeGameCanvas()
        {
            game = new GameHandler(GameCanvas, 5, 1);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Initialized)
            {
                InitializeGameCanvas();
                InitializeUIComponents();
                Initialized = true;
                StartButton.IsEnabled = false;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            game.next();
        }

        async private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            ClearButton.IsEnabled = false;
            NextButton.IsEnabled = false;
            SetRandomButton.IsEnabled = false;
            SpeedComboBox.IsEnabled = false;
            int speed = (int) SpeedComboBox.SelectedValue;
            int i = 1;
            RepeatButton.Click -= RepeatButton_Click;
            RoutedEventHandler func = (obj, ev) => { i = 0; };
            RepeatButton.Click += func;
            RepeatButton.Content = "Stop";
            for (i = 1; i!=0;)
            {
                await Task.Delay(1000/speed);
                game.next();
                Refresh(this);
            }
            RepeatButton.Click += RepeatButton_Click;
            RepeatButton.Content = "Repeat";
            SpeedComboBox.IsEnabled = true;
            ClearButton.IsEnabled = true;
            NextButton.IsEnabled = true;
            SetRandomButton.IsEnabled = true;
        }

        private static Action EmptyDelegate = delegate () { };


        public static void Refresh(UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            game.clear();
        }

        private void SetRandomButton_Click(object sender, RoutedEventArgs e)
        {
            game.random();
        }
    }
}
