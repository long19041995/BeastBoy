using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level12
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject elephant;
        [SerializeField] private GameObject mantis;

        [SerializeField] private GameObject flagTrap;
        [SerializeField] private GameObject flagStopBoyRunOut;
        [SerializeField] private GameObject flagStopCameraMoveWithBoyRunOut;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagTrap, Time.deltaTime * 2, async () =>
                {
                    Util.SetAni(boy, Const.Boy2.M24.BE_TRAPPED_1);

                    await Util.Delay(0.35f);
                    Util.SetAni(boy, Const.Boy2.M24.BE_TRAPPED_2, true);

                    await Util.Delay(1.5f);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowMantis();

            await Util.Delay(0.5f);
            ShowItem();
            Util.SetAni(mantis, Const.Mantis.CUT_ROPE);

            await Util.Delay(1.5f);
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN_SMILE, true, 0);
            NextWave();
            Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, async () =>
            {
                Util.SetAni(boy, Const.Boy2.M25.IDLE, true, 0);
                await Util.Delay(1);
                ShowOption();
            }));
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithBoyRunOut, Time.deltaTime * 3, () => { }));
        }

        public async override void OnFail()
        {
            ShowElephant();
            ShowItem();

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            elephant.SetActive(false);
            mantis.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowElephant()
        {
            elephant.SetActive(true);
            boy.SetActive(false);
            mantis.SetActive(false);

            ShowSmoke(elephant, 1, 1);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMantis()
        {
            mantis.SetActive(true);
            boy.SetActive(false);
            elephant.SetActive(false);

            ShowSmoke(mantis, 1, 1);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
