using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour
{
    public Transform targetTransform; // The target transform to rotate towards
    public float rotationSpeed = 5f; // The speed at which the rotation occurs

    private void Update()
    {
        if (targetTransform != null)
        {
        transform.LookAt(targetTransform);
            //// Get the direction from the current transform to the target transform
            //Vector3 direction = targetTransform.position - transform.position;

            //// Calculate the rotation needed to face the target direction
            //Quaternion targetRotation = Quaternion.LookRotation(-direction);

            //// Smoothly rotate the transform towards the target rotation
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
