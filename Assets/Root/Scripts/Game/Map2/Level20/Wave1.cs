using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level20
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject monkey;
        [SerializeField] private GameObject pangolin;
        [SerializeField] private GameObject laser1;
        [SerializeField] private GameObject laser2;
        [SerializeField] private GameObject security;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject smoke;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopPangolinMove;
        [SerializeField] private GameObject flagStopMonkeyJump;
        [SerializeField] private GameObject flagBoyPositionNextWave;
        [SerializeField] private GameObject flagCameraPositionNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagStopSecurityRunNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowMonkey();

            await Util.Delay(1);

            Util.SetAni(monkey, Const.Monkey.TURN, true);
            ShowItem();
            Move(new GameObjectMoved(monkey, flagStopMonkeyJump, Time.deltaTime * 3, () =>
            {
                boy.transform.position = monkey.transform.position;
                ShowBoy();
                NextWave();
                OnNextWave();
            }));

            await Util.Delay(0.5f);
            laser1.SetActive(true);

            await Util.Delay(0.8f);
            laser2.SetActive(true);
        }

        private void OnNextWave()
        {
            boy.transform.position = flagBoyPositionNextWave.transform.position;
            Camera.main.transform.position = flagCameraPositionNextWave.transform.position;

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () =>
            {
            }));

            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(security, flagStopSecurityRunNextWave, Time.deltaTime * 2, () =>
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                Util.SetAni(security1, Const.Security.IDLE, true);
                Util.SetAni(security2, Const.Security.IDLE, true);
                ShowOption();
            }));
        }

        public async override void OnFail()
        {
            ShowPangolin();

            Util.SetAni(pangolin, Const.Pangolin.ATTACK, true);
            Move(new GameObjectMoved(pangolin, flagStopPangolinMove, Time.deltaTime * 2, async () =>
            {
                ShowItem();
                smoke.SetActive(true);
                Util.SetAni(pangolin, Const.Pangolin.DIE2);

                await Util.Delay(1);
                ShowResult();
            }));

            laser1.SetActive(true);

            await Util.Delay(1);
            laser2.SetActive(true);
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            monkey.SetActive(false);
            pangolin.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMonkey()
        {
            monkey.SetActive(true);
            boy.SetActive(false);
            pangolin.SetActive(false);

            ShowSmoke(monkey);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowPangolin()
        {
            pangolin.SetActive(true);
            boy.SetActive(false);
            monkey.SetActive(false);

            ShowSmoke(pangolin);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
