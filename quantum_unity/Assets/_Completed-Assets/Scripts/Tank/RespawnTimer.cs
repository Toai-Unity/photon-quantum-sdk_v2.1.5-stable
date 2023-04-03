using Quantum;
using TMPro;
using UnityEngine;

namespace Complete
{
    public unsafe class RespawnTimer : QuantumCallbacks
    {
        public TMP_Text textRespawnTimer;
        public GameObject panelRespawnTimer;
        public QuantumGame Game => QuantumRunner.Default.Game;


        private void Awake()
        {
            QuantumEvent.Subscribe<EventOnTankDeath>(this, OnTankDeath);
            QuantumEvent.Subscribe<EventOnTankRespawn>(this, OnTankRespawn);
        }

        private void Start()
        {
            textRespawnTimer.text = "";
        }

        private void Update()
        {
            //if (QuantumRunner.Default == null || QuantumRunner.Default.Game.Frames.Verified == null)
            //    return;

            UpdateTimer();
        }

        private void UpdateTimer()
        {
            Frame frame = QuantumRunner.Default.Game.Frames.Verified;

            var tanks = frame.Filter<Status>();

            while(tanks.NextUnsafe(out var tank, out var statusComp))
            {
                var id = frame.Get<PlayerId>(tank);

                if (Game.PlayerIsLocal(id.PlayerRef))
                {
                    Status tankStatus = frame.Get<Status>(tank);
                    if (tankStatus.RespawnTimer >= 0)
                    {
                        textRespawnTimer.text = string.Format("{0} seconds to respawn", tankStatus.RespawnTimer.AsInt);
                    }
                    else
                    {
                        textRespawnTimer.text = string.Format("{0} seconds to respawn", 0);
                    }
                }
            }
        }

        private void OnTankDeath(EventOnTankDeath eventdata)
        {
            Frame frame = QuantumRunner.Default.Game.Frames.Verified;
            var id = frame.Get<PlayerId>(eventdata.Tank);

            if (Game.PlayerIsLocal(id.PlayerRef))
            {
                panelRespawnTimer.SetActive(true);
            }
        }
        private void OnTankRespawn(EventOnTankRespawn eventdata)
        {
            Frame frame = QuantumRunner.Default.Game.Frames.Verified;
            var id = frame.Get<PlayerId>(eventdata.Tank);

            if (Game.PlayerIsLocal(id.PlayerRef))
            {
                panelRespawnTimer.SetActive(false);
            }
        }
    }

}