using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoTower : MonoBehaviour
{
    [SerializeField] Image imageTower;
    [SerializeField] TMP_Text textLevel;
    [SerializeField] TMP_Text textDamage;
    [SerializeField] TMP_Text textRate;
    [SerializeField] TMP_Text textRange;
    [SerializeField] TowerAttackRange towerAttackRange; // 공격 범위 나타내는 이미지
    TowerWeapon currentTower;

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
        textLevel.text = $"Level : {currentTower.Level}";
        textDamage.text = $"Damage : {currentTower.Damage}";
        textRate.text = $"Rate : {currentTower.Rate}";
        textRange.text = $"Range : {currentTower.Range}";
    }
}
