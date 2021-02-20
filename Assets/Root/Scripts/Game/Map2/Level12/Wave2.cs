using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level12
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject kangaroo;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopDinoWalk;
        [SerializeField] private GameObject flagStopCameraMoveWithDino;
        [SerializeField] private GameObject flagStopKangarooJump;
        [SerializeField] private GameObject flagStopBoyRunOut;
        [SerializeField] private GameObject flagStopKangarooMove;
        [SerializeField] private GameObject flagCameraPositionNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, async () =>
                {
                    Util.SetAni(boy, Const.Boy2.M25.IDLE, true, 0);
                    await Util.Delay(1);
                    ShowOption();
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () => { }));
            }
        }

        public async override void OnPass()
        {
            ShowDino();

            await Util.Delay(1);
            Util.SetAni(dino, Const.Dino.WALK, true);
            ShakeCamera();

            Move(new GameObjectMoved(dino, flagStopDinoWalk, Time.deltaTime * 2, async () =>
            {
                StopShakeCamera();
                Util.SetAni(dino, Const.Dino.IDLE);

                await Util.Delay(1);
                ShowItem();
                boy.transform.position = dino.transform.position;
                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);

                NextWave();
                Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M26.FALL, true);
                    ShowOption();
                }));

                await Util.Delay(2);
                Camera.main.transform.position = flagCameraPositionNextWave.transform.position;
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithDino, Time.deltaTime * 2, () => { }));
        }

        public async override void OnFail()
        {
            ShowKangaroo();

            await Util.Delay(0.5f);
            Util.SetAni(kangaroo, Const.Kangaroo.JUMP);

            await Util.Delay(1);
            ShowItem();
            Util.SetAni(kangaroo, Const.Kangaroo.DIE, true);
            Move(new GameObjectMoved(kangaroo, flagStopKangarooMove, Time.deltaTime * 0.5f, () => { }));

            await Util.Delay(3);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            dino.SetActive(false);
            kangaroo.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            boy.SetActive(false);
            kangaroo.SetActive(false);

            ShowSmoke(dino, 2);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowKangaroo()
        {
            kangaroo.SetActive(true);
            boy.SetActive(false);
            dino.SetActive(false);

            ShowSmoke(kangaroo);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
