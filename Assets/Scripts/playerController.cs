using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : unitBehavior
{
    // Start is called before the first frame update
    public float colorCap = 255;
    public AudioClip absorbSound;
    // Update is called once per frame
    void Update()
    {
        if(stats.colorValues[1] <= 0)
        {
            gameObject.SetActive(false);
        }
        if (stats.colorValues[1] > colorCap)
        {
            stats.colorValues[1] = colorCap;
        }
        updateColor();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        if (Input.GetKey("w"))
        {
            if ((transform.position + Vector3.up * stats.speed / 100).y < 7)
            {
                transform.position += Vector3.up * stats.speed / 100;
            }
        }
        if (Input.GetKey("d"))
        {
            if ((transform.position + Vector3.right * stats.speed / 100).x < 12.5)
            {
                transform.position += Vector3.right * stats.speed / 100;
            }
        }
        if (Input.GetKey("s"))
        {
            if ((transform.position + Vector3.down * stats.speed / 100).y > -7)
            {
                transform.position += Vector3.down * stats.speed / 100;
            }
        }
        if (Input.GetKey("a"))
        {
            if ((transform.position + Vector3.left * stats.speed / 100).x > -12.5)
            {
                transform.position += Vector3.left * stats.speed / 100;
            }
        }
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            Vector2 rayDir = (mousePos - transform.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDir);
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<enemyController>())
                {
                    enemyController e = hit.collider.gameObject.GetComponent<enemyController>();
                    stats.colorValues[1] -= 1f;
                    foreach(GameObject p in GameObject.Find("gameManager").GetComponent<managementScript>().particlePool)
                    {
                        if (!p.activeSelf)
                        {
                            Color c = new Color(0,255,0);
                            p.GetComponent<SpriteRenderer>().color = c;
                            p.GetComponent<particleBehavior>().target = hit.collider.gameObject;
                            p.GetComponent<particleBehavior>().colorIndex = 1;
                            p.GetComponent<particleBehavior>().colorAmount = 5;
                            p.transform.position = new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f),transform.position.y + Random.Range(-0.5f,0.5f));
                            p.SetActive(true);
                            break;
                        }
                    }
                    break;
                }
            }  
        } else if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftShift))
        {
            Vector2 rayDir = (mousePos - transform.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDir);
            foreach (RaycastHit2D hit in hits)
            {   
                if (hit.collider.gameObject.GetComponent<enemyController>())
                {
                    enemyController e = hit.collider.gameObject.GetComponent<enemyController>();
                    if (stats.colorValues[1] + 30 < colorCap && e.stats.colorValues[1] >= 5f)
                    {
                        e.stats.colorValues[1] -= 1f;
                        foreach (GameObject p in GameObject.Find("gameManager").GetComponent<managementScript>().particlePool)
                        {
                            if (!p.activeSelf)
                            {
                                Color c = new Color(0, 255, 0);
                                p.GetComponent<SpriteRenderer>().color = c;
                                p.GetComponent<particleBehavior>().target = gameObject;
                                p.GetComponent<particleBehavior>().colorIndex = 1;
                                p.GetComponent<particleBehavior>().colorAmount = 0.5f;
                                p.transform.position = new Vector2(hit.collider.transform.position.x + Random.Range(-0.5f, 0.5f), hit.collider.transform.position.y + Random.Range(-0.5f, 0.5f));
                                p.SetActive(true);
                                break;
                            }
                        }
                    }
                } else
                if(hit.collider.gameObject.GetComponent<energyCoreBehavior>())
                {
                    energyCoreBehavior e = hit.collider.gameObject.GetComponent<energyCoreBehavior>();
                    if (stats.colorValues[1] < colorCap)
                    {
                        e.colorAmount -= 1f;
                        foreach (GameObject p in GameObject.Find("gameManager").GetComponent<managementScript>().particlePool)
                        {
                            if (!p.activeSelf)
                            {
                                Color c = new Color(0, 255, 0);
                                p.GetComponent<SpriteRenderer>().color = c;
                                p.GetComponent<particleBehavior>().target = gameObject;
                                p.GetComponent<particleBehavior>().colorIndex = 1;
                                p.GetComponent<particleBehavior>().colorAmount = 1;
                                p.transform.position = new Vector2(hit.collider.transform.position.x + Random.Range(-0.5f, 0.5f), hit.collider.transform.position.y + Random.Range(-0.5f, 0.5f));
                                p.SetActive(true);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
