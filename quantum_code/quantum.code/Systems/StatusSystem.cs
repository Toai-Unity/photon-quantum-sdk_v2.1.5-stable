using Photon.Deterministic;
using System.Threading.Tasks;

namespace Quantum.Systems
{
    public unsafe class StatusSystem : SystemSignalsOnly, ISignalOnTankTakeDamage, ISignalOnTankRespawn
    {
        void ISignalOnTankTakeDamage.OnTankTakeDamage(Frame frame, EntityRef bullet, EntityRef tankGetDamage, FP damage)
        {
            EntityRef shooter = frame.Get<BulletFields>(bullet).Source;
            TakeDamage(frame, shooter, tankGetDamage, damage);
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
            PhysicsCollider3D* collider = frame.Unsafe.GetPointer<PhysicsCollider3D>(tank);
            tankStatus->RespawnTimer = respawnTime;
            tankStatus->IsDead = true;

            // Call event when tank death on Unity side
            frame.Signals.OnTankDeath(tank, killer);   // Dung de cong diem cho nguoi choi
            frame.Events.OnTankDeath(tank, killer);   // Goi toi ben unity side de cap nhat render
            collider->IsTrigger = true;
        }

        void ISignalOnTankRespawn.OnTankRespawn(Frame frame, EntityRef tank)
        {
            Status* status = frame.Unsafe.GetPointer<Status>(tank);
            PhysicsCollider3D* collider = frame.Unsafe.GetPointer<PhysicsCollider3D>(tank);
            StatusData statusData = frame.FindAsset<StatusData>(status->StatusData.Id);

            status->IsDead = false;
            status->CurrentHealth = statusData.MaxHealth;
            collider->IsTrigger = false;
        }
    }
}
