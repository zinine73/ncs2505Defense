using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f; // 이동 속도
    int currentIndex; // 현재 경로 인덱스

    public void Init()
    {
        // 인덱스를 0로 시작
        currentIndex = 0;
        // 위치는 시작인덱스에서 해당하는 경로 위치로 지정
        //transform.position = ;
    }
}
