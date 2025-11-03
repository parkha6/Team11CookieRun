using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    public int damage = 10; // 장애물 충돌시 피해량
    public ObstacleType type = ObstacleType.Spike_Under;
    public float value = 0f;
    public float duration = 5f;
    public Animator animator;
    public string destroyTrigger = "Destroy";
    public float destroyDuration = 1f;

    private bool isDestroyed = false;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        
        if (_collider == null)
        {
            Debug.LogError("collider 설정이 되지 않았습니다.");
        }

        if (animator == null && (type == ObstacleType.Drone))
        {
            animator = GetComponent<Animator>();
        }

        // 장애물 collider에 isTrigger 꼭 설정하기
    }

    private void OnEnable()
    {
        isDestroyed = false;

        if (_collider != null)
        {
            _collider.enabled = true;
        }

        if (animator != null && (type == ObstacleType.Drone))
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (isDestroyed)
            return;

        if(collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            if(player.IsInvincible == false)
            {
                ApplyEffect(player);
                player.TakeDamage(damage);
                if(type == ObstacleType.Drone)
                {
                    isDestroyed = true;
                    StartCoroutine(DroneDestroy());
                }
            }
        }
    }

    private IEnumerator DroneDestroy()
    {
        if (_collider != null)
            _collider.enabled = false;

        if (animator != null && !string.IsNullOrEmpty(destroyTrigger))
        {
            animator.SetTrigger(destroyTrigger);
        }
        else
        {
            Debug.LogError($"드론 파괴 애니메이터가 설정되지 않았습니다.");
        }

        yield return new WaitForSeconds(destroyDuration);

        gameObject.SetActive(false);
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
