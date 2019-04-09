using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "statProfile")]
public class playerStats : ScriptableObject
{
    public float[] colorValues = new float[3];
    public float speed;
}
