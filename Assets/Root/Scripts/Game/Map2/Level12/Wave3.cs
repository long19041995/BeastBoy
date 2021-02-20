using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level12
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject squiltel;
        [SerializeField] private GameObject bird;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopSquiltelFly;
        [SerializeField] private GameObject flagStopCameraMoveWithSquiltel;
        [SerializeField] private GameObject flagStopCameraMoveWithBird;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M26.FALL, true);
                    ShowOption();
                }));
            }
        }

        public override void OnPass()
        {
            ShowSquiltel();

            Move(new GameObjectMoved(squiltel, flagStopSquiltelFly, Time.deltaTime, () =>
            {
                boy.transform.position = squiltel.transform.position;
                Util.SetAni(boy, Const.Boy2.M27.IDLE, true);
                ShowBoy();
                ShowItem();
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithSquiltel, Time.deltaTime, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowBird();
            Util.SetAni(bird, Const.Bird.FLY, true);
            bird.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);
            bird.GetComponent<Rigidbody2D>().AddForce(transform.right * 200);

            await Util.Delay(0.5f);
            ShowItem();
            Util.SetAni(bird, Const.Bird.FALL, true);

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            squiltel.SetActive(false);
            bird.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowSquiltel()
        {
            squiltel.SetActive(true);
            boy.SetActive(false);
            bird.SetActive(false);

            ShowSmoke(squiltel);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowBird()
        {
            bird.SetActive(true);
            boy.SetActive(false);
            squiltel.SetActive(false);

            ShowSmoke(bird, 0);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
