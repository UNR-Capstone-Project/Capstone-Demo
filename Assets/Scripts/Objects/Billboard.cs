using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraPos;

    void Start()
    {
        if (Camera.main != null)
        {
            cameraPos = Camera.main.transform;
        }
        else
        {
            Debug.Log("Billboard failed: No main camera has been tagged.");
        }
    }

    void LateUpdate()
    {
        transform.LookAt(cameraPos.position);
    }
}
