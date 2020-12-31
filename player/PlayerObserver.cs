using Godot;

public class PlayerObserver : PlayerEventListenerInterface
{
    public Player player;
    public PlayerObserver(Player player)
    {
        this.player = player;
    }
    public void OnPlayerChangeAnimation(Animation animation)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerChat(string text)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerCommand(string cmd)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerDamage(float healthLoss, float armorLoss)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerDeath(PlayerDeathReason reason, Player killer)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerJoin()
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerMove(Vector3 direction)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPlayerQuit(PlayerExitType exitType, string reason)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerReady()
    {
        //throw new System.NotImplementedException();
    }

    public void OnPlayerSpawn()
    {
        throw new System.NotImplementedException();
    }
}