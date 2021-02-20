using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popup
{
    public class GameOver : MonoBehaviour
    {
        private void Start()
        {
            AudioController.Instance.Play(Const.Common.AUDIOS.GAME_OVER);
            FirebaseController.FailTheLevel();    
        }
    }
}
