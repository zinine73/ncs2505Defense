using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f; // 이동 시간
    Transform target; // 공격 목표
    float damage = 0f; // 데미지
    public void SetTarget(Transform tr, float dmg)
    {
        // 공격 목표 지정
        target = tr;
        // 데미지 값 설정
        damage = dmg;
    }

    void Update()
    {
        // 목표가 있으면
        if (target != null)
        {
            // 방향 설정
            Vector3 direction = 
                (target.position - transform.position).normalized;
            // 이동
            transform.position += moveSpeed * Time.deltaTime * direction;
        }   
        else
        {
            // 목표가 사라지면 삭제
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 태그가 적이 아니면 리턴
        if (!collision.CompareTag("ENEMY")) return;
        // 충돌한 오브젝트가 목표가 아니면 리턴
        if (collision.transform != target) return;
        // 충돌한 적에게 데미지 주기
        collision.GetComponent<Enemy>().TakeDamage(damage);
        // 발사체 삭제
        Destroy(gameObject);
    }
}
