using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonManager<UIManager>
{
    const string LevelKey = "Level";
    const string highScoreKey = "High Score";
    const string expKey = "Current Exp";
    const string hpKey = "Current Hp";

    int playerLevel = 1; //�÷��̾� ����
    int Exp { get { return 100 * playerLevel; } }//�� ����ġ ���
    int currentExp = 0;//���� Exp
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
    int Hp { get { return 100 * playerLevel; } }//�� hp.
    int currentHp = 100;//���� Hp
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
    int score = 0; //����
    int highestScore = 0;//�ְ� ���� 
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
    {//Ű�� �ҷ�����  
    }
    internal int LevelUp()
    { return ++playerLevel; }
}
