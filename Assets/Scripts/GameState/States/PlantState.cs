using NotDecided.InputManagament;
using UnityEngine;

public class PlantState : IState, IStateTickable
{
    private const float PlantTime = 3f;


    private GameStateController gameStateController;

    private float plantProgress;

    private Cup cup;

    private DraggableObject cupDraggable;


    public int Id => (int) GameState.PlantState;

    public bool IsStateDone { get; private set; }


    public PlantState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
        this.cup                 = gameStateController.Cup.GetComponent<Cup>();
        this.cupDraggable        = gameStateController.Cup;
    }

    public void OnEnter()
    {
        cupDraggable.OnDragStarted      += OnDragStarted;
        InputManager.OnAnyPointerUp     += OnAnyPointerUp;
    }

    public void OnExit()
    {
        cupDraggable.OnDragStarted      -= OnDragStarted;
        InputManager.OnAnyPointerUp     -= OnAnyPointerUp;
    }

    public void Tick(float deltaTime)
    {
        if(cupDraggable.IsDragging == false)
            return;

        var hit = gameStateController.Raycast(gameStateController.CupLayerMask);

        if(hit.collider != null)
        {
            var plant = hit.collider.GetComponent<Plant>();
            if(plant != null)
            {
                plantProgress += Time.deltaTime;
                if(plantProgress >= PlantTime)
                {
                    OnWateringFinished(plant);
                }
                else
                {
                    UpdateCupColor();
                    ContinueWatering();
                }
            }
            else
            {
                StopWatering();
            }
        }        
    }

    private void ContinueWatering()
    {
        var cupAngles               = cupDraggable.transform.eulerAngles;
        var cupPlantTransformAngle  = gameStateController.CupPlantTransform.eulerAngles;

        if(LeanTween.isTweening(cupDraggable.gameObject) == false && cupAngles != cupPlantTransformAngle)
        {
            LeanTween.rotate(cupDraggable.gameObject, cupPlantTransformAngle, 0.3f);
        }

        cup.PlayDropletParticle();
    }

    private void StopWatering()
    {
        var cupAngles               = cupDraggable.transform.eulerAngles;
        var dropCupTransformAngle   = gameStateController.DropCupTransform.eulerAngles;

        if(LeanTween.isTweening(cupDraggable.gameObject) == false && cupAngles != dropCupTransformAngle)
        {
            LeanTween.rotate(cupDraggable.gameObject, dropCupTransformAngle, 0.3f);
        }

        cup.StopDropletParticle();
    }

    private void UpdateCupColor()
    {
        var color = Color.Lerp(Color.blue, Color.white, plantProgress / PlantTime);
        var renderer = cupDraggable.GetComponent<Renderer>();
        renderer.material.color = color;
    }

    private void OnWateringFinished(Plant plant)
    {
        StopWatering();
        LeanTween.scale(plant.gameObject, plant.transform.localScale * 1.3f, 0.7f).setEaseInOutBack();
        
        IsStateDone = true;
    }

    private void OnDragStarted()
    {
        // NOTE: We disabled collider of water dispenser because cup was inside of it and not clickable
        // Now, we need to enable collider back to make sure cup position updates are correct
        gameStateController.WaterDispenser.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnAnyPointerUp()
    {
        LeanTween.move(cupDraggable.gameObject, gameStateController.InitialCupPos, 0.3f);
        LeanTween.rotate(cupDraggable.gameObject, gameStateController.DropCupTransform.eulerAngles, 0.3f);

        cup.StopDropletParticle();
    }

}
