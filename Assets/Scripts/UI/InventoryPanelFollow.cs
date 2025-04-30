using UnityEngine;

public class InventoryPanelFollow : MonoBehaviour
{
    public FollowVision followVision;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float offset;
    bool isCentered = false;
    [SerializeField] private GameObject canvas;

    private void Update()
    {
        CenteredVision();
    }
    #region FollowVision
    private Vector3 FindTargetPosition()
    {
        // Let's get a position infront of the player's camera
        return cameraTransform.position + (cameraTransform.forward * offset);
    }
    private void MoveTowards(Vector3 targetPosition)
    {
        // Instead of a tween, that would need to be constantly restarted
        canvas.transform.position += (targetPosition - canvas.transform.position) * 0.025f;
    }
    private bool ReachedPosition(Vector3 targetPosition)
    {
        // Simple distance check, can be replaced if you wish
        return Vector3.Distance(targetPosition, canvas.transform.position) < 0.1f;
    }
    private Quaternion InitRotation()
    {
        return canvas.transform.rotation = cameraTransform.transform.rotation.normalized;
    }
    private void CenteredVision()
    {
        if (!followVision) { return; }
        isCentered = followVision.centered;


        if (!isCentered)
        {
            //Initialize Rotation
            InitRotation();
            // Find the position we need to be at
            Vector3 targetPosition = FindTargetPosition();

            // Move just a little bit at a time
            MoveTowards(targetPosition);

            // If we've reached the position, don't do anymore work
            if (ReachedPosition(targetPosition))
                isCentered = true;
        }
    }
    #endregion FollowVision

}
