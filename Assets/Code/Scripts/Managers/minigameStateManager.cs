using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "minigameStateManager", menuName = "Scriptable Objects/minigameStateManager")]
public class minigameStateManager : ScriptableObject
{
    private string currentMinigame = "drums";
    private bool onCooldown = false;

    //in seconds
    private float minigameDuration = 30f;
    private float cooldownTime = 20f;
    private float slowdownFactor = 0.8f;

    public void changeMinigame(string newMinigame)
    {
        if (newMinigame == "Drums")
        {
            currentMinigame = "drums";
        }
        else
        {
            Debug.Log("New minigame entered in does not match any existing minigames");
        }
    }

    public void changeCooldown()
    {
        onCooldown = !onCooldown;
    }

    public void changeMinigameDuration(float newFloat)
    {
        minigameDuration = newFloat;
    }

    public void changeCooldownTime(float newFloat)
    {
        cooldownTime = newFloat;
    }

    public void changeSlowdownFactor(float newFloat)
    {
        slowdownFactor = newFloat;
    }
}
