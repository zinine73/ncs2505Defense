using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoTower : MonoBehaviour
{
    [SerializeField] Image imageTower;      // 타워 이미지
    [SerializeField] TMP_Text textLevel;    // 타워 레벨
    [SerializeField] TMP_Text textDamage;   // 타워 공격력
    [SerializeField] TMP_Text textRate;     // 공격 속도
    [SerializeField] TMP_Text textRange;    // 공격 범위
    [SerializeField] TowerAttackRange towerAttackRange; // 공격 범위 표시
    [SerializeField] TMP_Text textBtnUpgrade; // 업그레이드 비용
    [SerializeField] Button buttonUpgrade;  // 업그레이드 버튼
    [SerializeField] TMP_Text textBtnSell; // 판매 비용
    [SerializeField] Button buttonSell; // 판매 버튼
    [SerializeField] ToastMessage toastMsg; // 토스트 메시지
    TowerWeapon currentTower;               // 현재 타워

    void Start()
    {
        // 시작했을 때는 패널이 꺼져 있어야 한다
        OffPanel();
    }

    void Update()
    {
        // esc키가 눌리면 패널을 끄자
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerData();
        // 공격범위 이미지 켜기
        towerAttackRange.OnAttackRange(
            currentTower.transform.position,
            currentTower.Range);
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
        // 공격범위 이미지 끄기
        towerAttackRange.OffAttackRange();
    }

    void UpdateTowerData()
    {
        // 타워 정보 표시
        imageTower.sprite = currentTower.TowerSprite;
        textLevel.text = $"Level : {currentTower.Level}";
        textDamage.text = $"Damage : {currentTower.Damage}";
        textRate.text = $"Rate : {currentTower.Rate}";
        textRange.text = $"Range : {currentTower.Range}";
        textBtnUpgrade.text = $"Upgrade:\n{currentTower.CostUpgrade}";
        textBtnSell.text = $"Sell:\n{currentTower.CostSell}";

        // 더이상 업그레이드가 안되는 상황이면 버튼을 안 눌리게 처리
        buttonUpgrade.interactable = 
            currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickTowerUpgrade()
    {
        // 업그레이드 성공이면
        if (currentTower.Upgrade())
        {
            // 데이터 갱신하고
            UpdateTowerData();
            // 공격범위표시 갱신
            towerAttackRange.OnAttackRange(
                currentTower.transform.position,
                currentTower.Range);
        }
        else
        {
            // 안된다고 메시지 표시
            toastMsg.ShowToast(ToastType.MoneyUpgrade);
        }
    }

    public void OnClickTowerSell()
    {
        currentTower.Sell();
        OffPanel();
    }
}
