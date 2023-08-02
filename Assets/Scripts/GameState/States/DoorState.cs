public class DoorState : IState
{
    public int Id => (int) GameState.DoorState;

    public void OnEnter()
    {
        UnityEngine.Debug.Log("Door State Entered");
    }

    public void OnExit()
    {

    }
}
