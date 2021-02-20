using Spine.Unity;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Util
{
  public static IEnumerator Wait(float time, Action callback)
  {
    yield return new WaitForSeconds(time);
    callback?.Invoke();
  }

  public static float GetDurationAnimation(SkeletonAnimation skeleton, string nameAnimation)
  {
    Spine.Animation[] result = skeleton.SkeletonDataAsset.GetSkeletonData(true).Animations.Items;
    for (int i = 0; i < result.Length; i++)
    {
      if (nameAnimation.Equals(result[i].Name))
      {
        return result[i].Duration;
      }
    }
    return 0;
  }

  public static void SetTurnBack(GameObject gameObject, float degree = 180)
  {
    Quaternion rotation = gameObject.transform.rotation;
    rotation.y = degree;
    gameObject.transform.rotation = rotation;
  }

  public static void SetRotate(GameObject gameObject, float degree = 180)
  {
    gameObject.transform.Rotate(new Vector3(0, 0, degree));
  }

  public static void SetScale(GameObject gameObject, float scale = 1)
  {
    gameObject.transform.localScale = new Vector3(scale, scale, scale);
  }

  public static void SetAni(GameObject gameObject, string ani, bool loop = false, float mixDuration = -1, float timeScale = 1)
  {
    SkeletonAnimation skeleton = gameObject.GetComponent<SkeletonAnimation>();
    Spine.TrackEntry trackEntry = skeleton.AnimationState.SetAnimation(0, ani, loop);
    skeleton.timeScale = timeScale;

    if (mixDuration >= 0)
    {
      trackEntry.MixDuration = mixDuration;
    }

    if (ani == Const.Boy2.M20.RUN || ani == Const.Boy2.M20.RUN_SMILE)
    {
      AudioController.Instance.Play(Const.Common.AUDIOS.BREATHING, true, 0.2f);
    }
  }

  public static void SetAniDefault(GameObject gameObject)
  {
    gameObject.GetComponent<SkeletonAnimation>().AnimationName = "";
  }

  public static void SetSpeedAni(GameObject gameObject, float speed = 1)
  {
    gameObject.GetComponent<SkeletonAnimation>().timeScale = speed;
  }

  public static Task Delay(float time)
  {
    int delay = (int)(time * 1000);
    return Task.Delay(delay);
  }

  public static void MoveGameObject(GameObject current, GameObject target, float speed, Action action)
  {
    current.transform.position = Vector3.MoveTowards(current.transform.position, target.transform.position, speed);
    if (current.transform.position == target.transform.position)
    {
      AudioController.Instance.Stop(Const.Common.AUDIOS.BREATHING);
      action();
    }
  }

  public static void ShowMessage(GameObject gameObject, GameObject message, float offsetX = 0, float offsetY = 0)
  {
    Vector2 position = new Vector2(gameObject.transform.position.x + offsetX, gameObject.transform.position.y + offsetY);
    message.transform.position = Camera.main.WorldToScreenPoint(position);
  }

  public static double GetSecondCurrent()
  {
    var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
    var timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
    return timestamp;
  }
}
