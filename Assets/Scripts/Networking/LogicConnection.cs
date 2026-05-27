using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Extension;
using Extensions;

namespace Networking {
	public class LogicConnection: MonoSingleton<LogicConnection>, IDisposable {

		//==================================================||Fields 
		public const string ServerIp = "1.237.69.214";
		public const int ServerPort = 51321;

		public event Action<PacketData> OnReceive;
		public event Action<PacketData> OnReceiveInGame;
		
		private readonly ConcurrentQueue<byte[]> _receives = new();
		private readonly IPEndPoint _serverAddress;
		private readonly UdpClient _client;
		private readonly CancellationTokenSource _receiveTokenSource;
 
		//==================================================||Constructors 
		
		public LogicConnection() {
			_serverAddress = new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort);
			_client = new();
			_receiveTokenSource = new CancellationTokenSource();
			var token = _receiveTokenSource.Token;
			Task.Run(() => Receive(token), token);
		}

		
		//==================================================||Methods	

		public void GameStart() {
			OnReceiveInGame = null;
		}

		private async Task Receive(CancellationToken pToken) {
			while (!pToken.IsCancellationRequested) {
				var result = await _client.ReceiveAsync();
				_receives.Enqueue(result.Buffer);
			}
		}

		public void SendRaw(object pData) {
			var bytes = Encoding.UTF8.GetBytes(pData.ToString());
			_client.Send(bytes, bytes.Length, _serverAddress);
		}
        
		public void Send(byte[] pBytes) {
			_client.Send(pBytes, pBytes.Length, _serverAddress);
		}

		public void Dispose() {
			_receiveTokenSource.Cancel();
			_receiveTokenSource.Dispose();
            
			_client.Close();
			_client.Dispose();
		}
		
		//==================================================||Unity	
		private void OnDestroy() {
			Dispose();
		}

		private void Update() {
			while (!_receives.IsEmpty) {
				if(!_receives.TryDequeue(out var data))
					continue;
				var packet = PacketData.Generate(data);
				OnReceive?.Invoke(packet);			
				OnReceiveInGame?.Invoke(packet);			
			}
		}

	}
}