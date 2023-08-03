using NotDecided.InputManagament;
using UnityEngine;

public class PlantState : IState, IStateTickable
{
    private const float PlantTime = 3f;


    private GameStateController gameStateController;

    private float plantProgress;


    public int Id => (int) GameState.PlantState;

    public bool IsStateDone { get; private set; }


    public PlantState(GameStateController gameStateController)
    {
        this.gameStateController = gameStateController;
    }

    public void OnEnter()
    {
        gameStateController.Cup.OnDragStarted += OnDragStarted;
        InputManager.OnAnyPointerUp           += OnAnyPointerUp;
    }

    public void OnExit()
    {
        gameStateController.Cup.OnDragStarted -= OnDragStarted;
        InputManager.OnAnyPointerUp           -= OnAnyPointerUp;
    }

    public void Tick(float deltaTime)
    {
        if(gameStateController.Cup.IsDragging == false)
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
                    StopPlanting();
                    IsStateDone = true;
                }
                else
                {
                    var color = Color.Lerp(Color.blue, Color.white, plantProgress / PlantTime);
                    var renderer = gameStateController.Cup.GetComponent<Renderer>();
                    renderer.material.color = color;

                    ContinuePlanting();
                }
            }
            else
            {
                StopPlanting();
            }
        }        
    }

    private void ContinuePlanting()
    {
        gameStateController.Cup.GetComponent<Cup>().PlayDropletParticle();

        if(LeanTween.isTweening(gameStateController.Cup.gameObject) == false &&
            gameStateController.Cup.transform.eulerAngles != gameStateController.CupPlantTransform.eulerAngles)
        {
            LeanTween.rotate(gameStateController.Cup.gameObject, gameStateController.CupPlantTransform.eulerAngles, 0.3f);
        }

    }

    private void StopPlanting()
    {
        if(LeanTween.isTweening(gameStateController.Cup.gameObject) == false &&
            gameStateController.Cup.transform.eulerAngles != gameStateController.DropCupTransform.eulerAngles)
        {
            LeanTween.rotate(gameStateController.Cup.gameObject, gameStateController.DropCupTransform.eulerAngles, 0.3f);
        }

        gameStateController.Cup.GetComponent<Cup>().StopDropletParticle();
    }

    private void OnDragStarted()
    {
        // NOTE: We disabled collider of water dispenser because cup was inside of it and not clickable
        // Now, we need to enable collider back to make sure cup position updates are correct
        gameStateController.WaterDispenser.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnAnyPointerUp()
    {
        LeanTween.move(gameStateController.Cup.gameObject, gameStateController.InitialCupPos, 0.3f);
        LeanTween.rotate(gameStateController.Cup.gameObject, gameStateController.DropCupTransform.eulerAngles, 0.3f);
    }

}
