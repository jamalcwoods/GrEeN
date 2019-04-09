using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitBehavior : MonoBehaviour
{
    public playerStats stats;
    public void updateColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(stats.colorValues[0] / 255, stats.colorValues[1] / 255, stats.colorValues[2] / 255);
    }
}
