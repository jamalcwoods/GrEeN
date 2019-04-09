using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Passive")]
public class passiveData : ScriptableObject
{
    public string title;
    public string description;
    public Sprite icon;
    public int cost;
    public int index;
}
