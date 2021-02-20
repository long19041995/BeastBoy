using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RemoteConfigController : Singleton<RemoteConfigController>
{
    public static int LevelDelay;
    public static int TimeDelay;
    public static int FirstOpenDelay;
    public static float CurrentVersionAndroid;

    private const string LEVEL_DELAY = "LEVEL_DELAY";
    private const string TIME_DELAY = "TIME_DELAY";
    private const string FIRST_OPEN_DELAY = "FIRST_OPEN_DELAY";
    private const string CURRENT_VERSION_ANDROID = "CURRENT_VERSION_ANDROID";
    private readonly Dictionary<string, object> defaults = new Dictionary<string, object>();
    private bool isFetched = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                InitalizeFirebase();
                FetchDataAsync();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    private void Update()
    {
        if (isFetched)
        {
            isFetched = false;
            AdMobController.Instance.Init();
        }
    }

    private void InitalizeFirebase()
    {
        defaults.Add(LEVEL_DELAY, 2);
        defaults.Add(FIRST_OPEN_DELAY, 5);
        defaults.Add(TIME_DELAY, 30);
        defaults.Add(CURRENT_VERSION_ANDROID, "");

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
    }

    public Task FetchDataAsync()
    {
        Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWith(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.LogError("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.LogError("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.LogError("Latest Fetch call still pending.");
                break;
        }

        if (fetchTask.IsCanceled)
        {
        }
        else if (fetchTask.IsFaulted)
        {
        }
        else if (fetchTask.IsCompleted)
        {
        }

        LevelDelay = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(LEVEL_DELAY).StringValue);
        TimeDelay = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(TIME_DELAY).StringValue);
        FirstOpenDelay = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(FIRST_OPEN_DELAY).StringValue);
        try
        {
            CurrentVersionAndroid = float.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue(CURRENT_VERSION_ANDROID).StringValue);
        }
        catch (Exception e)
        {
            CurrentVersionAndroid = 0;
        }

        isFetched = true;
    }
}
