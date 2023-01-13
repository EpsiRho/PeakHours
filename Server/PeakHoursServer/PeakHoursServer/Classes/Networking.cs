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
            // Open beacon on local ip and port 69420
            IPAddress ipAddr = IPAddress.Parse("0.0.0.0");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 69420);

            try
            {
                // Create socket
                Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Bind socket to end point
                listener.Bind(localEndPoint);

                // Wait
                Display.Messages.Add($"[+] Beacon Active");
                listener.Listen(10);
                try
                {
                    while (true)
                    {
                        // Accept a connection and push it to another thread, continue listening.
                        allDone.Reset();
                        listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                        allDone.WaitOne();
                    }
                }
                catch(Exception e)
                {
                    // Fuck
                    Console.WriteLine(e.Message);
                }
            }
            catch(Exception e)
            {
                // Fuck again
                Console.WriteLine(e.Message);
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            // Tell the main thread to continue without us
            allDone.Set();

            // Get the socket
            Socket listener = (Socket)ar.AsyncState;
            Socket conn = listener.EndAccept(ar);
            Display.Messages.Add($"[!] Connection Acquired");

            // Create the byte buffers for sending and recieving
            byte[] idBuffer = new byte[4];
            byte[] dateTimeBuffer = new byte[22];
            byte[] acceptBuffer = Encoding.ASCII.GetBytes("OK");

            // Exchange
            conn.Receive(idBuffer);
            conn.Send(acceptBuffer);
            conn.Receive(dateTimeBuffer);

            // Close
            conn.Close();
            Display.Messages.Add($"[+] Connection Terminated by server");

            // Convert
            string id = Encoding.ASCII.GetString(idBuffer);
            DateTime time = DateTime.Parse(Encoding.ASCII.GetString(dateTimeBuffer));

            Entry e = new Entry(id, time);

            Records.SaveEntry(e);

            Display.Messages.Add($"[!] Entry Saved");
        }
    }
}
