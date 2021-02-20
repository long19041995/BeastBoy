using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Popup
{
    public class Completed : MonoBehaviour
    {
        [SerializeField] private GameObject dog;
        [SerializeField] private GameObject dogFoot;
        [SerializeField] private GameObject boy2;
        [SerializeField] private GameObject scoreDog;
        [SerializeField] private GameObject scoreBoy;
        [SerializeField] private GameObject bone;
        [SerializeField] private GameObject girl3;
        [SerializeField] private Text textScore;
        [SerializeField] private GameObject rotation;
        [SerializeField] private GameObject fireCracker;
        [SerializeField] private GameObject tapToContinueButton;

        private bool isClick = false;
        private TextCounter scoreCounter;

        private async void Start()
        {
            Vector2 position = transform.position;
            position.x += Camera.main.transform.position.x;
            transform.position = position;

            switch (DataController.Instance.CurrentLevelData.type)
            {
                case COMPLETE_TYPE.DOG:
                    dog.SetActive(true);
                    scoreDog.SetActive(true);
                    bone.SetActive(true);
                    break;
                case COMPLETE_TYPE.DOG_FOOT:
                    dogFoot.SetActive(true);
                    scoreDog.SetActive(true);
                    bone.SetActive(true);
                    break;
                case COMPLETE_TYPE.BOY2:
                    boy2.SetActive(true);
                    scoreBoy.SetActive(true);
                    break;
                case COMPLETE_TYPE.GIRL3:
                    girl3.SetActive(true);
                    scoreBoy.SetActive(true);
                    break;
            }

            boy2.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
            rotation.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.6f, 1));
            fireCracker.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.6f, 1));

            textScore.text = DataController.Instance.Coin.ToString();
            scoreCounter = textScore.GetComponent<TextCounter>();

            AudioController.Instance.Play(Const.Common.AUDIOS.WIN_POPUP);
            AudioController.Instance.Play(Const.Common.AUDIOS.WIN_POPUP_ADDED);

            FirebaseController.CompleteTheLevel();

            await Util.Delay(1);
            scoreCounter.Init(DataController.Instance.Coin, DataController.Instance.Coin + 50, 1);
            DataController.Instance.Coin += 50;

            await Util.Delay(1);
            tapToContinueButton.SetActive(true);
        }

        public void OnContinue()
        {
            Gamemanager.Instance.NextWave();
        }

        public async void OnClaim()
        {
            if (isClick) return;

            isClick = true;
            FirebaseController.TapToX5Coin();
            AdMobController.Instance.ShowRewardedAd();
            scoreCounter.Init(DataController.Instance.Coin, DataController.Instance.Coin + 250, 1);
            DataController.Instance.Coin += 250;

            await Util.Delay(3);
            Gamemanager.Instance.NextWave();
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        private void OnDestroy()
        {
            AudioController.Instance.Stop(Const.Common.AUDIOS.WIN_POPUP);
            AudioController.Instance.Stop(Const.Common.AUDIOS.WIN_POPUP_ADDED);
        }
    }
}
