using Photon.Deterministic;

namespace Quantum
{
    partial class RuntimePlayer
    {
        public AssetRefEntityPrototype TankPrototype;
        public string PlayerName;
        partial void SerializeUserData(BitStream stream)
        {
            // implementation
            stream.Serialize(ref TankPrototype);
            stream.Serialize(ref PlayerName);
        }
    }
}
