

using Photon.Deterministic;

namespace Quantum
{
    public unsafe class MovementSystem : SystemMainThread
    {
        public override void Update(Frame frame)
        {
            var tanksFilter = frame.Filter<Transform3D, PlayerId, Status,Movement, CharacterController3D>();

            while(tanksFilter.NextUnsafe(out var tank, out var transform, out var player, out var status,out var movement, out var kcc))
            {
                if (status->IsDead)
                {
                    continue;
                }

                Input* input = frame.GetPlayerInput(player->PlayerRef);
                FPVector3 direction = new FPVector3(input->Direction.X, FP._0, input->Direction.Y).Normalized;

                MoveData moveData = frame.FindAsset<MoveData>(movement->MoveData.Id);
                // Apply Move
                kcc->Move(frame, tank, direction);

                // Apply Rotate
                if(direction != FPVector3.Zero)
                {
                    FPQuaternion lookTo = FPQuaternion.LookRotation(direction, transform->Up);
                    FPVector3 rotation = FPQuaternion.Slerp(transform->Rotation, lookTo, frame.DeltaTime * moveData.SpeedRotate).AsEuler;
                    transform->Rotation = FPQuaternion.Euler(rotation);
                }
            }
        }
    }
}
