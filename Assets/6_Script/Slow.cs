using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    TowerWeapon towerWeapon;

    void Start()
    {
        towerWeapon = GetComponentInParent<TowerWeapon>();    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 적이 아니면 리턴
        if (!collision.CompareTag("ENEMY")) return;
        // 적일 경우 slow 값을 적에게 적용
        collision.GetComponent<Enemy>().Slow = towerWeapon.Slow;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 적이 아니면 리턴
        if (!collision.CompareTag("ENEMY")) return;
        // 적일 경우 slow 값을 원래대로
        collision.GetComponent<Enemy>().Slow = 0f;
    }

}
