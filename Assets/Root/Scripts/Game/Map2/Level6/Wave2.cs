using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Map2.Level6
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private float speedBoyRun = 1.5f;
        [SerializeField] private float speedSpiderJump = 4f;

        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject spider;
        [SerializeField] private GameObject fish;
        [SerializeField] private GameObject electric;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopSpiderJump;
        [SerializeField] private GameObject flagStopSpiderWalk;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * speedBoyRun, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M26.FALL, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * speedBoyRun, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            Util.SetAni(boy, Const.Boy2.M20.JUMP_SPIDER);

            await Util.Delay(0.5f);
            ShowSpider();

            await Util.Delay(0.5f);
            ShowItem();
            Move(new GameObjectMoved(spider, flagStopSpiderWalk, Time.deltaTime * 2, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowItem();
            ShowFish();
            Rigidbody2D fishRigidbody2D = fish.GetComponent<Rigidbody2D>();
            fishRigidbody2D.AddForce(transform.up * 150);
            fishRigidbody2D.AddForce(transform.right * -100);

            await Util.Delay(1);
            AudioController.Instance.Play(Const.Common.AUDIOS.ELECTRIC);
            electric.SetActive(true);
            fish.transform.DOShakePosition(10, 0.1f);

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            spider.SetActive(false);
            fish.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowSpider()
        {
            spider.SetActive(true);
            boy.SetActive(false);
            fish.SetActive(false);

            ShowSmoke(spider);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowFish()
        {
            fish.SetActive(true);
            boy.SetActive(false);
            spider.SetActive(false);

            ShowSmoke(fish);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
