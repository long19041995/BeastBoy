using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level10
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject mole;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject robot;
        [SerializeField] private GameObject bullet;
        [SerializeField] private GameObject bg;
        [SerializeField] private GameObject bg2;
        [SerializeField] private GameObject pit;
        [SerializeField] private GameObject dino3;
        [SerializeField] private GameObject smoke;
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject team2;
        [SerializeField] private GameObject earthMole;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBulletFly;
        [SerializeField] private GameObject flagStopRobotWalk;
        [SerializeField] private GameObject flagStopMoleMove;
        [SerializeField] private GameObject flagStopCameraMoveWithDino3;
        [SerializeField] private GameObject flagStopDino3Fly;
        [SerializeField] private GameObject flagStopBoyJump;
        [SerializeField] private GameObject flagStopBulletToDino;
        [SerializeField] private GameObject flagStopBulletFlyOut;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagTeam2Position;

        private async void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                team2.transform.position = flagTeam2Position.transform.position;
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                Util.SetTurnBack(boy);

                Util.SetTurnBack(robot);
                Util.SetAni(robot, Const.Robot.SHOOT);

                await Util.Delay(0.5f);
                AudioController.Instance.Play(Const.Common.AUDIOS.WALL_FALL);

                await Util.Delay(0.5f);
                bullet.SetActive(true);
                Move(new GameObjectMoved(bullet, flagStopBulletFly, Time.deltaTime * 6, () => { }));
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 6, () =>
                {
                    AudioController.Instance.Pause(Const.Common.AUDIOS.WALL_FALL);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowMole();
            AudioController.Instance.Play(Const.Common.AUDIOS.WALL_FALL);
            pit.SetActive(true);

            Move(new GameObjectMoved(mole, flagStopMoleMove, Time.deltaTime * 2, () =>
            {
                Move(new GameObjectMoved(mole, boy, Time.deltaTime * 2, () =>
                {
                    ShowItem();
                    ShowBoy();
                    Util.SetTurnBack(boy, 0);
                    Util.SetAni(boy, Const.Boy2.M20.JUMP, false, -1, 0.5f);
                    earthMole.SetActive(false);

                    Move(new GameObjectMoved(boy, flagStopBoyJump, Time.deltaTime * 3, () =>
                    {
                        boy.SetActive(false);

                        dino3.SetActive(true);
                        ShowSmoke(dino3);
                        AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);

                        Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithDino3, Time.deltaTime * 3, () =>
                        {
                            ShowResult();
                        }));

                        Move(new GameObjectMoved(dino3, flagStopDino3Fly, Time.deltaTime * 2, () => { }));
                    }));
                }));
            }));

            AudioController.Instance.Play(Const.Common.AUDIOS.WALL_FALL);

            await Util.Delay(0.5f);
            Move(new GameObjectMoved(bullet, flagStopBulletFlyOut, Time.deltaTime * 6, () => { }));

            await Util.Delay(0.2f);
            bg.SetActive(false);
            bg2.SetActive(true);
        }

        public async override void OnFail()
        {
            ShowDino();
            AudioController.Instance.Play(Const.Common.AUDIOS.WALL_FALL);

            await Util.Delay(0.5f);
            Move(new GameObjectMoved(bullet, flagStopBulletToDino, Time.deltaTime * 4, async () =>
            {
                ShowItem();
                bullet.SetActive(false);
                Util.SetAni(dino, Const.Dino.DIE);
                smoke.SetActive(true);
                explosion.SetActive(true);

                await Util.Delay(1);
                ShowResult();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            mole.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMole()
        {
            mole.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(mole);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            mole.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
