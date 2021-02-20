using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobController : Singleton<AdMobController>
{
  private BannerView bannerView;
  private InterstitialAd interstitial;
  private RewardedAd rewardedAd;
  private bool isTestAds
  {
    get
    {
      return float.Parse(Application.version) >= RemoteConfigController.CurrentVersionAndroid;
    }
  }
  private bool isShowAds
  {
    get
    {
      return DataController.Instance.IndexPassed >= RemoteConfigController.FirstOpenDelay;
    }
  }

  private bool isShowReward
  {
    get
    {
      return Gamemanager.TimeDoneLevel >= RemoteConfigController.TimeDelay && Gamemanager.NumberLevelAds >= RemoteConfigController.LevelDelay;
    }
  }

  private void Start()
  {
    DontDestroyOnLoad(gameObject);
  }

  public void Init()
  {
    MobileAds.Initialize(initStatus => { });
    RequestBanner();
    RequestInterstitial();
    RequestRewardedAd();
  }

  private void RequestBanner()
  {
    string adUnitId = isTestAds ? "ca-app-pub-3940256099942544/6300978111" : "ca-app-pub-8566745611252640/2179135380";
    bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
    AdRequest request = new AdRequest.Builder().Build();
    bannerView.LoadAd(request);
  }

  public void ShowBanner()
  {
    if (bannerView != null && isShowAds)
    {
      bannerView.Show();
    }
  }

  public void HideBanner()
  {
    if (bannerView != null)
    {
      bannerView.Hide();
    }
  }

  public void RequestInterstitial()
  {
    string adUnitId = isTestAds ? "ca-app-pub-3940256099942544/1033173712" : "ca-app-pub-8566745611252640/1987563698";
    interstitial = new InterstitialAd(adUnitId);
    AdRequest request = new AdRequest.Builder().Build();
    interstitial.LoadAd(request);
  }

  public void ShowInterstitial()
  {
    if (interstitial.IsLoaded() && isShowAds && isShowReward)
    {
      interstitial.Show();
      interstitial.OnAdClosed += RequestInterstitialOnAdClosed;
      EndAds();
    }
  }

  private void RequestInterstitialOnAdClosed(object sender, EventArgs args)
  {
    RequestInterstitial();
  }

  public void RequestRewardedAd()
  {
    string adUnitId = isTestAds ? "ca-app-pub-3940256099942544/5224354917" : "ca-app-pub-8566745611252640/3109073672";
    rewardedAd = new RewardedAd(adUnitId);
    AdRequest request = new AdRequest.Builder().Build();
    rewardedAd.LoadAd(request);
  }

  public void ShowRewardedAd()
  {
    if (rewardedAd.IsLoaded() && isShowAds && isShowReward)
    {
      rewardedAd.Show();
      rewardedAd.OnAdClosed += RequestRewardedAdOnAdClosed;
      EndAds();
    }
  }

  private void RequestRewardedAdOnAdClosed(object sender, EventArgs args)
  {
    ShowRewardedAd();
  }

  private void EndAds()
  {
    Gamemanager.TimeDoneLevel = 0;
    Gamemanager.NumberLevelAds = 0;
    Gamemanager.TimeLevelStart = Util.GetSecondCurrent();
  }
}
