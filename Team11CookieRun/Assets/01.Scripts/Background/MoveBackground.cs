using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 배경을 움직이기 위한 스크립트.
/// </summary>
public class MoveBackground : MonoBehaviour
{
    [Tooltip("이미지를 입력하는 곳")]
    [SerializeField] Transform inputChanger;
    [Tooltip("이미지의 넓이를 입력하는 곳")]
    [SerializeField] float imageWide;
    /// <summary>
    /// 움직이는 속도를 입력하는 곳
    /// </summary>
    [SerializeField]
    byte speed = 1;
    /// <summary>
    /// 초기 위치 저장 
    /// </summary>
    Vector3 savePosition;
    private void Start()
    { savePosition = inputChanger.transform.localPosition; }
    private void Update()
    {
        if (GameManager.Instance.currentStage != GameStage.End && GameManager.Instance.currentStage != GameStage.Pause)
        {
            inputChanger.transform.localPosition += Vector3.left * speed * Time.deltaTime;
            if (inputChanger.transform.localPosition.x < savePosition.x - imageWide)
            { inputChanger.transform.localPosition = savePosition; }
        }
    }
}
