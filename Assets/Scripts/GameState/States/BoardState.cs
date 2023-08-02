using System;

public class BoardState : IState
{
    public int Id => (int) GameState.BoardState;

    private Pen selectedPen;

    public bool IsStateDone { get; private set; }

    public void OnEnter()
    {
        UnityEngine.Debug.Log("Board State entered");

        ClickableObject.OnAnyObjectClicked += OnAnyObjectClicked;
    }

    public void OnExit()
    {

    }

    private void OnAnyObjectClicked(ClickableObject obj)
    {
        var pen = obj.GetComponent<Pen>();
        if(pen != null)
        {
            selectedPen = pen;
            return;
        }

        var board = obj.GetComponent<Board>();
        if(board != null && selectedPen != null)
        {
            // Draw board here

            IsStateDone = true;
        }
    }
}
