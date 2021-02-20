using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level1
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private float speedMoveBoyShadow = 1f;
        [SerializeField] private float speedBoyRun = 2f;
        [SerializeField] private float speedMosquito = 2f;
        [SerializeField] private float speedBoyJump = 2f;

        [SerializeField] private GameObject boyShadow;
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject dark;
        [SerializeField] private List<GameObject> cameras;
        [SerializeField] private GameObject demo;
        [SerializeField] private List<GameObject> animals;
        [SerializeField] private GameObject doctor1;
        [SerializeField] private GameObject doctor2;
        [SerializeField] private GameObject laser;
        [SerializeField] private GameObject mosquito;
        [SerializeField] private GameObject mouse;
        [SerializeField] private GameObject electricDie;
        [SerializeField] private GameObject messageDoctor1;
        [SerializeField] private GameObject messageDoctor2;
        [SerializeField] private GameObject messageBoy;
        [SerializeField] private GameObject electricMouse;
        [SerializeField] private GameObject electric1;
        [SerializeField] private GameObject electric2;
        [SerializeField] private GameObject smokeFire;
        [SerializeField] private GameObject smokeMosquito;
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject machine1;
        [SerializeField] private GameObject machine2;
        [SerializeField] private GameObject dust;
        [SerializeField] private GameObject earthquake;

        [SerializeField] private GameObject flagStopBoyWalk;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagBoyShadowPosition;
        [SerializeField] private GameObject flagStopBoyShadowMove;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopBoyWalk2;
        [SerializeField] private GameObject flagStopFly;
        [SerializeField] private GameObject flagStopBoyRun2;
        [SerializeField] private GameObject flagStopBoyJump;

        private int indexAnimal = 0;
        private bool isMoveBoyShadow = false;
        private bool isBoyWalk = false;
        private bool isBoyRun = false;
        private bool isBoyWalkNextScene = false;
        private bool isMosquitoFly = false;
        private bool isBoyRun2 = false;
        private bool isBoyJump = false;

        private void Start()
        {
            if (Gamemanager.Instance.IsSkipBeforeWave1)
            {
                boy.transform.position = flagStopBoyWalk.transform.position;
                ChangeToCamera(2);
                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                isBoyRun = true;
            }
            else
            {
                ChangeToCamera(0);
                boyShadow.transform.position = flagBoyShadowPosition.transform.position;
                boyShadow.SetActive(true);
                isMoveBoyShadow = true;
                HideItem();

                InvokeRepeating("ChangeAnimal", 0, 1);
            }
        }

        private void ChangeAnimal()
        {
            animals.ForEach((animal) =>
            {
                animal.SetActive(false);
            });

            indexAnimal++;
            if (indexAnimal >= animals.Count)
            {
                indexAnimal = 0;
            }

            animals[indexAnimal].SetActive(true);
        }

        public async override void OnPass()
        {
            ShowMouse();

            await Util.Delay(1);
            Util.SetAni(mouse, Const.Mouse.ATTACK, true);

            await Util.Delay(2);
            Util.SetAni(mouse, Const.Mouse.IDLE, true);
            electricMouse.SetActive(true);
            electricDie.SetActive(true);
            laser.SetActive(false);
            ShowItem();

            await Util.Delay(1);
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            isBoyRun2 = true;
        }

        public async override void OnFail()
        {
            AudioController.Instance.Play(Const.Common.AUDIOS.FLY);
            ShowMosquito();

            await Util.Delay(1);
            isMosquitoFly = true;
        }

        public void ShowBoy()
        {
            boy.SetActive(true);
            mosquito.SetActive(false);
            mouse.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        public void ShowMosquito()
        {
            mosquito.SetActive(true);
            boy.SetActive(false);
            mouse.SetActive(false);

            ShowSmoke(mosquito, 0);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        public void ShowMouse()
        {
            mouse.SetActive(true);
            boy.SetActive(false);
            mosquito.SetActive(false);

            ShowSmoke(mouse);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void Update()
        {
            if (isMoveBoyShadow)
            {
                Util.MoveGameObject(boyShadow, flagStopBoyShadowMove, speedMoveBoyShadow * Time.deltaTime, async () =>
                {
                    isMoveBoyShadow = false;
                    await Util.Delay(1);

                    ShowDark();

                    await Util.Delay(1);
                    HideDark();
                    SetNextScene();
                    SetAniNextScene();
                });
            }

            if (isBoyWalk)
            {
                Util.MoveGameObject(boy, flagStopBoyWalk, speedBoyRun * Time.deltaTime, () =>
                {
                    isBoyWalk = false;
                    SetNextScene();
                    SetAniNextScene2();
                    HideDark();
                });
            }

            if (isBoyWalkNextScene)
            {
                Util.MoveGameObject(boy, flagStopBoyWalk2, Time.deltaTime, async () => {
                    isBoyWalkNextScene = false;
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    messageBoy.SetActive(true);
                    Util.ShowMessage(boy, messageBoy, 0.3f, 1.2f);

                    await Util.Delay(2);
                    messageBoy.SetActive(false);
                    Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                    isBoyRun = true;

                    await Util.Delay(0.5f);
                    ShowDark();

                    await Util.Delay(1.5f);
                    boy.transform.position = flagStopBoyWalk.transform.position;
                    dust.SetActive(false);
                    ChangeToCamera(2);
                    HideDark();
                });
            }

            if (isBoyRun)
            {
                Util.MoveGameObject(boy, flagStopBoyRun, speedBoyRun * Time.deltaTime, async () =>
                {
                    isBoyRun = false;
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID2, true);
                    earthquake.GetComponent<AudioSource>().Stop();
                    StopShakeCamera();
                    laser.SetActive(true);

                    await Util.Delay(0.5f);
                    ShowOption();
                    ShowProgressBar();
                });
            }

            if (isMosquitoFly)
            {
                Util.MoveGameObject(mosquito, flagStopFly, speedMosquito * Time.deltaTime, async () =>
                {
                    AudioController.Instance.Stop(Const.Common.AUDIOS.FLY);
                    isMosquitoFly = false;
                    Util.SetAni(mosquito, Const.Mosquito.DIE, true);
                    mosquito.GetComponent<Rigidbody2D>().gravityScale = 1;
                    smokeMosquito.SetActive(true);
                    ShowItem();

                    await Util.Delay(1);
                    ShowResult();
                });
            }

            if (isBoyRun2)
            {
                Util.MoveGameObject(boy, flagStopBoyRun2, speedBoyRun * Time.deltaTime, () =>
                {
                    isBoyRun2 = false;
                    ShowResult();
                });
            }

            if (isBoyJump)
            {
                Util.MoveGameObject(boy, flagStopBoyJump, Time.deltaTime * speedBoyJump, () => isBoyJump = false);
            }
        }

        private void SetNextScene()
        {
            ChangeToCamera(1);
            demo.SetActive(true);
        }

        private async void SetAniNextScene()
        {
            await Util.Delay(0.5f);
            messageDoctor2.SetActive(true);
            Util.ShowMessage(doctor2, messageDoctor2, -1.2f, 1.5f);

            await Util.Delay(2);
            earthquake.GetComponent<AudioSource>().Play();
            dust.SetActive(true);
            ShakeCamera();

            await Util.Delay(0.5f);
            messageDoctor1.SetActive(true);
            messageDoctor2.SetActive(false);
            Util.ShowMessage(doctor1, messageDoctor1, 0.5f, 2);
            Util.SetAni(doctor1, Const.Doctor.WORRY1);
            Util.SetAni(doctor2, Const.Doctor.WORRY1);

            await Util.Delay(1);
            messageDoctor1.SetActive(false);
            Util.SetAni(doctor1, Const.Doctor.WORRY2, true);
            Util.SetAni(doctor2, Const.Doctor.WORRY2, true);

            await Util.Delay(2);
            ShowDark();

            await Util.Delay(1.5f);
            HideDark();
            SetPreviewScene();

            await Util.Delay(1);
            SetAniPreviewScene();
        }

        private void SetAniNextScene2()
        {
            boy.transform.position = flagBoyPosition.transform.position;
            isBoyWalkNextScene = true;
        }

        private void SetPreviewScene()
        {
            ChangeToCamera(0);
            demo.SetActive(false);
        }

        private async void SetAniPreviewScene()
        {
            await Util.Delay(1);
            electric1.SetActive(true);

            await Util.Delay(0.5f);
            electric2.SetActive(true);

            await Util.Delay(1);
            boyShadow.GetComponent<Flickering>().StartFlicker();

            await Util.Delay(1);
            explosion.SetActive(true);

            await Util.Delay(1);
            boyShadow.GetComponent<Flickering>().StopFlicker();
            boyShadow.SetActive(false);
            smokeFire.SetActive(true);
            boy.SetActive(true);
            Util.SetSpeedAni(boy, 0.7f);

            await Util.Delay(0.2f);
            machine1.SetActive(false);
            machine2.SetActive(true);
            isBoyJump = true;

            await Util.Delay(1f);
            Util.SetSpeedAni(boy, 1);
            Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);

            await Util.Delay(2);
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            isBoyWalk = true;
        }

        private async void SetMoveBoy()
        {
            boy.transform.position = flagBoyPosition.transform.position;
            boy.SetActive(true);
            isBoyWalk = true;

            await Util.Delay(1);
            ShowDark();
        }

        private async void ShowDark()
        {
            dark.SetActive(true);
            await Util.Delay(0.1f);
            dark.GetComponent<Dark>().FadeIn(1);
            dark.GetComponent<Dark>().SetFadeTime(0.02f);
        }

        private void HideDark()
        {
            dark.GetComponent<Dark>().FadeOut();
        }

        private void ChangeToCamera(int index)
        {
            Camera.main.transform.position = cameras[index].transform.position;
        }

        private void FlickLaser()
        {
            laser.GetComponent<Flickering>().StartFlicker();
        }

        private void StopFlickLaser()
        {
            laser.GetComponent<Flickering>().StopFlicker();
        }
    }
}
