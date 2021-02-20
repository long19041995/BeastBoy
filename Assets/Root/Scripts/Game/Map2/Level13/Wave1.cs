using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level13
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject snake;
        [SerializeField] private GameObject fish;
        [SerializeField] private GameObject corcodie;
        [SerializeField] private GameObject wood;
        [SerializeField] private GameObject wood2;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopFishJump;
        [SerializeField] private GameObject flagStopSnakeSwim;
        [SerializeField] private GameObject flagStopCameraMoveWithSnake;
        [SerializeField] private GameObject flagCameraPositionNextWave;
        [SerializeField] private GameObject flagStopWoodDrift;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                wood2.SetActive(false);
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;
                Util.SetAni(boy, Const.Boy2.M27.IDLE, true);
            }
        }

        public async override void OnPass()
        {
            ShowSnake();
            Util.SetAni(snake, Const.Snake.SWIM_WATER, true);
            Move(new GameObjectMoved(snake, flagStopSnakeSwim, Time.deltaTime, async () =>
            {
                ShowItem();
                boy.transform.position = wood.transform.position;
                Util.SetAni(boy, Const.Boy2.M27.SIT, true);
                ShowBoy();
                NextWave();

                await Util.Delay(2);
                Camera.main.transform.position = flagCameraPositionNextWave.transform.position;
                Util.SetAni(boy, Const.Boy2.M28.IDLE, true);
                wood2.SetActive(true);
                Move(new GameObjectMoved(wood2, flagStopWoodDrift, Time.deltaTime, () =>
                {
                    ShowOption();
                }));
            }));

            await Util.Delay(1);
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithSnake, Time.deltaTime, () => { }));
        }

        public async override void OnFail()
        {
            ShowItem();
            ShowFish();
            Move(new GameObjectMoved(fish, flagStopFishJump, Time.deltaTime * 3, () => { }));
            fish.GetComponent<Rigidbody2D>().AddForce(transform.up * 400);

            await Util.Delay(1f);
            fish.SetActive(false);
            corcodie.SetActive(true);
            Util.SetAni(corcodie, Const.Corcodie.BITE);

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            snake.SetActive(false);
            fish.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowSnake()
        {
            snake.SetActive(true);
            boy.SetActive(false);
            fish.SetActive(false);

            ShowSmoke(snake, 0);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowFish()
        {
            fish.SetActive(true);
            boy.SetActive(false);
            snake.SetActive(false);

            ShowSmoke(fish, 0);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
