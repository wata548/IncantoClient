namespace Networking {
	public interface IPacketEvent {
		public void Invoke(PacketData pPacket);
	}
}