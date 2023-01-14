using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PeakHoursClient.Classes
{
    public static class Networking
    {
        public static bool SendRQ(string ID, DateTime time, DateTime timeUTC)
        {
            try
            {
                // Get the remote end point of the server
                IPAddress ipAddr = IPAddress.Parse("");
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddr, 25565);

                // Create the socket
                Socket sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                sock.SendTimeout = 1000;
                sock.ReceiveTimeout = 1000;

                // Connect to the server
                sock.Connect(remoteEndPoint);

                // Exchange
                if (sock.Connected)
                {
                    byte[] idBytes = Encoding.ASCII.GetBytes(ID);
                    byte[] timeBytes = Encoding.ASCII.GetBytes(time.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    byte[] timeUTCBytes = Encoding.ASCII.GetBytes(timeUTC.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    byte[] ok = new byte[2];

                    sock.Send(idBytes);
                    sock.Receive(ok);
                    sock.Send(timeBytes);
                    sock.Send(timeUTCBytes);
                    sock.Receive(ok);
                }

                sock.Shutdown(SocketShutdown.Both);
                sock.Disconnect(true);
                sock.Close();
            }
            catch(Exception e)
            {
                Filesystem.SaveLocalEntry(new Entry(ID, time, timeUTC));
                return false;
            }
            return true;
        }
    }
}
