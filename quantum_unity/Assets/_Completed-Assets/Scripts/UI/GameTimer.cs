using Quantum;
using TMPro;
using UnityEngine;

public unsafe class GameTimer : QuantumCallbacks
{
    public TMP_Text timerText;

    private void Start()
    {
        timerText.text = "";
    }

    private void Update()
    {
        if (QuantumRunner.Default == null || QuantumRunner.Default.Game.Frames.Verified == null)
            return;

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        Frame frame = QuantumRunner.Default.Game.Frames.Verified;

        GameControllerData gameConfigData = frame.FindAsset<GameControllerData>(QuantumRunner.Default.Game.Configurations.Runtime.GameConfigData.Id);

        GameController gameController = frame.Global->GameController;

        int gameTime = gameController.GameTimer.AsInt;
        int gameDuration = gameConfigData.GameDuration.AsInt;

        if(gameDuration <= gameTime)
        {
            timerText.text = "GAME OVER";
        }
        else
        {
            int timeLeft = gameDuration - gameTime;
            timerText.text = string.Format("{0:00}:{1:00}", timeLeft / 60, timeLeft % 60);
        }
    }
}
