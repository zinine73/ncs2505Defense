using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    Enemy enemy;
    Slider slider;

    public void Setup(Enemy enemy)
    {
        this.enemy = enemy;
        slider = GetComponent<Slider>();
    }
    void Update()
    {
        // 슬라이더의 값은 0.0f ~ 1.0f 사이 값으로 지정
        slider.value = enemy.CurrentHP / enemy.MaxHP;
    }
}
