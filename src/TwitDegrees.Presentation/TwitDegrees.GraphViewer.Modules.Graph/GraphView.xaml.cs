// this is based off of the work by Mike Hodnik, to the point that its practically
// word for word -- this was a big help as a starting off place for this code.
// http://www.kindohm.com/archive/2008/08/13/radial-network-graph-in-silverlight.aspx

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TwitDegrees.GraphViewer.Infrastructure.Model;

namespace TwitDegrees.GraphViewer.Modules.Graph
{
    public interface IGraphView
    {
        void RunFadeOutStoryboard();
        void RunFadeInStoryboard();
        void LoadModel(TwitterUser user);
    }
    
    public partial class GraphView : IGraphView
    {
        readonly ScaleTransform scale;
        Dictionary<TwitterUser, TwitterUserControl> buttonRegistry;

        public GraphView() 
        {
        }

        // injected
        public GraphView(GraphViewModel viewModel) 
        {
            DataContext = viewModel;
            InitializeComponent();
            scale = new ScaleTransform();
            entityCanvas.RenderTransform = scale;
            ViewModel.View = this;

            Loaded += (s, e) =>
            {
                // required the first time around to fix the canvas points and such.
                DoZoom();
                Scroll(new Point(100, 100));
            };
        }

        protected GraphViewModel ViewModel { get { return (GraphViewModel)DataContext;}}

        private void fadeOutStoryboard_Completed(object sender, EventArgs e)
        {
            ViewModel.FadeOutCompleteCommand.Execute(null);
        }

        private void mainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.CanvasSelectedCommand.Execute(null);
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DoZoom();
        }


        void user_Clicked(object sender, TwitterUserControlClickedEventArgs e)
        {
            // TODO
//            this.cancelScroll = true;
            ViewModel.SelectedUserName = e.TwitterUser.Name;
            fadeOutStoryboard.Begin();
            
        }

        private void DoZoom()
        {
            if (zoomSlider != null)
            {
                double factor = zoomSlider.Value / 50;
                scale.ScaleX = factor;
                scale.ScaleY = factor;
            }
        }

        public void RunFadeOutStoryboard()
        {
            fadeOutStoryboard.Begin();
        }

        public void RunFadeInStoryboard()
        {
            fadeInStoryboard.Begin();
        }


        void Scroll(Point clickPoint)
        {
            var midPoint = new Point(this.mainCanvas.ActualWidth / 2, this.mainCanvas.ActualHeight / 2);
            var difference = new Point(midPoint.X - clickPoint.X, midPoint.Y - clickPoint.Y);

            double currentLeft = Canvas.GetLeft(entityCanvas);
            double currentTop = Canvas.GetTop(this.entityCanvas);

            double newLeft = currentLeft + difference.X;
            double newTop = currentTop + difference.Y;

            leftAnimation.To = newLeft;
            topAnimation.To = newTop;

            leftAnimationStoryboard.Begin();
            topAnimationStoryboard.Begin();
        }

       







        public void LoadModel(TwitterUser user)
        {
            buttonRegistry = new Dictionary<TwitterUser, TwitterUserControl>();
            entityCanvas.Children.Clear();
            
            // TODO: not sure if we want this here - the user model should already have dictated what data we retrieved.
            int depth = int.Parse(depthTextBox.Text);

            var startControl = new TwitterUserControl(user) {Title = user.Name};
            var trans = new ScaleTransform {ScaleX = 1.3, ScaleY = 1.3};
            startControl.RenderTransform = trans;
            startControl.Color = Colors.Blue;
            entityCanvas.Children.Add(startControl);

            Canvas.SetLeft(startControl, entityCanvas.ActualWidth / 2);
            Canvas.SetTop(startControl, entityCanvas.ActualHeight / 2);

            buttonRegistry.Add(user, startControl);

            ICollection<TwitterUser> ringEntities = user.Friends;
            for (int i = 1; i <= depth; i++)
            {
                ringEntities = LoadRing(ringEntities, i);
            }
            MakeFriends();
            infoBlock.Text = "Node Count: " + buttonRegistry.Count;
        }

        void MakeFriends()
        {
            var exclusions = new Dictionary<int, int>();
            foreach (var entity in buttonRegistry.Keys)
            {
                foreach (var friend in entity.Friends)
                {
                    EvaluateFriend(entity, friend, exclusions);
                }
            }
        }


        void EvaluateFriend(TwitterUser entity, TwitterUser friend, IDictionary<int, int> exclusions)
        {
            int key = entity.GetHashCode() * friend.GetHashCode();
            if (!exclusions.ContainsKey(key))
            {
                if (buttonRegistry.ContainsKey(friend))
                {
                    DrawConnection(entity, friend);
                    exclusions.Add(key, 0);
                }
            }
        }


        void DrawConnection(TwitterUser user1, TwitterUser user2)
        {
            var button1 = buttonRegistry[user1];
            var button2 = buttonRegistry[user2];


            var line = new Line
                           {
                               StrokeThickness = 1,
                               Stroke = new SolidColorBrush(Colors.Black),
                               Opacity = .5,
                               X1 = Canvas.GetLeft(button1) + button1.Width / 2,
                               Y1 = Canvas.GetTop(button1) + button1.Height / 2,
                               X2 = Canvas.GetLeft(button2) + button2.Width / 2,
                               Y2 = Canvas.GetTop(button2) + button2.Height / 2
                           };
            button1.ConnectedControls.Add(button2);
            button2.ConnectedControls.Add(button1);
            button1.ConnectionLines.Add(line);
            button2.ConnectionLines.Add(line);
            entityCanvas.Children.Insert(0, line);
        }


        TwitterUser[] LoadRing(ICollection<TwitterUser> ringEntities, int depth)
        {
            var nextRing = new List<TwitterUser>();
            //1 radian = ~57.3 degrees
            //~6 radians / circle
            double radiusFactor = Convert.ToDouble(depth) * 1;
            double radius = 160 * radiusFactor;
            double stepConstant = .06 * Convert.ToDouble(spacingSlider.Value);
            double radianStep = stepConstant / Convert.ToDouble(ringEntities.Count);
            double radians = 0;
            infoBlock.Text = ringEntities.Count.ToString();

            foreach (var entity in ringEntities)
            {
                Point point = CalculateRingPoint(radians, radius);
                Point relativePoint = TranslateRingPoint(point);
                var button = new TwitterUserControl(entity);
                button.Clicked += user_Clicked;
                button.Opacity = 1 / (Convert.ToDouble(depth) * .3);
                button.Title = entity.Name;
                Canvas.SetLeft(button, relativePoint.X);
                Canvas.SetTop(button, relativePoint.Y);
                entityCanvas.Children.Add(button);
                buttonRegistry.Add(entity, button);
                radians = radians + radianStep;
            }

            foreach (var entity in ringEntities)
            {
                foreach (var nextConnection in entity.Friends)
                {
                    if (!buttonRegistry.ContainsKey(nextConnection))
                    {
                        if (!nextRing.Contains(nextConnection))
                        {
                            nextRing.Add(nextConnection);
                        }
                    }
                }
            }
            return nextRing.ToArray();
        }


        private static Point CalculateRingPoint(double radians, double radius)
        {
            double x = Math.Cos(radians) * radius;
            double y = Math.Sin(radians) * radius;
            return new Point(x, y);
        }


        private Point TranslateRingPoint(Point source)
        {
            double centerWidth = entityCanvas.ActualWidth / 2;
            double centerHeight = entityCanvas.ActualHeight / 2;

            double relativeX = source.X + centerWidth;
            double relativeY = source.Y + centerHeight;
            return new Point(relativeX, relativeY);
        }
    }
}
