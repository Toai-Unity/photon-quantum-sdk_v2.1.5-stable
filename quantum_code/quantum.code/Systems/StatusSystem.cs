using Photon.Deterministic;

namespace Quantum.Systems
{
    public unsafe class StatusSystem : SystemMainThread, ISignalOnTankTakeDamage, ISignalOnTankRespawn
    {
        void ISignalOnTankTakeDamage.OnTankTakeDamage(Frame frame, EntityRef bullet, EntityRef tankGetDamage, FP damage)
        {
            EntityRef shooter = frame.Get<BulletFields>(bullet).Source;
            TakeDamage(frame, shooter, tankGetDamage, damage);
        }

        public override void Update(Frame frame)
        {
        }

        private void TakeDamage(Frame frame, EntityRef enemy, EntityRef tank, FP damage)
        {
            Status* tankStatus = frame.Unsafe.GetPointer<Status>(tank);
            StatusData statusData = frame.FindAsset<StatusData>(tankStatus->StatusData.Id);

            tankStatus->CurrentHealth -= damage;

            frame.Events.OnTankTakeDamage(tank, damage);

            if (tankStatus->CurrentHealth <= 0)
            {
                KillTank(frame, enemy, tank, statusData.TimeToRespawn);
            }
        }

        private void KillTank(Frame frame, EntityRef killer, EntityRef tank, FP respawnTime)
        {
            Status* tankStatus = frame.Unsafe.GetPointer<Status>(tank);
            tankStatus->RespawnTimer = respawnTime;
            tankStatus->IsDead = true;

            // Call event when tank death on Unity side
            frame.Signals.OnTankDeath(tank, killer);
            frame.Events.OnTankDeath(tank, killer);
        }

        void ISignalOnTankRespawn.OnTankRespawn(Frame frame, EntityRef robot)
        {
            Status* status = frame.Unsafe.GetPointer<Status>(robot);
            StatusData statusData = frame.FindAsset<StatusData>(status->StatusData.Id);

            status->IsDead = false;
            status->CurrentHealth = statusData.MaxHealth;
        }
    }
}
