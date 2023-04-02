using Photon.Deterministic;

namespace Quantum
{
    public partial class BulletData
    {
        public AssetRefEntityPrototype BulletPrototype;
        public FP Speed;
        public FP Damage;
        public FP Range;

        public unsafe void BulletAction(Frame frame, EntityRef bullet, EntityRef targerTank)
        {
            // Do some thing
            if(targerTank != EntityRef.None)
            {
                Log.Info("Start Damage");
                // Call signals damage on this tank
                frame.Signals.OnTankTakeDamage(bullet, targerTank, Damage);
            }

            frame.Destroy(bullet);
        }
    }
}
