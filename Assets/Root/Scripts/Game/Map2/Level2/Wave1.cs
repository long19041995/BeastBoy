using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level2
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private float speedSecurity = 1f;
        [SerializeField] private float speedDoctor = 1f;
        [SerializeField] private float speedBoyRun = 2f;

        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject gateGreen;
        [SerializeField] private GameObject gateRed;
        [SerializeField] private List<GameObject> animals;
        [SerializeField] private GameObject doctor1;
        [SerializeField] private GameObject doctor2;
        [SerializeField] private List<GameObject> cameras;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject security3;
        [SerializeField] private GameObject rhino;
        [SerializeField] private GameObject geckoGreen;
        [SerializeField] private GameObject geckoBlue;
        [SerializeField] private GameObject electricRhino;
        [SerializeField] private GameObject electricBoy;
        [SerializeField] private GameObject messageBoy;
        [SerializeField] private GameObject messageDoctor1;
        [SerializeField] private GameObject smokeFire;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopDoctor1Run;
        [SerializeField] private GameObject flagStopDoctor1Run2;
        [SerializeField] private GameObject flagStopSecurity1Run;
        [SerializeField] private GameObject flagStopSecurity2Run;
        [SerializeField] private GameObject flagStopSecurity3Run;
        [SerializeField] private GameObject flagStopRunOut;
        [SerializeField] private GameObject flagStopCameraMoveWithBoy;

        private int indexAnimal = 0;
        private bool isBoyRun = false;
        private bool isGateGreen = false;
        private bool isDoctor1Run = false;
        private bool isSecurity1Run = false;
        private bool isSecurity2Run = false;
        private bool isSecurity3Run = false;
        private bool isSecurity1RunOut = false;
        private bool isSecurity2RunOut = false;
        private bool isSecurity3RunOut = false;
        private bool isBoyRunOut = false;
        private bool isCameraMoveWithBoy = false;

        private void Start()
        {
            boy.transform.position = flagBoyPosition.transform.position;
            ChangeToCamera(1);
            SetDoctorScene();
            HideItem();

            InvokeRepeating("ChangeAnimal", 0, 1);
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
            ShowGeckoGreen();

            await Util.Delay(1);
            geckoGreen.SetActive(false);
            geckoBlue.SetActive(true);
            Util.SetAni(geckoBlue, Const.Gecko.FADE_OUT);
            isSecurity1RunOut = true;

            await Util.Delay(0.5f);
            isSecurity2RunOut = true;

            await Util.Delay(0.5f);
            isSecurity3RunOut = true;

            await Util.Delay(2);
            gateRed.SetActive(false);
            gateGreen.SetActive(true);

            await Util.Delay(3);
            ShowItem();
            Vector3 position = boy.transform.position;
            position.x -= 1;
            boy.transform.position = position;

            ShowBoy();
            isBoyRunOut = true;
            Util.SetAni(boy, Const.Boy2.M20.RUN_SMILE, true);

            await Util.Delay(3);
            ShowResult();
        }

        public async override void OnFail()
        {
            ShowRhino();

            await Util.Delay(1);
            Util.SetAni(rhino, Const.Rhino.ATTACK, true);

            await Util.Delay(0.1f);
            rhino.GetComponent<AudioSource>().Play();
            ShakeCamera();

            await Util.Delay(1.5f);
            electricRhino.SetActive(true);
            ShowItem();
            AudioController.Instance.Play(Const.Common.AUDIOS.ELECTRIC);
            Util.SetAni(rhino, Const.Rhino.DIE, true);

            await Util.Delay(0.1f);
            smokeFire.SetActive(true);
            StopShakeCamera();

            await Util.Delay(1.5f);
            electricRhino.SetActive(false);
            ShowBoy();
            Util.SetTurnBack(boy, 0);
            Util.SetAni(boy, Const.Boy2.M20.ELECTRIC_DIE, true);
            electricBoy.SetActive(true);
            isDoctor1Run = true;
            isSecurity1Run = true;
            isSecurity2Run = true;
            isSecurity3Run = true;

            await Util.Delay(3);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            rhino.SetActive(false);
            geckoGreen.SetActive(false);
            geckoBlue.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowRhino()
        {
            rhino.SetActive(true);
            boy.SetActive(false);
            geckoGreen.SetActive(false);
            geckoBlue.SetActive(false);

            ShowSmoke(rhino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowGeckoGreen()
        {
            geckoGreen.SetActive(true);
            geckoBlue.SetActive(false);
            boy.SetActive(false);
            rhino.SetActive(false);

            ShowSmoke(geckoGreen);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void Update()
        {
            if (isBoyRun)
            {
                Util.MoveGameObject(boy, flagStopBoyRun, Time.deltaTime * speedBoyRun, () =>
                {
                    isBoyRun = false;
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID3, true);
                    ProgressBarController.Instance.SetActive(true);
                    ShowOption();
                });
            }

            if (isDoctor1Run)
            {
                if (doctor1.transform.position.x < flagStopDoctor1Run.transform.position.x)
                {
                    Util.MoveGameObject(doctor1, flagStopDoctor1Run, speedDoctor * Time.deltaTime, () =>
                    {
                        isDoctor1Run = false;
                    });
                }
                else
                {
                    Util.MoveGameObject(doctor1, flagStopDoctor1Run2, speedDoctor * Time.deltaTime, () =>
                    {
                        isDoctor1Run = false;
                        Util.SetAni(doctor1, Const.Doctor.IDLE, true);
                    });
                }
            }

            if (isSecurity1Run)
            {
                if (doctor1.transform.position.x < flagStopDoctor1Run.transform.position.x)
                {
                    Util.MoveGameObject(security1, flagStopDoctor1Run, speedSecurity * Time.deltaTime, () =>
                    {
                        isSecurity1Run = false;
                    });
                }
                else
                {
                    Util.MoveGameObject(security1, flagStopSecurity1Run, speedSecurity * Time.deltaTime, () =>
                    {
                        isSecurity1Run = false;
                        Util.SetAni(security1, Const.Security.IDLE, true);
                    });
                }
            }

            if (isSecurity2Run)
            {
                if (security2.transform.position.x < flagStopDoctor1Run.transform.position.x)
                {
                    Util.MoveGameObject(security2, flagStopDoctor1Run, speedSecurity * Time.deltaTime, () =>
                    {
                        isSecurity2Run = false;
                    });
                }
                else
                {
                    Util.MoveGameObject(security2, flagStopSecurity2Run, speedSecurity * Time.deltaTime, () =>
                    {
                        isSecurity2Run = false;
                        Util.SetAni(security2, Const.Security.IDLE, true);
                    });
                }
            }

            if (isSecurity3Run)
            {
                if (security3.transform.position.x < flagStopDoctor1Run.transform.position.x)
                {
                    Util.MoveGameObject(security3, flagStopDoctor1Run, speedSecurity * Time.deltaTime, () =>
                    {
                        isSecurity3Run = false;
                        SetBoyRun();
                    });
                }
                else
                {
                    Util.MoveGameObject(security3, flagStopSecurity3Run, speedSecurity * Time.deltaTime, () =>
                    {
                        isSecurity3Run = false;
                        Util.SetAni(security3, Const.Security.IDLE, true);
                    });
                }
            }

            if (isSecurity1RunOut)
            {
                Util.MoveGameObject(security1, flagStopRunOut, speedSecurity * Time.deltaTime, () => { });
            }

            if (isSecurity2RunOut)
            {
                Util.MoveGameObject(security2, flagStopRunOut, speedSecurity * Time.deltaTime, () => { });
            }

            if (isSecurity3RunOut)
            {
                Util.MoveGameObject(security3, flagStopRunOut, speedSecurity * Time.deltaTime, () => { });
            }

            if (isBoyRunOut)
            {
                Util.MoveGameObject(boy, flagStopRunOut, Time.deltaTime * speedBoyRun, () => { });
            }

            if (isCameraMoveWithBoy)
            {
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, flagStopCameraMoveWithBoy.transform.position, Time.deltaTime);
                if (Camera.main.transform.position == flagStopCameraMoveWithBoy.transform.position)
                {
                    isCameraMoveWithBoy = false;
                }
            }
        }

        private IEnumerator ChangeGate()
        {
            while (true)
            {
                if (isGateGreen)
                {
                    gateGreen.SetActive(true);
                    gateRed.SetActive(false);
                }
                else
                {
                    gateRed.SetActive(true);
                    gateGreen.SetActive(false);
                }

                yield return new WaitForSeconds(0.1f);
                isGateGreen = !isGateGreen;
            }
        }

        private async void SetBoyRun()
        {
            ChangeToCamera(0);
            await Util.Delay(1);
            messageBoy.SetActive(true);
            Util.ShowMessage(boy, messageBoy, -0.5f, 1.5f);

            await Util.Delay(1);
            Util.SetTurnBack(boy, 0);
            messageBoy.SetActive(false);
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            isBoyRun = true;
            isCameraMoveWithBoy = true;
        }

        private async void SetDoctorScene()
        {
            await Util.Delay(1);
            Util.SetAni(doctor1, Const.Doctor.SHOUT);
            Util.SetTurnBack(doctor2, 0);

            await Util.Delay(1);
            messageDoctor1.SetActive(true);
            Util.ShowMessage(doctor1, messageDoctor1, 0.5f, 2);
            Util.SetAni(doctor2, Const.Doctor.SHOUT, true);
            isSecurity1Run = true;
            isSecurity2Run = true;
            isSecurity3Run = true;

            await Util.Delay(1);
            Util.SetAni(doctor1, Const.Doctor.IDLE, true);

            await Util.Delay(2);
            messageDoctor1.SetActive(false);
            Util.SetAni(doctor1, Const.Doctor.WALK, true);
            isDoctor1Run = true;
        }

        private void ChangeToCamera(int index)
        {
            Camera.main.transform.position = cameras[index].transform.position;
        }

        protected override void ShakeCamera()
        {
            ChangeAmount(0.04f);
            base.ShakeCamera();
        }
    }
}
