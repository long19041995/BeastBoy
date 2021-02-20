using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Map2.Level11
{
    public class Wave1 : WaveMap
    {
        [SerializeField] private GameObject boy;
        [SerializeField] private GameObject dog;
        [SerializeField] private GameObject wolf;
        [SerializeField] private GameObject dino3;
        [SerializeField] private GameObject footPrint;
        [SerializeField] private GameObject messageBoy;
        [SerializeField] private GameObject messageDog;

        [SerializeField] private GameObject flagBoyPosition;
        [SerializeField] private GameObject flagCameraPosition;
        [SerializeField] private GameObject flagStopDogSmell;
        [SerializeField] private GameObject flagStopCameraMoveWithDog;
        [SerializeField] private GameObject flagStopWolfSmell;
        [SerializeField] private GameObject flagStopCameraMoveWithWolf;
        [SerializeField] private GameObject flagBoyRunNextWave;
        [SerializeField] private GameObject flagStopCameraMoveNextWave;

        private bool isWolfSmell = false;
        private bool isDogSmell = false;
        private bool isCameraMove = false;
        private Coroutine coroutine;

        private void Start()
        {
            if (DataController.Instance.IndexWave == 0)
            {
                boy.transform.position = flagBoyPosition.transform.position;
                Camera.main.transform.position = flagCameraPosition.transform.position;
                ShowDino3();

                Move(new GameObjectMoved(dino3, boy, Time.deltaTime * 2, async () =>
                {
                    ShowBoy();
                    Util.SetAni(boy, Const.Boy2.M21.LOST, true);

                    await Util.Delay(1);
                    messageBoy.SetActive(true);
                    Util.ShowMessage(boy, messageBoy, 0.5f, 1.5f);

                    await Util.Delay(1);
                    messageBoy.SetActive(false);

                    await Util.Delay(1);
                    ShowOption();
                }));
            }
        }

        public async override void OnPass()
        {
            ShowWolf();

            await Util.Delay(1);
            Util.SetAni(wolf, Const.Wolf.M2.SMELL);

            await Util.Delay(2);
            Util.SetAni(wolf, Const.Wolf.M2.WALK, true);
            coroutine = StartCoroutine("ShowFootPrint");

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithWolf, Time.deltaTime * 1.5f, () => { }));

            Move(new GameObjectMoved(wolf, flagStopWolfSmell, Time.deltaTime * 1.5f, async () =>
            {
                ShowItem();
                HideFootPrint();
                Util.SetAni(wolf, Const.Wolf.IDLE, true);

                await Util.Delay(1);
                NextWave();
                boy.transform.position = wolf.transform.position;
                Util.SetAni(boy, Const.Boy2.M20.RUN, true);
                ShowBoy();

                Move(new GameObjectMoved(boy, flagBoyRunNextWave, Time.deltaTime * 2, () =>
                {
                    Util.SetAni(boy, Const.Boy2.M22.IDLE, true);
                }));

                Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveNextWave, Time.deltaTime * 2, () =>
                {
                    ShowOption();
                }));
            }));
        }

        private IEnumerator ShowFootPrint()
        {
            while (true)
            {
                GameObject footPrintInstance = Instantiate(footPrint);
                footPrintInstance.SetActive(true);
                footPrintInstance.transform.SetParent(gameObject.transform);

                Vector3 position = wolf.transform.position;
                position.x += 2;
                footPrintInstance.transform.position = position;
                yield return new WaitForSeconds(1);
            }
        }

        private void HideFootPrint()
        {
            footPrint.SetActive(false);
            StopCoroutine(coroutine);
        }

        public async override void OnFail()
        {
            ShowDog();

            await Util.Delay(1);
            Util.SetAni(dog, Const.DogMain.M21.SMELL, true);

            Move(new GameObjectMoved(dog, flagStopDogSmell, Time.deltaTime, async () =>
            {
                messageDog.SetActive(true);
                Util.ShowMessage(dog, messageDog, 0.5f, 0.5f);
                Util.SetAni(dog, Const.DogMain.M21.FAINT);
                ShowItem();
                await Util.Delay(2);
                messageDog.SetActive(false);
                ShowResult();
            }));

            Move(new GameObjectMoved(Camera.main.gameObject, flagStopCameraMoveWithDog, Time.deltaTime, () => { }));
        }

        private void ShowBoy()
        {
            boy.SetActive(true);
            dog.SetActive(false);
            wolf.SetActive(false);
            dino3.SetActive(false);

            ShowSmoke(boy);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDog()
        {
            dog.SetActive(true);
            boy.SetActive(false);
            wolf.SetActive(false);
            dino3.SetActive(false);

            ShowSmoke(dog);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowWolf()
        {
            wolf.SetActive(true);
            boy.SetActive(false);
            dog.SetActive(false);
            dino3.SetActive(false);

            ShowSmoke(wolf);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }

        private void ShowDino3()
        {
            dino3.SetActive(true);
            wolf.SetActive(false);
            boy.SetActive(false);
            dog.SetActive(false);

            ShowSmoke(dino3);
            AudioController.Instance.Play(Const.Common.AUDIOS.TRANSFIGURE, false, 0.1f);
        }
    }
}
