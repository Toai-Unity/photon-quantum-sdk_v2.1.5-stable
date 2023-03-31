

using Photon.Deterministic;

namespace Quantum
{
    public unsafe class BulletSystem : SystemMainThread
    {
        // Check Collision 
        public override void Update(Frame frame)
        {
            var bulletsFieldsFilter = frame.Filter<Transform3D, BulletFields>();
            while(bulletsFieldsFilter.NextUnsafe(out var bullet, out var bulletTransformComp, out var bulletFieldComp))
            {
                // Check if bullet collision with other entity
                if(CheckBulletCollisionWithRaycast(frame, bullet))
                {
                    continue;
                }

                BulletData bulletData = frame.FindAsset<BulletData>(bulletFieldComp->BulletData.Id);

                bulletTransformComp->Position += bulletFieldComp->Direction * bulletData.Speed  * frame.DeltaTime;
                bulletFieldComp->Time += frame.DeltaTime;

                FP distance = FPVector3.Distance(bulletTransformComp->Position, bulletFieldComp->SourcePosition);
                bool isBulletOutOfRange = distance > bulletData.Range;

                if(isBulletOutOfRange)
                {
                    bulletData.BulletAction(frame, bullet, EntityRef.None);
                }
            }
        }

        private bool CheckBulletCollisionWithRaycast(Frame frame, EntityRef bullet)
        {
            BulletFields bulletFields = frame.Get<BulletFields>(bullet);
            if (bulletFields.Direction.Magnitude <= 0)
            {
                return false;
            }


            return false;
        }
    }
}
