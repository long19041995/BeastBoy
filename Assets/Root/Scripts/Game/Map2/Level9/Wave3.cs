using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level9
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject gorilla;
        [SerializeField] private GameObject monkey;
        [SerializeField] private GameObject cat;
        [SerializeField] private GameObject laser1;
        [SerializeField] private GameObject laser2;
        [SerializeField] private GameObject column1;
        [SerializeField] private GameObject column2;
        [SerializeField] private GameObject electric;
        [SerializeField] private Coop coop;
        [SerializeField] private GameObject messageBoy;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRunOut;

        private async void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;
                Util.SetAni(boy, Const.Boy2.M20.OPEN_DOOR, true);
                messageBoy.SetActive(true);
                Util.ShowMessage(boy, messageBoy, 0.3f, 1.2f);
                ShowOption();

                await Util.Delay(1);
                messageBoy.SetActive(false);
            }
        }

        public async override void OnPass()
        {
            ShowDino();

            await Util.Delay(1);
            Util.SetAni(dino, Const.Dino.STAMP, true);
            ShakeCamera();

            await Util.Delay(1);
            column1.GetComponent<Rigidbody2D>().AddForce(-transform.right * 100);
            column2.GetComponent<Rigidbody2D>().AddForce(transform.right * 100);

            await Util.Delay(1);
            Util.SetAni(dino, Const.Dino.IDLE, true);
            StopShakeCamera();
            ShowItem();
            coop.StopInvoke();

            await Util.Delay(1);

            Util.SetAni(monkey, Const.Monkey.TURN, true);
            Util.SetAni(cat, Const.Cat2.RUN, true);
            Move(new GameObjectMoved(cat, flagStopBoyRunOut, Time.deltaTime * 2, () => { }));

            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            AudioController.Instance.Play(Const.Common.AUDIOS.GORILLA);
            ShowGorilla();

            await Util.Delay(1);
            Util.SetAni(gorilla, Const.Gorilla.HIT);

            await Util.Delay(1);
            ShowItem();
            Util.SetAni(gorilla, Const.Gorilla.ELECTRIC, true);
            AudioController.Instance.Play(Const.Common.AUDIOS.ELECTRIC);
            electric.SetActive(true);

            await Util.Delay(2);
            AudioController.Instance.Stop(Const.Common.AUDIOS.GORILLA);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            dino.SetActive(false);
            gorilla.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            gorilla.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowGorilla()
        {
            gorilla.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(gorilla);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
