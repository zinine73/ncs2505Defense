using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ToastType // 메시지 종류 설정
{
    MoneyBuild,  // 건설 골드 모자랄 때
    MoneyUpgrade // 업그레이드 골드 모자랄 때
}

public class ToastMessage : MonoBehaviour
{
    TMP_Text toastMsg;
    TMPAlpha tmpAlpha;
    void Start()
    {
        toastMsg = GetComponent<TMP_Text>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }
    public void ShowToast(ToastType type)
    {
        switch (type)
        {
            case ToastType.MoneyBuild:
                toastMsg.text = "Not enough money for build";
                break;
            case ToastType.MoneyUpgrade:
                toastMsg.text = "Not enough money for upgrade";
                break;
        }
        tmpAlpha.FadeOut();
    }
}
