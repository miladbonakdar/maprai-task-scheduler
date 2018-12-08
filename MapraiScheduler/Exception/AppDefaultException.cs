using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Log.Error(message, innerException);
        }

        public AppDefaultException(System.Exception innerException) : base(_messageString, innerException)
        {
            Log.Error(_messageString, innerException);
        }
    }
}