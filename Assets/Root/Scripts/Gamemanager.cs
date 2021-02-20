using System;
using UnityEngine;

public class Gamemanager : Singleton<Gamemanager>
{
  public static double TimeDoneLevel = 0;
  public static int NumberLevelAds = 0;
  public static double TimeLevelStart;

  private GameObject currentLevel;
  private bool isSkipBeforeWave1 = false;

  public bool IsSkipBeforeWave1
  {
    get
    {
      return isSkipBeforeWave1;
    }
  }

  public Level CurrentLevel
  {
    get
    {
      if (currentLevel)
      {
        return currentLevel.GetComponent<Level>();
      }

      return null;
    }
  }

  private void Start()
  {
    TimeLevelStart = Util.GetSecondCurrent();
    LoadGame();
    LoadAudio();
  }

  private void LoadAudio()
  {
    AudioController.Instance.Play(Const.Common.AUDIOS.IN_GAME_BACKGROUND, true, 0.1f);
    AudioController.Instance.Stop(Const.Common.AUDIOS.UI_BACKGROUND);
  }

  public void LoadGame()
  {
    LoadLevel();
    LoadProgressBar();
    LoadOption();
    AdMobController.Instance.ShowBanner();
  }

  public void LoadLevel()
  {
    currentLevel = Instantiate(DataController.Instance.CurrentLevel);
    DataController.Instance.IndexIngame++;
    FirebaseController.StartTheLevel();
  }

  public void LoadProgressBar()
  {
    if (DataController.Instance.CurrentProgressBar != null)
    {
      GameObject progressBar = Instantiate(DataController.Instance.CurrentProgressBar);
      ProgressBarController.Instance.Init(progressBar);
    }
  }

  public void LoadOption()
  {
    if (DataController.Instance.CurrentOption != null)
    {
      GameObject option = Instantiate(DataController.Instance.CurrentOption);
      OptionController.Instance.Init(option);
    }
  }

  public void OnAction(bool state)
  {
    if (state)
    {
      CurrentLevel.OnPass();
    }
    else
    {
      CurrentLevel.OnFail();
    }
  }

  public void IncreaseLevel()
  {
    DataController.Instance.IndexLevel++;
  }

  public void IncreaseWave()
  {
    DataController.Instance.IndexWave++;
  }

  public void NextLevel()
  {
    DestroyAll();
    IncreaseLevel();
    LoadGame();
  }

  public void NextWave()
  {
    IncreaseWave();
    if (DataController.Instance.IndexWave > 0)
    {
      LoadOption();
    }
    else
    {
      NextLevel();
    }
  }

  public void ResetLevel()
  {
    DestroyAll();
    LoadGame();
  }

  public void DestroyAll()
  {
    DestroyCurrentLevel();
    DestroyCurrentPopup();
    DestroyCurrentOption();
    DestroyCurrentProgressBar();
  }

  public void DestroyCurrentLevel()
  {
    Destroy(currentLevel);
  }

  public void DestroyCurrentOption()
  {
    OptionController.Instance.Destroy();
  }

  public void DestroyCurrentPopup()
  {
    PopupController.Instance.Destroy();
  }

  public void DestroyCurrentProgressBar()
  {
    ProgressBarController.Instance.Destroy();
  }

  public void ShowPopup()
  {
    if (OptionController.Instance.GetState() == Const.Common.WAVE_STATE.PASS)
    {
      CompleteLevel();
    }

    if (OptionController.Instance.GetState() == Const.Common.WAVE_STATE.FAIL)
    {
      if (DataController.Instance.IndexWave == 0)
      {
        GameOverLevel();
      }
      else
      {
        ContinueLevel();
      }
    }
  }

  public void ShowItemResult()
  {
    ProgressBarController.Instance.SetState();
    OptionController.Instance.ShowImg();
  }

  public void HideItem()
  {
    ProgressBarController.Instance.SetActive(false);
    OptionController.Instance.SetActive(false);
  }

  public void CompleteLevel()
  {
    HideItem();
    PopupController.Instance.Init(Const.Common.POPUP.COMPLETE);

    TimeDoneLevel = Util.GetSecondCurrent() - TimeLevelStart;
    NumberLevelAds++;
  }

  public void ContinueLevel()
  {
    HideItem();
    PopupController.Instance.Init(Const.Common.POPUP.CONTINUE);
  }

  public void GameOverLevel()
  {
    isSkipBeforeWave1 = DataController.Instance.IndexLevel == 0;
    HideItem();
    PopupController.Instance.Init(Const.Common.POPUP.GAME_OVER);
    StartCoroutine(Util.Wait(3, () => ResetLevel()));
  }
}
