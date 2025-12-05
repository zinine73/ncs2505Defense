using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // 유니티 에디터 메뉴에 나오게 된다
public class TowerTemplate : ScriptableObject // 스크립터블 오브젝트
{
    public GameObject towerPrefab; // 타워생성을 위한 프리펩
    public GameObject followTowerPrefab; // 임시 타워 프리펩
    public Weapon[] weapon; // 레벨별 타워(무기) 정보

    [System.Serializable] // 직렬화
    public struct Weapon
    {
        public Sprite   sprite; // 타워 이미지
        public float    damage; // 공격력
        public float    slow;   // 감속 퍼센트(0.2 = 20%)
        public float    rate;   // 공격 속도
        public float    range;  // 공격 범위
        public int      cost;   // 필요 골드 (0:건설, 1~:업그레이드)
        public int      sell;   // 판매 골드
    }
}
