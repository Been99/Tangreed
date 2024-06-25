using UnityEngine;

public class BGfollowingPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(-0.63f, 0.49f, 0f);
    public float smoothTime = 0.01f;

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 targetPosition = playerTransform.position + offset;
            targetPosition.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}