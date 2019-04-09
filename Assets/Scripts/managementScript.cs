using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class managementScript : MonoBehaviour
{
    public int kills;
    public int killCounter;
    float spawnCoreTimer;
    public float spawnCoreDelay;
    float spawnEnemyTimer;
    public float spawnEnemyDelay;
    float greenRegenTimer;
    public float greenRegenDelay;
    float increaseSpawnRateTimer;
    public float increaseSpawnRateDelay;
    public GameObject player;
    private playerStats stats;
    public GameObject canvasObject;
    public GameObject[] enemyPool;
    public GameObject[] particlePool;
    public GameObject[] corePool;
    public GameObject endGameUI;
    public Text newRecord;
    public Text endMessage;
    public gameState state = gameState.Playing;
    public int maxCores = 0;
    public float greenRegen = 0;
    public float capacityRegen = 0;
    public List<Sprite> passiveIconMap = new List<Sprite>();
    public List<Sprite> abilityIconMap = new List<Sprite>();
    public AudioClip playerDeath, enemyDeath;
    public enum gameState
    {
        Playing,
        GameOver,
        Paused
    }
    // Start is called before the first frame update
    void Start()
    {
        DiscordRpc.RichPresence presence = gameObject.GetComponent<DiscordController>().presence;
        if (accountStats.record > 0)
        {
            presence.details = "High Score: " + accountStats.record;
        }
        DiscordRpc.UpdatePresence(ref presence);
        enemyPool  = new GameObject[50];
        particlePool = new GameObject[100];
        corePool = new GameObject[3];
        player = Instantiate(Resources.Load<GameObject>("Prefabs/player"));
        playerStats p = new playerStats();
        stats = p;
        p.speed = 6;
        p.colorValues = new float[] { 0, 255, 0 };
        player.GetComponent<playerController>().stats = p;
        if (accountStats.passives.Contains(1))
        {
            maxCores = 1;
        }
        if (accountStats.passives.Contains(2))
        {
            maxCores = 2;
        }
        if (accountStats.passives.Contains(3))
        {
            maxCores = 3;
        }
        if (accountStats.passives.Contains(5))
        {
            greenRegen = 1;
        }
        if (accountStats.passives.Contains(6))
        {
            maxCores = 2;
        }
        if (accountStats.passives.Contains(7))
        {
            maxCores = 3;
        }
        if (accountStats.passives.Contains(8))
        {
            capacityRegen = 5;
        }    
        for (int i = 0; i < 50; i++)
        {
            enemyPool[i] = Instantiate(Resources.Load<GameObject>("Prefabs/enemy"));
            playerStats e = new playerStats();
            e.speed = 2;
            e.colorValues = new float[] { Random.Range(1,256),0, Random.Range(1, 256) };
            enemyPool[i].GetComponent<enemyController>().stats = e;
            enemyPool[i].SetActive(false);
        }
        for (int i = 0; i < 100; i++)
        {
            particlePool[i] = Instantiate(Resources.Load<GameObject>("Prefabs/particle"));
            particlePool[i].GetComponent<particleBehavior>().target = player;
            particlePool[i].SetActive(false);
        }
        for (int i = 0; i < maxCores; i++)
        {
            corePool[i] = Instantiate(Resources.Load<GameObject>("Prefabs/energyCore"));
            corePool[i].GetComponent<energyCoreBehavior>().player = player;
            corePool[i].SetActive(false);
        }
    }

    public void loadHome()
    {
        SceneManager.LoadScene("home");
    }

    public void loadShop()
    {
        SceneManager.LoadScene("shop");
    }

    public void loadGame()
    {
        playerStats p = new playerStats();  
        p.speed = 6;
        p.colorValues = new float[] { 0, 255, 0 };
        stats = p;
        player.GetComponent<playerController>().stats = p;
        player.transform.position = Vector2.zero;
        player.GetComponent<playerController>().colorCap = 255;
        kills = 0;
        player.SetActive(true);
        greenRegenTimer = 0;
        spawnEnemyTimer = 0;
        spawnCoreTimer = 0;
        for (int i = 0; i < 50; i++)
        {
            enemyPool[i].SetActive(false);
        }
        for (int i = 0; i < 100; i++)
        {
            particlePool[i].SetActive(false);
        }
        for (int i = 0; i < maxCores; i++)
        {
            corePool[i].SetActive(false);
        }
        state = gameState.Playing;
        endGameUI.SetActive(false);
    }

    public void enemyKilled(int i)
    {
        kills += i;
        gameObject.GetComponent<AudioSource>().volume = 0.3f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(enemyDeath);
        gameObject.GetComponent<AudioSource>().volume = 0.4f;
    }
    // Update is called once per frame
    void Update()
    {
        /*
        for (int i = 0; i < accountStats.abilties.Count; i++)
        {
            canvasObject.transform.Find("ability" + (i+1).ToString() + "Icon").GetComponent<Image>().sprite = abilityIconMap[accountStats.abilties[i]];
        }
        */
        for (int i = 0; i < accountStats.passives.Count; i++)
        {
            /*
            Debug.Log(passiveIconMap[1]);
            Debug.Log(i + " : " + accountStats.passives[i]);
            */
            canvasObject.transform.Find("passive" + (i + 1).ToString() + "Icon").GetComponent<Image>().sprite = passiveIconMap[accountStats.passives[i]];
        }
        switch (state)
        {
            case gameState.Playing:
                if (stats.colorValues[1] <= 0)
                {
                    gameObject.GetComponent<AudioSource>().volume = 0.8f;
                    gameObject.GetComponent<AudioSource>().PlayOneShot(playerDeath);
                    gameObject.GetComponent<AudioSource>().volume = 0.4f;
                    state = gameState.GameOver;
                    endGameUI.SetActive(true);
                    accountStats.resource += kills;
                    if(kills > accountStats.record)
                    {
                        accountStats.record = kills;
                        newRecord.text = "New High Score!!\n" + kills + " points";
                    }
                    endMessage.text = "Total Points: " + accountStats.resource;
                    accountStats.Save();
                    DiscordRpc.RichPresence presence = gameObject.GetComponent<DiscordController>().presence;
                    if (accountStats.record > 0)
                    {
                        presence.details = "High Score: " + accountStats.record;
                    }
                    DiscordRpc.UpdatePresence(ref presence);
                }
                canvasObject.transform.Find("Resource").GetComponent<RectTransform>().sizeDelta = new Vector2(stats.colorValues[1] / 255 * 1270, 20);
                canvasObject.transform.Find("Border").GetComponent<RectTransform>().sizeDelta = new Vector2(player.GetComponent<playerController>().colorCap / 255 * 1278, 28);
                canvasObject.transform.Find("Resource").GetComponent<Image>().color = new Color(0, stats.colorValues[1] / 255, 0);
                canvasObject.transform.Find("Text").GetComponent<Text>().text = "Points Earned : " + kills + "\nHigh Score: " + accountStats.record;
                if (Time.timeSinceLevelLoad > spawnEnemyTimer)
                {
                    spawnEnemyTimer = Time.timeSinceLevelLoad + spawnEnemyDelay;
                    foreach (GameObject g in enemyPool)
                    {
                        if (!g.activeSelf)
                        {
                            g.SetActive(true);
                            g.transform.position = new Vector2(Random.Range(-17,17),8 * (Random.Range(0f, 1f) > 0.5f ? 1 : -1));
                            playerStats e = new playerStats();
                            e.speed = 6;
                            e.colorValues = new float[] { Random.Range(128, 256),0, Random.Range(128, 256) };
                            g.GetComponent<enemyController>().stats = e;
                            break;
                        }
                    }

                }
                if (Time.timeSinceLevelLoad > greenRegenTimer)
                {
                    greenRegenTimer = Time.timeSinceLevelLoad + greenRegenDelay;
                    stats.colorValues[1] += greenRegen;

                    if(stats.colorValues[1] >= player.GetComponent<playerController>().colorCap)
                    {
                        stats.colorValues[1] = player.GetComponent<playerController>().colorCap;
                    }
                }
                if (Time.timeSinceLevelLoad > increaseSpawnRateTimer)
                {
                    increaseSpawnRateTimer= Time.timeSinceLevelLoad + increaseSpawnRateDelay;
                    if(spawnEnemyDelay >= 0.8)
                    {
                        spawnEnemyDelay -= 0.2f;
                    }
                }
                if (Time.timeSinceLevelLoad > spawnCoreTimer)
                {
                    spawnCoreTimer = Time.timeSinceLevelLoad + spawnCoreDelay;
                    if (spawnCoreDelay > 0.4)
                    {
                        spawnCoreDelay -= 0.2f;
                    }
                    for (int i = 0;i <maxCores;i++)
                    {
                        GameObject g = corePool[i];
                        if(!g.activeSelf){
                            g.GetComponent<energyCoreBehavior>().colorAmount = 255;
                            g.SetActive(true);
                            g.transform.position = new Vector2(Random.Range(-10, 11),Random.Range(-6,6));
                            break;
                        }
                    }
                }
                if(killCounter >= 10)
                {
                    killCounter = 0;
                    player.GetComponent<playerController>().colorCap += capacityRegen;
                    if (player.GetComponent<playerController>().colorCap > 255)
                    {
                        player.GetComponent<playerController>().colorCap = 255;
                    }
                }
                break;
            case gameState.GameOver:
                break;
            case gameState.Paused:
                break;
        }
    }
}
