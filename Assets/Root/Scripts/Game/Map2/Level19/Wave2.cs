using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level19
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject locust;
        [SerializeField] private GameObject tiger;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject electric;
        [SerializeField] private GameObject gateGreen;
        [SerializeField] private GameObject gateRed;
        [SerializeField] private GameObject robot;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopLocustMove;
        [SerializeField] private GameObject flagStopSecurity1Run;
        [SerializeField] private GameObject flagStopSecurity2Run;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopRobotRun;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                Camera.main.transform.position = flagCameraPosition.transform.position;
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () => { }));
                Move(new GameObjectMoved(security1, flagStopSecurity1Run, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(security1, Const.Security.IDLE, true);
                    Move(new GameObjectMoved(security2, flagStopSecurity2Run, Time.deltaTime * 2, () =>
                    {
                        Util.SetAni(security2, Const.Security.IDLE, true);
                        ShowOption();
                    }));
                }));
            }
        }

        public async override void OnPass()
        {
            ShowLotust();

            await Util.Delay(1);
            ShowItem();

            Move(new GameObjectMoved(locust, flagStopLocustMove, Time.deltaTime * 4, () =>
            {
                boy.transform.position = locust.transform.position;
                ShowBoy();
                NextWave();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                }));
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 5, async () =>
                {
                    await Util.Delay(0.5f);
                    gateGreen.SetActive(true);
                    gateRed.SetActive(false);

                    AudioController.Instance.Play(Const.Common.AUDIOS.ROBOT, true);
                    gateGreen.GetComponent<AudioSource>().Play();
                    Move(new GameObjectMoved(robot, flagStopRobotRun, Time.deltaTime * 2, () =>
                    {
                        Util.SetAni(robot, Const.Robot.IDLE, true);
                        ShowOption();
                    }));
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowTiger();

            await Util.Delay(1);
            AudioController.Instance.Play(Const.Common.AUDIOS.ELECTRIC);
            Util.SetAni(security1, Const.Security.ELECTRIC2, true);
            Util.SetAni(security2, Const.Security.ELECTRIC, true);
            Util.SetAni(tiger, Const.Tiger.ATTACK);

            await Util.Delay(1);
            ShowItem();
            Util.SetAni(tiger, Const.Tiger.DIE);
            electric.SetActive(true);

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            locust.SetActive(false);
            tiger.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowLotust()
        {
            locust.SetActive(true);
            boy.SetActive(false);
            tiger.SetActive(false);

            ShowSmoke(locust);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowTiger()
        {
            tiger.SetActive(true);
            boy.SetActive(false);
            locust.SetActive(false);

            ShowSmoke(tiger);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
