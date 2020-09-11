using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TheGameOfLife.Classes
{
    class GameHandler
    {
        private Canvas GameCanvas;
        private Rectangle[][] Fields;
        private int CurrentSize;
        private Boolean Working;
        private int Speed;
        public int BrushMode { get; set; }
        private readonly Brush DeactivatedColor = Brushes.LightGray;
        private readonly Brush ActivatedColor = Brushes.Yellow;
        public GameHandler(Canvas canvas, int startSize, int startSpeed)
        {
            GameCanvas = canvas;
            Speed = startSpeed;
            Fields = new Rectangle[100][];
            for(int row = 0; row<Fields.Length; row++)
            {
                Fields[row] = new Rectangle[100];
                for(int column = 0; column<Fields[row].Length; column++)
                {
                    Fields[row][column] = new Rectangle();
                    Fields[row][column].Stroke = DeactivatedColor;
                    Fields[row][column].StrokeThickness = 1;
                    Fields[row][column].Fill = DeactivatedColor;
                    GameCanvas.Children.Add(Fields[row][column]);
                    Fields[row][column].Visibility = System.Windows.Visibility.Collapsed;
                    Fields[row][column].MouseLeftButtonDown += (obj, e) =>
                    {
                       if( ((Rectangle)obj).Fill == DeactivatedColor)
                        {
                            ((Rectangle)obj).Fill = ActivatedColor;
                        }
                        else
                        {
                            ((Rectangle)obj).Fill = DeactivatedColor;
                        }
                    };
                }
            }
            GameCanvas.SizeChanged += (obj,e) => InitializeTheField();
            setNewSizeAndRestart(startSize);
            BrushMode = 0;
        }

        public void setNewSizeAndRestart(int size)
        {
            if (size <= 100 && size >= 5)
            {
                CurrentSize = size;
                InitializeTheField();
            }
            else
            {

            }
        }
        private void InitializeTheField()
        {
            for (int row = 0; row < Fields.Length; row++)
                for (int column = 0; column < Fields[row].Length; column++)
                {
                    if (row < CurrentSize && column < CurrentSize)
                    {
                        Fields[row][column].Height = (GameCanvas.ActualHeight / CurrentSize - 2) > 0 ? (GameCanvas.ActualHeight / CurrentSize - 2) : 1;
                        Fields[row][column].Width = (GameCanvas.ActualWidth / CurrentSize - 2) > 0 ? (GameCanvas.ActualWidth / CurrentSize - 2) : 1;
                        Canvas.SetTop(Fields[row][column], (GameCanvas.ActualHeight / CurrentSize * row));
                        Canvas.SetLeft(Fields[row][column], (GameCanvas.ActualWidth / CurrentSize * column));
                        Fields[row][column].Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        Fields[row][column].Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
        }

        public void next()
        {
                    bool[][] help = new bool[CurrentSize][];
                    for (int row = 0; row < CurrentSize; row++)
                    {
                        help[row] = new bool[CurrentSize];
                    }
                        for (int row = 0; row < CurrentSize; row++)
                            for (int column = 0; column < CurrentSize; column++)
                            {
                                help[row][column] = (Fields[row][column].Fill == ActivatedColor) ? true : false;
                            }
                        for (int row = 0; row < CurrentSize; row++)
                            for (int column = 0; column < CurrentSize; column++)
                            {
                                int sum = 0;
                                if (row + 1 < CurrentSize)
                                    sum += (help[row + 1][column]) ? 1 : 0;
                                if (column + 1 < CurrentSize)
                                    sum += (help[row][column + 1]) ? 1 : 0;
                                if (row - 1 >= 0)
                                    sum += (help[row - 1][column]) ? 1 : 0;
                                if (column - 1 >= 0)
                                    sum += (help[row][column - 1]) ? 1 : 0;
                                if (column + 1 < CurrentSize && row + 1 < CurrentSize)
                                    sum += (help[row + 1][column + 1]) ? 1 : 0;
                                if (column + 1 < CurrentSize && row - 1 >= 0)
                                    sum += (help[row - 1][column + 1]) ? 1 : 0;
                                if (column - 1 >= 0 && row + 1 < CurrentSize)
                                    sum += (help[row + 1][column - 1]) ? 1 : 0;
                                if (column - 1 >= 0 && row - 1 >= 0)
                                    sum += (help[row - 1][column - 1]) ? 1 : 0;

                                if ((sum == 3 )&& !help[row][column])
                                {
                                    Fields[row][column].Fill = ActivatedColor;
                                }
                                if ((sum < 2 || sum > 3) && help[row][column])
                                {
                                    Fields[row][column].Fill = DeactivatedColor;
                                }
                            }
                    
                
            
        }

        public void clear()
        {
            for (int row = 0; row < Fields.Length; row++)
            {
                for (int column = 0; column < Fields[row].Length; column++)
                {
                    Fields[row][column].Fill = DeactivatedColor;
                }
            }
        }

        public void random()
        {
            Random random = new Random();
            for (int row = 0; row < Fields.Length; row++)
            {
                for (int column = 0; column < Fields[row].Length; column++)
                {
                    Fields[row][column].Fill = (random.NextDouble()<0.8) ? DeactivatedColor : ActivatedColor;
                }
            }
        }
    }
}
