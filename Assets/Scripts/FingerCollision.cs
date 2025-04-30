using UnityEngine;

public class FingerCollision : MonoBehaviour
{
    public Transform fingerBone1; // Proximal phalanx=

    public float fingerAnimationValue = 0f; // 0 (open) to 1 (fully closed)
    public float animationSpeed = 1f; // Speed of procedural animation

    private bool isColliding = false;
    public LayerMask collisionLayer; // Specify the layer to detect collisions

    void Update()
    {
        // If the finger is colliding, stop increasing the animation value
        if (isColliding)
        {
            return;
        }

        // Procedurally animate the finger closing
        if (fingerAnimationValue < 1f)
        {
            fingerAnimationValue += Time.deltaTime * animationSpeed;
        }

        // Apply the animation to the finger bones
        ApplyFingerAnimation();
    }

    private void ApplyFingerAnimation()
    {
        // Example: Procedurally rotate the finger bones based on animation value
        fingerBone1.localRotation = Quaternion.Euler(Mathf.Lerp(0, 60, fingerAnimationValue), 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // // Check if the collided object is on the specified layer
        // if (collision.gameObject.layer == 10)
        // {
            isColliding = true; // Stop animation
            Debug.Log($"{name} collided with {collision.gameObject.name} on layer {LayerMask.LayerToName(collision.gameObject.layer)}");
        // }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the exited collision is on the specified layer
        if ((1 << collision.gameObject.layer & collisionLayer) != 0)
        {
            isColliding = false; // Resume animation
            Debug.Log($"{name} stopped colliding with {collision.gameObject.name}");
        }
    }
}
