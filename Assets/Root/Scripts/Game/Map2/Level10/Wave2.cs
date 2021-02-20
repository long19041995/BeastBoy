using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level10
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject doctor1;
        [SerializeField] private GameObject monkey;
        [SerializeField] private GameObject eel;
        [SerializeField] private GameObject robot1;
        [SerializeField] private GameObject robot2;
        [SerializeField] private GameObject team2;
        [SerializeField] private GameObject net;
        [SerializeField] private GameObject bullet;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopTeam2Move;
        [SerializeField] private GameObject flagStopMonkeyTurn;
        [SerializeField] private GameObject flagStopEelMove;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopRobotWalk;
        [SerializeField] private GameObject flagStopEelMoveUp;
        [SerializeField] private GameObject flagStopEelMoveNextWave;
        [SerializeField] private GameObject flagStopNetMove;
        [SerializeField] private GameObject flagCameraPosition2;
        [SerializeField] private GameObject flagStopBulletFly;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                doctor1.SetActive(false);
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    ShowOption();
                }));

                AudioController.Instance.Play(Const.Common.AUDIOS.ROBOT, true);
                Move(new GameObjectMoved(team2, flagStopTeam2Move, Time.deltaTime, () =>
                {
                    Util.SetAni(robot1, Const.Robot.IDLE, true);
                    Util.SetAni(robot2, Const.Robot.IDLE, true);
                }));
            }
        }

        public async override void OnPass()
        {
            Util.SetAni(robot2, Const.Robot.ATTACK);

            await Util.Delay(0.5f);
            Util.SetAni(boy, Const.Boy2.M20.FALL, true);

            Move(new GameObjectMoved(boy, flagStopEelMoveUp, Time.deltaTime * 3, async () =>
            {
                await Util.Delay(1.5f);

                eel.transform.position = flagStopEelMoveUp.transform.position;
                ShowEel();
                Util.SetAni(eel, Const.Eel.IDLE2, true);
                ShowItem();

                await Util.Delay(0.5f);
                Move(new GameObjectMoved(eel, flagStopEelMove, Time.deltaTime * 6, async () =>
                {
                    Move(new GameObjectMoved(eel, flagStopEelMoveNextWave, Time.deltaTime * 4, () =>
                    {
                        boy.transform.position = eel.transform.position;
                        ShowBoy();

                        Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                        Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
                        {
                            Util.SetAni(robot1, Const.Robot.IDLE, true);
                            Util.SetTurnBack(boy);
                            Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                        }));
                    }));

                    Util.SetAni(robot1, Const.Robot.SHOOT);

                    await Util.Delay(0.5f);
                    net.SetActive(true);
                    Move(new GameObjectMoved(net, flagStopNetMove, Time.deltaTime * 4, () => { }));

                    await Util.Delay(1);
                    AudioController.Instance.Play(Const.Common.AUDIOS.ROBOT, true);
                    Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 3, async () =>
                    {
                        NextWave();
                        await Util.Delay(1);
                        Camera.main.transform.position = flagCameraPosition2.transform.position;

                        await Util.Delay(1);
                        Util.SetTurnBack(robot1);

                        await Util.Delay(1);
                        Util.SetAni(robot1, Const.Robot.SHOOT);

                        await Util.Delay(0.5f);
                        AudioController.Instance.Play(Const.Common.AUDIOS.WALL_FALL);

                        await Util.Delay(0.5f);
                        bullet.SetActive(true);
                        Move(new GameObjectMoved(bullet, flagStopBulletFly, Time.deltaTime * 6, () => { }));
                        Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 6, () =>
                        {
                            AudioController.Instance.Pause(Const.Common.AUDIOS.WALL_FALL);
                            ShowOption();
                        }));
                    }));
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowMonkey();

            await Util.Delay(1);
            Util.SetAni(monkey, Const.Monkey.TURN);
            Util.SetAni(robot2, Const.Robot.IDLE2);

            await Util.Delay(0.3f);
            Util.SetAni(robot1, Const.Robot.SHOOT);
            Move(new GameObjectMoved(monkey, flagStopMonkeyTurn, Time.deltaTime * 8, async () =>
            {
                net.SetActive(true);
                Move(new GameObjectMoved(net, flagStopNetMove, Time.deltaTime * 4, () => { }));

                ShowItem();
                Util.SetAni(monkey, Const.Monkey.DIE, true);

                await Util.Delay(2);
                ShowResult();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            monkey.SetActive(false);
            eel.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMonkey()
        {
            monkey.SetActive(true);
            boy.SetActive(false);
            eel.SetActive(false);

            ShowSmoke(monkey);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowEel()
        {
            eel.SetActive(true);
            boy.SetActive(false);
            monkey.SetActive(false);

            ShowSmoke(eel);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
