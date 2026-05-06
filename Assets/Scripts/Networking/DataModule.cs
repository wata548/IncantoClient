using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Networking {
    public class DataModule: IDisposable {
        
       //==================================================||Fields 
        public const string ServerIp = "1.237.69.214";
        public const int ServerPort = 51321;

        private readonly ConcurrentQueue<string> _receives = new();
        private readonly IPEndPoint _serverAddress;
        private readonly UdpClient _client;
        private readonly CancellationTokenSource _receiveTokenSource;

        #if UNITY_EDITOR
        public string LastMessage =>
            _receives.TryDequeue(out var message) 
                ? message 
                : "";
        #endif
        
       //==================================================||Constructors 
        public DataModule() {
            _serverAddress = new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort);
            _client = new();
            _receiveTokenSource = new CancellationTokenSource();
            var token = _receiveTokenSource.Token;
            Task.Run(() => Receive(token), token);
        }

        private async Task Receive(CancellationToken pToken) {
            while (!pToken.IsCancellationRequested) {
                var result = await _client.ReceiveAsync();
                var message = Encoding.UTF8.GetString(result.Buffer);
                _receives.Enqueue(message);
            }
        }

        public void SendRaw(object pData) {
            var bytes = Encoding.UTF8.GetBytes(pData.ToString());
            _client.Send(bytes, bytes.Length, _serverAddress);
        }
        
        public void Send(object pData) {
            var context = JsonConvert.SerializeObject(pData);
            var bytes = Encoding.UTF8.GetBytes(context);
            _client.Send(bytes, bytes.Length);
        }

        public void Dispose() {
            _receiveTokenSource.Cancel();
            _receiveTokenSource.Dispose();
            
            _client.Close();
            _client.Dispose();
        }
    }
}