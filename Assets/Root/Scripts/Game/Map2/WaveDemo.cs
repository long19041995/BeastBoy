using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level16
{
    public class WaveDemo : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject hippo;
        [SerializeField] private GameObject cow;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }
        }

        public override void OnPass()
        {
            ShowHippo();
        }

        public override void OnFail()
        {
            ShowCow();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            hippo.SetActive(false);
            cow.SetActive(false);

            ShowSmoke(boy);
        }

        private void ShowHippo()
        {
            hippo.SetActive(true);
            boy.SetActive(false);
            cow.SetActive(false);

            ShowSmoke(hippo);
        }

        private void ShowCow()
        {
            cow.SetActive(true);
            boy.SetActive(false);
            hippo.SetActive(false);

            ShowSmoke(cow);
        }
    }
}
