public class CupState : IState
{
    public int Id => (int) GameState.CupState;

    public bool IsStateDone { get; private set; }

    public void OnEnter()
    {
        UnityEngine.Debug.Log("Cup State entered");
    }

    public void OnExit()
    {

    }
}
