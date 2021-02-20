using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level14
{
    public class Wave2 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject wolf;
        [SerializeField] private GameObject rabit;
        [SerializeField] private GameObject carnivorous;

        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopWoflWalk;
        [SerializeField] private GameObject flagStopRabitRun;
        [SerializeField] private GameObject flagRabitInCarnivorousPosition;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 1)
            {
                Camera.main.transform.position = flagCameraPosition.transform.position;
                boy.transform.position = flagBoyPosition.transform.position;
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M211.IDLE, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowRabit();
            Util.SetAniDefault(rabit);

            await Util.Delay(1);
            Util.SetAni(rabit, Const.Rabit.RUN, true);
            Move(new GameObjectMoved(rabit, flagStopRabitRun, Time.deltaTime, async () =>
            {
                Util.SetAni(rabit, Const.Rabit.AFRAID, true);
                await Util.Delay(1);

                Util.SetAni(carnivorous, Const.Carnivorous.EAT);
                await Util.Delay(0.3f);

                rabit.GetComponent<MeshRenderer>().sortingOrder = 0;
                Util.SetAni(rabit, Const.Rabit.IDLE, true);
                rabit.SetActive(false);

                await Util.Delay(0.5f);
                Util.SetAni(carnivorous, Const.Carnivorous.CHEW, true);

                await Util.Delay(0.5f);
                rabit.transform.position = flagRabitInCarnivorousPosition.transform.position;
                rabit.SetActive(true);
                Util.SetAni(rabit, Const.Rabit.EAT, true);
                Util.SetAni(carnivorous, Const.Carnivorous.DIE);
                ShowItem();

                await Util.Delay(1);
                rabit.GetComponent<Rigidbody2D>().gravityScale = 1;

                await Util.Delay(1);
                boy.transform.position = rabit.transform.position;
                ShowBoy();
                NextWave();
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M214.IDLE, true, 0, 1);
                }));
                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () =>
                {
                    rabit.GetComponent<Rigidbody2D>().gravityScale = 0;
                    ShowOption();
                }));
            }));
        }

        public async override void OnFail()
        {
            ShowWolf();
            await Util.Delay(1);

            Util.SetAni(carnivorous, Const.Carnivorous.EAT);

            await Util.Delay(0.5f);
            wolf.SetActive(false);
            Util.SetAni(carnivorous, Const.Carnivorous.CHEW, true);
            ShowItem();

            await Util.Delay(2);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            wolf.SetActive(false);
            rabit.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowWolf()
        {
            wolf.SetActive(true);
            boy.SetActive(false);
            rabit.SetActive(false);

            ShowSmoke(wolf);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowRabit()
        {
            rabit.SetActive(true);
            boy.SetActive(false);
            wolf.SetActive(false);

            ShowSmoke(rabit, 0);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
