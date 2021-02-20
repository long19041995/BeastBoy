using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level17
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject airplane;
        [SerializeField] private GameObject pangolin;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject rocket1;
        [SerializeField] private GameObject rocket2;
        [SerializeField] private GameObject explosion1;
        [SerializeField] private GameObject explosion2;
        [SerializeField] private GameObject explosion3;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagAirplanePosition;
        [SerializeField] private GameObject flagStopPangolinOut;
        [SerializeField] private GameObject flagStopDinoOut;
        [SerializeField] private GameObject flagStopRocket1Fly;
        [SerializeField] private GameObject flagStopRocket2Fly;
        [SerializeField] private GameObject flagStopRocket3Fly;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                airplane.transform.position = flagAirplanePosition.transform.position;
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                airplane.SetActive(true);
                Util.SetAni(airplane, Const.Airplane.ANIM1, true);
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, async () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);

                    await Util.Delay(1);
                    ShowOption();
                }));
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                    ChangeAmount(0.01f);
                }));
            }
        }

        public async override void OnPass()
        {
            pangolin.transform.position = boy.transform.position;
            ShowPangolin();

            Move(new GameObjectMoved(rocket1, flagStopRocket1Fly, Time.deltaTime * 4, () => { explosion1.SetActive(true); rocket1.SetActive(false); }));

            await Util.Delay(0.5f);
            Move(new GameObjectMoved(rocket2, flagStopRocket2Fly, Time.deltaTime * 4, () => { explosion2.SetActive(true); rocket2.SetActive(false); }));

            ShowItem();
            Util.SetAni(pangolin, Const.Pangolin.ATTACK, true);
            Move(new GameObjectMoved(pangolin, flagStopPangolinOut, Time.deltaTime * 4, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            dino.transform.position = boy.transform.position;
            ShowDino();
            Util.SetAni(dino, Const.Dino.IDLE, true);

            await Util.Delay(1);
            Move(new GameObjectMoved(rocket1, flagStopRocket3Fly, Time.deltaTime * 4, async () =>
            {
                explosion3.SetActive(true);
                rocket1.SetActive(false);
                Util.SetAni(dino, Const.Dino.DIE);

                ShowItem();

                await Util.Delay(2);
                ShowResult();
            }));
            //Util.SetAni(dino, Const.Dino.WALK, true);
            //Move(new GameObjectMoved(dino, flagStopDinoOut, Time.deltaTime * 2, async () =>
            //{
            //    ShowItem();
            //    Util.SetAni(dino, Const.Dino.DIE);

            //    await Util.Delay(2);
            //    ShowResult();
            //}));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            pangolin.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowPangolin()
        {
            pangolin.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(pangolin);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            pangolin.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
