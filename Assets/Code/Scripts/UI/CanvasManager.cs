using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI hitQualityText;

    void Start()
    {
        //Setup UI with main camera.
        Canvas mCanvas = GetComponent<Canvas>();
        mCanvas.worldCamera = Camera.main;
        mCanvas.planeDistance = 1;
    }

    public void UpdateHitQualityText(string quality)
    {
        hitQualityText.GetComponent<TextMeshProUGUI>().text = quality;
    }

    void Update()
    {
        
    }
}
