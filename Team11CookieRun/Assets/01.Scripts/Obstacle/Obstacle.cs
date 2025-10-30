using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    public int damage = 10; // 장애물 충돌시 피해량
    public ObstacleType type = ObstacleType.Spike_Under;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        
        if (_collider == null)
        {
            Debug.LogError("collider 설정이 되지 않았습니다.");
        }

        // 장애물 collider에 isTrigger 꼭 설정하기
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        /*플레이어와 충돌 했을 때
            플레이어의 체력이 감소하는 함수(아마 플레이어 클래스 내부에 존재할 듯)*/
    }


}
