using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class DemoReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        PlayerPrefs.DeleteKey(PrefsHelper.Key_CurCharSelected);
        PlayerPrefs.DeleteKey(PrefsHelper.Key_CurrentScore);
        PlayerPrefs.DeleteKey(PrefsHelper.Key_offWallRewardAmount);

        PlayerPrefs.DeleteKey(PrefsHelper.Key_PrefixCharUnlocked + "2");
        PlayerPrefs.DeleteKey(PrefsHelper.Key_PrefixCharUnlocked + "3");
        PlayerPrefs.DeleteKey(PrefsHelper.Key_PrefixCharUnlocked + "4");

    }


}
