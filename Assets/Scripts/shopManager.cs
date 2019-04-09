using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class shopManager : MonoBehaviour
{
    public Canvas canvas;
    public Text pointsIndicator;
    // Start is called before the first frame update

    private void Start()
    {
        DiscordRpc.RichPresence presence = gameObject.GetComponent<DiscordController>().presence;
        if (accountStats.record > 0)
        {
            presence.details = "High Score: " + accountStats.record;
        }
        DiscordRpc.UpdatePresence(ref presence);
    }
    private void Update()
    {
        pointsIndicator.text = "Points To Spend: " + accountStats.resource;
        /*
        Debug.Log(accountStats.passives[0] + ": " + accountStats.passives[1] + ": " + accountStats.passives[2] + ": " + accountStats.passives[3]);
        Debug.Log(accountStats.ownedPassives.Count);
        Debug.Log(accountStats.resource);
        */  
    }

    public void returnToTitle()
    {
        accountStats.Save();
        SceneManager.LoadScene("home");
    }
}
