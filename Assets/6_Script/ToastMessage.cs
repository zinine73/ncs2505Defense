using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ToastType // 메시지 종류 설정
{
    Money,  // 골드 모자랄 때
    Build   // 건설 불가능 할 때
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
            case ToastType.Money:
                toastMsg.text = "Not enough money";
                break;
            case ToastType.Build:
                toastMsg.text = "Invalid build tower";
                break;
        }
        tmpAlpha.FadeOut();
    }
}
