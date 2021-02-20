using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level20
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject dragon;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject security;
        [SerializeField] private GameObject doctor;
        [SerializeField] private GameObject net;
        [SerializeField] private GameObject fire;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagSecurityPosition;
        [SerializeField] private GameObject flagNetPosition;
        [SerializeField] private GameObject flagStopNetMove;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                Util.SetAni(security, Const.Security.IDLE, true);
                Util.SetAni(doctor, Const.Doctor.IDLE_MACHINE, true);
                security.transform.position = flagSecurityPosition.transform.position;
                net.transform.position = flagNetPosition.transform.position;
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
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
            ShowDragon();

            await Util.Delay(1);
            Util.SetAni(dragon, Const.Dragon.FIRE2);

            await Util.Delay(0.5f);
            ShowItem();
            fire.SetActive(true);
            Util.SetAni(doctor, Const.Doctor.DIE_FIRE);
            Util.SetAni(security, Const.Security.DIE);

            await Util.Delay(1);
            fire.SetActive(false);
            ShowResult();
        }

        public async override void OnFail()
        {
            ShowDino();

            await Util.Delay(1);
            Util.SetAni(security, Const.Security.NET);

            await Util.Delay(0.3f);
            net.SetActive(true);
            Util.SetRotate(net, -60);
            net.transform.localScale = new Vector3(1, 0.7f, 0.7f);
            Move(new GameObjectMoved(net, flagStopNetMove, Time.deltaTime * 6, async () =>
            {
                ShowItem();
                await Util.Delay(1);
                ShowResult();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            dragon.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDragon()
        {
            dragon.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(dragon);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            dragon.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
