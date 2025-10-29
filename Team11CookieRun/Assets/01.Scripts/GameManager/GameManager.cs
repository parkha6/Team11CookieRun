using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static GameManager instance; //¿œ¥‹ ≥°.
    private void Awake()
    { instance = this; } 
}
