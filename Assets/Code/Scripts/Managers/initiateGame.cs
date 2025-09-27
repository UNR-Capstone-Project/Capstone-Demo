using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using SoundSystem;
using Unity.Cinemachine;
public class initiateGame : MonoBehaviour
{
    [SerializeField] private GameObject mainManager;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Volume mainGlobalVolume;
    [SerializeField] private Light mainDirectionalLight;
    [SerializeField] private GameObject mainPlayer;

    //Loads all relevant data at start of each level.
    public void Start()
    {
        bindObjects();

        //ISSUE: Show loading screen!
    }

    private void bindObjects()
    {
        //Bind objects to prefabs

        Instantiate(mainManager);
        Instantiate(mainCanvas);
        Instantiate(mainPlayer);
        Instantiate(mainCamera);
        Instantiate(mainGlobalVolume);
        Instantiate(mainDirectionalLight);
    }

    private void createObjects()
    {
        Debug.Log("Function here will be rewritten later if needing to initialize 3rd party services/other services etc. Or tweak object settings after instantiation.");
    }

    private void backgroundGamePreparations()
    {
        
        Debug.Log("Function here will be rewritten later when needing to load heavy objects async like through Resources, Asset Bundles, or Addressables. Or to move positions of objects around or things of that nature. ");
    }

}
