using Quantum.Demo;
using Quantum;
using UnityEngine;

public class LocalPlayer : QuantumCallbacks
{
    public AssetRefEntityPrototype Prototype;
    public override void OnGameStart(QuantumGame game)
    {
        if (game.Session.IsPaused) return;
        Application.targetFrameRate = 60;
        SendLocalPlayers(game);
    }

    private void SendLocalPlayers(QuantumGame game)
    {
        var localPlayers = game.GetLocalPlayers();
        for (var i = 0; i < localPlayers.Length; ++i)
        {
            var data = new Quantum.RuntimePlayer();

            if (UIMain.Client != null && UIMain.Client.IsConnected)
            {

                data.PlayerName = UIMain.Client.LocalPlayer.NickName;
            }

            data.TankPrototype = Prototype;
            game.SendPlayerData(localPlayers[i], data);
        }
    }
}
