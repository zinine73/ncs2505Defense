using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance; // 싱글톤 인스턴스
    [SerializeField] float maxHP = 20.0f; // 최대 체력
    [SerializeField] int currentGold = 100; // 현재 골드
    [SerializeField] Image imageRed; // 데미지 받았을 때 화면 깜박임
    [SerializeField] GameObject gameoverUI; // 게임오버 표시
    float currentHP; // 현재 체력

    public float MaxHP => maxHP; // 최대 체력 프로퍼티
    public float CurrentHP => currentHP; // 현재 체력 프로퍼티
    public int CurrentGold
    {
        get => currentGold;
        // 음수로 만드는 경우 대비
        set => currentGold = Mathf.Max(0, value);
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // 현재 체력을 최대체력으로 초기화
        currentHP = maxHP;
        // 게임오버UI는 끄고 시작
        gameoverUI.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        // 데미지의 양 만큼 체력 감소시키고
        currentHP -= damage;
        // 돌아가는 코루틴이 있다면 멈추고
        StopCoroutine(HitAlphaAnimation());
        // 체력이 0 이하가 되면
        if (currentHP <= 0)
        {
            // 게임오버 표시
            gameoverUI.SetActive(true);
            // 화면깜박임 도중일 수 있으므로 알파값을 0으로 만든다
            Color color = imageRed.color;
            color.a = 0;
            imageRed.color = color;
            // 게임의 시간을 멈춰서 게임 진행이 안되게 한다
            Time.timeScale = 0f;
        }
        else
        {
            // 화면 깜박이는 코루틴 실행
            StartCoroutine(HitAlphaAnimation());
        }
    }

    IEnumerator HitAlphaAnimation()
    {
        // 이미지의 칼라값을 얻어와서
        Color color = imageRed.color;
        // 알파값만 40%로 설정
        color.a = 0.4f;
        imageRed.color = color;
        // 알파값이 0 이상이면
        while (color.a >= 0f)
        {
            // 조금씩 알파값을 감소시킨다
            color.a -= Time.deltaTime;
            imageRed.color = color;
            // 코루틴 반복
            yield return null;
        }
    }
}
