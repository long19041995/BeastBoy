using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level15
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject deer;
        [SerializeField] private GameObject turtle;
        [SerializeField] private GameObject treeTop;
        [SerializeField] private GameObject pointCollider;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    AudioController.Instance.Play(Const.Common.AUDIOS.SCREAM);
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowDeer();
            Transform parentParent = treeTop.transform.parent.transform.parent;
            treeTop.transform.SetParent(parentParent);
            treeTop.AddComponent<Rigidbody2D>();
            treeTop.AddComponent<BoxCollider2D>();

            await Util.Delay(1);
            ShowItem();

            await Util.Delay(1);
            NextWave();
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN, true);
            Move(new GameObjectMoved(boy, flagStopBoyRunNextWave, Time.deltaTime * 2, () =>
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
            }));
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () =>
            {
                ShowOption();
            }));
        }

        public async override void OnFail()
        {
            pointCollider.SetActive(false);
            ShowTurtle();

            await Util.Delay(0.5f);
            ShowItem();
            Util.SetAni(turtle, Const.Turtle.DIE);

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            deer.SetActive(false);
            turtle.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDeer()
        {
            deer.SetActive(true);
            boy.SetActive(false);
            turtle.SetActive(false);

            ShowSmoke(deer);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowTurtle()
        {
            turtle.SetActive(true);
            boy.SetActive(false);
            deer.SetActive(false);

            ShowSmoke(turtle);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
