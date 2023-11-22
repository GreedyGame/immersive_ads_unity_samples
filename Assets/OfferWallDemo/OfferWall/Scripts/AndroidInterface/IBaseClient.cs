using System;

namespace PubScale.OfferWall
{
    public interface IBaseClient
    {
        event Action OnOfferWallShowed;
        event Action OnOfferWallClosed;
        event Action<string, string> OnOfferWallRewardClaimed;
        event Action<string> OnOfferWallShowFailed;
        event Action OnOfferWallInitSuccess;
        event Action<string> OnOfferWallInitFailed;
        //event Action<string> OnDataEncrypted;
        //event Action<string> OnAppographyDataFetched;
    }
}
