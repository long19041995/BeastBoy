using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2
{
    public class WaveMap : MonoBehaviour
    {
        public virtual void OnPass()
        {
        }

        public virtual void OnFail()
        {
        }

        protected void ShowProgressBar()
        {
            ProgressBarController.Instance.SetActive(true);
        }

        protected void HideProgressBar()
        {
            ProgressBarController.Instance.SetActive(false);
        }

        protected void ShowOption()
        {
            OptionController.Instance.SetActive(true);
        }

        protected void HideOption()
        {
            OptionController.Instance.SetActive(false);
        }

        protected async void ShowItem()
        {
            Gamemanager.Instance.ShowItemResult();

            if (OptionController.Instance.GetState() == Const.Common.WAVE_STATE.FAIL)
            {
                AudioController.Instance.Play(Const.Common.AUDIOS.FAIL_CHOOSE1);
                await Util.Delay(1);
                AudioController.Instance.Stop(Const.Common.AUDIOS.FAIL_CHOOSE1);
            }
            else
            {
                AudioController.Instance.Play(Const.Common.AUDIOS.PASS);
            }
        }

        protected void HideItem()
        {
            Gamemanager.Instance.HideItem();
        }

        protected void ShowResult()
        {
            Gamemanager.Instance.ShowPopup();
        }

        protected Coroutine Move(GameObjectMoved gameObjectMoved)
        {
            return StartCoroutine("MoveGameObject", gameObjectMoved);
        }

        private IEnumerator MoveGameObject(GameObjectMoved gameObject)
        {
            while (gameObject.current.transform.position != gameObject.target.transform.position)
            {
                gameObject.current.transform.position = Vector3.MoveTowards(gameObject.current.transform.position, gameObject.target.transform.position, gameObject.speed);
                yield return null;
            }


            AudioController.Instance.Stop(Const.Common.AUDIOS.BREATHING);
            AudioController.Instance.Stop(Const.Common.AUDIOS.BREATHING);
            gameObject.action();
        }

        protected async void ShowSmoke(GameObject gameObject, int offsetTop = 1, int offsetRight = 0)
        {
            GameObject smoke = Instantiate(DataController.Instance.Smoke);
            Vector3 position = gameObject.transform.position;
            position.y += offsetTop;
            position.x += offsetRight;
            smoke.transform.position = position;

            await Util.Delay(2);
            Destroy(smoke);
        }

        protected virtual void ShakeCamera()
        {
            Camera.main.GetComponent<CameraShake>().Shake();
        }

        protected virtual void StopShakeCamera()
        {
            Camera.main.GetComponent<CameraShake>().StopShake();
        }

        protected virtual void ChangeAmount(float amount = 0)
        {
            Camera.main.GetComponent<CameraShake>().ChangeAmount(amount);
        }

        protected virtual void NextWave()
        {
            Gamemanager.Instance.NextWave();
        }
    }
}
