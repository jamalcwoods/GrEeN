using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class homeManager : MonoBehaviour
{
    public Text highscore;
    public void loadGame()
    {
        SceneManager.LoadScene("game");
    }

    public void loadShop()
    {
        SceneManager.LoadScene("shop");
    }

    public void loadInstructions()
    {
        SceneManager.LoadScene("instructions");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        try
        {
            Stream stream = File.OpenRead("userlog.acc");
            BinaryReader reader = new BinaryReader(stream);
            accountStats.resource = int.Parse(reader.ReadString());
            accountStats.record = int.Parse(reader.ReadString());
            foreach (string s in reader.ReadString().Split('|'))
            {
                accountStats.ownedPassives.Add(int.Parse(s));
            }
            int i = 0;
            foreach (string s in reader.ReadString().Split('|'))
            {
                accountStats.passives[i] = int.Parse(s);
                i++;
            }
            reader.Close();
        } catch (System.Exception e)
        {
            Debug.Log(e);
        }
        DiscordRpc.RichPresence presence = gameObject.GetComponent<DiscordController>().presence;
        if (accountStats.record > 0)
        {
            highscore.text = "High score: " + accountStats.record;
            presence.details = "High Score: " + accountStats.record;
        }
        DiscordRpc.UpdatePresence(ref presence);
    }
}
