using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f; // 이동 속도
    int currentIndex; // 현재 경로 인덱스
    EnemyManager emi; // 자주 사용할 인스턴스 간소화

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
        //transform.position = ;
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
            OnDie();
        }
    }

    /// <summary>
    /// 적이 Goal에 도달하거나 체력이 다해 죽을 경우 호출
    /// 내부에서는 이 적을 관리하는 매니저에서 관련된 처리를 하게 한다
    /// </summary>
    public void OnDie()
    {
        // 매니저에서 삭제 처리
        emi.DestroyEnemy(this);
    }
}
