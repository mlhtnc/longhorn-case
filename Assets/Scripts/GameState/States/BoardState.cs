using UnityEngine;

public class BoardState : IState
{
    public int Id => (int) GameState.BoardState;

    private Pen selectedPen;

    private GameStateController gameStateController;

    private bool isDrawingStarted;

    public bool IsStateDone { get; private set; }


    public BoardState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
    }

    public void OnEnter()
    {
        ClickableObject.OnAnyObjectClicked += OnAnyObjectClicked;
    }

    public void OnExit()
    {
        ClickableObject.OnAnyObjectClicked -= OnAnyObjectClicked;
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
        if(isDrawingStarted == false && board != null && selectedPen != null)
        {
            isDrawingStarted = true;

            var seq = LeanTween.sequence();

            seq.append(() => {
                LeanTween.move(selectedPen.gameObject, gameStateController.PenTransformOnBoard.position, 0.5f);
                LeanTween.rotate(selectedPen.gameObject, gameStateController.PenTransformOnBoard.eulerAngles, 0.5f);
            });
            seq.append(0.5f);

            seq.append(() => {
                LeanTween.move(selectedPen.gameObject, selectedPen.transform.position + Vector3.right * 0.5f, .3f).setLoopPingPong(2);
            });
            seq.append(0.3f * 4);


            seq.append(() => {
                board.PaintBoardToBlack();

                LeanTween.move(selectedPen.gameObject, gameStateController.DropPenTransform.position, 0.7f);
                LeanTween.rotate(selectedPen.gameObject, gameStateController.DropPenTransform.eulerAngles, 0.7f);
            });
            seq.append(0.3f);

            seq.append(() => {
                IsStateDone = true;
            });
        }
    }
}
