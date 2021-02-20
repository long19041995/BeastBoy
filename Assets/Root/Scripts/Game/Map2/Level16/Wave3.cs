using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level16
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject eagle;
        [SerializeField] private GameObject bird;
        [SerializeField] private GameObject wind;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopEagleFly;
        [SerializeField] private GameObject flagStopBirdFly;
        [SerializeField] private GameObject flagStopBirdFlyOut;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M26.FALL, true);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowEagle();

            await Util.Delay(1);
            ShowItem();
            Move(new GameObjectMoved(eagle, flagStopEagleFly, Time.deltaTime * 4, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowBird();
            Move(new GameObjectMoved(bird, flagStopBirdFly, Time.deltaTime * 2, async () =>
            {
                Util.SetAni(bird, Const.Bird.FALL, true);
                Move(new GameObjectMoved(bird, flagStopBirdFlyOut, Time.deltaTime * 2, () =>
                {

                }));

                await Util.Delay(1);
                ShowResult();
            }));

            await Util.Delay(0.5f);
            wind.SetActive(true);
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            eagle.SetActive(false);
            bird.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowEagle()
        {
            eagle.SetActive(true);
            boy.SetActive(false);
            bird.SetActive(false);

            ShowSmoke(eagle);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowBird()
        {
            bird.SetActive(true);
            boy.SetActive(false);
            eagle.SetActive(false);

            ShowSmoke(bird);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
