using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dynmaic_n_and_c
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        //true = circles, false = crosses
        bool turn = true;
        public int gridheight, gridwidth, colomnLength, rowLength, buttonWidthHeight = 150, CirclesScoreCount = 0, CrossesScoreCount = 0;
        public Grid grid = new Grid();

        public Game(int heighttxt, int widthtxt)
        {
            this.Height = heighttxt;
            this.Width = widthtxt;
            colomnLength = heighttxt;
            rowLength = widthtxt;
            InitializeComponent();
            GridSize();
        }

        private void buttonUpdate(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;
            int bheight = Int32.Parse(button.Name.Substring(1, 1));
            int bwidth = Int32.Parse(button.Name.Substring(2));
            Image dynamicImage = new Image();
            dynamicImage.Width = 200;
            dynamicImage.Height = 200;

            //bitmapsource https://www.c-sharpcorner.com/UploadFile/mahesh/using-xaml-image-in-wpf/
            BitmapImage circle = new BitmapImage();
            circle.BeginInit();
            circle.UriSource = new Uri(@"D:\116-1168880_letter-o-png-small-letter-o-clipart.png");
            circle.EndInit();

            BitmapImage cross = new BitmapImage();
            cross.BeginInit();
            cross.UriSource = new Uri(@"D:\png-clipart-symbolize-x.png");
            cross.EndInit();

            //change image based on whose turn it in
            if (turn == true)
            {
                button.BorderBrush = Brushes.Black;
                dynamicImage.Source = circle;
                turn = false;
            }
            else
            {
                button.BorderBrush = Brushes.White;
                dynamicImage.Source = cross;
                turn = true;
            }
            button.Content = dynamicImage;
            gridUpdate(gridheight, gridwidth, button);
        }

        public void GridSize()
        {
            mainWindow.Height = colomnLength * buttonWidthHeight + 30;
            mainWindow.Width = rowLength * buttonWidthHeight + 10;
            //Creating grid
            grid.Height = mainWindow.Height;
            grid.Width = mainWindow.Width;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(0, 0, 0, 0);
            //Inserting buttons into grid
            for (int i = 1; i <= rowLength; i++)
            {
                for (int j = 1; j <= colomnLength; j++)
                {
                    //create button
                    Button button = new Button();
                    button.Height = buttonWidthHeight;
                    button.Width = buttonWidthHeight;
                    button.Margin = new Thickness(i * buttonWidthHeight - buttonWidthHeight, j * buttonWidthHeight - buttonWidthHeight, 0, 0);
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.Name = "_" + i + j;
                    button.Content = "_" + i + j;
                    button.Background = Brushes.White;
                    button.Click += buttonUpdate;
                    button.BorderBrush = Brushes.Red;
                    grid.Children.Add(button);
                }
            }
            mainWindow.Content = grid;
        }
        private void gridUpdate(int height, int width, Button button)
        {
            Grid grid = (Grid)VisualTreeHelper.GetParent(button);
            //CheckAcross();
            //CheckUpDown();
            //CheckDiagnolLeftRight();
            CheckDiagnolRightLeft();
        }
        public Button GetChildButton(int colomn, int row)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grid); i++)
            {
                Button button = (Button)VisualTreeHelper.GetChild(grid, i);
                string name = ("_" + colomn.ToString() + row.ToString());
                if (button.Name == name)
                {
                    return button;
                }
            }
            return null;
        }

        public void CreateWinningTextBox()
        {
            //textbox that contains who has won, once someone has
            TextBox txtbox = new TextBox
            {
                Height = 100,
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0)
            };
            if (turn == false)
            {
                txtbox.Text = "Circles Win";
            }
            else
            {
                txtbox.Text = "Crosses Win";
            }
            grid.Children.Add(txtbox);
        }

        public void WinCheck(int colomnOrRow, int colomn, int row, bool diag, bool mid)
        {
            //Score count - keeps track of how many crosses/circles are in a row/colomn
            Button button = new Button();
            button = GetChildButton(colomn, row);
            if (button.BorderBrush == Brushes.Black)
            {
                CirclesScoreCount++;
                //if the score is equal to the length of the row they have completed a colomn, therefore winning
                ///win condition broken
                if ((CirclesScoreCount == colomnOrRow) || ((diag == true) && (CirclesScoreCount > 2) && (CirclesScoreCount == Math.Min(colomn, row)) && (row != colomn)))
                {
                    CreateWinningTextBox();
                    return;
                }
                else if ((mid == true) && (CirclesScoreCount == colomnLength))
                {
                    CreateWinningTextBox();
                    return;
                }
            }
            else if (button.BorderBrush == Brushes.White)
            {
                CrossesScoreCount++;
                if ((CrossesScoreCount == colomnOrRow) || ((diag == true) && (CrossesScoreCount > 2) && (CrossesScoreCount == Math.Min(colomn, row)) && (row != colomn)))
                {
                    CreateWinningTextBox();
                    return;
                }
                else if ((mid == true) && (CrossesScoreCount == colomnLength))
                {
                    CreateWinningTextBox();
                    return;
                }
            }
        }

        public void ScoreReset()
        {
            CrossesScoreCount = 0;
            CirclesScoreCount = 0;
        }

        public void CheckUpDown()
        {
            for (int colomn = 1; colomn <= colomnLength; colomn++)
            {
                ScoreReset();
                for (int row = 1; row <= rowLength; row++)
                {
                    WinCheck(rowLength, colomn, row, false, false);
                }
            }
        }
        public void CheckAcross()
        {
            for (int row = 1; row <= colomnLength; row++)
            {
                ScoreReset();
                for (int colomn = 1; colomn <= rowLength; colomn++)
                {
                    WinCheck(colomnLength, colomn, row, false, false);
                }
            }
        }

        public void CheckDiagnolLeftRight()
        {
            int row, colomn;
            //check middle going left to right
            for (row = 1, colomn = 1; row <= rowLength; row++, colomn++)
            {
                WinCheck(rowLength, colomn, row, true, true);
            }
            //check top right side tempcolomn/row allows it to check all diagnols and not just one
            for (int tempcolomn = 2; tempcolomn < colomnLength; tempcolomn++)
            {
                ScoreReset();
                for (row = 1, colomn = tempcolomn; colomn <= colomnLength; row++, colomn++)
                {
                    WinCheck(colomnLength, colomn, row, true, false);
                }
            }
            //check bottom left side
            for(int temprow = 2; temprow < rowLength; temprow++)
            {
                ScoreReset();
                for (row = temprow, colomn = 1; row <= rowLength; row++, colomn++)
                {
                    WinCheck(rowLength, colomn, row, true, false);
                }
            }
        }

        public void CheckDiagnolRightLeft()
        {
            int row, colomn;
            //check middle going right to left
            for (row = 1, colomn = colomnLength; colomn >= 1; row++, colomn--)
            {
                WinCheck(rowLength, colomn, row, true, true);
            }
            //check bottom right side tempcolomn.row allows it to check all diagnols and not just one
            for (int temprow = 2; temprow < rowLength; temprow++)
            {
                ScoreReset();
                for (row = temprow, colomn = colomnLength; row <= rowLength; row++, colomn--)
                {
                    WinCheck(colomnLength, colomn, row, true, false);
                }
            }
            //check top left side
            for (int tempColomn = colomnLength - 1; tempColomn > 1; tempColomn--)
            {
                ScoreReset();
                for (row = 1, colomn = tempColomn; colomn >= 1; row++, colomn--)
                {
                    WinCheck(rowLength, colomn, row, true, false);
                }
            }
        }
    }
}
