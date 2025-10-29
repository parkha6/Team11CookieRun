using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Startbutton;
        
    public GameObject ExitButton;

    public void Start()
    {
        if (ExitButton == null)
        {
            return;
        }

        if (Startbutton == null)
        {

          
        }

        ExitButton.gameObject.SetActive(true);
    }

    public void SetRestart()
    {
        

    }

    public void UpdateStartButton(int StartButton)
    {

        StartButton.ToString();
    }
}