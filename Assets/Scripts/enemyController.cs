using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : unitBehavior
{
    GameObject target;
    public AudioClip transferSound;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("gameManager").GetComponent<managementScript>().player;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<playerController>())
        {
            gameObject.SetActive(false);
            for(int i = 0;i<collision.gameObject.GetComponent<playerController>().stats.colorValues.Length;i++)
            {
                GameObject.Find("gameManager").GetComponent<managementScript>().enemyKilled(0);
                playerController player = collision.gameObject.GetComponent<playerController>();
                player.colorCap -= 20;
                if(player.stats.colorValues[1] > player.colorCap)
                {
                    player.stats.colorValues[1] = player.colorCap;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (stats.colorValues[1] >= 255)
        {
            GameObject.Find("gameManager").GetComponent<managementScript>().enemyKilled(1);
            gameObject.SetActive(false);
            if (accountStats.passives.Contains(4))
            {
                RaycastHit2D[] rebounds = Physics2D.CircleCastAll(transform.position, 6, Vector2.zero);
                foreach (RaycastHit2D r in rebounds)
                {
                    if (r.collider.gameObject.GetComponent<enemyController>())
                    {
                        int transfered = 15;
                        foreach (GameObject p in GameObject.Find("gameManager").GetComponent<managementScript>().particlePool)
                        {
                            if (!p.activeSelf)
                            {
                                Color c = new Color(0, 255, 0);
                                p.GetComponent<SpriteRenderer>().color = c;
                                p.GetComponent<particleBehavior>().target = r.collider.gameObject;
                                p.GetComponent<particleBehavior>().colorIndex = 1;
                                p.GetComponent<particleBehavior>().colorAmount = 4.2f;
                                p.transform.position = new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f));
                                p.SetActive(true);
                                transfered--;
                                if (transfered == 0)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        updateColor();
        if (target.activeSelf)
        {
            Vector3 targetPos = target.transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPos - transform.position);
            if (accountStats.passives.Contains(10))
            {
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPos, (3 - (3 *(0.5f * (stats.colorValues[1]/255)))) * Time.deltaTime);
            } else
            {
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPos, 3 * Time.deltaTime);
            }

        }
    }
}
