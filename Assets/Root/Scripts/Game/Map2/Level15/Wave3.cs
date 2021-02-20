using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level15
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject skunk;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject dragon;
        [SerializeField] private GameObject smokeSkunk;
        [SerializeField] private GameObject fireDragon;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopBoyRunOut;
        [SerializeField] private GameObject flagStopDragonRunOut;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    dragon.GetComponent<AudioSource>().Play();
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowSkunk();

            await Util.Delay(1);
            ShowItem();
            Util.SetTurnBack(skunk, 0);
            Util.SetAni(skunk, Const.Skunk.DEFLATE);

            await Util.Delay(0.5f);
            smokeSkunk.SetActive(true);

            await Util.Delay(0.5f);
            Util.SetTurnBack(dragon);
            Util.SetAni(dragon, Const.Dragon.WALK, true);
            Move(new GameObjectMoved(dragon, flagStopDragonRunOut, Time.deltaTime * 2, () =>
            {
                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, () =>
                {
                    ShowResult();
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowDino();

            await Util.Delay(1);
            Util.SetAni(dragon, Const.Dragon.FIRE);

            await Util.Delay(0.5f);
            ShowItem();
            fireDragon.SetActive(true);
            Util.SetAni(dino, Const.Dino.DIE);

            await Util.Delay(1);
            fireDragon.SetActive(false);

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            skunk.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowSkunk()
        {
            skunk.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(skunk);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            skunk.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
