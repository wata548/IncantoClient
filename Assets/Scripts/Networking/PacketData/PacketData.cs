using System;
using System.Text;
using Newtonsoft.Json;

namespace Networking {
    public class Packet {
        public int Id { get; set; }
        public PacketCommand Command { get; set; }
        public IPacketData[] Args { get; set; } = {};

        private static readonly JsonSerializerSettings _serializeSetting = new() {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static byte[] Serialize(Packet[] pContext) =>
            Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(pContext,_serializeSetting)
            );
        public static byte[] Serialize(Packet pContext) =>
            Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(pContext,_serializeSetting)
            );
        public static Packet[] DeserializeManyData(byte[] pBytes) =>
            JsonConvert.DeserializeObject<Packet[]>(
                Encoding.UTF8.GetString(pBytes),
                _serializeSetting
            );
        public static Packet Deserialize(byte[] pBytes) =>
            JsonConvert.DeserializeObject<Packet>(
                Encoding.UTF8.GetString(pBytes),
                _serializeSetting
            );
    }

    public enum PacketCommand {
        //S -> C
        IdentifyPlayer,
        GameStart,
        SpawnMagic,
        SendResult,
        
        //C -> S
        NATPunch,
        SelectMagic,
        JudgeMagic,
        
        //Shared
        Move,
        Death,
        Rebirth,
        
    }

    public interface IPacketData {}

    public class ValueData<T> : IPacketData {
        public T Value { get; set; }
    }
    
    public class VectorData: IPacketData {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
    [Flags]
    public enum InputFlags {
        Forward    = 0b1,
        Backward   = 0b10,
        Left       = 0b100,
        Right      = 0b1000,
        Focus      = 0b10000,
    }

    public class PlayerData: IPacketData {
        
      public InputFlags Input { get; set; }
      public VectorData Pos { get; set; }
      public VectorData Rotation { get; set; }
      public VectorData MouseDelta { get; set; }
      public string Paint { get; set; }
      public bool IsPainting { get; set; }
    } 
    
}