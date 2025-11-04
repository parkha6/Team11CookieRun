using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    /// <summary>
    /// 이미지들이 출력될 렌더러
    /// </summary>
    [Tooltip("이미지들이 출력될 렌더러")]
    [SerializeField] SpriteRenderer mainSprite;
    /// <summary>
    /// 출력할 이미지를 넣을 배열
    /// </summary>
    [Tooltip("출력할 이미지를 넣을 배열")]
    [SerializeField] Sprite[] sprites;
    /// <summary>
    /// 아이템의 인덱스 넘버
    /// </summary>
    byte indexNum = 0;
    /// <summary>
    /// 아이템의 인덱스 넘버
    /// </summary>
    byte IndexNum
    {
        get { return indexNum; }
        set
        {
            if (value < 0)
            { value = 0; }
            else if (value > 255)
            { value = 255; }
            indexNum = value;
        }
    }
    /// <summary>
    /// 생성될 때 랜덤으로 스프라이트 배치
    /// </summary>
    private void Awake()
    {
        IndexNum = (byte)Random.Range(0f, sprites.Length);
        mainSprite.sprite = sprites[indexNum];
    }
}
