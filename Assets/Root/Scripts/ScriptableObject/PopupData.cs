using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Popup", menuName = "Scriptable Object / Create Popup", order = 0)]
public class PopupData : ScriptableObject
{
    public Const.Common.POPUP type;
    public GameObject popup;
}
