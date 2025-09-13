using UnityEngine;
using UnityEngine.TextCore;

public class BillboardManager : MonoBehaviour
{
    private GameObject[] _billboardTransforms;
    [SerializeField] private Transform cameraPos;

    private void Awake()
    {
        _billboardTransforms = GameObject.FindGameObjectsWithTag("Billboard");
    }

    void Start()
    {
        float cameraXRotation = cameraPos.rotation.eulerAngles.x;
        float cameraYRotation = cameraPos.rotation.eulerAngles.y;
        foreach (GameObject gameObjects in _billboardTransforms)
        {
            gameObjects.transform.rotation = Quaternion.Euler(cameraXRotation, cameraYRotation, 0f);
        }
    }
}
