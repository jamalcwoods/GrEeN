using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleBehavior : MonoBehaviour
{
    public GameObject target;
    public float colorAmount;
    public int colorIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            gameObject.SetActive(false);
            unitBehavior u = null;
            if (collision.gameObject.GetComponent<enemyController>())
            {
                u = collision.gameObject.GetComponent<enemyController>();
                collision.gameObject.GetComponent<AudioSource>().PlayOneShot(collision.gameObject.GetComponent<enemyController>().transferSound);
            }
            if (collision.gameObject.GetComponent<playerController>())
            {
                u = collision.gameObject.GetComponent<playerController>();
                collision.gameObject.GetComponent<AudioSource>().PlayOneShot(collision.gameObject.GetComponent<playerController>().absorbSound);
            }
            u.stats.colorValues[colorIndex] += colorAmount;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!target.activeSelf)
        {
            gameObject.SetActive(false);
        }
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 18);
    }
}
