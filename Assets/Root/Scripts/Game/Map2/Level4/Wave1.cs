using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level4
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private float speedBoyJump = 1f;
        [SerializeField] private float speedBoyRun = 2f;
        [SerializeField] private float speedRobotWalk = 1.5f;

        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject mouse;
        [SerializeField] private GameObject mosquito;
        [SerializeField] private GameObject robot;
        [SerializeField] private GameObject door;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopDoorMove;
        [SerializeField] private GameObject flagStopBoyJump;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopMosquitoFly;
        [SerializeField] private GameObject flagStopRobotWalk;
        [SerializeField] private GameObject flagStopMouseDie;

        private void Start()
        {
            boy.transform.position = flagBoyPosition.transform.position;
            Camera.main.transform.position = flagCameraPosition.transform.position;

            Move(new GameObjectMoved(door, flagStopDoorMove, Time.deltaTime, () =>
            {
                Util.SetAni(boy, Const.Boy2.M20.JUMP);
                Move(new GameObjectMoved(boy, flagStopBoyJump, Time.deltaTime * speedBoyJump, async () =>
                {
                    Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * speedBoyRun, () =>
                    {
                        ShowOption();
                    }));

                    await Util.Delay(1);
                    Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                    Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * speedBoyRun, () =>
                    {
                        AudioController.Instance.Play(Const.Common.AUDIOS.ROBOT, true);
                        Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    }));
                }));
            }));
        }

        public async override void OnPass()
        {
            AudioController.Instance.Play(Const.Common.AUDIOS.FLY);
            ShowMosquito();

            await Util.Delay(1);
            ShowItem();
            Move(new GameObjectMoved(mosquito, flagStopMosquitoFly, Time.deltaTime * speedBoyRun, () =>
            {
                AudioController.Instance.Stop(Const.Common.AUDIOS.FLY);
                ShowResult();
            }));

            await Util.Delay(0.5f);
            Util.SetAni(robot, Const.Robot.IDLE2);
        }

        public async override void OnFail()
        {
            ShowMouse();

            await Util.Delay(1);
            Util.SetAni(robot, Const.Robot.WALK, true);
            Move(new GameObjectMoved(robot, flagStopRobotWalk, Time.deltaTime * speedRobotWalk, async () =>
            {
                Util.SetAni(robot, Const.Robot.ATTACK);

                await Util.Delay(0.4f);
                ShowItem();
                Util.SetAni(mouse, Const.Mouse.DIE, true);
                Move(new GameObjectMoved(mouse, flagStopMouseDie, Time.deltaTime * speedBoyRun, async () =>
                {
                    await Util.Delay(1.5f);
                    ShowResult();
                }));
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            mouse.SetActive(false);
            mosquito.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMouse()
        {
            mouse.SetActive(true);
            boy.SetActive(false);
            mosquito.SetActive(false);

            ShowSmoke(mouse);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMosquito()
        {
            mosquito.SetActive(true);
            boy.SetActive(false);
            mouse.SetActive(false);

            ShowSmoke(mosquito);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
