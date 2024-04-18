using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pass Item", menuName = "Ice Pass")]
public class PassItem : ScriptableObject
{
    public int itemId;
    public new string name;
    public GameObject prefab;
    public int level;
    public bool isPremium;
    public int passId;
    public PassItemColor color;
}

