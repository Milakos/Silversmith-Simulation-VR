using UnityEngine;

public class PhysicsHand : MonoBehaviour
{
    [System.Serializable]
    public class Finger
    {
        public Transform[] joints; // The joints for this finger (3 for most fingers)
        public Rigidbody[] rigidbodies; // Rigidbody components for the joints
        public ConfigurableJoint[] jointConfigs; // Configurable joints for the joints
        public float bendForce = 100f; // Force to bend the finger
    }

    public Finger[] fingers; // Array of all fingers
    public float handStrength = 500f; // General strength of the hand for bending

    void FixedUpdate()
    {
        // Loop through all fingers
        foreach (Finger finger in fingers)
        {
            // Simulate bending the finger based on input
            SimulateFinger(finger, Input.GetMouseButton(0)); // Example: Left mouse button to grab
        }
    }

    void SimulateFinger(Finger finger, bool isGrabbing)
    {
        for (int i = 0; i < finger.joints.Length; i++)
        {
            ConfigurableJoint joint = finger.jointConfigs[i];
            if (joint == null) continue;

            // Adjust target rotation based on grabbing or idle position
            Quaternion targetRotation = isGrabbing
                ? Quaternion.Euler(-90, 0, 0) // Bending position
                : Quaternion.Euler(0, 0, 0);  // Default straight position

            // Use the joint's target rotation to simulate bending
            JointDrive drive = joint.angularXDrive;
            drive.positionSpring = finger.bendForce * handStrength;
            joint.angularXDrive = drive;
            joint.targetRotation = targetRotation;
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize finger joints in the editor
        if (fingers == null) return;
        Gizmos.color = Color.green;

        foreach (Finger finger in fingers)
        {
            foreach (Transform joint in finger.joints)
            {
                if (joint != null)
                {
                    Gizmos.DrawSphere(joint.position, 0.01f);
                }
            }
        }
    }
}

