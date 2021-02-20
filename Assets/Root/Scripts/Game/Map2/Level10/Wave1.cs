using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level10
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject cow;
        [SerializeField] private GameObject gecko;
        [SerializeField] private GameObject doctor1;
        [SerializeField] private GameObject team2;
        [SerializeField] private GameObject robot1;
        [SerializeField] private GameObject robot2;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopDoctorRun;
        [SerializeField] private GameObject flagCameraPositionNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagStopTeam2Move;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    ShowOption();
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                }));
            }
        }

        public async override void OnPass()
        {
            ShowCow();

            await Util.Delay(1);
            Util.SetAni(cow, Const.Cow.BUTT);
            ShowItem();

            await Util.Delay(0.3f);
            Util.SetAni(doctor1, Const.Doctor.FLY);

            await Util.Delay(1);
            doctor1.SetActive(false);

            Camera.main.transform.position = flagCameraPositionNextWave.transform.position;
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            ShowBoy();

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime, () =>
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                ShowOption();
            }));

            AudioController.Instance.Play(Const.Common.AUDIOS.ROBOT, true);
            Move(new GameObjectMoved(team2, flagStopTeam2Move, Time.deltaTime, () =>
            {
                Util.SetAni(robot1, Const.Robot.IDLE, true);
                Util.SetAni(robot2, Const.Robot.IDLE, true);
            }));

            NextWave();
        }

        public async override void OnFail()
        {
            ShowGecko();

            await Util.Delay(1);
            Util.SetAni(doctor1, Const.Doctor.CATCH, true);

            await Util.Delay(1);
            ShowItem();
            Util.SetAni(gecko, Const.Gecko.IDLE, true);

            await Util.Delay(1);
            Util.SetAni(doctor1, Const.Doctor.RUN_MACHINE, true);
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
            Move(new GameObjectMoved(doctor1, flagStopDoctorRun, Time.deltaTime, () =>
            {
                Util.SetAni(doctor1, Const.Doctor.SHOUT, true);
            }));

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            cow.SetActive(false);
            gecko.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowCow()
        {
            cow.SetActive(true);
            boy.SetActive(false);
            gecko.SetActive(false);

            ShowSmoke(cow);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowGecko()
        {
            gecko.SetActive(true);
            boy.SetActive(false);
            cow.SetActive(false);

            ShowSmoke(gecko);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
