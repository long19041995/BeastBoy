using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level8
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject spider;
        [SerializeField] private GameObject elephant;
        [SerializeField] private GameObject fan;
        [SerializeField] private GameObject backgroundNew;
        [SerializeField] private GameObject spiderWeb;
        [SerializeField] private GameObject winFan;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopSpiderFly;
        [SerializeField] private GameObject flagStopBoyRunOut;
        [SerializeField] private GameObject flagElephantPosition;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                fan.GetComponent<AudioSource>().Play();
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetTurnBack(boy, 0);
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 3, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M20.FLY, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 5, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowSpider();

            await Util.Delay(0.5f);
            spiderWeb.SetActive(true);

            Move(new GameObjectMoved(spider, flagStopSpiderFly, Time.deltaTime * 8, async () =>
            {
                Util.SetAni(spider, Const.Spider.SHOOT_SILK, true, 0, 1);
                await Util.Delay(1);

                Util.SetAniDefault(fan);

                await Util.Delay(1);
                ShowItem();
                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                winFan.SetActive(false);
                fan.GetComponent<AudioSource>().Stop();
                Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, () =>
                {
                    ShowResult();
                }));
            }));
        }

        public async override void OnFail()
        {
            Util.SetRotate(elephant, -79);
            ShowElephant();
            ShowItem();
            elephant.transform.position = flagElephantPosition.transform.position;
            elephant.GetComponent<Rigidbody2D>().gravityScale = 2;
            Util.SetRotate(elephant, -20);

            await Util.Delay(0.5f);
            backgroundNew.SetActive(true);

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            spider.SetActive(false);
            elephant.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowSpider()
        {
            spider.SetActive(true);
            boy.SetActive(false);
            elephant.SetActive(false);

            ShowSmoke(spider);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowElephant()
        {
            elephant.SetActive(true);
            boy.SetActive(false);
            spider.SetActive(false);

            ShowSmoke(elephant);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
