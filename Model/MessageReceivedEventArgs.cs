using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz
{
    public class MessageReceivedEventArgs : EventArgs
    {

        public Message Message { get; }

        public MessageReceivedEventArgs(Message message)
        {
            Message = message;
        }

    }
}
