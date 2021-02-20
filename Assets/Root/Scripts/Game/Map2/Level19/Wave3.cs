using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level19
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject mouse;
        [SerializeField] private GameObject rhino;
        [SerializeField] private GameObject robot;
        [SerializeField] private GameObject gateGreen;
        [SerializeField] private GameObject gateRed;
        [SerializeField] private GameObject net;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopRobotRun;
        [SerializeField] private GameObject flagStopNetMove;
        [SerializeField] private GameObject flagStopBoyRunOut;
        [SerializeField] private GameObject flagStopMouseJump;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                boy.SetActive(true);
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, async () =>
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
            }
        }

        public async override void OnPass()
        {
            mouse.transform.position = boy.transform.position;
            ShowMouse();

            await Util.Delay(0.5f);
            Util.SetAni(mouse, Const.Mouse.JUMP, true, -1, 0.7f);

            await Util.Delay(0.2f);
            Move(new GameObjectMoved(mouse, flagStopMouseJump, Time.deltaTime * 4, async () =>
            {
                ShowItem();
                mouse.SetActive(false);
                Util.SetAni(robot, Const.Robot.SUDDEN);

                await Util.Delay(0.5f);
                AudioController.Instance.Play(Const.Common.AUDIOS.ELECTRIC);
                Util.SetAni(robot, Const.Robot.DIE);

                await Util.Delay(2);
                boy.transform.position = mouse.transform.position;
                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, () =>
                {
                    ShowResult();
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowRhino();

            Util.SetAni(robot, Const.Robot.SHOOT);

            await Util.Delay(0.5f);
            net.SetActive(true);
            Move(new GameObjectMoved(net, flagStopNetMove, Time.deltaTime * 4, async () =>
            {
                ShowItem();
                Util.SetAni(robot, Const.Robot.IDLE, true);
                Util.SetAni(rhino, Const.Rhino.DIE2);

                await Util.Delay(2);
                ShowResult();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            mouse.SetActive(false);
            rhino.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMouse()
        {
            mouse.SetActive(true);
            boy.SetActive(false);
            rhino.SetActive(false);

            ShowSmoke(mouse);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowRhino()
        {
            rhino.SetActive(true);
            boy.SetActive(false);
            mouse.SetActive(false);

            ShowSmoke(rhino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
