using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static GameManager instance; //�ϴ� ��.
    private void Awake()
    { instance = this; } 
}
