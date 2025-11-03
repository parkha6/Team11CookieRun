using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    public int damage = 10; // 장애물 충돌시 피해량
    public ObstacleType type = ObstacleType.Spike_Under;
    public float value = 0f;
    public float duration = 5f;

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
        if(collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            if(player.IsInvincible == false)
            {
                ApplyEffect(player);
                player.TakeDamage(damage);
            }
        }
    }

    void ApplyEffect(Player player)
    {
        switch (type)
        {
            case ObstacleType.Drone: player.ApplySlow(value, duration);
                break;
        }
    }
}
