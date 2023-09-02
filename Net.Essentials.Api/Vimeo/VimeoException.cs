using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo
{
    public class VimeoException : Exception
    {
        public VimeoException(string message) : base(message)
        {
        }

        public VimeoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
