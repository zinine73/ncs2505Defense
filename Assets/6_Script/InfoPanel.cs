using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] TMP_Text textPlayerHP; // 유저 체력
    [SerializeField] TMP_Text textGold; // 골드

    void Update()
    {
        // 체력 표시
        textPlayerHP.text = $"{PlayerManager.Instance.CurrentHP} / {PlayerManager.Instance.MaxHP}";
        // 골드 표시
        textGold.text = $"{PlayerManager.Instance.CurrentGold}";
    }
}
