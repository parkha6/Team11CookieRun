using System.Collections;
using UnityEngine;
/// <summary>
/// 이미지의 색상을 변하게 하는 클래스
/// </summary>
public class ChangeImageColor : MonoBehaviour
{
    /// <summary>
    /// 순차적으로 전환할 모든 스프라이트
    /// </summary>
    [Tooltip("순차적으로 전환할 모든 스프라이트")]
    [SerializeField] private SpriteRenderer[] allSprites;
    /// <summary>
    /// 이미지가 변하는 시간
    /// </summary>
    [Tooltip("이미지가 변하는 시간")]
    [SerializeField] float fadeDuration = 1f;
    /// <summary>
    /// 아래 레이어의 순서
    /// </summary>
    [Tooltip("아래 레이어의 순서")]
    [SerializeField] byte lowerLayer;
    /// <summary>
    /// 위 레이어의 순사
    /// </summary>
    [Tooltip("위 레이어의 순서")]
    [SerializeField] byte upperLayer;
    /// <summary>
    /// 시간재기용
    /// </summary>
    float timer = 0f;
    /// <summary>
    /// 이미지 투명 컬러
    /// </summary>
    Color invisibleImage = new Color(1f, 1f, 1f, GmConst.invisibleAlphaRatio);
    /// <summary>
    /// 이미지 불투명 컬러
    /// </summary>
    Color visibleImage = new Color(1f, 1f, 1f, GmConst.visibleAlphaRatio);
    /// <summary>
    /// 스타트에서 코루틴 루프. 근데 코루틴이 뭐지?
    /// </summary>
    void Start()
    {
        if (allSprites != null && allSprites.Length > 1)
        { StartCoroutine(LoopImageSequence()); }
        else
        { Debug.Log("배열이 할당되지 않았거나 갯수가 너무 적습니다."); }
    }
    /// <summary>
    /// AllSprites의 모든 이미지를 세팅한 뒤 코루틴을 돌려서 하나씩 돌린다. 근데 코루틴이 뭐지.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoopImageSequence()
    {
        for (byte i = 0; i < allSprites.Length; ++i)
        {
            allSprites[i].color = (i == 0) ? visibleImage : invisibleImage;
            allSprites[i].sortingOrder = lowerLayer;
        }
        byte currentIndex = 0;
        while (true)//?
        {
            SpriteRenderer spriteA = allSprites[currentIndex];
            byte nextIndex = (byte)((currentIndex + 1) % allSprites.Length);
            SpriteRenderer spriteB = allSprites[nextIndex];
            yield return StartCoroutine(FadeTransition(spriteA,spriteB));
            currentIndex = nextIndex;
        }
    }
    /// <summary>
    /// spriteA위에 spriteB를 서서히 덮어씌운다.
    /// </summary>
    /// <param name="spriteA"></param>
    /// <param name="spriteB"></param>
    /// <returns></returns>
    IEnumerator FadeTransition(SpriteRenderer spriteA, SpriteRenderer spriteB)
    {
        spriteA.sortingOrder = lowerLayer;
        spriteB.sortingOrder = upperLayer;
        spriteB.color = invisibleImage;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            spriteB.color = Color.Lerp(invisibleImage, visibleImage, t);
            yield return null;
        }
        spriteA.color = invisibleImage;
        spriteB.color = visibleImage;
        spriteA.sortingOrder = lowerLayer;
    }
}
