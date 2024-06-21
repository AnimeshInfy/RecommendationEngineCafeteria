using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class SocketClient
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public SocketClient(string ipAddress, int port)
        {
            try
            {
                _client = new TcpClient(ipAddress, port);
                _stream = _client.GetStream();
                Console.WriteLine("Connected to server.");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
                
        }

        public void SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                _stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
          
        }

        public string ReceiveMessage()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = _stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
    }
}
