using UnityEngine;

public class WaterDispenser : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem dropletParticle;

    public void PlayDropletParticle()
    {
        dropletParticle.gameObject.SetActive(true);
        dropletParticle.Play();
    }

    public void StopDropletParticle()
    {
        dropletParticle.gameObject.SetActive(false);
        dropletParticle.Stop();
    }
}
