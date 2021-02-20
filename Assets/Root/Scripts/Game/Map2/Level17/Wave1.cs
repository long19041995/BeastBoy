using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level17
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject lion;
        [SerializeField] private GameObject gecko;
        [SerializeField] private GameObject airplane;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject eagle;
        [SerializeField] private GameObject doctor;
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject tiger;
        [SerializeField] private GameObject hippo;
        [SerializeField] private GameObject electric;

        [SerializeField] private GameObject flagStopDoorMove;
        [SerializeField] private GameObject flagAirplanePosition2;
        [SerializeField] private GameObject flagStopAirplaneMove2;
        [SerializeField] private GameObject flagCameraPosition2;
        [SerializeField] private GameObject flagStopCameraMove2;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopEagleFly;
        [SerializeField] private GameObject flagStopDinoRun;
        [SerializeField] private GameObject flagStopSecurity1Fly;
        [SerializeField] private GameObject flagStopSecurity2Fly;
        [SerializeField] private GameObject flagStopDoctorFly;
        [SerializeField] private GameObject flagStopDoctorRunOut;
        [SerializeField] private GameObject flagStopAirplaneFlyOut;
        [SerializeField] private GameObject flagAirplanePosition;
        [SerializeField] private GameObject flagStopAirplaneFlyIn;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagAirplanePositionNextWave;
        [SerializeField] private GameObject flagStopHippoRun;
        [SerializeField] private GameObject flagStopTigerRun;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                BeforeStart();
            }
        }

        private void BeforeStart()
        {
            HideProgressBar();
            Camera.main.transform.position = flagCameraPosition2.transform.position;
            airplane.transform.position = flagAirplanePosition2.transform.position;
            Util.SetAni(airplane, Const.Airplane.ANIM1, true);
            Util.SetTurnBack(airplane);

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove2, Time.deltaTime * 2, () =>
            {
                Move(new GameObjectMoved(door, flagStopDoorMove, Time.deltaTime * 4, () =>
                {
                    Move(new GameObjectMoved(airplane, flagStopAirplaneMove2, Time.deltaTime * 6, () =>
                    {
                        EnterStart();
                    }));
                }));
            }));
        }

        private void EnterStart()
        {
            ShowProgressBar();
            airplane.transform.position = flagAirplanePosition.transform.position;
            boy.transform.position = flagBoyPosition.transform.position;
            Camera.main.transform.position = flagCameraPosition.transform.position;

            Util.SetAni(airplane, Const.Airplane.ANIM1, true);
            Util.SetTurnBack(airplane, 0);
            Move(new GameObjectMoved(eagle, flagStopBoyRun, Time.deltaTime * 2, () =>
            {
                boy.transform.position = eagle.transform.position;
                ShowBoy();
                eagle.SetActive(false);
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                ShowOption();
            }));

            Move(new GameObjectMoved(airplane, flagStopAirplaneFlyIn, Time.deltaTime * 2, async () =>
            {
                security1.SetActive(true);
                security2.SetActive(true);
                doctor.SetActive(true);

                Move(new GameObjectMoved(security1, flagStopSecurity1Fly, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(security1, Const.Security.IDLE, true);
                }));

                await Util.Delay(0.1f);
                Move(new GameObjectMoved(security2, flagStopSecurity2Fly, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(security2, Const.Security.IDLE, true);
                }));

                await Util.Delay(0.1f);
                Move(new GameObjectMoved(doctor, flagStopDoctorFly, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(doctor, Const.Doctor.IDLE_MACHINE, true);
                }));
            }));
        }

        public async override void OnPass()
        {
            ShowLion();

            Move(new GameObjectMoved(tiger, flagStopTigerRun, Time.deltaTime * 3, () => { tiger.SetActive(false); }));
            Move(new GameObjectMoved(hippo, flagStopHippoRun, Time.deltaTime * 2, () =>
            {
                hippo.SetActive(false);

                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    Util.SetAni(airplane, Const.Airplane.ANIM2, true);
                    ShowOption();
                }));
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () => { }));
            }));

            await Util.Delay(1);

            Util.SetAni(security1, Const.Security.AFRAID, true);
            Util.SetAni(security2, Const.Security.AFRAID, true);

            await Util.Delay(0.2f);
            ShowItem();
            Util.SetTurnBack(doctor);
            Util.SetAni(doctor, Const.Doctor.RUN_MACHINE, true);
            Move(new GameObjectMoved(doctor, flagStopDoctorRunOut, Time.deltaTime * 2, () => { doctor.SetActive(false); }));

            await Util.Delay(0.2f);
            Util.SetTurnBack(security1, 0);
            Util.SetAni(security1, Const.Security.RUN_AFRAID, true);
            Move(new GameObjectMoved(security1, flagStopDoctorRunOut, Time.deltaTime * 2, () => { security1.SetActive(false); }));

            await Util.Delay(0.2f);
            Util.SetTurnBack(security2, 0);
            Util.SetAni(security2, Const.Security.RUN_AFRAID, true);
            Move(new GameObjectMoved(security2, flagStopDoctorRunOut, Time.deltaTime * 2, () => { security2.SetActive(false); }));

            await Util.Delay(0.2f);
            Util.SetTurnBack(airplane);
            Move(new GameObjectMoved(airplane, flagAirplanePositionNextWave, Time.deltaTime * 8, () => { }));

            await Util.Delay(1);
            NextWave();

            await Util.Delay(1);
            Util.SetTurnBack(airplane, 0);
        }

        public async override void OnFail()
        {
            ShowGecko();

            await Util.Delay(0.5f);
            Util.SetAni(gecko, Const.Gecko.FADE_OUT);

            await Util.Delay(1);
            Util.SetAni(doctor, Const.Doctor.CATCH, true);
            Util.SetAni(gecko, Const.Gecko.FADE_OUT, true);

            await Util.Delay(2);
            ShowItem();
            Util.SetAni(gecko, Const.Gecko.IDLE, true);
            Util.SetAni(security1, Const.Security.ELECTRIC, true);

            await Util.Delay(0.3f);
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.ELECTRIC_DIE, true);
            electric.SetActive(true);

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            lion.SetActive(false);
            gecko.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowLion()
        {
            lion.SetActive(true);
            boy.SetActive(false);
            gecko.SetActive(false);

            ShowSmoke(lion);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowGecko()
        {
            gecko.SetActive(true);
            boy.SetActive(false);
            lion.SetActive(false);

            ShowSmoke(gecko);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
