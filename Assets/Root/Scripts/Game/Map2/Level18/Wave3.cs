using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2.Level18
{
    public class Wave3 : WaveMap
    {
        [SerializeField] private GameObject dino;
        [SerializeField] private GameObject bird;
        [SerializeField] private GameObject eagle;
        [SerializeField] private GameObject airplane;
        [SerializeField] private GameObject laser;
        [SerializeField] private BackgroundInfinite background;
        [SerializeField] private List<GameObject> birds;

        [SerializeField] private GameObject flagDinoPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopDinoFly;
        [SerializeField] private GameObject flagStopCameraMove;
        [SerializeField] private GameObject flagStopAirplaneFly;
        [SerializeField] private GameObject flagStopAirplaneFlyOut;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 2)
            {
                background.Move();
                dino.SetActive(true);
                dino.transform.position = flagDinoPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;

                Util.SetTurnBack(airplane);
                airplane.SetActive(true);
                Move(new GameObjectMoved(airplane, flagStopAirplaneFly, Time.deltaTime * 4, () => { }));

                Move(new GameObjectMoved(dino, flagStopDinoFly, Time.deltaTime * 2, () => { }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMove, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            bird.transform.position = dino.transform.position;
            Util.SetTurnBack(bird);
            ShowBird();

            await Util.Delay(1);
            ShowItem();
            Move(new GameObjectMoved(airplane, flagStopAirplaneFlyOut, Time.deltaTime * 4, () =>
            {
                ShowResult();
            }));
        }

        public async override void OnFail()
        {
            ShowEgale();

            MoveBirds();

            await Util.Delay(0.5f);
            laser.SetActive(true);

            await Util.Delay(0.2f);
            ShowItem();
            //laser.SetActive(false);
            Util.SetAni(eagle, Const.Eagle.DIE, true);
            eagle.GetComponent<Rigidbody2D>().gravityScale = 1;

            await Util.Delay(1);
            ShowResult();
        }

        private void MoveBirds()
        {
            birds.ForEach(bird =>
            {
                float positionX = Random.Range(-5, -20);
                float positionY = Random.Range(-20, 20);
                Debug.Log(positionX + " " + positionY);
                GameObject gameObject = new GameObject();
                gameObject.transform.position = new Vector2(positionX, positionY);
                Move(new GameObjectMoved(bird, gameObject, Time.deltaTime * 4, () => { }));
            });
        }

        private void ShowDino()
        {
            dino.SetActive(true);
            bird.SetActive(false);
            eagle.SetActive(false);

            ShowSmoke(dino);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowBird()
        {
            bird.SetActive(true);
            dino.SetActive(false);
            eagle.SetActive(false);

            ShowSmoke(bird);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowEgale()
        {
            eagle.SetActive(true);
            dino.SetActive(false);
            bird.SetActive(false);

            ShowSmoke(eagle);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
