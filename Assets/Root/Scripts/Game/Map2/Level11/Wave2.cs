using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level11
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject snake;
        [SerializeField] private GameObject eagle;
        [SerializeField] private GameObject wind;
        [SerializeField] private GameObject messageBoy;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopCameraveMoveWithBoy;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopSnakeMoveToStone;
        [SerializeField] private GameObject flagStopSnakeMoveToEnd;
        [SerializeField] private GameObject flagStopCameraMoveWithSnake;
        [SerializeField] private GameObject flagStopEagleFly;
        [SerializeField] private GameObject flagStopCameraMoveWithEagle;
        [SerializeField] private GameObject flagStopEgleFlyWithWind;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M22.IDLE, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraveMoveWithBoy, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowSnake();

            await Util.Delay(1);
            Move(new GameObjectMoved(snake, flagStopSnakeMoveToStone, Time.deltaTime, () =>
            {
                wind.SetActive(true);
                Move(new GameObjectMoved(snake, flagStopSnakeMoveToEnd, Time.deltaTime, () =>
                {
                    wind.SetActive(false);
                    ShowItem();
                    boy.transform.position = snake.transform.position;
                    ShowBoy();
                    Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                    Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, async () =>
                    {
                        NextWave();
                        Util.SetAni(boy, Const.Boy2.M23.HUNGRY, true);

                        await Util.Delay(1);
                        messageBoy.SetActive(true);
                        Util.ShowMessage(boy, messageBoy, 0.5f, 1.5f);

                        await Util.Delay(1);
                        messageBoy.SetActive(false);

                        await Util.Delay(1);
                        ShowOption();
                    }));

                    Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () => { }));
                }));
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithSnake, Time.deltaTime, () => { }));
        }

        public async override void OnFail()
        {
            ShowEagle();

            await Util.Delay(1);
            Move(new GameObjectMoved(eagle, flagStopEagleFly, Time.deltaTime * 2, async () =>
            {
                ShowItem();
                Util.SetAni(eagle, Const.Eagle.OUT, true);
                wind.SetActive(true);
                Move(new GameObjectMoved(eagle, flagStopEgleFlyWithWind, Time.deltaTime, () => { }));

                await Util.Delay(2);
                ShowResult();
            }));

            await Util.Delay(1);
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithEagle, Time.deltaTime, () => { }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            snake.SetActive(false);
            eagle.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowSnake()
        {
            snake.SetActive(true);
            boy.SetActive(false);
            eagle.SetActive(false);

            ShowSmoke(snake, 0);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowEagle()
        {
            eagle.SetActive(true);
            boy.SetActive(false);
            snake.SetActive(false);

            ShowSmoke(eagle);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
