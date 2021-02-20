using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Object / Create Wave", order = 0)]
public class WaveData : ScriptableObject
{
    public GameObject option;
    public bool hideOptionOnLoad;
}
