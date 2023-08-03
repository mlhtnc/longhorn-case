using UnityEngine;

public class CupState : IState
{
    private GameStateController gameStateController;

    private bool isCupPlacedIntoWaterDispenser;

    private bool isWaterFilling;


    public bool IsStateDone { get; private set; }

    public int Id => (int) GameState.CupState;


    public CupState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
    }

    public void OnEnter()
    {
        this.gameStateController.Cup.EnableDrag();

        gameStateController.Cup.OnDragStopped += OnDragStopped;
        ClickableObject.OnAnyObjectClicked += OnAnyObjClicked;
    }

    public void OnExit()
    {
        gameStateController.Cup.OnDragStopped -= OnDragStopped;
        ClickableObject.OnAnyObjectClicked -= OnAnyObjClicked;
    }

    private void OnDragStopped()
    {
        if(isCupPlacedIntoWaterDispenser)
            return;

        var hit = gameStateController.Raycast(gameStateController.CupLayerMask);

        if(hit.collider != null)
        {
            var waterDispenser = hit.collider.GetComponent<WaterDispenser>();
            if(waterDispenser != null)
            {
                this.gameStateController.Cup.DisableDrag();
                LeanTween.move(gameStateController.Cup.gameObject, gameStateController.DropCupTransform.position, 0.2f);

                isCupPlacedIntoWaterDispenser = true;

                return;
            }            
        }

        LeanTween.move(gameStateController.Cup.gameObject, gameStateController.InitialCupPos, 0.3f);
    }
    
    private void OnAnyObjClicked(ClickableObject obj)
    {
        if(isWaterFilling || isCupPlacedIntoWaterDispenser == false)
            return;
        
        var waterDispenser = obj.GetComponent<WaterDispenser>();
        if(waterDispenser != null)
        {
            isWaterFilling = true;

            var seq = LeanTween.sequence();

            var animTime = 2f;

            seq.append(() => {
                waterDispenser.PlayDropletParticle();

                var renderer = gameStateController.Cup.GetComponent<Renderer>();

                LeanTween.value(gameStateController.Cup.gameObject, 0f, 1f, animTime)
                .setOnUpdate((float val) => {
                    var color = Color.Lerp(Color.white, Color.blue, val);
                    renderer.material.color = color;
                });
            });
            seq.append(animTime);

            seq.append(() => {
                waterDispenser.StopDropletParticle();
            });

            seq.append(() => {
                

                IsStateDone = true;

                this.gameStateController.Cup.EnableDrag();
                waterDispenser.GetComponent<BoxCollider>().enabled = false;
            });
        }
    }
}
