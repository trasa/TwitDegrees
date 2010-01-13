using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TwitDegrees.GraphViewer.Infrastructure;

namespace TwitDegrees.GraphViewer.Modules.Graph
{
    public class TwitterUserViewModel : ViewModelBase
    {
        private string userName;
        public string UserName { get { return userName; } set { userName = value; InvokePropertyChanged("UserName"); } }
    }
}
