
using Photon.Realtime;
using Quantum;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public unsafe class ScoreBoardUI : QuantumCallbacks
{
    [Serializable]
    private class RowScoreUI
    {
        public TMP_Text textName;
        public TMP_Text textKill;
        public TMP_Text textDeath;

        public void SetData(string name, int kill, int death)
        {
            textName.text = name;
            textKill.text = kill.ToString();
            textDeath.text = death.ToString();
        }

        public void ClearData()
        {
            textName.text = "";
            textKill.text = "";
            textDeath.text = "";
        }
    }

    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private List<RowScoreUI> rowsScore = new List<RowScoreUI>();

    private List<EntityRef> sortedTanks = new List<EntityRef>();

    private void Start()
    {
        QuantumEvent.Subscribe<EventOnTankDeath>(this, HandleTankDeath);
    }

    private void UpdateScoreBoard()
    {
        sortedTanks.Clear();

        var frame = QuantumRunner.Default.Game.Frames.Verified;
        foreach (var (tankRef, playerId) in frame.Unsafe.GetComponentBlockIterator<PlayerId>())
        {
            sortedTanks.Add(tankRef);
        }

        sortedTanks.Sort((a, b) =>
        {
            var ra = frame.Get<Score>(a);
            var rb = frame.Get<Score>(b);

            if (ra.Kills != rb.Kills)
            {
                return rb.Kills - ra.Kills;
            }
            return ra.Deaths - rb.Deaths;
        });

        ClearScoreBoard();

        for (int i = 0; i < sortedTanks.Count; i++)
        {
            UpdateRowScoreUI(sortedTanks[i], i);
        }
    }

    private void HandleTankDeath(EventOnTankDeath eventData)
    {
        UpdateScoreBoard();
    }

    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener(this);
    }

    private void UpdateRowScoreUI(EntityRef tank, int index)
    {
        var frame = QuantumRunner.Default.Game.Frames.Verified;
        if (!frame.Exists(tank))
            return;

        var id = frame.Get<PlayerId>(tank);
        var data = frame.GetPlayerData(id.PlayerRef);
        var score = frame.Get<Score>(tank);

        rowsScore[index].SetData(data.PlayerName, score.Kills, score.Deaths);
    }

    private void ClearScoreBoard()
    {
        for(int i=0;i<rowsScore.Count; i++)
        {
            rowsScore[i].ClearData();
        }
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.G))
        {
            UpdateScoreBoard();
            scoreBoard.SetActive(true);
        }
        if(UnityEngine.Input.GetKeyUp(KeyCode.G))
        {
            scoreBoard.SetActive(false);
        }
    }
}
