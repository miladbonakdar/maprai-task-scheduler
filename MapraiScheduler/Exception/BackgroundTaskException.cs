using Serilog;

namespace MapraiScheduler.Exception
{
    public class BackgroundTaskException : System.Exception
    {
        public BackgroundTaskException(string message) : base(message)
        {
            Log.Error(message);
        }
    }
}