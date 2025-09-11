using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform cameraFollowTarget;
    public float cameraMoveSpeed;
    public Vector3 cameraOffset = new Vector3(-10f, 10f, -10f);

    private Camera mCamera;

    void Start()
    {
        mCamera = GetComponentInChildren<Camera>();
        if (mCamera == null) { Debug.Log("Camera was not found!"); }
    }

    void LateUpdate()
    {
        if (cameraFollowTarget == null || mCamera == null) { return; }
        
        transform.position = Vector3.Lerp(transform.position, cameraFollowTarget.position, cameraMoveSpeed * Time.deltaTime);

        mCamera.transform.position = transform.position + cameraOffset;
        mCamera.transform.LookAt(transform.position);
    }
}
