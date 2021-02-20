using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level8
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private float speedBoyFall = 2f;

        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject elephant;
        [SerializeField] private GameObject eagle;
        [SerializeField] private GameObject security;
        [SerializeField] private GameObject backgroundNew;
        [SerializeField] private GameObject fan;
        [SerializeField] private GameObject wind;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyCrawl;
        [SerializeField] private GameObject flagStopBoyFall;
        [SerializeField] private GameObject flagStopEagleFly;
        [SerializeField] private GameObject flagStopSecurityCrawl;
        [SerializeField] private GameObject flagStopElephantFall;
        [SerializeField] private GameObject flagStopCameraMoveWithElephant;
        [SerializeField] private GameObject flagStopBoyFallNextWave;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagBoyPosition2;
        [SerializeField] private GameObject flagStopEagleFlyOut;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyCrawl, Time.deltaTime, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M20.FALL, true);
                    Move(new GameObjectMoved(boy, flagStopBoyFall, Time.deltaTime * speedBoyFall, () =>
                    {
                        ShowOption();
                    }));
                }));
            }
        }

        public override void OnPass()
        {
            ShowElephant();
            ShowItem();
            backgroundNew.SetActive(true);

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithElephant, Time.deltaTime * speedBoyFall, () => { }));

            Move(new GameObjectMoved(elephant, flagStopElephantFall, Time.deltaTime * speedBoyFall, async () =>
            {
                NextWave();

                await Util.Delay(2f);
                boy.transform.position = flagBoyPosition2.transform.position;
                ShowBoy();
                Util.SetTurnBack(boy, 0);

                Move(new GameObjectMoved(boy, flagStopBoyFallNextWave, Time.deltaTime * speedBoyFall, () =>
                {
                    fan.GetComponent<AudioSource>().Play();
                    backgroundNew.SetActive(false);
                    Util.SetAni(boy, Const.Boy2.M20.RUN, true);

                    Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 3, () =>
                    {
                        AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                        Util.SetAni(boy, Const.Boy2.M20.FLY, true);
                        ShowOption();
                    }));

                    Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 10, () =>
                    {
                    }));
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowEagle();

            await Util.Delay(1);
            Move(new GameObjectMoved(eagle, flagStopEagleFly, Time.deltaTime * 2, async () =>
            {
                wind.SetActive(true);
                ShowItem();

                await Util.Delay(0.5f);
                Util.SetAni(eagle, Const.Eagle.OUT, true);
                Move(new GameObjectMoved(eagle, flagStopEagleFlyOut, Time.deltaTime, () => { }));

                await Util.Delay(2);
                ShowResult();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            elephant.SetActive(false);
            eagle.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowElephant()
        {
            elephant.SetActive(true);
            boy.SetActive(false);
            eagle.SetActive(false);

            ShowSmoke(elephant);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowEagle()
        {
            eagle.SetActive(true);
            boy.SetActive(false);
            elephant.SetActive(false);

            ShowSmoke(eagle);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
