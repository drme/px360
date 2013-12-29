using System;
using System.Diagnostics;
#if WINDOWS
using System.IO.Ports;
using System.Threading;
using System.Net.Sockets;
using System.Net;
#endif

namespace ThW.X360.Controller.Windows
{
    public class Controller : IDisposable
    {
        public Controller(Config config)
        {
            this.config = config;
            this.config.ArduinoPortChanged += this.ArduinoPortChanged;
            this.state = new X360State(this.config);
        }

        private void ArduinoPortChanged(Config config, EventArgs e)
        {
            ChangePort(config.ArduinoPort);
        }
#if WINDOWS
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Debug.Write(this.port.ReadExisting());
        }
#endif
        public X360State GetState(Bindings bindings, int w, int h)
        {
            this.state.UpdateState(bindings, w, h);

            return this.state;
        }

        public void SendState(X360State state)
        {
            byte[] data = state.GetState();

            if (null != this.netduinoPlusAddress)
            {
                Debug.WriteLine("Sending:" + state.ToString());
#if WINDOWS
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                SocketAsyncEventArgs socketArgs = new SocketAsyncEventArgs();

                socketArgs.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(this.netduinoPlusAddress), 9999);
                socketArgs.SetBuffer(data, 0, data.Length);

                socketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e) { });

                socket.SendToAsync(socketArgs);
#endif
            }
            else
            {
#if WINDOWS
                try
                {
                    if ((false == IsTheSame(data)) || (null == this.port))
                    {
                        if (null == this.port)
                        {
                            if (this.framesToIgnore-- <= 0)
                            {
                             //   if (Array.Exists<String>(SerialPort.GetPortNames(), s => s == this.portName))
                                {
                                    this.port = new SerialPort(this.portName, 115200/* this.config.ArduinoPortBaud*/, Parity.None, 8, StopBits.One);
                                    this.port.DataReceived += this.DataReceived;
                                    this.port.Open();
                                    Thread.Sleep(2000); // wait for bloody arduino nano.
                                }
                            }
                        }

                        if (null != this.port)
                        {
                            this.port.Write(data, 0, data.Length);

                            this.lastState = data;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.port = null;
                    Debug.WriteLine(ex.Message);
                    this.framesToIgnore = 60; // too lazy to do some tread sleep magic.
                }
#endif
            }
        }

        private void ChangePort(String newPort)
        {
#if WINDOWS
            if (null != this.port)
            {
                try
                {
                    this.port.DataReceived -= this.DataReceived;
                    this.port.Close();
                }
                catch (Exception)
                {
                }
            }
            this.port = null;
            this.portName = newPort;
#endif
        }

        private bool IsTheSame(byte[] state)
        {
            if (null == this.lastState)
            {
                return false;
            }

            for (int i = 0; i < this.lastState.Length; i++)
            {
                if (state[i] != this.lastState[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool Connected
        {
            get
            {
#if WINDOWS
                if (null == this.port)
                {
                    return false;
                }

                if (false == this.port.IsOpen)
                {
                    return false;
                }
#endif
                return true;
            }
        }

        public void Dispose()
        {
#if WINDOWS
            if (null != this.port)
            {
                this.port.Dispose();
                this.port = null;
            }
#endif
        }

        private int framesToIgnore = 0;
        private byte[] lastState = null;
        private String portName = "COM12";
        private X360State state = null;
#if WINDOWS
        private SerialPort port = null;
#endif
        private Config config = null;
        private String netduinoPlusAddress = null;//"10.3.1.145";
    }
}
