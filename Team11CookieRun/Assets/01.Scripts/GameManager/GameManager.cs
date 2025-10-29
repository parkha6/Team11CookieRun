using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : SingletonManager<GameManager>
{
    private void Awake()//점수 로드 으아아 주석 깨지지마
    { UIManager.Instance.LoadKey(); }
}
