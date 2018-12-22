using Serilog;

namespace MapraiScheduler.Exception
{
    public class AppDefaultException : System.Exception
    {
        private static readonly string _messageString = "somthing bad happend. please check the inner exceptions for mor details";

        public AppDefaultException(string message) : base(message)
        {
            Log.Error(message);
        }

        public AppDefaultException(string message, System.Exception innerException) : base(message, innerException)
        {
            Log.Error(this.ToString(), innerException);
        }

        public AppDefaultException(System.Exception innerException, string thrownFrom = "Unknown") : base($"{_messageString} - thrownFrom : {thrownFrom}", innerException)
        {
            Log.Error(this.ToString(), innerException);
        }

        public sealed override string ToString()
        {
            return base.ToString();
        }
    }
}