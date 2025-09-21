using UnityEngine;

public class TempoManager : MonoBehaviour
{
    public const float TEMPO = 85.0f; //Beats per second.
    public const float REACTION_INTERVAL = 0.25f; //How many seconds around the rythm the player gets to react
    private const float RYTHM = TEMPO / 60; //How many beats in one second.
     
    void Start()
    {
        GameObject.Find("ManagerObject").GetCompoenent<MusicManager>().playMusicEvent += songHasStarted //Subscribing to an event
    }

    void Update()
    {
        
    }

    void songHasStarted()
    {
        StartCorutine()
    }

    void OnDestroy()
    {
        GameObject.Find("ManagerObject").GetCompoenent<MusicManager>().playMusicEvent -= songHasStarted //Avoid memoryleaks.
}
