using Godot;
interface PlayerMovimentInterface
{
    void OnMoviment(float delta);
    Vector3 CalculateDirectionBasedOnInput();
    Vector3 CalculateNewVelocity(Vector3 direction, float delta);
    void ListenToInput();
}