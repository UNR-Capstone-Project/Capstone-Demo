using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class initiateGame : MonoBehaviour
{
    [SerializeField] private GameObject Ground;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Volume main3DVolume;
    [SerializeField] private Light mainDirectionalLight;
    [SerializeField] private GameObject mainPlayer;

    //load all relevant data at start of game
    public void Start()
    {
        bindObjects();
        //show a loading screen

    }

    private void bindObjects()
    {
        //bind objects to prefabs
        Instantiate(Ground);
        Instantiate(mainCamera);
        Instantiate(main3DVolume);
        Instantiate(mainDirectionalLight);
        Instantiate(mainPlayer);
    }

    private void createObjects()
    {
        Debug.Log("Function here will be rewritten later if needing to intialize 3rd party servics/other services etc");
    }

    private void backgroundGamePreparations()
    {
        
        Debug.Log("Function here will be rewritten later when needing to load heavy objects async like through Resources, Asset Bundles, or Addressables");
    }

}
