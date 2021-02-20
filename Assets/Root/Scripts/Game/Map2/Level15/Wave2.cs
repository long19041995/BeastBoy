using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level15
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject rabit;
        [SerializeField] private GameObject dolphin;
        [SerializeField] private GameObject tree;
        [SerializeField] private GameObject dragon;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopDolphinSwim;
        [SerializeField] private GameObject flagStopCameraMoveDolphin;
        [SerializeField] private GameObject flagStopRabitJumpOut;
        [SerializeField] private GameObject flagStopCameraMoveWithRabit;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private bool isRabitJump = false;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                tree.SetActive(false);
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M27.IDLE, true);
                    ShowOption();
                }));
            }
        }

        private void Update()
        {
            if (isRabitJump)
            {
                rabit.transform.position = Vector2.MoveTowards(rabit.transform.position, flagStopRabitJumpOut.transform.position, Time.deltaTime * 3.5f);
                if (rabit.transform.position == flagStopRabitJumpOut.transform.position)
                {
                    isRabitJump = false;
                }
            }
        }

        public override void OnPass()
        {
            ShowRabit();

            Util.SetAni(rabit, Const.Rabit.RUN2, true, -1, 0.5f);
            isRabitJump = true;
            ShowItem();
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithRabit, Time.deltaTime * 2, () =>
            {
                NextWave();
                boy.transform.position = rabit.transform.position;
                ShowBoy();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
                {
                    dragon.GetComponent<AudioSource>().Play();
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                }));
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowDolphin();

            await Util.Delay(1);
            Move(new GameObjectMoved(dolphin, flagStopDolphinSwim, Time.deltaTime * 4, async () =>
            {
                ShowItem();
                Util.SetAni(dolphin, Const.Dolphin.DIE, true);

                await Util.Delay(2);
                ShowResult();
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveDolphin, Time.deltaTime * 4, () => { }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            rabit.SetActive(false);
            dolphin.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowRabit()
        {
            rabit.SetActive(true);
            boy.SetActive(false);
            dolphin.SetActive(false);

            ShowSmoke(rabit);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDolphin()
        {
            dolphin.SetActive(true);
            boy.SetActive(false);
            rabit.SetActive(false);

            ShowSmoke(dolphin);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
