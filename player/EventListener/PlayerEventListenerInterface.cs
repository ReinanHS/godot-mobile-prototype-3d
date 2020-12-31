using Godot;
public interface PlayerEventListenerInterface
{
    void OnPlayerChat(string text);
    void OnPlayerCommand(string cmd);
    void OnPlayerDamage(float healthLoss, float armorLoss);
    void OnPlayerDeath(PlayerDeathReason reason, Player killer);
    void OnPlayerJoin();
    void OnPlayerQuit(PlayerExitType exitType, string reason);
    void OnPlayerReady();
    void OnPlayerSpawn();
    void OnPlayerMove(Vector3 direction);
    void OnPlayerChangeAnimation(Animation animation);
}
