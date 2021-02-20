using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : Singleton<PopupController>
{
    public GameData gameData;
    GameObject currentPopup;

    public void Init (Const.Common.POPUP type)
    {
        Destroy();
        foreach(PopupData popup in gameData.listPopup)
        {
            if (popup.type == type)
            {
                currentPopup = Instantiate(popup.popup);
            }
        }
    }

    public void Destroy()
    {
        Destroy(currentPopup);
    }
}
