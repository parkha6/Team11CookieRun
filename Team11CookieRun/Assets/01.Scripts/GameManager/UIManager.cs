using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonManager<UIManager>
{
    const string LevelKey = "Level";
    const string highScoreKey = "High Score";
    const string expKey = "Current Exp";
    const string hpKey = "Current Hp";

    int playerLevel = 1; //플레이어 레벨
    int Exp { get { return 100 * playerLevel; } }//총 경험치 계산
    int currentExp = 0;//현재 Exp
    internal int CurrentExp
    {
        get { return currentExp; }
        set
        {
            if (value <= 0)
            { value = 0; }
            currentExp = value;
        }
    }
    int Hp { get { return 100 * playerLevel; } }//총 hp.
    int currentHp = 100;//현재 Hp
    internal int CurrentHp
    {
        get { return currentHp; }
        set
        {
            if (value <= 0)
            { value = 0; }
            currentHp = value;
        }
    }
    int score = 0; //점수
    int highestScore = 0;//최고 점수 
    private void Awake()
    {
        LoadKey();
    }
    private void Update()
    {
        if (CurrentExp >= Exp)
        { playerLevel = LevelUp(); }
    }
    void LoadKey()
    {//키값 불러오기  
    }
    internal int LevelUp()
    { return ++playerLevel; }

}
