using Photon.Deterministic;

namespace Quantum
{
    partial class RuntimePlayer
    {
        public AssetRefEntityPrototype TankPrototype;
        partial void SerializeUserData(BitStream stream)
        {
            // implementation
            stream.Serialize(ref TankPrototype);
        }
    }
}
