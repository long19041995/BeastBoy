using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Map", menuName ="Scriptable Object / Create Map", order =0)]
public class MapData : ScriptableObject
{
    public int indexLevel;
    public List<LevelData> listLevel;
}
