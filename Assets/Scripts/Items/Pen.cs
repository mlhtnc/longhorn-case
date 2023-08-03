using UnityEngine;

public class Pen : MonoBehaviour, ISelectable
{
    private Renderer rnderer;

    public Renderer Renderer => rnderer;

    private void Awake()
    {
        rnderer = GetComponentInChildren<Renderer>();
    }
}
