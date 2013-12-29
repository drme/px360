using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ThW.X360.Controller.Windows
{
    /// <summary>
    /// Waits for commands from cell phone. Listens on UDP port.
    /// </summary>
    internal class RemoteService 
    {
        public RemoteService(Controller controller, Config config)
        {
            this.controller = controller;

            config.RemoteServerChanged += this.RemoteServerPortChanged;
        }

        private void RemoteServerPortChanged(Config config, EventArgs args)
        {
            Stop();

            this.port = config.RemoteServerPort;

            if (port > 0)
            {
                Start();
            }
        }

        public void Start()
        {
            if (false == this.running)
            {
                this.running = true;

                Thread thread = new Thread(Work);

                thread.Start();
            }
        }

        private void Work()
        {
#if WINDOWS
            byte[] buffer = new byte[1024];
            
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, this.port);

            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Bind(endPoint);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint clientEndPoint = (EndPoint)(sender);

            while (true == this.running)
            {
                try
                {
                    int received = socket.ReceiveFrom(buffer, ref clientEndPoint);

                    if (received > 0)
                    {
                        String command = Encoding.ASCII.GetString(buffer, 0, received);

                        Console.WriteLine("Received Data " + clientEndPoint.ToString() + " {0}", command);

                        this.controller.SendState(new X360State(command));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            
            socket.Close();
#endif
        }

        public void Stop()
        {
            this.running = false;
#if WINDOWS            
            if (null != this.socket)
            {
                this.socket.Close();
            }
#endif
        }

        private bool running = false;
        private int port = 9999;
        private Controller controller = null;
#if WINDOWS
        private Socket socket = null;
#endif
    }
}
