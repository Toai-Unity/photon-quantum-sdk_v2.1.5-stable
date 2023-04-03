using Photon.Deterministic;
using Quantum.Systems;

namespace Quantum
{
    public unsafe class GameControllerSystem : SystemMainThread, ISignalOnGameEnded
    {
        public void OnGameEnded(Frame frame, GameController* gameController)
        {
            frame.Global->GameController.GameTimer = FP._0;
        }

        public override void Update(Frame frame)
        {
            GameControllerData gameConfigData = frame.FindAsset<GameControllerData>(frame.RuntimeConfig.GameConfigData.Id);

            if(frame.Global->GameController.GameTimer >= gameConfigData.GameDuration)
            {
                frame.Signals.OnGameEnded(&frame.Global->GameController);
                frame.Events.OnGameEnded();
            }
            else
            {
                frame.Global->GameController.GameTimer += frame.DeltaTime;
            }
        }

        void ISignalOnGameEnded.OnGameEnded(Frame frame, GameController* gameController)
        {
            frame.SystemDisable<ShootingSystem>();
            frame.SystemDisable<MovementSystem>();
            frame.SystemDisable<BulletSystem>();
        }
    }
}
