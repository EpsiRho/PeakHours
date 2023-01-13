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
        public static void SendRQ(string ID, DateTime time)
        {
            // Get the remote end point of the server
            IPAddress ipAddr = IPAddress.Parse("");
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddr, 69420);

            // Create the socket
            Socket sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the server
            sock.Connect(remoteEndPoint);

            // Exchange
            if(sock.Connected)
            {
                //byte[] ID = 
                //sock.Send()
            }
        }
    }
}
