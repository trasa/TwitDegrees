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
using System.Windows.Threading;

namespace TwitDegrees.GraphViewer.Infrastructure.Services
{
    public static class DispatchHostLocator
    {
        public static DispatchHost DispatchHost { get; set; }
    }


    public class DispatchHost
    {
        public DispatchHost(Dispatcher dispatcher)
        {
            RootDispatcher = dispatcher;
        }

        public Dispatcher RootDispatcher { get; private set; }

        /// <summary>
        /// Used to invoke an action against the UI Thread so that UI elements can be altered safely.
        /// </summary>
        public void BeginInvoke(Action a)
        {
            RootDispatcher.BeginInvoke(a);
        }
    }
}
