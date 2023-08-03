using System;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField]
    private Transform throwPoint;

    [SerializeField]
    private ParticleSystem dropletParticle;

    [SerializeField]
    private Transform dropletParticlePoint;

    private DraggableObject draggable;

    private Rigidbody rgBody;

    private void Awake()
    {
        rgBody      = GetComponent<Rigidbody>();
        draggable   = GetComponent<DraggableObject>();
    }

    private void Update()
    {
        if(dropletParticle.isPlaying)
        {
            dropletParticle.transform.position = dropletParticlePoint.position;
            dropletParticle.transform.rotation = dropletParticlePoint.rotation;
        }
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

    public void PlayDropletParticle()
    {
        if(dropletParticle.gameObject.activeSelf)
            return;

        dropletParticle.gameObject.SetActive(true);
        dropletParticle.Play();
    }

    public void StopDropletParticle()
    {
        if(dropletParticle.gameObject.activeSelf == false)
            return;

        dropletParticle.gameObject.SetActive(false);
        dropletParticle.Stop();
    }
}
