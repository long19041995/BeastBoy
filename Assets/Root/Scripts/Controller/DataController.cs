using UnityEngine;

public class DataController : Singleton<DataController>
{
    [SerializeField] private GameData gameData;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public int IndexPassed
    {
        get
        {
            return PlayerPrefs.GetInt(Const.Common.INDEX_PASSED, 0);
        }

        set
        {
            PlayerPrefs.SetInt(Const.Common.INDEX_PASSED, value);
        }
    }

    public int IndexMap
    {
        get
        {
            if (gameData.usePlayerPrefs)
            {
                return PlayerPrefs.GetInt(Const.Common.INDEX_MAP, 0);
            }
            else
            {
                return gameData != null ? gameData.indexMap : 0;
            }

        }

        set
        {
            PlayerPrefs.SetInt(Const.Common.INDEX_MAP, value);
        }
    }

    public MapData CurrentMapData
    {
        get
        {
            if (IndexMap > -1 && IndexMap < gameData.listMap.Count)
            {
                return gameData.listMap[IndexMap];
            }

            return null;
        }
    }

    public int IndexLevel
    {
        get
        {
            if (gameData.usePlayerPrefs)
            {
                return PlayerPrefs.GetInt(Const.Common.INDEX_LEVEL, 0);
            }
            else
            {
                return CurrentMapData != null ? CurrentMapData.indexLevel : 0;
            }
        }

        set
        {
            int result = 0;
            if (CurrentMapData != null && value > -1 && value < CurrentMapData.listLevel.Count)
            {
                result = value;
            }

            if (gameData.usePlayerPrefs)
            {
                PlayerPrefs.SetInt(Const.Common.INDEX_LEVEL, result);

                if (result > IndexPassed)
                {
                    IndexPassed = result;
                }
            }
            else
            {
                CurrentMapData.indexLevel = result;
            }
        }
    }

    public LevelData CurrentLevelData
    {
        get
        {
            if (IndexLevel > -1 && IndexLevel < CurrentMapData.listLevel.Count)
            {
                return CurrentMapData.listLevel[IndexLevel];
            }

            return null;
        }
    }

    public GameObject CurrentLevel
    {
        get
        {
            return CurrentLevelData != null ? CurrentLevelData.level : null;
        }
    }

    public GameObject CurrentProgressBar
    {
        get
        {
            if (IndexLevel > -1 && IndexLevel < CurrentMapData.listLevel.Count)
            {
                return CurrentMapData.listLevel[IndexLevel].progressBar;
            }

            return null;
        }
    }

    public int IndexWave
    {
        get
        {
            if (gameData.usePlayerPrefs)
            {
                return PlayerPrefs.GetInt(Const.Common.INDEX_WAVE, 0);
            }
            else
            {
                return CurrentLevelData != null ? CurrentLevelData.IndexWave : 0;
            }
        }

        set
        {
            int result = 0;
            if (CurrentLevelData != null && value > -1 && value < CurrentLevelData.listWave.Count)
            {
                result = value;
            }

            if (gameData.usePlayerPrefs)
            {
                PlayerPrefs.SetInt(Const.Common.INDEX_WAVE, result);
            }
            else
            {
                CurrentLevelData.IndexWave = result;
            }
        }
    }

    public WaveData CurrentWaveData
    {
        get
        {
            if (IndexWave > -1 && IndexWave < CurrentLevelData.listWave.Count)
            {
                return CurrentLevelData.listWave[IndexWave];
            }

            return null;
        }
    }

    public GameObject CurrentOption
    {
        get
        {
            return CurrentWaveData != null ? CurrentWaveData.option : null;
        }
    }

    public int Coin
    {
        get
        {
            if (gameData.usePlayerPrefs)
            {
                int coin = PlayerPrefs.GetInt(Const.Common.COIN, 0);
                return coin < 0 ? 0 : coin;
            }
            else
            {
                return gameData.coin;
            }
        }

        set
        {
            if (gameData.usePlayerPrefs)
            {
                PlayerPrefs.SetInt(Const.Common.COIN, value);
            }
            else
            {
                gameData.coin = value;
            }
        }
    }

    public int IndexIngame
    {
        get
        {
            if (gameData.usePlayerPrefs)
            {
                return PlayerPrefs.GetInt(Const.Common.INDEX_INGAME, 0);
            }
            else
            {
                return gameData.indexIngame;
            }
        }

        set
        {
            if (gameData.usePlayerPrefs)
            {
                PlayerPrefs.SetInt(Const.Common.INDEX_INGAME, value);
            }
            else
            {
                gameData.indexIngame = value;
            }
        }
    }

    public bool IsBeta
    {
        get
        {
            return gameData.isBeta;
        }
    }

    public bool IsMuteSound
    {
        get
        {
            if (gameData.usePlayerPrefs)
            {
                return PlayerPrefs.GetInt(Const.Common.IS_MUTE_SOUND, 0) == 1 ? true : false;
            }
            else
            {
                return gameData.isMuteSound;
            }
        }

        set
        {
            if (gameData.usePlayerPrefs)
            {
                PlayerPrefs.SetInt(Const.Common.IS_MUTE_SOUND, value ? 1 : 0);
            }
            else
            {
                gameData.isMuteSound = value;
            }
        }
    }

    public bool IsMuteSoundAll
    {
        get
        {
            if (gameData.usePlayerPrefs)
            {
                return PlayerPrefs.GetInt(Const.Common.IS_MUTE_SOUND_ALL, 0) == 1 ? true : false;
            }
            else
            {
                return gameData.isMuteSoundAll;
            }
        }

        set
        {
            if (gameData.usePlayerPrefs)
            {
                PlayerPrefs.SetInt(Const.Common.IS_MUTE_SOUND_ALL, value ? 1 : 0);
            }
            else
            {
                gameData.isMuteSoundAll = value;
            }
        }
    }

    public GameObject Smoke
    {
        get
        {
            return gameData.smoke;
        }
    }
}
