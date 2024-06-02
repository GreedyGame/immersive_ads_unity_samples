using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PubScale.SdkOne.NativeAds.Hightower;

public class StoreCharacterDisplay : MonoBehaviour
{

    public int CharID = -1;

    public Sprite CharCap;
    public Color CharColr;

    public int Cost;

    public TextMeshProUGUI Txt_Cost;
    public Image Lock;
    public Button Btn_Coins;
    public Button Btn_Selected;

    public TextMeshProUGUI Txt_SelectStatus;

    int IsUnlocked = 0;

    bool IsSelected = false;


    public void UpdateDisplay(UIManager uiMgr)
    {
        if (CharID == 1)
        {
            IsUnlocked = 1;
        }
        else
        {
            IsUnlocked = PlayerPrefs.GetInt(PrefsHelper.Key_PrefixCharUnlocked + CharID, 0);
        }

        int selectedCharID = PlayerPrefs.GetInt(PrefsHelper.Key_CurCharSelected, 1);

        if (selectedCharID == CharID)
        {
            IsSelected = true;
        }
        else
            IsSelected = false;


        if (IsUnlocked >= 1)
        {
            if (Btn_Coins)
                Btn_Coins.gameObject.SetActive(false);

            if (Lock)
                Lock.enabled = false;

            if (Btn_Selected)
                Btn_Selected.gameObject.SetActive(true);

            if (IsSelected)
            {
                Btn_Selected.image.color = uiMgr.ClrBtnCurrentSelected;
                Txt_SelectStatus.text = "SELECTED";
            }
            else
            {
                Btn_Selected.image.color = uiMgr.ClrBtnChoose;
                Txt_SelectStatus.text = "CHOOSE";
            }
        }
        else
        {
            Txt_Cost.text = Cost.ToString();

            int curCoins = GameManager.instance.GetCurrentCoins();

            if (curCoins >= Cost)
            {
                if (Lock)
                    Lock.enabled = false;

                if (Btn_Coins)
                    Btn_Coins.interactable = true;
            }
            else
            {
                if (Lock)
                    Lock.enabled = true;

                if (Btn_Coins)
                    Btn_Coins.interactable = false;
            }
        }
    }



}
