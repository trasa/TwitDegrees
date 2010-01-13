using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitDegrees.Core.Messaging
{
    public interface ITwitterQueue
    {
        void BeginReceive();
    }
}
