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
         Brush controlPointColor = Brushes.Red;
        // used for the drag feature
         bool isMousePress = false;
        //used to move object
         ControlPoint rectToBeMoved;
         bool movingRect = false;
        //tracks when canvas has been painted on
         bool isCanvasPainted = false;

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
                case 0:
                    controlPointColor = Brushes.Red;
                    break;
                case 1:
                    controlPointColor = Brushes.Blue;
                    break;
                case 2:
                    controlPointColor = Brushes.BlueViolet;
                    break;
                case 3:
                    controlPointColor = Brushes.Green;
                    break;
                case 4:
                    controlPointColor = Brushes.Orange;
                    break;
            }
            Console.WriteLine("new color was selected ");
            Console.WriteLine(selected);
        }
        //size 2
        private void RadioButton_Size_2(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 6;
        }
        //size 4
        private void RadioButton_Size_4(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 4;
        }
        //side 6
        private void RadioButton_Size_6(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 2;
        }
        //start button
        private void Start_Button_Click(object sender, RoutedEventArgs e)
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
            isCanvasPainted = true;
        }   
        //clear button
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            points.Clear();
            isCanvasPainted = false;
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
                    movingRect = true;
                }
            }
            if(movingRect == true)
             points.Remove(rectToBeMoved);

          //checks to see if a rect was found
          if (points.Count > 1 && movingRect != false)
          {
              isMousePress = true;
              
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
                      Fill = controlPointColor
                  };
                  Point mouseLocation = e.GetPosition(myCanvas);
                  ControlPoint temp = new ControlPoint(rect, mouseLocation,controlPointColor);
                  //adds the rect to the list 
                  points.Add(temp);
                  //adds the rect to the canvas
                  Canvas.SetLeft(rect, mouseLocation.X);
                  Canvas.SetTop(rect, mouseLocation.Y);
                  myCanvas.Children.Add(rect);
              }
          }
        }
        
        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (movingRect != false && isMousePress)
            {
                rectToBeMoved.rect.SetValue(Canvas.LeftProperty, e.GetPosition(myCanvas).X);
                rectToBeMoved.rect.SetValue(Canvas.TopProperty, e.GetPosition(myCanvas).Y);
               
                Console.WriteLine("im moving");  
            }
        }

        private void myCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMousePress = false;
            movingRect = false;
            Console.WriteLine("hey dont move"); 
            //checks if we have to redraw
            if (isCanvasPainted == true)
            {
                //clears the canvas
                myCanvas.Children.Clear();
                //readds the point
                Point tempPoint = new Point(Canvas.GetLeft(rectToBeMoved.rect),Canvas.GetTop(rectToBeMoved.rect));
                ControlPoint temp = new ControlPoint(rectToBeMoved.rect, tempPoint, rectToBeMoved.color);
                points.Add(temp);
                //readds the points
                foreach (ControlPoint tempCP in points)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = 10,
                        Height = 10,
                        Fill = temp.color
                    };
                   
                    Canvas.SetLeft(rect, tempCP.point.X);
                    Canvas.SetTop(rect, tempCP.point.Y);
                    myCanvas.Children.Add(rect);
                }
                Start_Button_Click(null, null);
            }
            
        }
    }

}
