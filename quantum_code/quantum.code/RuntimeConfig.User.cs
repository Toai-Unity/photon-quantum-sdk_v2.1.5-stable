using Photon.Deterministic;
using System;

namespace Quantum {
    partial class RuntimeConfig {
        public AssetRefGameControllerData GameConfigData;

        partial void SerializeUserData(BitStream stream)
        {
            stream.Serialize(ref GameConfigData);
        }
    }
}