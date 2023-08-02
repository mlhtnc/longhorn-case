public class BinState : IState
{
    public int Id => (int) GameState.BinState;

    public bool IsStateDone { get; private set; }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
