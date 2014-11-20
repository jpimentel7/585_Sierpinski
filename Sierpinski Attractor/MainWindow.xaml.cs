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
         
         int numberOfControlPoints = 0;
         bool mousePress = false;
        //used to move object
         Rectangle rectToBeMoved = null;

         struct ControlPoint
         {
             public Rectangle rect;
             public Point point;
             public ControlPoint(Rectangle currentRectangle,Point currentPoint)
             {
                 this.rect = currentRectangle;
                 this.point = currentPoint;
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
        //start
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //need to check that we have more then 3 control points selected
            Random rand = new Random();
      
            Point last, next;
            last = points[rand.Next(points.Count)].point;
            for (int i = 0; i < 2000; i++)
            {
                next = points[rand.Next(points.Count)].point;
                last = new Point((next.X + last.X) / 2, (next.Y + last.Y) / 2);
                //adds the new rect
                Rectangle rect = new Rectangle
                {
                    Width = 15,
                    Height = 15,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                //adds the rect
                Canvas.SetLeft(rect, last.X);
                Canvas.SetTop(rect, last.Y);
                myCanvas.Children.Add(rect);
                Console.WriteLine("hello");
            
                
            }
        }   
        //quit
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }
        //handles clicks on the canvas
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
          Rectangle closestRect = findRect(e.GetPosition(myCanvas));
          //checks to see if a rect was found
          if (numberOfControlPoints > 1 && closestRect != null)
          {
              mousePress = true;
              rectToBeMoved = closestRect;
              Console.WriteLine("get ready to move"); 
          }
          else
          {
              if (numberOfControlPoints <= 6)
              {


                  Rectangle rect = new Rectangle
                  {
                      Width = 15,
                      Height = 15,
                      Stroke = Brushes.Black,
                      StrokeThickness = 2
                  };
                  Point mouseLocation = e.GetPosition(myCanvas);
                  ControlPoint temp = new ControlPoint(rect, mouseLocation);
                  //adds the rect to the list 
                  points.Add(temp);

                  Canvas.SetLeft(rect, mouseLocation.X);
                  Canvas.SetTop(rect, mouseLocation.Y);
                  myCanvas.Children.Add(rect);

                  numberOfControlPoints++;
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
            if (rectToBeMoved != null && mousePress)
            {
                rectToBeMoved.SetValue(Canvas.LeftProperty, e.GetPosition(myCanvas).X);
                rectToBeMoved.SetValue(Canvas.TopProperty, e.GetPosition(myCanvas).Y);
                Console.WriteLine("im moving");  
            }
        }

        private void myCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePress = false;
            rectToBeMoved = null;
            Console.WriteLine("hey dont move"); 
        }
    }





}
