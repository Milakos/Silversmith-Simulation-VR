using UnityEngine;

public class FollowVision : MonoBehaviour
{
    public bool centered = false;

    private void OnBecameInvisible()
    {
        centered = false;
    }
}
