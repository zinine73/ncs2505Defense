using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance; // 싱글톤 인스턴스
    [SerializeField] GameObject enemyHPSliderPrefab; // 적 체력을 나타내는 프리펩
    [SerializeField] Transform canvasTransform; // UI를 표시할 캔버스의 transform
    [SerializeField] Transform[] waypoints; // 이동 위치 배열
    Wave currentWave; // 현재 웨이브 정보
    int currentEnemyCount; // 현재 남은 적 수
    List<Enemy> enemyList; // 생성된 적 리스트

    public Transform[] Waypoints => waypoints; // 이동위치배열 프로퍼티
    public List<Enemy> EnemyList => enemyList; // 적리스트 프로퍼티
    public int CurrentEnemyCount => currentEnemyCount; // 현재남은 적 수 프로퍼티
    public int MaxEnemyCount => currentWave.maxEnemyCount; // 현재 웨이브 적 수

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // 생성된 적 리스트 초기화
        enemyList = new List<Enemy>();
    }

    public void StartWave(Wave wave)
    {
        // 현재 웨이브 정보 전달
        currentWave = wave;
        // 현재 웨이브 최대 적 수를 현재 남은 적 수로 지정
        currentEnemyCount = currentWave.maxEnemyCount;
        // 코루틴 실행
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        // 생성한 적 숫자
        int spawnEnemyCount = 0;
        // 웨이브 정보에 있는 최대 생성 숫자에 도달할 때까지
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            int enemyIndex;
            if (currentWave.isStatic &&
                currentWave.maxEnemyCount == currentWave.enemyPrefabs.Length)
            {
                // 고정인 경우 카운트가 곧 인덱스
                enemyIndex = spawnEnemyCount;    
            }
            else
            {
                // 웨이브 정보에 있는 적 종류 중 랜덤으로 생성
                enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            }
            // 적 프레팝으로 오브젝트를 생성하고 Enemy 스크립트 연결
            Enemy enemy = Instantiate(currentWave.enemyPrefabs[enemyIndex], 
                transform).GetComponent<Enemy>();
            // 적 초기화
            enemy.Init();
            // 적을 리스트에 넣기
            enemyList.Add(enemy);
            // 적 체력 슬라이드 표시
            SpawnEnemyHPSlider(enemy);
            // 생성한 적 숫자 증가
            spawnEnemyCount++;
            // 생성 시간 기다렸다가 다음 적 생성
            float waitTime;
            if (currentWave.isStatic &&
                currentWave.maxEnemyCount == currentWave.spawnTimeStatic.Length)
            {
                waitTime = currentWave.spawnTimeStatic[enemyIndex];
            }
            else
            {
                waitTime = currentWave.spawnTime;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// 적 삭제 처리
    /// </summary>
    /// <param name="enemy">삭제해야할 오브젝트</param>
    /// <param name="gold">골도착이 아닌 경우 추가할 골드</param>
    /// <param name="isArrivedGoal">골 도착 여부</param>
    public void DestroyEnemy(Enemy enemy, int gold, bool isArrivedGoal)
    {
        // 골에 도착한 것이냐
        if (isArrivedGoal)
        {
            // 도착했다면 유저에게 데미지
            // todo 적의 공격력
            PlayerManager.Instance.TakeDamage(1);
        }
        else
        {
            // 아니면 골드 증가
            PlayerManager.Instance.CurrentGold += gold;
        }
        // 현재 적 수에서 하나 감소
        currentEnemyCount--;
        // 적리스트에서 지정한 적 지우기
        enemyList.Remove(enemy);
        // 적 오브젝트 삭제
        Destroy(enemy.gameObject);
    }

    void SpawnEnemyHPSlider(Enemy enemy)
    {
        // 슬라이더 클론 생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab, canvasTransform);
        // 크기 지정
        sliderClone.transform.localScale = Vector3.one;
        // 위치 지정
        sliderClone.GetComponent<SliderPosAuto>().Setup(enemy.transform);
        // 값 지정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy);
    }
}
