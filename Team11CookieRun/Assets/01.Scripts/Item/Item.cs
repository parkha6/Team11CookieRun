using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Coin, Heal, Invincible, Slow }
    public ItemType itemType;

    //아이템 수치 설정
    public float value = 0f;     // 코인 점수 or 회복량(%)
    public float duration = 5f;  // 무적 지속 시간


    //아이템 사운드
    public AudioClip coinSound;
    public AudioClip healSound;
    public AudioClip invincibleSound;
    public AudioClip slowSound;

    //볼륨설정
    [Range(0f, 1f)] public float coinVolume = 1f;
    [Range(0f, 1f)] public float healVolume = 1f;
    [Range(0f, 1f)] public float invincibleVolume = 1f;
    [Range(0f, 1f)] public float slowVolume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        Player player = collision.GetComponent<Player>();
        if (player != null)
            ApplyEffect(player);

        PlaySound();
        Destroy(gameObject);
        
    }

    void ApplyEffect(Player player)
    {
        switch (itemType)
        {
            case ItemType.Coin: player.AddScore((int)value); break;
            case ItemType.Heal: player.HealPercent(value); break;
            case ItemType.Invincible: player.ActivateInvincibility(duration); break;
            case ItemType.Slow: /*player.ApplySlow(value, duration);*/ break;
        }
    }

    void PlaySound()
    {
        AudioClip clip = null;
        float vol = 1f;

        switch (itemType)
        {
            case ItemType.Coin: clip = coinSound; break;
            case ItemType.Heal: clip = healSound; break;
            case ItemType.Invincible: clip = invincibleSound; break;
            case ItemType.Slow: clip = slowSound; break;
        }

        if (clip != null)
        {
            GameObject tmp = new GameObject("TempAudio");
            tmp.transform.position = Camera.main.transform.position; // 2D 소리용
            AudioSource src = tmp.AddComponent<AudioSource>();
            src.clip = clip;
            src.volume = audioSource.volume * vol; // Inspector 볼륨과 아이템 볼륨 적용
            src.spatialBlend = 0f; // 2D
            src.Play();
            Destroy(tmp, clip.length);
        }
    }
}