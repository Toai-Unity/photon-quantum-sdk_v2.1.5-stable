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

                FPVector3 futurePosition = bulletTransformComp->Position + bulletFieldComp->Direction * bulletData.Speed * frame.DeltaTime;

                bulletTransformComp->Position = FPVector3.Slerp(bulletTransformComp->Position, futurePosition, FP.FromFloat_UNSAFE(0.1f));
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
            Transform3D* bulletTransform = frame.Unsafe.GetPointer<Transform3D>(bullet);
            BulletData data = frame.FindAsset<BulletData>(bulletFields.BulletData.Id);

            FP distancePerFrame = (bulletFields.Direction * data.Speed * frame.DeltaTime).Magnitude;

            Physics3D.HitCollection3D hits = frame.Physics3D.RaycastAll(bulletTransform->Position, bulletFields.Direction, distancePerFrame);

            for (int i = 0; i < hits.Count; i++)
            {
                var entity = hits[i].Entity;
                if (entity != EntityRef.None && frame.Has<Status>(entity) && entity != bulletFields.Source)
                {
                    if (frame.Get<Status>(entity).IsDead)
                    {
                        continue;
                    }
                    bulletTransform->Position = hits[i].Point;
                    // Applies polymorphic behavior on the bullet action
                    data.BulletAction(frame, bullet, entity);
                    return true;
                }

                if (entity == EntityRef.None)
                {
                    bulletTransform->Position = hits[i].Point;
                    // Applies polymorphic behavior on the bullet action
                    data.BulletAction(frame, bullet, EntityRef.None);
                    return true;
                }
            }

            return false;
        }
    }
}
