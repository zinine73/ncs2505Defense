using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f; // 이동 속도
    [SerializeField] float maxHP = 5.0f; // 최대 체력
    [SerializeField] int gold = 10; // 적을 처치했을 때 얻는 골드
    bool isDie; // 사망 상태
    float currentHP; // 현재 체력
    int currentIndex; // 현재 경로 인덱스
    Animator anim; // 애니메이션 제어용 애니메이터
    SpriteRenderer spriteRenderer; // 이미지 반전용 SR
    EnemyManager emi; // 자주 사용할 인스턴스 간소화

    // property
    public float MaxHP => maxHP; // 최대체력 프로퍼티
    public float CurrentHP => currentHP; // 현재체력 프로퍼티

    /// <summary>
    /// 적을 생성한 후 반드시 처음에 한번 호출
    /// </summary>
    public void Init()
    {
        // 간소화한 인스턴스 연결
        emi = EnemyManager.Instance;
        // 인덱스를 0로 시작
        currentIndex = 0;
        // 위치는 시작인덱스에서 해당하는 경로 위치로 지정
        transform.position = emi.Waypoints[currentIndex].position;
        // 애니메이터 연결
        anim = GetComponent<Animator>();
        // SpriteRenderer 연결
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 현재 체력을 최대치로 초기화
        currentHP = maxHP;
        // 살아 있는 상태로 시작
        isDie = false;
    }

    void Update()
    {
        // 이동지점 배열의 인덱스 0부터 배열크기-1까지
        if (currentIndex < emi.Waypoints.Length)
        {
            // 현재위치를 frame 처리시간비율로 계산한 속도만큼 옮겨줌
            // 즉 1개의 프레임 단위로 움직임 처리
            transform.position = Vector3.MoveTowards(
                transform.position,
                emi.Waypoints[currentIndex].position,
                moveSpeed * Time.deltaTime);
            // 현재 오브젝트가 어느 방향으로 이동하는지 검사
            // MoveTowards에서 target - current 값의 x가 0보타 크냐
            Vector3 direction = emi.Waypoints[currentIndex].position 
                - transform.position;
            // 0보다 크면 오른쪽으로 가는 것이므로 SR의 FlipX를 true
            // 위로 올라가는 경우에도 오른쪽을 보게 하자
            spriteRenderer.flipX = (direction.x > 0) || (direction.y > 0);
            // 현재위치가 이동지점의 위치라면 
            if (Vector3.Distance(emi.Waypoints[currentIndex].position,
                transform.position) == 0f)
            {
                // 배열 인덱스 +1 해서 다음 포인트로 이동하도록
                currentIndex++;
            }
        }
        else
        {
            // 목표에 도달했으므로 삭제 처리
            OnDie(true);
        }
    }

    /// <summary>
    /// 적이 Goal에 도달하거나 체력이 다해 죽을 경우 호출
    /// 내부에서는 이 적을 관리하는 매니저에서 관련된 처리를 하게 한다
    /// </summary>
    /// <param name="isArrivedGoal">골에 도착했는지 여부</param>
    public void OnDie(bool isArrivedGoal = false)
    {
        // 매니저에서 삭제 처리, 골드 추가
        emi.DestroyEnemy(this, gold, isArrivedGoal);
    }

    public void TakeDamage(float damage)
    {
        // 죽은상태에서는 더 이상 데미지를 받지 않도록
        if (isDie) return;
        // 데미지만큼 현재 체력 감소
        currentHP -= damage;
        // 체력이 0보다 작은지 검사
        if (currentHP <= 0)
        {
            // 죽은 상태로 만들고 삭제 처리
            isDie = true;
            OnDie();
        }
        else
        {
            // todo 피격 애니메이션 실행
        }
    }
}
