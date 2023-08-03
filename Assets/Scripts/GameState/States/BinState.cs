using UnityEngine;

public class BinState : IState
{
    private GameStateController gameStateController;

    private Cup cup;

    private DraggableObject cupDraggable;


    public bool IsStateDone { get; private set; }

    public int Id => (int) GameState.BinState;


    public BinState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
        this.cup                 = gameStateController.Cup.GetComponent<Cup>();
        this.cupDraggable        = gameStateController.Cup;
    }

    public void OnEnter()
    {
        if(cupDraggable.IsDragging == false)
        {
            ThrowCup();
        }

        cupDraggable.OnDragStarted += OnDragStarted;
        cupDraggable.OnDragStopped += OnDragStopped;
    }

    public void OnExit()
    {
        cupDraggable.OnDragStarted -= OnDragStarted;
        cupDraggable.OnDragStopped -= OnDragStopped;
    }

    private void OnDragStarted()
    {
        cup.DisableRigidbody();
        cupDraggable.EnableDrag();
    }

    private void OnDragStopped()
    {
        var hit = gameStateController.Raycast(gameStateController.CupLayerMask);

        if(hit.collider != null)
        {
            var bin = hit.collider.GetComponent<Bin>();
            if(bin != null)
            {
                var animTime = 0.4f;

                var cupGo = cupDraggable.gameObject;
                
                LeanTween.scale(cupGo, Vector3.one * 0.1f, animTime).setEaseInExpo();
                LeanTween.moveY(cupGo, bin.transform.position.y, animTime).setEaseInBack();
                LeanTween.moveX(cupGo, bin.transform.position.x, animTime);
                LeanTween.moveZ(cupGo, bin.transform.position.z, animTime)
                .setOnComplete(() => {
                    cupGo.SetActive(false);
                });

                IsStateDone = true;
            }
            else
            {
                ThrowCup();
            }
        }
    }

    private void ThrowCup()
    {
        cup.ThrowCup();
    }
}
