using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level18
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject spider;
        [SerializeField] private GameObject bird;
        [SerializeField] private GameObject laser1;
        [SerializeField] private GameObject laser2;
        [SerializeField] private GameObject airplane;
        [SerializeField] private GameObject messageBoy;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopSpiderWalk;
        [SerializeField] private GameObject flagStopBirdFly;
        [SerializeField] private GameObject flagStopCameraMoveWithSpider;
        [SerializeField] private GameObject flagStopCameraMoveWithBird;
        [SerializeField] private GameObject flagBoyPositionNextWave;
        [SerializeField] private GameObject flagStopAirplaneFly;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, async () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    messageBoy.SetActive(true);
                    Util.ShowMessage(boy, messageBoy, 0.3f, 1.2f);

                    await Util.Delay(2);
                    messageBoy.SetActive(false);

                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowSpider();

            Move(new GameObjectMoved(spider, flagStopSpiderWalk, Time.deltaTime * 2, () =>
            {
                NextWave();
                boy.transform.position = flagBoyPositionNextWave.transform.position;
                Util.SetTurnBack(boy);
                ShowBoy();

                airplane.SetActive(true);
                Move(new GameObjectMoved(airplane, flagStopAirplaneFly, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithSpider, Time.deltaTime * 2, () => { }));

            await Util.Delay(2);
            laser1.SetActive(true);
            laser2.SetActive(true);
            ShowItem();
        }

        public override void OnFail()
        {
            ShowBird();

            Move(new GameObjectMoved(bird, flagStopBirdFly, Time.deltaTime * 2, async () =>
            {
                ShowItem();
                laser1.SetActive(true);
                laser2.SetActive(true);
                Util.SetAni(bird, Const.Bird.DIE, true);
                bird.GetComponent<Rigidbody2D>().gravityScale = 1;

                await Util.Delay(2);
                ShowResult();
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithBird, Time.deltaTime * 2, () =>
            {
                ShowOption();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            spider.SetActive(false);
            bird.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowSpider()
        {
            spider.SetActive(true);
            boy.SetActive(false);
            bird.SetActive(false);

            ShowSmoke(spider);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowBird()
        {
            bird.SetActive(true);
            boy.SetActive(false);
            spider.SetActive(false);

            ShowSmoke(bird);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
