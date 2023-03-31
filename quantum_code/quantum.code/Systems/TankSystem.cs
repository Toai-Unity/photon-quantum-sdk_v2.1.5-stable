

using Photon.Deterministic;

namespace Quantum
{
    public unsafe class TankSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
    {
        public void OnPlayerDataSet(Frame frame, PlayerRef player)
        {
            RuntimePlayer data = frame.GetPlayerData(player);
            EntityPrototype prototypeAsset = frame.FindAsset<EntityPrototype>(data.TankPrototype.Id.Value);
            EntityRef tank = frame.Create(prototypeAsset);

            PlayerId* playerid = frame.Unsafe.GetPointer<PlayerId>(tank);
            playerid->PlayerRef = player;

            frame.Unsafe.TryGetPointer<Transform3D>(tank, out var tankTransform);
            tankTransform->Position = FPVector3.Zero + FPVector3.One;

            // Set position for tank
            //RespawnHelper.RespawnTank(frame, tank);

            // call events

        }
    }
}
