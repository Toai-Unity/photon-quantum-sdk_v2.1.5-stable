

using Photon.Deterministic;

namespace Quantum
{
    public unsafe class StatusSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
        }

        private static void TakeDamage(Frame frame, EntityRef sourceEnemyDamage, EntityRef tank, FP damage)
        {
            Status* tankStatusComp = frame.Unsafe.GetPointer<Status>(tank);
            StatusData statusData = frame.FindAsset<StatusData>(tankStatusComp->StatusData.Id);

            // Do something to take damage

            // Check if tank die
            if (tankStatusComp-> CurrentHealth <= 0)
            {
                KillRobot(frame, sourceEnemyDamage, tank, statusData.TimeToRespawn);
            }

        }

        private static void KillRobot(Frame frame, EntityRef killer, EntityRef robot, FP respawnTime)
        {

        }
    }
}
