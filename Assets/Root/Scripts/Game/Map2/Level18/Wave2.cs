using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level18
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject monkey;
        [SerializeField] private GameObject airplane;
        [SerializeField] private GameObject laser1;
        [SerializeField] private GameObject laser2;
        [SerializeField] private BackgroundInfinite background;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopAirplaneFly;
        [SerializeField] private GameObject flagStopDinoFlyNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagStopAirplaneFlyNextWave;
        [SerializeField] private GameObject flagStopMonkeyJump;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                Util.SetTurnBack(boy);
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                airplane.SetActive(true);
                Move(new GameObjectMoved(airplane, flagStopAirplaneFly, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }
        }

        public override void OnPass()
        {
            ShowDino();
            ShowItem();

            Move(new GameObjectMoved(dino, flagStopDinoFlyNextWave, Time.deltaTime * 4, () =>
            {
                NextWave();
                background.Move();
                Util.SetTurnBack(airplane);
                Move(new GameObjectMoved(airplane, flagStopAirplaneFlyNextWave, Time.deltaTime * 4, () =>
                {
                    ShowOption();
                }));
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 4, () => { }));
        }

        public async override void OnFail()
        {
            ShowMonkey();

            await Util.Delay(1);
            Util.SetAni(monkey, Const.Monkey.JUMP1);

            await Util.Delay(0.3f);
            Move(new GameObjectMoved(monkey, flagStopMonkeyJump, Time.deltaTime * 2, () => { }));
            Util.SetAni(monkey, Const.Monkey.JUMP2);

            await Util.Delay(0.1f);
            laser1.SetActive(true);
            laser2.SetActive(true);
            Util.SetAni(monkey, Const.Monkey.DIE_BLACK);

            await Util.Delay(0.1f);
            laser1.SetActive(false);
            laser2.SetActive(false);
            Util.SetAni(monkey, Const.Monkey.DIE_BLACK2);
            monkey.GetComponent<Rigidbody2D>().gravityScale = 2;

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            dino.SetActive(false);
            monkey.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            monkey.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMonkey()
        {
            monkey.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(monkey);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
