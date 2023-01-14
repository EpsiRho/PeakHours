using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("[1] Server");
Console.WriteLine("[2] Client");
string input = Console.ReadLine();
if(input == "1")
{
    Console.WriteLine("[!] Server Mode");
    Beacon();
}
else
{
    Console.WriteLine("[!] Client Mode");
    Client();
}

static void Client()
{
    IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
    IPEndPoint remoteEndPoint = new IPEndPoint(ipAddr, 42);

    while (true)
    {
        try
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            
            Socket sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(remoteEndPoint);

            // Get bytes and send
            byte[] encodedMSG = Encoding.ASCII.GetBytes(input);
            int len = encodedMSG.Length;
            byte[] encodedLen = Encoding.ASCII.GetBytes($"{len}");

            sock.Send(encodedLen);

            byte[] getOK = new byte[2];
            sock.Receive(getOK);
            string isOK = Encoding.ASCII.GetString(getOK);
            
            if(isOK == "OK")
            {
                sock.Send(encodedMSG);
            }

            sock.Close();


        }
        catch (Exception e)
        {

        }
    }
}

static void Beacon()
{
    IPAddress ipAddr = IPAddress.Parse("0.0.0.0");
    IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 42);

    while (true)
    {
        try
        {
            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);

            listener.Listen(10);
            try
            {
                var conn = listener.Accept();

                byte[] bytesToRecv = new byte[16];
                conn.Receive(bytesToRecv);
                int num = Convert.ToInt16(Encoding.ASCII.GetString(bytesToRecv));

                Console.WriteLine($"[!] {num} byte message incoming, accept?");
                Console.Write("[y/n] > ");
                string input = Console.ReadLine();
                if(input == "y")
                {
                    byte[] sendOK = Encoding.ASCII.GetBytes("OK");
                    conn.Send(sendOK);

                    byte[] buffer = new byte[num];
                    conn.Receive(buffer);
                    string recv = new string(Encoding.ASCII.GetChars(buffer));
                    Console.WriteLine($"[+] Recieved Message: \"{recv}\"");
                }
                else
                {
                    byte[] sendNO = Encoding.ASCII.GetBytes("NO");
                    conn.Send(sendNO);
                }
                listener.Close();
                conn.Close();
            }
            catch (Exception)
            {

            }
        }
        catch (Exception)
        {

        }
    }
}