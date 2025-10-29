using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : SingletonManager<GameManager>
{
    private void Awake()//시작시점에 필요한 변수를 로드하게 만들었음.
    { UIManager.Instance.LoadKey(); }
}
