using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level7
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private float speedBoyCrawl = 1f;

        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject cockroach;
        [SerializeField] private GameObject mouse;
        [SerializeField] private GameObject smokeFire;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopBoyCrawl;
        [SerializeField] private GameObject flagStopCockroachRun;
        [SerializeField] private GameObject flagStopMouseRun;

        private async void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.CREEP, true);
                Move(new GameObjectMoved(boy, flagStopBoyCrawl, Time.deltaTime * speedBoyCrawl, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.IDLE_SMOKE, true);
                    ShowOption();
                }));

                await Util.Delay(2);
                Util.SetAni(boy, Const.Boy2.M20.CREEP_SMOKE, true);
                smokeFire.SetActive(true);
            }
        }

        public async override void OnPass()
        {
            ShowCockroach();

            await Util.Delay(0.5f);
            ShowItem();

            Move(new GameObjectMoved(cockroach, flagStopCockroachRun, Time.deltaTime * 3, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowMouse();

            await Util.Delay(0.5f);
            Util.SetAni(mouse, Const.Mouse.WALK, true);
            Move(new GameObjectMoved(mouse, flagStopMouseRun, Time.deltaTime * 2, async () =>
            {
                ShowItem();
                Util.SetAni(mouse, Const.Mouse.CHOKE);
                await Util.Delay(3);
                ShowResult();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            cockroach.SetActive(false);
            mouse.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowCockroach()
        {
            cockroach.SetActive(true);
            boy.SetActive(false);
            mouse.SetActive(false);

            ShowSmoke(cockroach);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMouse()
        {
            mouse.SetActive(true);
            boy.SetActive(false);
            cockroach.SetActive(false);

            ShowSmoke(mouse);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
