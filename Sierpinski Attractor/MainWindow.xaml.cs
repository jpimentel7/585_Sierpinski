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

namespace Sierpinski_Attractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml   
    /// </summary>
    public partial class MainWindow : Window
    {
        //holds the value from the radio box
         int size = 0;
        //holds the color from the comobo box
         Brush pointColor = Brushes.Red;
        // used for the drag feature
         bool mousePress = false;
        //used to move object
         ControlPoint rectToBeMoved;
         bool foundRec = false;
        //tracks when canvas has been painted on
         bool painted = false;

         struct ControlPoint
         {
             public Rectangle rect;
             public Point point;
             public Brush color;

             public ControlPoint(Rectangle currentRectangle,Point currentPoint, Brush currentColor)
             {
                 this.rect = currentRectangle;
                 this.point = currentPoint;
                 this.color = currentColor;
             }
             
         }

         List<ControlPoint> points = new List<ControlPoint>(6);

        public MainWindow()
        {
            InitializeComponent();
        }
        //combo box
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //gets the index of the selected value
            /*
             * 1. Red
             * 2. Blue
             * 3. Blue Violet 
             * 4. Green 
             * 5. Orange 
            */
            ComboBox comboBox = (ComboBox)sender;
            int selected = comboBox.SelectedIndex;
            switch (selected)
            {
                case 1:
                    pointColor = Brushes.Red;
                    break;
                case 2:
                    pointColor = Brushes.Blue;
                    break;
                case 3:
                    pointColor = Brushes.BlueViolet;
                    break;
                case 4:
                    pointColor = Brushes.Green;
                    break;
                case 5:
                    pointColor = Brushes.Orange;
                    break;
            }
            Console.WriteLine("new color was selected ");
            Console.WriteLine(selected);
        }
        //size 2
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 2;
        }
        //size 4
        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 4;
        }
        //side 6
        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 6;
        }
        //start button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //need to check that we have more then 3 control points selected
            Random rand = new Random();
            ControlPoint temp;
            Point last, next;
            last = points[rand.Next(points.Count)].point;
            for (int i = 0; i < 2000; i++)
            {
                //random control point
                temp = points[rand.Next(points.Count)];
                next = temp.point;
                last = new Point((next.X + last.X) / 2, (next.Y + last.Y) / 2);
                //adds the new rect
                Rectangle rect = new Rectangle
                {
                    Width = size,
                    Height = size,
                    Fill = temp.color
                };
                //adds the rect
                Canvas.SetLeft(rect, last.X);
                Canvas.SetTop(rect, last.Y);
                myCanvas.Children.Add(rect);
                Console.WriteLine("hello");
            }
            // used to determine if the canvas has been painted on
            painted = true;
        }   
        //clear button
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            points.Clear();
            painted = false;
        }
        //handles clicks on the canvas
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (ControlPoint temp in points)
            {
                //uses the distances formula to find the closest rectangle
                int distances = (int)Math.Sqrt(Math.Pow((e.GetPosition(myCanvas).X - temp.point.X), 2) +
                    Math.Pow((e.GetPosition(myCanvas).Y - temp.point.Y), 2));
                if (distances < 25)
                {
                    rectToBeMoved = temp;
                    foundRec = true;
                }
            }
          //checks to see if a rect was found
          if (points.Count > 1 && foundRec != false)
          {
              mousePress = true;
              Console.WriteLine("get ready to move"); 
          }
          else
          {
              if (points.Count <= 6)
              {


                  Rectangle rect = new Rectangle
                  {
                      Width = 10,
                      Height = 10,
                      Fill = pointColor
                  };
                  Point mouseLocation = e.GetPosition(myCanvas);
                  ControlPoint temp = new ControlPoint(rect, mouseLocation,pointColor);
                  //adds the rect to the list 
                  points.Add(temp);

                  Canvas.SetLeft(rect, mouseLocation.X);
                  Canvas.SetTop(rect, mouseLocation.Y);
                  myCanvas.Children.Add(rect);
              }
          }
        }
        private Rectangle findRect(Point mouseLocation)
        {
               
            foreach (ControlPoint temp in points)
            {
                //uses the distances formula to find the closest rectangle
                int distances = (int)Math.Sqrt(Math.Pow((mouseLocation.X - temp.point.X), 2) + 
                    Math.Pow((mouseLocation.Y - temp.point.Y), 2));
                if (distances < 25)
                    return temp.rect;
            }

            return null;
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (foundRec != false && mousePress)
            {
                rectToBeMoved.rect.SetValue(Canvas.LeftProperty, e.GetPosition(myCanvas).X);
                rectToBeMoved.rect.SetValue(Canvas.TopProperty, e.GetPosition(myCanvas).Y);
                int i;
                for (i = 0; i < points.Count();i++ )
                {
                    if (points[i].Equals(rectToBeMoved))
                    {
                        break;
                    }
                }
                points[i].point = new Point(e.GetPosition(myCanvas).X, e.GetPosition(myCanvas).Y);
                Console.WriteLine("im moving");  
            }
        }

        private void myCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePress = false;
            foundRec = false;
            Console.WriteLine("hey dont move"); 
            //checks if we have to redraw
            if (painted == true)
            {
                //clears the canvas
                myCanvas.Children.Clear();
                //readds the points
                foreach (ControlPoint temp in points)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = 10,
                        Height = 10,
                        Fill = temp.color
                    };
                   
                    Canvas.SetLeft(rect, temp.point.X);
                    Canvas.SetTop(rect, temp.point.Y);
                    myCanvas.Children.Add(rect);
                }
                //Button_Click(null, null);
            }
            
        }
    }





}
