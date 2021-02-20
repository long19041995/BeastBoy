using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level11
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject giraffe;
        [SerializeField] private GameObject monkey;
        [SerializeField] private GameObject apple;
        [SerializeField] private GameObject aboriginal;
        [SerializeField] private GameObject messageBoy;
        [SerializeField] private GameObject stone;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopBoyRun;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopBoyRunOut;
        [SerializeField] private GameObject flagStopMonkeyEatApple;
        [SerializeField] private GameObject flagStopMoveStone;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);

                Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * 2, async () =>
                {
                    Util.SetAni(boy, Const.Boy2.M23.HUNGRY, true);

                    await Util.Delay(1);
                    messageBoy.SetActive(true);
                    Util.ShowMessage(boy, messageBoy, 0.5f, 1.5f);

                    await Util.Delay(1);
                    messageBoy.SetActive(false);

                    await Util.Delay(1);
                    ShowOption();
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () => { }));
            }
        }

        public async override void OnPass()
        {
            ShowGiraffe();

            Util.SetAni(giraffe, Const.Giraffe.EAT, true);

            await Util.Delay(2);
            ShowItem();
            ShowBoy();
            Util.SetAni(boy, Const.Boy2.M20.RUN_SMILE, true);

            Move(new GameObjectMoved(boy, flagStopBoyRunOut, Time.deltaTime * 2, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowMonkey();

            await Util.Delay(0.5f);
            Util.SetAni(monkey, Const.Monkey.JUMP1);

            await Util.Delay(0.2f);
            Move(new GameObjectMoved(monkey, flagStopMonkeyEatApple, Time.deltaTime * 4, () => { }));

            await Util.Delay(0.5f);
            Util.SetAni(monkey, Const.Monkey.JUMP2);

            await Util.Delay(0.5f);
            apple.SetActive(false);
            Util.SetAni(monkey, Const.Monkey.JUMP3);
            aboriginal.SetActive(true);
            Util.SetAni(aboriginal, Const.Aboriginal.PREVENT, true);

            await Util.Delay(0.5f);
            Util.SetAni(monkey, Const.Monkey.EAT_APPLE, true);

            await Util.Delay(1.5f);
            Util.SetAni(aboriginal, Const.Aboriginal.THROW_MONKEY);

            await Util.Delay(0.3f);
            stone.SetActive(true);
            stone.GetComponent<Rigidbody2D>().gravityScale = 1;
            stone.GetComponent<Rigidbody2D>().AddForce(transform.up * 500);
            Move(new GameObjectMoved(stone, flagStopMoveStone, Time.deltaTime * 2, () => { }));

            await Util.Delay(1);
            Util.SetAni(aboriginal, Const.Aboriginal.IDLE, true);
            ShowItem();
            Util.SetAni(monkey, Const.Monkey.BE_FIRED, true);

            await Util.Delay(1);
            ShowResult();
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            giraffe.SetActive(false);
            monkey.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowGiraffe()
        {
            giraffe.SetActive(true);
            boy.SetActive(false);
            monkey.SetActive(false);

            ShowSmoke(giraffe, 2);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowMonkey()
        {
            monkey.SetActive(true);
            boy.SetActive(false);
            giraffe.SetActive(false);

            ShowSmoke(monkey);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
