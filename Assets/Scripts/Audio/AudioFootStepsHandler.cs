using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioFootStepsHandler : MonoBehaviour
{
    private enum CURRENT_TERRAIN { GRASS, WOOD_FLOOR };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    private EventInstance foosteps;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<ButtonActionsController>().movingOrRotating += isMoving;
    }
    public void isMoving(bool movingOrRotating)
    {
        if (movingOrRotating)
        {
            SelectAndPlayFootstep(); 
        }
        else
        {
            foosteps.setPaused(true);
        }
    }

    private void Update()
    {
        DetermineTerrain();
    }

    private void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("LabFloor"))
            {
                currentTerrain = CURRENT_TERRAIN.WOOD_FLOOR;
                break;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Nature"))
            {
                currentTerrain = CURRENT_TERRAIN.GRASS;
            }

        }
    }

    public void SelectAndPlayFootstep()
    {     
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.GRASS:
                PlayFootstep(0, "GrassParameter");
                break;

            case CURRENT_TERRAIN.WOOD_FLOOR:
                PlayFootstep(1, "FloorParameter");
                break;
        }
    }

    private void PlayFootstep(int terrain, string name)
    {
        foosteps.setPaused(false);
        foosteps = RuntimeManager.CreateInstance("event:/FootSteps");
        foosteps.setParameterByName(name, terrain);
        foosteps.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }
}
