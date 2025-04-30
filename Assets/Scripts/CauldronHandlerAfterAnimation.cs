using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronHandlerAfterAnimation : MonoBehaviour
{
    [SerializeField] private float timeElapsed;
    [SerializeField] private GameObject SmeltedIngot;
    [SerializeField] private ParticleSystem particle;
    public MeshRenderer ren;
    [SerializeField] SmelterHandler smelter;
    private void Awake()
    {
        SmeltedIngot.SetActive(false);
        this.gameObject.SetActive(false);

    }
    public void ParticlePlay() 
    {
        particle.Play();
        smelter.AudioReward();
    }
    public IEnumerator WaitTimeUntilDestory() 
    {
        SmeltedIngot.SetActive(true);
        
        yield return new WaitForSeconds(timeElapsed);
        ren.enabled = false;
        Destroy(particle);
        Destroy(this.gameObject);
    }
}
