using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instructionsManager : MonoBehaviour
{
    private void Start()
    {
        DiscordRpc.RichPresence presence = gameObject.GetComponent<DiscordController>().presence;
        if (accountStats.record > 0)
        {
            presence.details = "High Score: " + accountStats.record;
        }
        DiscordRpc.UpdatePresence(ref presence);
    }

    public void returnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("home");
    }
}
