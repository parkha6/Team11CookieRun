using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Coin, Heal, Invincible, Slow}
    public ItemType itemType;

    //아이템 수치 설정
    public float value = 0f;     // 코인 점수 or 회복량(%)
    public float duration = 5f;  // 무적 지속 시간

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
                ApplyEffect(player);

            Destroy(gameObject); // 아이템 먹으면 사라짐
        }
    }

    void ApplyEffect(Player player)
    {
        switch (itemType)
        {
            case ItemType.Coin:
                player.AddScore((int)value);
                break;

            case ItemType.Heal:
                player.HealPercent(value);
                break;

            case ItemType.Invincible:
                player.ActivateInvincibility(duration);
                break;

            case ItemType.Slow:
                //player.ApplySlow(value, duration);
                break;
        }
    }
}
