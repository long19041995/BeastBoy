using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level20
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject snake;
        [SerializeField] private GameObject bear;
        [SerializeField] private GameObject security;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject net;
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject doctor;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopSecurityRun;
        [SerializeField] private GameObject flagStopNetMove;
        [SerializeField] private GameObject flagStopSnakeMove;
        [SerializeField] private GameObject flagSecurityPositionNextWave;
        [SerializeField] private GameObject flagNetPositionNextWave;
        [SerializeField] private GameObject flagBoyPositionNextWave;
        [SerializeField] private GameObject flagCameraPositionNextWave;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                }));

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(security, flagStopSecurityRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    Util.SetAni(security1, Const.Security.IDLE, true);
                    Util.SetAni(security2, Const.Security.IDLE, true);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowSnake();
            // door.GetComponent<Rigidbody2D>().gravityScale = 1;

            await Util.Delay(0.5f);
            ShowItem();

            Move(new GameObjectMoved(snake, flagStopSnakeMove, Time.deltaTime * 4, () =>
            {
                NextWave();
                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                OnNextWave();
            }));
        }

        private void OnNextWave()
        {
            Util.SetAni(security1, Const.Security.IDLE, true);
            Util.SetAni(doctor, Const.Doctor.IDLE_MACHINE, true);
            security1.transform.position = flagSecurityPositionNextWave.transform.position;
            net.transform.position = flagNetPositionNextWave.transform.position;
            boy.transform.position = flagBoyPositionNextWave.transform.position;
            Camera.main.transform.position = flagCameraPositionNextWave.transform.position;

            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () =>
            {
                ShowOption();
            }));
        }

        public async override void OnFail()
        {
            ShowBear();

            Util.SetAni(bear, Const.Bear.ATTACK);
            Util.SetAni(security1, Const.Security.NET);

            await Util.Delay(0.5f);
            net.SetActive(true);
            Move(new GameObjectMoved(net, flagStopNetMove, Time.deltaTime * 8, async () =>
            {
                ShowItem();
                Util.SetAni(bear, Const.Bear.DIE);

                await Util.Delay(1);
                ShowResult();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            snake.SetActive(false);
            bear.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowSnake()
        {
            snake.SetActive(true);
            boy.SetActive(false);
            bear.SetActive(false);

            ShowSmoke(snake);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowBear()
        {
            bear.SetActive(true);
            boy.SetActive(false);
            snake.SetActive(false);

            ShowSmoke(bear);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
