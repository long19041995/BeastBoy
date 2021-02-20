using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level5
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject tiger;
        [SerializeField] private GameObject security;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject security3;
        [SerializeField] private GameObject electric;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopSecurityRun;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopBoyRunOut;
        [SerializeField] private GameObject flagStopSecurityRunOut;

        private void Start()
        {
            boy.transform.position = flagBoyPosition.transform.position;
            Camera.main.transform.position = flagCameraPosition.transform.position;

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
            {
                ShowOption();
            }));

            Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);

                AudioController.Instance.Play(Const.Common.AUDIOS.BREATHING, true, 0.2f);
                Move(new GameObjectMoved(security, flagStopSecurityRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(security1, Const.Security.IDLE, true);
                    Util.SetAni(security2, Const.Security.IDLE, true);
                    Util.SetAni(security3, Const.Security.IDLE, true);
                }));
            }));
        }

        public async override void OnPass()
        {
            ShowDino();

            Util.SetAni(security1, Const.Security.AFRAID, true);
            Util.SetAni(security2, Const.Security.AFRAID, true);
            Util.SetAni(security3, Const.Security.AFRAID, true);

            await Util.Delay(1);
            Util.SetAni(dino, Const.Dino.STAMP);
            ShakeCamera();

            await Util.Delay(0.5f);
            Util.SetTurnBack(security1, 0);
            Util.SetTurnBack(security2);
            Util.SetTurnBack(security3);
            Util.SetAni(security1, Const.Security.RUN_AFRAID, true);
            Util.SetAni(security2, Const.Security.RUN_AFRAID, true);
            Util.SetAni(security3, Const.Security.RUN_AFRAID, true);
            Move(new GameObjectMoved(security, flagStopSecurityRunOut, Time.deltaTime * 2, () => { }));
            ShowItem();

            await Util.Delay(1);
            Util.SetAni(dino, Const.Dino.IDLE, true);

            await Util.Delay(0.5f);
            StopShakeCamera();
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowTiger();

            await Util.Delay(0.5f);

            ShowItem();
            Util.SetAni(security1, Const.Security.ELECTRIC2);
            AudioController.Instance.Play(Const.Common.AUDIOS.ELECTRIC);

            await Util.Delay(0.5f);
            Util.SetAni(tiger, Const.Tiger.ELECTRIC, true);
            electric.SetActive(true);

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            dino.SetActive(false);
            tiger.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            tiger.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowTiger()
        {
            tiger.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(tiger);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
