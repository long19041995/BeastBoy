using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level13
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private float speedWood = 1f;
        [SerializeField] private float speedBird = 1f;
        [SerializeField] private float speedKangaroo = 1f;
        [SerializeField] private float speedDown = 1f;
        [SerializeField] private float speedBoyRun = 2;

        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject bird;
        [SerializeField] private GameObject kangaroo;
        [SerializeField] private GameObject wood;
        [SerializeField] private GameObject nest;
        [SerializeField] private GameObject dark;
        [SerializeField] private GameObject messageBoy;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopWoodDrift;
        [SerializeField] private GameObject flagStopBirdFlyUp;
        [SerializeField] private GameObject flagStopBirdFlyDown;
        [SerializeField] private GameObject flagStopKangarooJump;
        [SerializeField] private GameObject flagStopCameraMoveWithBird;
        [SerializeField] private GameObject flagDownKangaroo;
        [SerializeField] private GameObject flagDownNest;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagStopBoyRun;

        private bool isWoodDrift = false;
        private bool isBirdFlyUp = false;
        private bool isBirdflyDown = false;
        private bool isKangarooJump = false;
        private bool isCameraMoveWithBird = false;
        private bool isKangarooDown = false;
        private bool isNestDown = false;
        private bool isMoveCameraNextWave = false;
        private bool isBoyWalk = false;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                Camera.main.transform.position = flagCameraPosition.transform.position;
                wood.SetActive(true);
                Util.SetAni(boy, Const.Boy2.M28.IDLE, true);
                Move(new GameObjectMoved(wood, flagStopWoodDrift, Time.deltaTime, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowBird();
            Move(new GameObjectMoved(bird, flagStopBirdFlyUp, Time.deltaTime * 3, async () =>
            {
                await Util.Delay(0.5f);
                Move(new GameObjectMoved(bird, flagStopBirdFlyDown, Time.deltaTime * 3, async () =>
                {
                    boy.transform.position = bird.transform.position;
                    Util.SetAni(boy, Const.Boy2.M20.CONFUSION, true);
                    ShowBoy();
                    ShowItem();

                    await Util.Delay(1);
                    NextWave();

                    Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                    Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, async () =>
                    {
                        dark.SetActive(true);
                        Util.SetAni(boy, Const.Boy2.M29.IDLE, true);
                        ShowDark();

                        await Util.Delay(1);
                        messageBoy.SetActive(true);
                        Util.ShowMessage(boy, messageBoy, 0.3f, 1.2f);

                        await Util.Delay(1);
                        messageBoy.SetActive(false);

                        ShowOption();
                    }));
                    Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () => { }));
                }));
            }));

            await Util.Delay(1.5f);
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithBird, Time.deltaTime * 3, () => { }));
        }

        public async override void OnFail()
        {
            ShowKangaroo();

            await Util.Delay(0.5f);
            Util.SetAni(kangaroo, Const.Kangaroo.FALL);
            Move(new GameObjectMoved(kangaroo, flagStopKangarooJump, Time.deltaTime * 5, () =>
            {
                ShowItem();
                Move(new GameObjectMoved(kangaroo, flagDownKangaroo, Time.deltaTime, () =>
                {
                    ShowResult();
                }));
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            bird.SetActive(false);
            kangaroo.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowBird()
        {
            bird.SetActive(true);
            boy.SetActive(false);
            kangaroo.SetActive(false);

            ShowSmoke(bird, 0);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowKangaroo()
        {
            kangaroo.SetActive(true);
            boy.SetActive(false);
            bird.SetActive(false);

            ShowSmoke(kangaroo);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private async void ShowDark()
        {
            dark.SetActive(true);

            await Util.Delay(0.1f);
            dark.GetComponent<Dark>().FadeIn(0.5f);
        }

        private void SetCameraStopMoveNextWave()
        {
            isMoveCameraNextWave = false;
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            isBoyWalk = true;
        }
    }
}
