/* 
 * WPF Sierpinski Attractor 
 * Comp 585
 * Barnes
 * 11/23/14
 * 
 * Javier Pimentel 
 * javier.pimentel.791@my.csun.edu
 * 
 * Uyen Nguyen
 * uyen.nguyen.630@my.csun.edu
 * 
 */

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
        //holds the value for color 
         SolidColorBrush controlPointColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
         //holds the red value from the combo box
         byte redValue = 0;
         //holds the green value from the combo box
         byte greenValue = 0;
         //holds the blue value from the combo box
         byte blueValue = 0;
        // used for the drag feature
         bool isMousePress = false;
        //used to move object
         ControlPoint rectToBeMoved;
         bool movingRect = false;
        //tracks when canvas has been painted on
         bool isCanvasPainted = false;
        //stores control points
        List<ControlPoint> points = new List<ControlPoint>(6);

         struct ControlPoint
         {
             public Rectangle rect;
             public Point point;
             public SolidColorBrush color;

             public ControlPoint(Rectangle currentRectangle, Point currentPoint, SolidColorBrush currentColor)
             {
                 this.rect = currentRectangle;
                 this.point = currentPoint;
                 this.color = currentColor;
             }
         }

        public MainWindow()
        {
            InitializeComponent();
        }

        //combo box for red value
        private void RedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int redSelected = comboBox.SelectedIndex;
            switch (redSelected)
            {
                case 0:
                    redValue = 0;
                    break;
                case 1:
                    redValue = 85;
                    break;
                case 2:
                    redValue = 170;
                    break;
                case 3:
                    redValue = 255;
                    break;
            }
            Console.WriteLine("Red value was selected: " + redValue);
        }

        //combo box for green value
        private void GreenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int greenSelected = comboBox.SelectedIndex;
            switch (greenSelected)
            {
                case 0:
                    greenValue = 0;
                    break;
                case 1:
                    greenValue = 85;
                    break;
                case 2:
                    greenValue = 170;
                    break;
                case 3:
                    greenValue = 255;
                    break;
            }
            Console.WriteLine("Green value was selected: " + greenValue);
        }

        //combo box for blue value
        private void BlueComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int blueSelected = comboBox.SelectedIndex;
            switch (blueSelected)
            {
                case 0:
                    blueValue = 0;
                    break;
                case 1:
                    blueValue = 85;
                    break;
                case 2:
                    blueValue = 170;
                    break;
                case 3:
                    blueValue = 255;
                    break;
            }
            Console.WriteLine("Blue value was selected: " + blueValue);
        }

        //size 2
        private void RadioButton_Size_2(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 2;
        }

        //size 4
        private void RadioButton_Size_4(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 4;
        }

        //size 6
        private void RadioButton_Size_6(object sender, RoutedEventArgs e)
        {
            //sets the size
            size = 6;
        }

        //run button
        private void Run_Button_Click(object sender, RoutedEventArgs e)
        {
            //need to check that we have more than 3 control points selected
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
                    //size is controlled by the radio buttons 
                    Width = size,
                    Height = size,
                    //get the color from the random control point
                    Fill = temp.color 
                };
                //adds the rect to the canvas
                Canvas.SetLeft(rect, last.X);
                Canvas.SetTop(rect, last.Y);
                myCanvas.Children.Add(rect);
                Console.WriteLine("Hello");
            }
            //used to determine if the canvas has been painted on
            isCanvasPainted = true;
        }   

        //clear button
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            points.Clear();
            //used to determine if the canvas has been painted on
            isCanvasPainted = false;
            //reset color and size attributes
            colorRectangle.Fill = Brushes.AliceBlue;
            defaultRadioBtn.IsChecked = true;
            redComboBox.SelectedIndex = 0;
            greenComboBox.SelectedIndex = 0;
            blueComboBox.SelectedIndex = 0;
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
                    //save the closest rect variable for later
                    rectToBeMoved = temp;
                    movingRect = true;
                }
            }

          //checks to see if a rect was found
          if (points.Count > 1 && movingRect != false)
          {
              isMousePress = true;
              Console.WriteLine("get ready to move"); 
          }
          else
          {
              //if no Control Point is found we must add one
              if (points.Count <= 5)
              {
                  Rectangle rect = new Rectangle
                  {
                      Width = 10,
                      Height = 10,
                      Fill = controlPointColor
                  };
                  
                  //change control point color to rgb value color
                  controlPointColor.Color = Color.FromRgb(redValue, greenValue, blueValue);
                  ControlPoint temp = new ControlPoint(rect, e.GetPosition(myCanvas), controlPointColor);
                  //adds the rect to the list 
                  points.Add(temp);
                  //adds the rect to the canvas
                  Canvas.SetLeft(rect, e.GetPosition(myCanvas).X);
                  Canvas.SetTop(rect, e.GetPosition(myCanvas).Y);
                  myCanvas.Children.Add(rect);
                  Console.WriteLine("new point"+ points.Count); 
              }
          }
        }
        
        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (movingRect != false && isMousePress)
            {
                //updates the postition of the rect on the canvas
                rectToBeMoved.rect.SetValue(Canvas.LeftProperty, e.GetPosition(myCanvas).X);
                rectToBeMoved.rect.SetValue(Canvas.TopProperty, e.GetPosition(myCanvas).Y);
                Console.WriteLine("im moving");  
            }
        }

        private void myCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMousePress = false;

            //Console.WriteLine("hey dont move"); 
            //readds the point that was moved into the points arrray list
            if (movingRect == true)
            {
                //readds the point we removed with the X,Y values
                Point tempPoint = new Point(Canvas.GetLeft(rectToBeMoved.rect), Canvas.GetTop(rectToBeMoved.rect));
                ControlPoint temp = new ControlPoint(rectToBeMoved.rect, tempPoint, rectToBeMoved.color);
                points.Add(temp);
                movingRect = false;
                Console.WriteLine("Hey, don't move");
                //checks if we have to redraw
                if (isCanvasPainted == true)
                {
                    //clears the canvas
                    myCanvas.Children.Clear();
                    //readds all the points to the canvas
                    foreach (ControlPoint tempCP in points)
                    {
                        Rectangle rect = new Rectangle
                        {
                            Width = 10,
                            Height = 10,
                            Fill = tempCP.color
                        };
                        //adds the rects to the canvas
                        Canvas.SetLeft(rect, tempCP.point.X);
                        Canvas.SetTop(rect, tempCP.point.Y);
                        myCanvas.Children.Add(rect);
                    }
                    //adds 2000 rects
                    Run_Button_Click(null, null);
                }
            }
        }

        //About menu item click handler
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult author = MessageBox.Show(
                "Sierpinski Attractor \n\n"
                + "Javier Pimentel \njavier.pimentel.791@my.csun.edu\n\n"
                + "Uyen Nguyen \nuyen.nguyen.630@my.csun.edu\n\n"
                + "California State University, Northridge \nComp 585 \nBarnes",
                "About");
        }

        //usage menu item click handler
        private void Usage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult usage = MessageBox.Show(
                "User can select RGB values from the three combo boxes to create\n"
                + "a color and select size from one of the three radio buttons for the\n"
                + "control points.  User must click on canvas to create three to six\n"
                + "control points. Clicking the Run button will create a Sierpinski\n"
                + "Attractor.  Clicking the Clear button will clear the canvas and reset\n"
                + "to default values.", "Usage");
        }

        //change rectangle color to selected rgb values
        private void Get_Color_Click(object sender, RoutedEventArgs e)
        {
            colorRectangle.Fill = new SolidColorBrush(Color.FromRgb(redValue, greenValue, blueValue));
        }
    }
}
