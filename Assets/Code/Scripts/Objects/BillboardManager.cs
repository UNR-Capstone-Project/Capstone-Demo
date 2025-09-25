using UnityEngine;
using UnityEngine.TextCore;

public class BillboardManager : MonoBehaviour
{
    private GameObject[] _billboardTransforms;
    private Transform cameraPos;

    void Start()
    {
        cameraPos = Camera.main.transform;

        _billboardTransforms = GameObject.FindGameObjectsWithTag("Billboard");
        float cameraXRotation = cameraPos.rotation.eulerAngles.x;
        float cameraYRotation = cameraPos.rotation.eulerAngles.y;
        foreach (GameObject gameObjects in _billboardTransforms)
        {
            gameObjects.transform.rotation = Quaternion.Euler(cameraXRotation, cameraYRotation, 0f);
        }
    }
}
