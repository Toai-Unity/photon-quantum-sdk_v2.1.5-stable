
using Photon.Deterministic;
using System.Diagnostics;

namespace Quantum.Systems
{
    public unsafe class ShootingSystem : SystemMainThread
    {
        public override void Update(Frame frame)
        {
            var tanksFilter = frame.Filter<PlayerId, Transform3D, Status, Shooting>();
            while (tanksFilter.NextUnsafe(out var tank, out var playerIdComp, out var transformComp, out var statusComp, out var shootingComp))
            {
                if (statusComp->IsDead)
                {
                    continue;
                }
                shootingComp->FireRateTimer -= frame.DeltaTime;

                ShootingData shootingData = frame.FindAsset<ShootingData>(shootingComp->ShootingData.Id);

                // Shoot bullet
                if (shootingComp->FireRateTimer <= FP._0)
                {
                    Input* input = frame.GetPlayerInput(playerIdComp->PlayerRef);

                    if (input->Fire.WasPressed)
                    {
                        shootingComp->FireRateTimer = FP._1 / shootingData.FireRate;
                        SpawnBullet(frame, transformComp, tank, shootingComp);
                    }
                }
            }
        }

        private static void SpawnBullet(Frame frame, Transform3D* tankTransform, EntityRef sourceTank, Shooting* shootingComp)
        {
            ShootingData shootingData = frame.FindAsset<ShootingData>(shootingComp->ShootingData.Id);

            BulletData bulletData = frame.FindAsset<BulletData>(shootingData.BulletData.Id);

            EntityPrototype prototypeAsset = frame.FindAsset<EntityPrototype>(bulletData.BulletPrototype.Id.Value);

            // Create bullet
            EntityRef bullet = frame.Create(prototypeAsset);

            BulletFields* bulletFields = frame.Unsafe.GetPointer<BulletFields>(bullet);

            // Setup data for bullet
            frame.Unsafe.TryGetPointer<Transform3D>(bullet, out var bulletTransform);
            bulletTransform->Position = tankTransform->Position + tankTransform->Forward * shootingData.PositionOffset + tankTransform->Up;
            bulletTransform->Rotation = tankTransform->Rotation;
            bulletFields->Direction = tankTransform->Forward;
            bulletFields->Source = sourceTank;
            bulletFields->Time = FP._0;
            bulletFields->SourcePosition = bulletTransform->Position;
        }
    }
}
