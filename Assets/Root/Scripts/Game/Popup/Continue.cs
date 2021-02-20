using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popup
{
    public class Continue : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private Text textScore;
        [SerializeField] private GameObject claimButton;
        private TextCounter scoreCounter;
        private float time = 5;
        private float timeCircle = 6;
        private bool isClick = false;

        private void Start()
        {
            textScore.text = DataController.Instance.Coin.ToString();
            scoreCounter = textScore.GetComponent<TextCounter>();
            InvokeRepeating("DecreaseTime", 1, 1);

            FirebaseController.FailTheLevel();
            if (DataController.Instance.Coin < 200)
            {
                claimButton.SetActive(false);
            }
        }

        private void Update()
        {
            if (timeCircle > 0 && !isClick)
            {
                timeCircle -= Time.deltaTime;
                fillImage.fillAmount = timeCircle / 5;
            }
        }

        public void DecreaseTime()
        {
            if (isClick)
            {
                CancelInvoke();
                return;
            }

            if (time >= 0)
            {
                textMeshProUGUI.text = time.ToString();
                time--;
            }
            else
            {
                CancelInvoke();
                DataController.Instance.IndexWave = 0;
                Gamemanager.Instance.ResetLevel();
            }
        }

        public async void OnClickFree()
        {
            if (isClick) return;
            isClick = true;

            FirebaseController.TapToContinue();
            AdMobController.Instance.ShowRewardedAd();

            await Util.Delay(3);
            Gamemanager.Instance.ResetLevel();
        }

        public void OnClickDecreaseCoin()
        {
            if (isClick) return;
            isClick = true;

            FirebaseController.TapToClaim();
            //scoreCounter.Init(Data.Instance.Coin, Data.Instance.Coin - 200, 1);
            DataController.Instance.Coin -= 200;

            //await Util.Delay(3);
            Gamemanager.Instance.ResetLevel();
        }
    }
}
