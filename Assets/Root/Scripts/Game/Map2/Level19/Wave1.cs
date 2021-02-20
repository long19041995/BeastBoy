using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level19
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject bird;
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject cockroach;
        [SerializeField] private GameObject mouse;
        [SerializeField] private GameObject hologram;
        [SerializeField] private GameObject airplaneDie;
        [SerializeField] private GameObject airplane;
        [SerializeField] private GameObject security1;
        [SerializeField] private GameObject security2;
        [SerializeField] private GameObject messageHologram;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopToAirplaneDie;
        [SerializeField] private GameObject flagStopAirplaneIn;
        [SerializeField] private GameObject flagStopAirplaneOut;
        [SerializeField] private GameObject flagCameraPositionNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;
        [SerializeField] private GameObject flagStopSecurity1RunNextWave;
        [SerializeField] private GameObject flagStopSecurity2RunNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(bird, flagBoyPosition, Time.deltaTime * 2, async () =>
                {
                    ShowBoy();
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);

                    await Util.Delay(0.5f);
                    hologram.SetActive(true);

                    await Util.Delay(1);
                    messageHologram.SetActive(true);
                    Util.ShowMessage(hologram, messageHologram, 0.5f, 0.5f);

                    await Util.Delay(2);
                    messageHologram.SetActive(false);

                    await Util.Delay(1);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowCockroach();

            await Util.Delay(1);
            Move(new GameObjectMoved(cockroach, flagStopToAirplaneDie, Time.deltaTime * 2, () =>
            {
                cockroach.SetActive(false);
                hologram.SetActive(false);
                airplane.SetActive(true);
                Move(new GameObjectMoved(airplane, flagStopAirplaneIn, Time.deltaTime * 3, async () =>
                {
                    ShowItem();
                    airplaneDie.transform.SetParent(airplane.transform);
                    await Util.Delay(1);
                    Move(new GameObjectMoved(airplane, flagStopAirplaneOut, Time.deltaTime * 3, () =>
                    {
                        NextWave();
                        OnNextWave();
                    }));
                }));
            }));
        }

        public void OnNextWave()
        {
            airplane.SetActive(false);
            Camera.main.transform.position = flagCameraPositionNextWave.transform.position;
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () => { }));
            Move(new GameObjectMoved(security1, flagStopSecurity1RunNextWave, Time.deltaTime * 2, () =>
            {
                Util.SetAni(security1, Const.Security.IDLE, true);
                Move(new GameObjectMoved(security2, flagStopSecurity2RunNextWave, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(security2, Const.Security.IDLE, true);
                    ShowOption();
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowMouse();

            await Util.Delay(1);
            Util.SetAni(mouse, Const.Mouse.WALK, true);
            Move(new GameObjectMoved(mouse, flagStopToAirplaneDie, Time.deltaTime * 2, async () =>
            {
                ShowItem();
                Util.SetAni(mouse, Const.Mouse.CHOKE);

                await Util.Delay(3);
                ShowResult();
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            bird.SetActive(false);
            cockroach.SetActive(false);
            mouse.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowCockroach()
        {
            cockroach.SetActive(true);
            boy.SetActive(false);
            mouse.SetActive(false);

            ShowSmoke(cockroach);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMouse()
        {
            mouse.SetActive(true);
            boy.SetActive(false);
            cockroach.SetActive(false);

            ShowSmoke(mouse);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
