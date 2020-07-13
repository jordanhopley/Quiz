using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz
{
    public class Message
    {

        public enum MessageType
        {
            NME,
            BTN
        }

        public MessageType Type { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }

        public Message(MessageType type, string name, string time)
        {
            Type = type;
            Name = name;
            Time = time;
        }

        public Message(string request)
        {
            Type = (MessageType)Enum.Parse(typeof(MessageType), request[..3]);
            Name = request[(request.IndexOf('|') + 1)..request.LastIndexOf('|')];
            Time = request[(request.LastIndexOf('|') + 1)..];
        }

        override
        public string ToString()
        {
            return $"Message\n\tType: {Type}\n\tName: {Name}\n\tTime: {Time}";
        }

    }
}
