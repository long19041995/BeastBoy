using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Map2.Level16
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject hippo;
        [SerializeField] private GameObject cow;
        [SerializeField] private GameObject stone;
        [SerializeField] private GameObject stoneBreak;
        [SerializeField] private GameObject collider1;
        [SerializeField] private GameObject stone1;
        [SerializeField] private GameObject stone2;
        [SerializeField] private GameObject stone3;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                AudioController.Instance.Play(Const.Common.AUDIOS.MOUNTAIN);
                stone.SetActive(true);
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                ChangeAmount(0.02f);
                ShakeCamera();
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, async () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);

                    await Util.Delay(1);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            collider1.SetActive(false);
            hippo.transform.position = boy.transform.position;
            ShowHippo();

            await Util.Delay(1);
            ShowItem();
            Util.SetAni(hippo, Const.Hippo.ANIMATION);

            await Util.Delay(0.3f);
            stoneBreak.SetActive(true);
            stone.SetActive(false);

            await Util.Delay(1);
            NextWave();
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () => { }));
            StopShakeCamera();
            Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                stone1.GetComponent<Rigidbody2D>().gravityScale = 1;
                stone2.GetComponent<Rigidbody2D>().gravityScale = 1;
                stone3.GetComponent<Rigidbody2D>().gravityScale = 1;
                ShowOption();
            }));
        }

        public async override void OnFail()
        {
            cow.transform.position = boy.transform.position;
            ShowCow();
            Util.SetAni(cow, Const.Cow.BUTT2);

            await Util.Delay(0.5f);
            ShowItem();
            Util.SetAni(cow, Const.Cow.CRAZY, true);

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            hippo.SetActive(false);
            cow.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowHippo()
        {
            hippo.SetActive(true);
            boy.SetActive(false);
            cow.SetActive(false);

            ShowSmoke(hippo);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowCow()
        {
            cow.SetActive(true);
            boy.SetActive(false);
            hippo.SetActive(false);

            ShowSmoke(cow);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
