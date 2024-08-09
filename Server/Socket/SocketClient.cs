using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SocketClient : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public SocketClient(string ipAddress, int port)
        {
            try
            {
                _client = new TcpClient(ipAddress, port);
                _stream = _client.GetStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }

        public async Task<string> CommunicateWithStreamAsync(string message)
        {
            const int bufferLength = 4096;
            byte[] data = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(data, 0, data.Length);

            byte[] buffer = new byte[bufferLength];
            int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Close();
        }
    }
}

