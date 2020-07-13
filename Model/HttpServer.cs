using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Quiz
{
    public class HttpServer
    {

        public event EventHandler<MessageReceivedEventArgs> IncomingMessage;

        private readonly HttpListener server = new HttpListener();

        public HttpServer(string ip, int port)
        {
            server.Prefixes.Add($"http://{ip}:{port}/");
        }

        public void StartListening()
        {
            server.Start();
            HttpListenerPrefixCollection prefixes = server.Prefixes;
            Manager.WriteLine($"Server started at: ");
            foreach (string prefix in prefixes)
            {
                Console.WriteLine($"\t{prefix}");
            }

            new Thread(() =>
            {
                while (true)
                {
                    HttpListenerContext context = server.GetContext();
                    Thread thread = new Thread(new ParameterizedThreadStart(HandleRequest));
                    thread.Start(context);
                }
            }).Start();
        }

        public void StopListening()
        {
            server.Stop();
        }


        protected virtual void OnMessage(Message m)
        {
            EventHandler<MessageReceivedEventArgs> handler = IncomingMessage;
            if (handler == null)
            {
                return;
            }
            handler?.Invoke(this, new MessageReceivedEventArgs(m));
        }

        private void HandleRequest(object obj)
        {
            HttpListenerContext context = (HttpListenerContext)obj;
            HttpListenerResponse response = context.Response;

            using StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
            string req = reader.ReadToEnd();

            if (req.Length == 0)
            {
                HandlePageLoad(context);
            }
            else
            {
                OnMessage(new Message(req));
                response.StatusCode = 204;
                response.Close();
            }
        }

        private void HandlePageLoad(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            Manager.WriteLine($"User connected from: {request.RemoteEndPoint.ToString()}");
            byte[] buffer = Encoding.UTF8.GetBytes(File.ReadAllText(@"\index.html"));
            response.ContentLength64 = buffer.Length;
            using Stream stream = response.OutputStream;
            stream.Write(buffer, 0, buffer.Length);
            response.StatusCode = 200;
            response.Close();
        }

    }
}
