using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level6
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private float speedBoyRun = 2f;

        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject cat;
        [SerializeField] private GameObject fireFly;
        [SerializeField] private GameObject laserGun;
        [SerializeField] private GameObject laser1;
        [SerializeField] private GameObject laser2;
        [SerializeField] private GameObject dark;
        [SerializeField] private GameObject smoke;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopLaserGunMove;
        [SerializeField] private GameObject flagStopCatWalkOut;
        [SerializeField] private GameObject flagStopCameraMoveWithCat;
        [SerializeField] private GameObject flagStopFireFly;

        private async void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * speedBoyRun, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M29.IDLE, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * speedBoyRun, () =>
                {
                    ShowOption();
                }));

                await Util.Delay(0.5f);
                ShowDark();
            }
        }

        private async void ShowDark()
        {
            dark.SetActive(true);
            await Util.Delay(0.1f);
            dark.GetComponent<Dark>().FadeIn(0.7f);
            dark.GetComponent<Dark>().SetFadeTime(0.05f);
        }

        private void HideDark()
        {
            dark.GetComponent<Dark>().FadeOut();
        }

        public async override void OnPass()
        {
            ShowCat();

            await Util.Delay(1);
            ShowItem();
            Util.SetAni(cat, Const.Cat2.IDLE_FLASH, true);

            await Util.Delay(0.5f);
            Util.SetAni(cat, Const.Cat2.WALK_FLASH, true);
            Move(new GameObjectMoved(cat, flagStopCatWalkOut, Time.deltaTime, () => { }));
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithCat, Time.deltaTime * speedBoyRun, () =>
            {
            }));

            await Util.Delay(3);
            NextWave();
            HideDark();
            HideOption();
            boy.transform.position = cat.transform.position;
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(boy, flagStopCatWalkOut, Time.deltaTime * speedBoyRun, () =>
            {
                AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                Util.SetAni(boy, Const.Boy2.M26.FALL, true);
                ShowOption();
            }));
        }

        public async override void OnFail()
        {
            ShowFireFly();

            await Util.Delay(1);
            HideDark();

            Move(new GameObjectMoved(fireFly, flagStopFireFly, Time.deltaTime, () =>
            {
                Move(new GameObjectMoved(laserGun, flagStopLaserGunMove, Time.deltaTime * 4, async () =>
                {
                    ShowItem();

                    laser1.SetActive(true);
                    laser2.SetActive(true);

                    await Util.Delay(0.2f);
                    Util.SetAni(fireFly, Const.FireFly.DIE);
                    fireFly.GetComponent<Rigidbody2D>().gravityScale = 1;
                    smoke.SetActive(true);

                    await Util.Delay(1);
                    ShowResult();
                }));
            }));
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
    }
}
