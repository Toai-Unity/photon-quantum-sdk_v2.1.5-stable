using Quantum;
using UnityEngine;

namespace Complete
{
    public class CameraControl : QuantumCallbacks
    {
        [HideInInspector] public QuantumGame Game => QuantumRunner.Default.Game;

        public float smoothTime = 0.3f;
        public Vector3 lookOffset = Vector3.zero;



        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnUpdateView(QuantumGame game)
        {
            base.OnUpdateView(game);
            
            Vector3 currentPosition = transform.position;
            if(PlayerView.LocalTankTransform != null)
            {
                Vector3 targetPosition = PlayerView.LocalTankTransform.position + lookOffset;
                transform.position = Vector3.Slerp(currentPosition, targetPosition, smoothTime);
            }
        }

    }
}