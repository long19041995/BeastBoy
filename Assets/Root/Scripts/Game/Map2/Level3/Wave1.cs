using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level3
{
    public class Wave1 : WaveMap
    {
        [SerializeField] float speedBoyRun = 1.5f;
        [SerializeField] float speedWallMove = 1.5f;

        [SerializeField] GameObject boy;
        [SerializeField] GameObject gorilla;
        [SerializeField] GameObject turtle;
        [SerializeField] GameObject wall;
        [SerializeField] GameObject wallLeft;
        [SerializeField] GameObject wallRight;
        [SerializeField] GameObject door;

        [SerializeField] GameObject flagStopWallMove;
        [SerializeField] GameObject flagStopBoyRun;
        [SerializeField] GameObject flagStopCameraMove;
        [SerializeField] GameObject flagStopWallLeftMove;
        [SerializeField] GameObject flagStopWallRightMove;

        private void Start()
        {
            PreparePlay();
        }

        private void PreparePlay()
        {
            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * speedBoyRun, () => { }));
            Move(new GameObjectMoved(boy, flagStopBoyRun, Time.deltaTime * speedBoyRun, () => 
            {
                Util.SetAni(boy, Const.Boy2.M20.AFRAID, true);

                Move(new GameObjectMoved(wall, flagStopWallMove, Time.deltaTime * speedWallMove, () =>
                {
                    ShowOption();
                }));
            }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            gorilla.SetActive(false);
            turtle.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowGorilla()
        {
            gorilla.SetActive(true);
            boy.SetActive(false);
            turtle.SetActive(false);

            ShowSmoke(gorilla);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowTurtle()
        {
            turtle.SetActive(true);
            boy.SetActive(false);
            gorilla.SetActive(false);

            ShowSmoke(turtle);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        public async override void OnPass()
        {
            MoveWall();

            AudioController.Instance.Play(Const.Common.AUDIOS.GORILLA);
            ShowGorilla();

            await Util.Delay(1);
            Util.SetAni(gorilla, Const.Gorilla.SHOUT, true);

            await Util.Delay(2);
            ShowItem();
            Util.SetAni(gorilla, Const.Gorilla.CLIMB);

            await Util.Delay(0.1f);
            AudioController.Instance.Play(Const.Common.AUDIOS.DOOR);
            door.GetComponent<Rigidbody2D>().gravityScale = 3;

            await Util.Delay(0.3f);
            Util.SetRotate(gorilla, -25);
            AudioController.Instance.Stop(Const.Common.AUDIOS.GORILLA);

            await Util.Delay(3);
            ShowResult();
        }

        public async override void OnFail()
        {
            MoveWall(4);

            ShowTurtle();

            Util.SetAni(turtle, Const.Turtle.GATHER);

            await Util.Delay(0.3f);
            Util.SetAni(turtle, Const.Turtle.RING, true);

            await Util.Delay(3);
            ShowItem();
            Util.SetAni(turtle, Const.Turtle.BREAK);

            await Util.Delay(2);
            ShowResult();
        }

        private void MoveWall(float decrease = 20)
        {
            Move(new GameObjectMoved(wallLeft, flagStopWallLeftMove, Time.deltaTime * speedWallMove / decrease, () => { }));
            Move(new GameObjectMoved(wallRight, flagStopWallRightMove, Time.deltaTime * speedWallMove / decrease, () => { }));
        }
    }
}
