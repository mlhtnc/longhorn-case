using System;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField]
    private Transform throwPoint;

    private DraggableObject draggable;

    private Rigidbody rgBody;

    private void Awake()
    {
        rgBody      = GetComponent<Rigidbody>();
        draggable   = GetComponent<DraggableObject>();
    }

    public void DisableRigidbody()
    {
        rgBody.isKinematic = true;
    }

    public void ThrowCup()
    {
        draggable.DisableDrag();
        rgBody.isKinematic = false;
        
        var distance = Vector3.Distance(throwPoint.position, transform.position);
        var direction = (throwPoint.position - transform.position).normalized;
        var force = NormalizationHelper.MinMax(0f, 10f, 100f, 300f, distance);

        rgBody.AddForce(direction * force, ForceMode.Force);
    }
}
