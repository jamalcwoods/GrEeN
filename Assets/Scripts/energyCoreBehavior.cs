using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyCoreBehavior : MonoBehaviour
{
    public GameObject player;
    public float colorAmount;

    private void Update()
    {
        if(colorAmount < 0)
        {
            gameObject.SetActive(false);
        }
        if (accountStats.passives.Contains(9))
        {
            if(Vector2.Distance(player.transform.position,transform.position) <= 3 && player.GetComponent<playerController>().stats.colorValues[1] < player.GetComponent<playerController>().colorCap)
            {
                foreach (GameObject p in GameObject.Find("gameManager").GetComponent<managementScript>().particlePool)
                {
                    if (!p.activeSelf)
                    {
                        Color c = new Color(0, 255, 0);
                        p.GetComponent<SpriteRenderer>().color = c;
                        p.GetComponent<particleBehavior>().target =player;
                        p.GetComponent<particleBehavior>().colorIndex = 1;
                        p.GetComponent<particleBehavior>().colorAmount = 1;
                        p.transform.position = new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f),transform.position.y + Random.Range(-0.5f, 0.5f));
                        p.SetActive(true);
                        break;
                    }
                }
            }
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, colorAmount / 255,0);
    }
}
