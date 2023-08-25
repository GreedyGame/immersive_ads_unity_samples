using System;

namespace Pubscale.OfferWall
{
    public interface IBaseClient
    {
        event Action OnOfferWallShowed;
        event Action OnOfferWallClosed;
        event Action<string, string> OnRewardClaimed;
        event Action<string> OnFailed;
        event Action OnInitSuccess;
        event Action<string> OnInitFailed;
        event Action<string> OnDataEncrypted;
        event Action<string> OnAppographyDataFetched;
    }
}
