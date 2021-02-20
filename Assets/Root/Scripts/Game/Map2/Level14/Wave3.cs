using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level14
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject rabit;
        [SerializeField] private GameObject elephant;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopBoyRunOut;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M214.IDLE, true, 0, 1);
                }));
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowItem();
            elephant.transform.position = boy.transform.position;
            ShowElephant();
            Util.SetAni(elephant, Const.Elephant.TREE, false, 0, 1);

            await Util.Delay(2);
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowItem();
            rabit.transform.position = boy.transform.position;
            Util.SetAni(rabit, Const.Rabit.TREE, true, 0, 1);
            ShowRabit();

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            rabit.SetActive(false);
            elephant.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowRabit()
        {
            rabit.SetActive(true);
            boy.SetActive(false);
            elephant.SetActive(false);

            ShowSmoke(rabit);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowElephant()
        {
            elephant.SetActive(true);
            boy.SetActive(false);
            rabit.SetActive(false);

            ShowSmoke(elephant);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
