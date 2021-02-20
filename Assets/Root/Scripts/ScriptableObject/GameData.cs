using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "Scriptable Object / Create Game", order = 0)]
public class GameData : ScriptableObject
{
    public int indexMap;
    public List<MapData> listMap;
    public List<PopupData> listPopup;
    public GameObject smoke;
    public int coin;
    public bool isMuteSoundAll;
    public bool isMuteSound;
    public bool isBeta;
    public int indexIngame;
    public bool usePlayerPrefs;
}
