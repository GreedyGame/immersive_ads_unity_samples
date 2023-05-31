using UnityEngine;

namespace PubScale.SdkOne.NativeAds.Hightower
{

    [CreateAssetMenu(fileName = "GameSettings", menuName = "GameData/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public AudioClip[] gameClips;
        public AudioClip bgMusic;
        public PoolObjects[] ObjectPools;
        //Add Data here

    }
}