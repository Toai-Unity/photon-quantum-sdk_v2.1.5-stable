using Photon.Deterministic;

namespace Quantum
{
    public static unsafe class RespawnHelper
    {
        public static void RespawnTank(Frame frame, EntityRef robot)
        {
            FPVector3 position = FPVector3.One * 4;
            int spawnCount = frame.ComponentCount<SpawnIdentifier>();
            if (spawnCount != 0)
            {
                int index = frame.RNG->Next(0, spawnCount);
                int count = 0;
                foreach (var (spawn, spawnIdentifier) in frame.Unsafe.GetComponentBlockIterator<SpawnIdentifier>())
                {
                    if (count == index)
                    {
                        Transform3D spawnTransform = frame.Get<Transform3D>(spawn);
                        position = spawnTransform.Position;
                        break;
                    }
                    count++;
                }
            }

            Transform3D* robotTransform = frame.Unsafe.GetPointer<Transform3D>(robot);

            robotTransform->Position = position;
        }
    }
}
