using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level14
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject elephant;
        [SerializeField] private GameObject cheetah;
        [SerializeField] private GameObject woodBridge;
        [SerializeField] private GameObject mask;

        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMoveWithBoyRun;
        [SerializeField] private GameObject flagStopElephantJump;
        [SerializeField] private GameObject flagStopCameraMoveWithElephantJump;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPositionNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M210.IDLE, true);
                    Util.SetAni(woodBridge, Const.WoodBridge.BREAK, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithBoyRun, Time.deltaTime, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            elephant.transform.position = boy.transform.position;
            ShowElephant();

            woodBridge.GetComponent<Rigidbody2D>().gravityScale = 1;
            Util.SetAni(elephant, Const.Elephant.FALL);

            await Util.Delay(0.5f);
            Util.SetAni(elephant, Const.Elephant.CRAWL);
            ShowItem();

            await Util.Delay(0.3f);
            mask.SetActive(false);

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithElephantJump, Time.deltaTime * 2, () =>
            {
                NextWave();
                ShowOption();
            }));

            Move(new GameObjectMoved(elephant, flagStopElephantJump, Time.deltaTime * 2, () =>
            {
                HideOption();
            }));

            await Util.Delay(0.2f);
            Util.SetAniDefault(woodBridge);

            await Util.Delay(1.8f);
            boy.transform.position = flagBoyPositionNextWave.transform.position;
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
            {
                Util.SetAni(boy, Const.Boy2.M211.IDLE, true);
            }));
        }

        public async override void OnFail()
        {
            ShowCheetah();

            await Util.Delay(0.5f);
            cheetah.GetComponent<Rigidbody2D>().gravityScale = 1;
            woodBridge.GetComponent<Rigidbody2D>().gravityScale = 1;

            await Util.Delay(0.5f);
            Util.SetAniDefault(woodBridge);

            await Util.Delay(0.2f);
            ShowItem();
            Util.SetAni(cheetah, Const.Cheetah.FALL);

            await Util.Delay(1);
            Util.SetAni(cheetah, Const.Cheetah.DIE, true);

            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            elephant.SetActive(false);
            cheetah.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowElephant()
        {
            elephant.SetActive(true);
            boy.SetActive(false);
            cheetah.SetActive(false);

            ShowSmoke(elephant, 2);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowCheetah()
        {
            cheetah.SetActive(true);
            boy.SetActive(false);
            elephant.SetActive(false);

            ShowSmoke(cheetah);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
