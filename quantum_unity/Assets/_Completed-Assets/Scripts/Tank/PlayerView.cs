using Quantum;
using UnityEngine;

namespace Complete
{
    public class PlayerView : QuantumCallbacks
    {
        private QuantumGame Game => QuantumRunner.Default.Game;

        public static System.Action<PlayerView> onLocalPlayerInstantiated;

        public static EntityView LocalEntityView = null;

        public static Transform LocalTankTransform = null;

        private void Start()
        {
            var f = Game.Frames.Verified;
            EntityView entityView = GetComponent<EntityView>();

            var playerID = f.Get<PlayerId>(entityView.EntityRef);

            if (Game.PlayerIsLocal(playerID.PlayerRef))
            {
                LocalEntityView = entityView;
                LocalTankTransform = LocalEntityView.GetComponent<Transform>();
            }
        }

    }
}
