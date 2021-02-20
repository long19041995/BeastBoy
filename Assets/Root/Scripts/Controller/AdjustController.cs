using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using com.adjust.sdk;

public class AdjustController : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitAdjust("bnq91flam29s");
    }

    private void InitAdjust(string adjustAppToken)
    {
        var adjustConfig = new AdjustConfig(
            adjustAppToken,
            AdjustEnvironment.Production,
            true
        );
        adjustConfig.setLogLevel(AdjustLogLevel.Info);
        adjustConfig.setSendInBackground(true);
        new GameObject("Adjust").AddComponent<Adjust>();

        Adjust.start(adjustConfig);
    }
}