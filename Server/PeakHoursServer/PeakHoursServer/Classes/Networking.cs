using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PeakHoursServer.Classes
{
    public static class Networking
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        public static void Beacon()
        {
            Display.Messages.Add($"[+] Starting Beacon");
            // Open beacon on local ip and port 4444
            IPAddress ipAddr = IPAddress.Parse("0.0.0.0");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 25565);

            try
            {
                // Create socket
                Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.ReceiveTimeout = 1000;
                listener.SendTimeout = 1000;
                listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                // Bind socket to end point
                listener.Bind(localEndPoint);

                // Wait
                Display.Messages.Add($"[+] Beacon Active");
                listener.Listen();
                try
                {
                    while (true)
                    {
                        // Accept a connection and push it to another thread, continue listening.
                        allDone.Reset();
                        Display.Messages.Add($"1");
                        listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                        Display.Messages.Add($"2");
                        allDone.WaitOne();
                        Display.Messages.Add($"3");
                    }
                }
                catch(Exception e)
                {
                    // Fuck
                    Display.Messages.Add($"[!] Beacon Failure 1: {e.Message}");
                }
            }
            catch(Exception e)
            {
                // Fuck again
                Display.Messages.Add($"[!] Beacon Failure 2: {e.Message}");
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Tell the main thread to continue without us
                allDone.Set();

                // Get the socket
                Socket listener = (Socket)ar.AsyncState;
                Socket conn = listener.EndAccept(ar);

                conn.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                conn.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                conn.ReceiveTimeout = 1000;
                conn.SendTimeout = 1000;
                Display.Messages.Add($"\n[!] Connection Acquired");

                // Create the byte buffers for sending and recieving
                byte[] idBuffer = new byte[4];
                byte[] dateTimeBuffer = new byte[22];
                byte[] dateTimeUTCBuffer = new byte[22];
                byte[] acceptBuffer = Encoding.ASCII.GetBytes("OK");

                // Exchange
                conn.Receive(idBuffer);
                conn.Send(acceptBuffer);
                conn.Receive(dateTimeBuffer);
                conn.Receive(dateTimeUTCBuffer);
                conn.Send(acceptBuffer);
                

                // Close
                conn.Shutdown(SocketShutdown.Both);
                conn.Disconnect(true);
                Display.Messages.Add($"[-] Connection Terminated by server");

                // Convert
                string id = Encoding.ASCII.GetString(idBuffer);
                DateTime time = DateTime.Parse(Encoding.ASCII.GetString(dateTimeBuffer));
                DateTime timeUTC = DateTime.Parse(Encoding.ASCII.GetString(dateTimeUTCBuffer));

                Entry e = new Entry(id, time, timeUTC);

                Records.SaveEntry(e);
            }
            catch(Exception e)
            {
                Display.Messages.Add($"[!] Failed to recv: {e.Message}");
            }
            
        }
    }
}
