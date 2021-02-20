using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level13
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject fireFly;
        [SerializeField] private GameObject cat;
        [SerializeField] private List<GameObject> eyes;
        [SerializeField] private GameObject dark;
        [SerializeField] private GameObject flicker;
        [SerializeField] private GameObject messageBoy;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCatRun;
        [SerializeField] private GameObject flagStopFireFly;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                Camera.main.transform.position = flagCameraPosition.transform.position;
                boy.transform.position = flagBoyPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime, async () =>
                {
                    Util.SetAni(boy, Const.Boy2.M29.IDLE, true);
                    ShowDark();

                    await Util.Delay(1);
                    messageBoy.SetActive(true);
                    Util.ShowMessage(boy, messageBoy, 0.3f, 1.2f);

                    await Util.Delay(1);
                    messageBoy.SetActive(false);

                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowFireFly();

            await Util.Delay(1);
            flicker.SetActive(true);

            await Util.Delay(1);
            ShowItem();
            HideDark();

            await Util.Delay(1);
            Move(new GameObjectMoved(fireFly, flagStopFireFly, Time.deltaTime * 3, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowCat();

            await Util.Delay(0.5f);
            ShowEyes();
            Util.SetAni(cat, Const.Cat2.AFRAID, true);

            await Util.Delay(2);
            ShowItem();
            SetEyesAttack();
            Util.SetTurnBack(cat, 0);
            Util.SetAni(cat, Const.Cat2.RUN, true);
            Move(new GameObjectMoved(cat, flagStopCatRun, Time.deltaTime * 2, () => { }));

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowEyes()
        {
            foreach (GameObject eye in eyes)
            {
                eye.SetActive(true);
            }
        }

        private void SetEyesAttack()
        {
            foreach (GameObject eye in eyes)
            {
                Util.SetAni(eye, Const.Eyes.ATTACK, true);
            }
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            cat.SetActive(false);
            fireFly.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowCat()
        {
            cat.SetActive(true);
            boy.SetActive(false);
            fireFly.SetActive(false);

            ShowSmoke(cat);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowFireFly()
        {
            fireFly.SetActive(true);
            boy.SetActive(false);
            cat.SetActive(false);

            ShowSmoke(fireFly);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private async void ShowDark()
        {
            dark.SetActive(true);

            await Util.Delay(0.1f);
            dark.GetComponent<Dark>().FadeIn(0.5f);
        }

        private void HideDark()
        {
            dark.GetComponent<Dark>().FadeOut();
        }
    }
}
