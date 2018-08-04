using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Exception
{
    public class BackgroundTaskException : System.Exception
    {
        public BackgroundTaskException(string message):base(message)
        {
        }
    }
}
