using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Map2.Level17
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject elephant;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject airplane;
        [SerializeField] private GameObject electric;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagAirplanePosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopAirplaneOut;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;
                airplane.transform.position = flagAirplanePosition.transform.position;

                Util.SetAni(airplane, Const.Airplane.ANIM1, true);
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(airplane, Const.Airplane.ANIM2, true);
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                }));
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            //dino.transform.position = boy.transform.position;
            ShowDino();
            Util.SetAni(dino, Const.Dino.IDLE, true);

            await Util.Delay(0.5f);
            Util.SetAni(dino, Const.Dino.LASH, true);

            await Util.Delay(0.5f);
            ShowItem();
            Util.SetRotate(airplane, 30);
            Move(new GameObjectMoved(airplane, flagStopAirplaneOut, Time.deltaTime * 4, () =>
            {
                Util.SetRotate(airplane, -30);
                Util.SetAni(airplane, Const.Airplane.ANIM1, true);
            }));

            await Util.Delay(2);
            NextWave();
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);

            Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () =>
            {
                ChangeAmount(0.01f);
                ShowOption();
            }));
        }

        public async override void OnFail()
        {
            ShowElephant();

            await Util.Delay(1);
            ShowItem();
            Util.SetAni(elephant, Const.Elephant.DIE, true);
            electric.SetActive(true);

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            elephant.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowElephant()
        {
            elephant.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(elephant);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            elephant.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
