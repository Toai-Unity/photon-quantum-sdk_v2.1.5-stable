using Photon.Deterministic;

namespace Quantum
{
    public unsafe class RespawnSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            foreach (var (tank, tankStatus) in f.Unsafe.GetComponentBlockIterator<Status>())
            {
                if (tankStatus->IsDead)
                {
                    tankStatus->RespawnTimer -= f.DeltaTime;
                    if (tankStatus->RespawnTimer <= FP._0)
                    {
                        RespawnHelper.Respawn(f, tank);
                    }
                }
            }
        }
    }
}
