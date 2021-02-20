using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level16
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject mole;
        [SerializeField] private GameObject cheetah;
        [SerializeField] private GameObject stone1;
        [SerializeField] private GameObject stone2;
        [SerializeField] private GameObject stone3;
        [SerializeField] private GameObject stone4;
        [SerializeField] private GameObject pit;
        [SerializeField] private GameObject pit2;
        [SerializeField] private GameObject earthMole;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopMoleOut;
        [SerializeField] private GameObject flagStopMoleIn;
        [SerializeField] private GameObject flagStopMoleOut2;
        [SerializeField] private GameObject flagStopMoleIn2;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    stone1.GetComponent<Rigidbody2D>().gravityScale = 1;
                    stone2.GetComponent<Rigidbody2D>().gravityScale = 1;
                    stone3.GetComponent<Rigidbody2D>().gravityScale = 1;
                    ShowOption();
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }
        }

        public override void OnPass()
        {
            ShowMole();
            pit.SetActive(true);
            stone4.GetComponent<Rigidbody2D>().gravityScale = 1;

            Move(new GameObjectMoved(mole, flagStopMoleOut, Time.deltaTime, () =>
            {
                ShowItem();
                mole.transform.position = flagStopMoleOut2.transform.position;
                pit2.SetActive(true);
                earthMole.SetActive(false);
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () => { }));
                Move(new GameObjectMoved(mole, flagStopMoleIn2, Time.deltaTime, () =>
                {
                    NextWave();
                    pit.SetActive(false);
                    pit2.SetActive(false);
                    boy.transform.position = mole.transform.position;
                    ShowBoy();
                    Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                    Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
                    {
                        AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                        AudioController.Instance.Stop(Const.Common.AUDIOS.MOUNTAIN);
                        Util.SetAni(boy, Const.Boy2.M26.FALL, true);
                        ShowOption();
                    }));
                }));
            }));
        }

        public async override void OnFail()
        {
            cheetah.transform.position = boy.transform.position;
            ShowCheetah();

            stone4.GetComponent<Rigidbody2D>().gravityScale = 1;
            Util.SetAni(cheetah, Const.Cheetah.JUMP);
            await Util.Delay(1);
            ShowItem();
            Util.SetAni(cheetah, Const.Cheetah.DIE, true);

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            mole.SetActive(false);
            cheetah.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMole()
        {
            mole.SetActive(true);
            boy.SetActive(false);
            cheetah.SetActive(false);

            ShowSmoke(mole);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowCheetah()
        {
            cheetah.SetActive(true);
            boy.SetActive(false);
            mole.SetActive(false);

            ShowSmoke(cheetah);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
