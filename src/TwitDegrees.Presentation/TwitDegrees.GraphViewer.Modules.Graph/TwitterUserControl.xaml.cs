// this is based off of the work by Mike Hodnik, to the point that its practically
// word for word -- this was a big help as a starting off place for this code.
// http://www.kindohm.com/archive/2008/08/13/radial-network-graph-in-silverlight.aspx
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TwitDegrees.GraphViewer.Infrastructure.Model;

namespace TwitDegrees.GraphViewer.Modules.Graph
{
    public partial class TwitterUserControl 
    {
        Color startColor;
        //Color highlightColor;
        readonly TwitterUser twitterUser;
        List<TwitterUserControl> connectedControls;
        List<Line> connectionLines;

        public TwitterUserControl(TwitterUser user)
        {
            InitializeComponent();
            connectedControls = new List<TwitterUserControl>();
            connectionLines = new List<Line>();
            twitterUser = user;
            //highlightColor = Colors.Yellow;
            startColor = endStop.Color;
            MouseLeftButtonDown += EntityControl_MouseLeftButtonDown;
        }

        public TwitterUserControl()
        {
            // for blend
            twitterUser = new TwitterUser {Name = "blend"};
        }


        void EntityControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnClicked();
        }

        public TwitterUser TwitterUser
        {
            get { return twitterUser; }
        }

        public string Title
        {
            get { return titleBlock.Text; }
            set { titleBlock.Text = value; }
        }

        public double RectangleOpacity
        {
            get { return rectangle.Opacity; }
            set { rectangle.Opacity = value; }
        }

        public Color Color
        {
            get { return endStop.Color; }
            set
            {
                endStop.Color = value;
                startColor = endStop.Color;
            }
        }

        public List<Line> ConnectionLines
        {
            get { return connectionLines; }
            set { connectionLines = value; }
        }

        public List<TwitterUserControl> ConnectedControls
        {
            get { return connectedControls; }
            set { connectedControls = value; }
        }

        public void Highlight()
        {
            highlightStoryboard.Begin();
        }

        void HighlightSecondary()
        {
            highlightSecondaryStoryboard.Begin();
        }

        public void UnHighlight()
        {
            unHighlightAnimation.To = startColor;
            unHighlightStoryboard.Begin();
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Highlight();
            foreach (var control in connectedControls)
            {
                control.HighlightSecondary();
            }
            foreach (Line line in connectionLines)
            {
                line.Opacity = .6;
                line.StrokeThickness = 4;
                var brush = line.Stroke as SolidColorBrush;
                if (brush != null) brush.Color = Colors.White;
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            UnHighlight();
            foreach (var control in connectedControls)
            {
                control.UnHighlight();
            }
            foreach (Line line in connectionLines)
            {
                line.Opacity = .3;
                line.StrokeThickness = 1;
                var brush = line.Stroke as SolidColorBrush;
                if (brush != null) brush.Color = Colors.Black;
            }
        }

        public event EventHandler<TwitterUserControlClickedEventArgs> Clicked;

        protected void OnClicked()
        {
            if (Clicked != null)
            {
                var args = new TwitterUserControlClickedEventArgs(TwitterUser);
                Clicked(this, args);
            }
        }
    }

    public class TwitterUserControlClickedEventArgs : EventArgs
    {
        public TwitterUserControlClickedEventArgs(TwitterUser user)
        {
            TwitterUser = user;
        }

        public TwitterUser TwitterUser { get; set; }
    }
}
