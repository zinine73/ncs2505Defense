using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowMousePosition : MonoBehaviour
{
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 화면의 마우스 좌표를 기준으로 게임월드 상의 좌표를 구한다
        Vector3 position = new Vector3(Input.mousePosition.x,
            Input.mousePosition.y);
        transform.position = mainCamera.ScreenToWorldPoint(position);
        // z위치는 0으로 설정
        transform.position = new Vector3(transform.position.x,
            transform.position.y, 0);
    }
}
