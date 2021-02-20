using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Map2.Level9
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject mantis;
        [SerializeField] private GameObject cheetah;
        [SerializeField] private GameObject security;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject security3;
        [SerializeField] private GameObject electric;
        [SerializeField] private GameObject messageBoy;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopSecurityRun;
        [SerializeField] private GameObject flagStopMantisFly;
        [SerializeField] private GameObject flagStopMantisHit;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;
                security.SetActive(true);

                AudioController.Instance.Play(Const.Common.AUDIOS.BREATHING, true, 0.2f);
                Move(new GameObjectMoved(security, flagStopSecurityRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(security1, Const.Security.IDLE, true);
                    Util.SetAni(security2, Const.Security.IDLE, true);
                    Util.SetAni(security3, Const.Security.IDLE, true);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowMantis();

            Util.SetAni(mantis, Const.Mantis.IDLE2);

            await Util.Delay(1f);
            Util.SetAni(mantis, Const.Mantis.HIT1);
            AudioController.Instance.Play(Const.Common.AUDIOS.KARATE);

            Move(new GameObjectMoved(mantis, flagStopMantisFly, Time.deltaTime * 20, async () =>
            {
                Util.SetAni(mantis, Const.Mantis.HIT2);
                AudioController.Instance.Play(Const.Common.AUDIOS.KARATE);
                Util.SetAni(security1, Const.Security.DIE);
                Util.SetAni(security2, Const.Security.AFRAID, true);
                Util.SetTurnBack(security2, 0);

                await Util.Delay(0.5f);
                Util.SetAni(mantis, Const.Mantis.HIT3);
                AudioController.Instance.Play(Const.Common.AUDIOS.KARATE);
                Util.SetAni(security3, Const.Security.AFRAID, true);
                Move(new GameObjectMoved(mantis, flagStopMantisHit, Time.deltaTime * 20, async () =>
                {
                    await Util.Delay(0.4f);
                    Util.SetAni(mantis, Const.Mantis.HIT4);
                    AudioController.Instance.Play(Const.Common.AUDIOS.KARATE);
                    await Util.Delay(0.1f);
                    Util.SetAni(security3, Const.Security.DIE);

                    await Util.Delay(0.4f);
                    Util.SetAni(mantis, Const.Mantis.HIT5);
                    AudioController.Instance.Play(Const.Common.AUDIOS.KARATE);

                    await Util.Delay(0.3f);
                    Move(new GameObjectMoved(mantis, flagStopMantisFly, Time.deltaTime * 20, async () =>
                    {
                        Util.SetAni(security2, Const.Security.DIE);

                        await Util.Delay(0.1f);
                        Util.SetAni(mantis, Const.Mantis.IDLE2);
                        ShowItem();

                        await Util.Delay(0.1f);
                        Move(new GameObjectMoved(mantis, flagStopMantisHit, Time.deltaTime * 20, async () =>
                        {
                            await Util.Delay(2);
                            NextWave();
                            boy.transform.position = mantis.transform.position;
                            ShowBoy();
                            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                            Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
                            {
                                Util.SetAni(boy, Const.Boy2.M20.OPEN_DOOR, true);
                                security.SetActive(false);
                            }));
                            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, async () =>
                            {
                                messageBoy.SetActive(true);
                                Util.ShowMessage(boy, messageBoy, 0.3f, 1.2f);
                                ShowOption();

                                await Util.Delay(1);
                                messageBoy.SetActive(false);
                            }));
                        }));
                    }));
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowCheetah();

            await Util.Delay(0.5f);
            Util.SetAni(security1, Const.Security.ELECTRIC2);
            AudioController.Instance.Play(Const.Common.AUDIOS.ELECTRIC);

            await Util.Delay(0.5f);
            ShowItem();
            Util.SetAni(cheetah, Const.Cheetah.DIE, true);
            electric.SetActive(true);
            cheetah.transform.DOShakePosition(10, 0.01f);

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            mantis.SetActive(false);
            cheetah.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMantis()
        {
            mantis.SetActive(true);
            boy.SetActive(false);
            cheetah.SetActive(false);

            ShowSmoke(mantis);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowCheetah()
        {
            cheetah.SetActive(true);
            boy.SetActive(false);
            mantis.SetActive(false);

            ShowSmoke(cheetah);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
