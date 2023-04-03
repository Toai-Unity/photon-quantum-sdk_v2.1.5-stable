
namespace Quantum
{
    public unsafe class ScoreSystem : SystemSignalsOnly, ISignalOnTankDeath
    {
        void ISignalOnTankDeath.OnTankDeath(Frame frame, EntityRef deadTank, EntityRef killer)
        {
            Score* killerScore = frame.Unsafe.GetPointer<Score>(killer);
            Score* deadScore = frame.Unsafe.GetPointer<Score>(deadTank);

            if (killer != deadTank)
            {
                killerScore->Kills += 1;
            }
            deadScore->Deaths += 1;
        }

    }
}
