using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level7
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private float speedBoyRun = 2f;
        [SerializeField] private float speedLaserGunRun = 4f;
        [SerializeField] private float speedBoyJump = 1f;

        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject bat;
        [SerializeField] private GameObject laserGun;
        [SerializeField] private GameObject laser;
        [SerializeField] private GameObject smoke;
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject smokeFire;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagLaserGunPosition;
        [SerializeField] private GameObject flagStopLaserGunMove;
        [SerializeField] private GameObject flagStopBoyJump;
        [SerializeField] private GameObject flagCameraNextWave;
        [SerializeField] private GameObject flagBoyNextWave;
        [SerializeField] private GameObject flagStopBoyCrawlNextWave;
        [SerializeField] private GameObject flagStopBatFly;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;
                laserGun.transform.position = flagLaserGunPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * speedBoyRun, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.CONFUSION, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * speedBoyRun, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowDino();
            Util.SetTurnBack(dino, 180);

            await Util.Delay(0.5f);
            Util.SetAni(dino, Const.Dino.ATTACK, true);
            ChangeAmount(0.02f);
            ShakeCamera();

            await Util.Delay(1.5f);
            StopShakeCamera();
            ShowItem();
            door.GetComponent<Rigidbody2D>().gravityScale = 1;
            ShakeCamera();

            await Util.Delay(1);
            StopShakeCamera();
            ShowBoy();

            await Util.Delay(0.6f);
            Util.SetAni(boy, Const.Boy2.M20.JUMP, false, -1, 0.7f);

            await Util.Delay(0.2f);
            Move(new GameObjectMoved(boy, flagStopBoyJump, Time.deltaTime * speedBoyJump, async () =>
            {
                await Util.Delay(1);

                NextWave();

                Camera.main.transform.position = flagCameraNextWave.transform.position;
                boy.transform.position = flagBoyNextWave.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.CREEP, true);
                Move(new GameObjectMoved(boy, flagStopBoyCrawlNextWave, Time.deltaTime, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.IDLE_SMOKE, true);
                    ShowOption();
                }));

                await Util.Delay(2);
                Util.SetAni(boy, Const.Boy2.M20.CREEP_SMOKE, true);
                smokeFire.SetActive(true);
            }));
        }

        public override void OnFail()
        {
            ShowBat();

            Move(new GameObjectMoved(bat, flagStopBatFly, Time.deltaTime, () =>
            {
                Move(new GameObjectMoved(laserGun, flagStopLaserGunMove, Time.deltaTime * 4, async () =>
                {
                    laser.SetActive(true);
                    ShowItem();

                    Util.SetAni(bat, Const.Bat.DIE);
                    bat.GetComponent<Rigidbody2D>().gravityScale = 1;
                    smoke.SetActive(true);

                    await Util.Delay(2);
                    ShowResult();
                }));
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            dino.SetActive(false);
            bat.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            bat.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowBat()
        {
            bat.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(bat);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
