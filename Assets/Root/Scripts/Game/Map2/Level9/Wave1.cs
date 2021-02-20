using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level9
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject whale;
        [SerializeField] private GameObject shark;
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject sharkMachine;
        [SerializeField] private GameObject sharkMachine1;
        [SerializeField] private GameObject sharkMachine2;
        [SerializeField] private GameObject sharkMachine3;
        [SerializeField] private GameObject security;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject security3;
        [SerializeField] private GameObject sharkDie;
        [SerializeField] private GameObject water;
        [SerializeField] private GameObject messageBoy;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyCrawl;
        [SerializeField] private GameObject flagStopBoyFall;
        [SerializeField] private GameObject flagStopSharkMachineMove;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagBoyPositionNextWave;
        [SerializeField] private GameObject flagStopSecurityRun;
        [SerializeField] private GameObject flagStopSharkMachineMoveOut;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                door.GetComponent<Rigidbody2D>().gravityScale = 1;
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.CREEP, true);
                Move(new GameObjectMoved(boy, flagStopBoyCrawl, Time.deltaTime, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M20.FALL, true);
                    Move(new GameObjectMoved(boy, flagStopBoyFall, Time.deltaTime * 3, () =>
                    {
                        messageBoy.SetActive(true);
                        Util.ShowMessage(boy, messageBoy, 0.3f, 1.2f);
                        ShowOption();
                    }));
                }));
            }
        }

        public async override void OnPass()
        {
            messageBoy.SetActive(false);
            ShowWhale();
            ShowItem();

            Util.SetTurnBack(sharkMachine3, 0);
            Util.SetRotate(sharkMachine1, 20);
            Util.SetRotate(sharkMachine2, 20);
            Util.SetRotate(sharkMachine3, 20);
            Move(new GameObjectMoved(sharkMachine, flagStopSharkMachineMoveOut, Time.deltaTime * 4, () => { }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
            {
                NextWave();
                boy.transform.position = flagBoyPositionNextWave.transform.position;
                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);

                security.SetActive(true);
                Move(new GameObjectMoved(security, flagStopSecurityRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(security1, Const.Security.IDLE, true);
                    Util.SetAni(security2, Const.Security.IDLE, true);
                    Util.SetAni(security3, Const.Security.IDLE, true);
                    ShowOption();
                }));
            }));

            water.SetActive(true);
            await Util.Delay(0.5f);
            Util.SetAni(whale, Const.Whale.IDLE2, true);
        }

        public async override void OnFail()
        {
            messageBoy.SetActive(false);
            ShowShark();
            ShowItem();

            Util.SetAni(sharkMachine1, Const.Shark.ATTACK, true);
            Util.SetAni(sharkMachine2, Const.Shark.ATTACK, true);
            Util.SetAni(sharkMachine3, Const.Shark.ATTACK, true);

            Move(new GameObjectMoved(sharkMachine1, flagStopSharkMachineMove, Time.deltaTime * 2, () => { }));
            Move(new GameObjectMoved(sharkMachine2, flagStopSharkMachineMove, Time.deltaTime * 2, () => { }));
            Move(new GameObjectMoved(sharkMachine3, flagStopSharkMachineMove, Time.deltaTime * 2, () =>
            {
                sharkMachine1.SetActive(false);
                sharkMachine2.SetActive(false);
                sharkMachine3.SetActive(false);
                shark.SetActive(false);
                sharkDie.SetActive(true);
            }));

            Util.SetAni(shark, Const.Shark.AFRAID, true);

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            whale.SetActive(false);
            shark.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowWhale()
        {
            whale.SetActive(true);
            boy.SetActive(false);
            shark.SetActive(false);

            ShowSmoke(whale);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowShark()
        {
            shark.SetActive(true);
            boy.SetActive(false);
            whale.SetActive(false);

            ShowSmoke(shark);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
