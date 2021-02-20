using Firebase.Analytics;
using UnityEngine;

public static class FirebaseController
{
    #region event name
    private const string START_THE_LEVEL_EVENT = "start_the_level";
    private const string COMPLETE_THE_LEVEL_EVENT = "complete_the_level";
    private const string FAIL_THE_LEVEL_EVENT = "fail_the_level";
    private const string TAP_TO_X5_COIN_EVENT = "tap_to_x5_coin";
    private const string TAP_TO_CONTINUE_EVENT = "tap_to_continue";
    private const string TAP_TO_CLAIM_EVENT = "tap_to_claim";
    #endregion

    #region param name
    private const string INDEX_LEVEL = "index_level";
    private const string INDEX_WAVE = "index_wave";
    private const string INDEX_INGAME = "index_ingame";
    #endregion

    #region function
    public static void StartTheLevel()
    {
        LogEvent(START_THE_LEVEL_EVENT);
    }

    public static void CompleteTheLevel()
    {
        LogEvent(COMPLETE_THE_LEVEL_EVENT);
    }

    public static void FailTheLevel()
    {
        LogEvent(FAIL_THE_LEVEL_EVENT);
    }

    public static void TapToX5Coin()
    {
        LogEvent(TAP_TO_X5_COIN_EVENT);
    }

    public static void TapToContinue()
    {
        LogEvent(TAP_TO_CONTINUE_EVENT);
    }

    public static void TapToClaim()
    {
        LogEvent(TAP_TO_CLAIM_EVENT);
    }

    private static void LogEvent(string name)
    {
        if (!IsMobile() && !DataController.Instance.IsBeta) return;

        var param = new Parameter[]
        {
            new Parameter(INDEX_LEVEL, DataController.Instance.IndexLevel + 1),
            new Parameter(INDEX_WAVE, DataController.Instance.IndexWave + 1),
            new Parameter(INDEX_INGAME, DataController.Instance.IndexIngame)
        };
        FirebaseAnalytics.LogEvent(name, param);
    }
    #endregion


    public static bool IsMobile()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
