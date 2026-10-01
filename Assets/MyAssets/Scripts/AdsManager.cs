// using UnityEngine;
// using GoogleMobileAds.Api;

// public class AdsManager : MonoBehaviour
// {
//     private string intertestialId = "ca-app-pub-3940256099942544/1033173712";
//     private string bannerId = "ca-app-pub-3940256099942544/6300978111";
//     private string rewardId = "ca-app-pub-3940256099942544/5224354917";

//     private InterstitialAd interstitialAd;
//     private BannerView bannerView;
//     private RewardedAd rewardedAd;
//     private bool giveReward;

//     public static AdsManager Instance;

//     public bool enableAds;

//     private void Awake()
//     {
//         if (Instance != null)
//             Destroy(gameObject);
//         else
//             Instance = this;

//         DontDestroyOnLoad(gameObject);
//     }

//     public void Start()
//     {
//         if (!enableAds)
//             return;

//         MobileAds.Initialize(initStatus => { });

//         LoadBannerAd();

//         RequestInterstitial();

//         LoadRewardedAd();
//     }

//     public void LoadBannerAd()
//     {
//         if (bannerView == null)
//         {
//             CreateBannerView();
//         }
//         var adRequest = new AdRequest();
//         adRequest.Keywords.Add("unity-admob-sample");

//         Debug.Log("Loading banner ad.");
//         bannerView.LoadAd(adRequest);
//     }

//     public void CreateBannerView()
//     {
//         Debug.Log("Creating banner view");

//         if (bannerView != null)
//         {
//             DestroyAd();
//         }

//         bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
//     }

//     public void DestroyAd()
//     {
//         if (bannerView != null)
//         {
//             Debug.Log("Destroying banner ad.");
//             bannerView.Destroy();
//             bannerView = null;
//         }
//     }
//     private void RequestInterstitial()
//     {
//         if (interstitialAd != null)
//         {
//             interstitialAd.Destroy();
//             interstitialAd = null;
//         }

//         Debug.Log("Loading the interstitial ad.");

//         var adRequest = new AdRequest();
//         adRequest.Keywords.Add("unity-admob-sample");

//         InterstitialAd.Load(intertestialId, adRequest,
//             (InterstitialAd ad, LoadAdError error) =>
//             {
//                 if (error != null || ad == null)
//                 {
//                     Debug.LogError("interstitial ad failed to load an ad " +
//                                    "with error : " + error);
//                     return;
//                 }

//                 Debug.Log("Interstitial ad loaded with response : "
//                           + ad.GetResponseInfo());

//                 interstitialAd = ad;
//             });
//     }

//     public void ShowInterstitialAd()
//     {
//         if (!enableAds)
//             return;

//         if (interstitialAd != null && interstitialAd.CanShowAd())
//         {
//             Debug.Log("Showing interstitial ad.");
//             interstitialAd.Show();
//         }
//         else
//         {
//             Debug.LogError("Interstitial ad is not ready yet.");
//             RequestInterstitial();
//         }
//     }

//     public void ShowRewardedAd()
//     {
//         if (!enableAds)
//             return;

//         const string rewardMsg =
//             "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
//         if (rewardedAd != null && rewardedAd.CanShowAd())
//         {
//             rewardedAd.Show((Reward reward) =>
//             {
//                 giveReward = true;
//             });
//         }

//         LoadRewardedAd();
//     }

//     public void LoadRewardedAd()
//     {
//         if (rewardedAd != null)
//         {
//             rewardedAd.Destroy();
//             rewardedAd = null;
//         }

//         Debug.Log("Loading the rewarded ad.");

//         var adRequest = new AdRequest();
//         adRequest.Keywords.Add("unity-admob-sample");

//         RewardedAd.Load(rewardId, adRequest,
//             (RewardedAd ad, LoadAdError error) =>
//             {
//                 if (error != null || ad == null)
//                 {
//                     Debug.LogError("Rewarded ad failed to load an ad " +
//                                    "with error : " + error);
//                     return;
//                 }

//                 Debug.Log("Rewarded ad loaded with response : "
//                           + ad.GetResponseInfo());

//                 rewardedAd = ad;
//             });
//     }

//     private void Update()
//     {
//         if (giveReward)
//         {
//             giveReward = false;
//             GiveReward();
//         }
//     }

//     private void GiveReward()
//     {
//         AudioManager.Instance.Play("Reward");
//         FindFirstObjectByType<CoinsManager>().AddCoins(150);
//     }
// }