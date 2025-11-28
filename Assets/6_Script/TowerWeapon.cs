using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState // 타워 상태
{
    SearchTarget,   // 적을 찾기
    AttackToTarget  // 적을 공격 중
}

public class TowerWeapon : MonoBehaviour
{
    [SerializeField] TowerTemplate towerTemplate; // 타워정보
    [SerializeField] GameObject projectilePrefab; // 발사체 프리펩
    [SerializeField] Transform spawnPoint; // 발사체 생성 위치
    WeaponState weaponState = WeaponState.SearchTarget; // 타워상태 저장 변수
    Transform attackTarget = null; // 공격 목표
    SpriteRenderer spriteRenderer; // 타워 이미지 변경용
    int level = 0; // 타워 레벨

    #region property
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int CostUpgrade => Level < MaxLevel ? 
        towerTemplate.weapon[level + 1].cost: 0;
    public int CostSell => towerTemplate.weapon[level].sell;
     
    #endregion

    public void Init()
    {
        // 이미지 변경용 랜더러 연결
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 적 찾기 상태로 초기화
        ChangeState(WeaponState.SearchTarget);
    }

    void ChangeState(WeaponState newState)
    {
        // 이름을 코루틴 파라미터로 설정할 수 있는 것을 이용해서 현재 코루틴 종료
        StopCoroutine(weaponState.ToString());
        // 상태 바꾸기
        weaponState = newState;
        // 새로운 코루틴 시작
        StartCoroutine(weaponState.ToString());
    }

    void Update()
    {
        // 공격 중이면
        if (attackTarget != null)
        {
            // 타워돌리기(위를 향하고 있는 스프라이트이므로 up)
            transform.up = attackTarget.position - transform.position;
        }
    }

    IEnumerator SearchTarget()
    {
        while (true)
        {
            // 가장 가까운 거리를 찾기 위해 가장 큰 수로 먼저 초기화
            float closestDistance = Mathf.Infinity;
            // 리스트에 있는 모든 오브젝트 검사
            foreach (var item in EnemyManager.Instance.EnemyList)
            {
                // 두 오브젝트 사이의 거리 측정
                float distance = Vector3.Distance(item.transform.position, 
                    transform.position);
                // 공격 사정거리 안에 있으면서 가장 긴 거리보다 작으면
                if ((distance <= towerTemplate.weapon[level].range) 
                    && (distance <= closestDistance))
                {
                    // 현재 거리를 최단 거리로 지정
                    closestDistance = distance;
                    // 공격 목표를 현재 오브젝트로 지정
                    attackTarget = item.transform;
                }
            }
            // 공격목표가 null이 아니면
            if (attackTarget != null)
            {
                // 공격 상태로 변경
                ChangeState(WeaponState.AttackToTarget);
            }
            // 코루틴으로 계속 적 찾기
            yield return null;
        }
    }
    IEnumerator AttackToTarget()
    {
        while (true)
        {
            // 공격 목표가 사라지면
            if (attackTarget == null)
            {
                // 적찾기 상태로 변경
                ChangeState(WeaponState.SearchTarget);
                // 바로 while문에서 빠져나오기
                break;
            }
            // 적과의 거리 측정
            float distance = Vector3.Distance(attackTarget.position,
                transform.position);
            // 거리가 공격범위를 벗어나면
            if (distance > towerTemplate.weapon[level].range)
            {
                // 공격목표를 없애고
                attackTarget = null;
                // 적찾기 상태로 변경
                ChangeState(WeaponState.SearchTarget);
                // 바로 while문에서 빠져나오기
                break;
            }
            // 발사간격만큼 기다린 후 다시 공격
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
            // 발사체 생성
            SpawnProjectile();
        }
    }
    void SpawnProjectile()
    {
        // 발사체프리펩에서 발사체 생성
        GameObject clone = Instantiate(projectilePrefab,
            spawnPoint.position, Quaternion.identity, transform);
        // 발사체에 공격 목표 지정
        clone.GetComponent<Projectile>().SetTarget(attackTarget, 
            towerTemplate.weapon[level].damage);
    }

    public bool Upgrade()
    {
        // 가진 돈이 (현재 레벨보다 1큰)비용보다 적은지 검사
        if (PlayerManager.Instance.CurrentGold <
            towerTemplate.weapon[level + 1].cost)
        {
            // 실패 리턴
            return false;
        }

        // 레벨 올리고
        level++;
        // 스프라이트 이미지도 바꾸고
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        // 골드에서 건설비용 차감하고
        PlayerManager.Instance.CurrentGold -= 
            towerTemplate.weapon[level].cost;
        // 성공 리턴
        return true;
    }

    public void Sell()
    {
        // 판매 비용 추가하고
        PlayerManager.Instance.CurrentGold += 
            towerTemplate.weapon[level].sell;
        // 타워 오브젝트 지우기
        Destroy(gameObject);
    }
}
