using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Object / Create Level", order = 0)]
public class LevelData : ScriptableObject
{
    public COMPLETE_TYPE type;
    public GameObject level;
    public GameObject progressBar;
    public int IndexWave;
    public List<WaveData> listWave;
}

public enum COMPLETE_TYPE { DOG, DOG_FOOT, BOY2, GIRL3 }
