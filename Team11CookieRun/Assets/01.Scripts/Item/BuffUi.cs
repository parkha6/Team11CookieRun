using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BuffUi : MonoBehaviour
{
    private float timer;
    private Coroutine coroutine;
    [SerializeField] private Image coolTimeImage;
    public float duration;
    public Item.ItemType type;

    private void OnEnable()
    {
        StartCoolDown();
    }

    public void StartCoolDown()
    {
        coroutine = StartCoroutine(CoolDown(duration));      
    }

    public void StopCooldown()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            Destroy(this.gameObject);
        }
    }


    IEnumerator CoolDown(float duration)
    {
        coolTimeImage.fillAmount = 0f;
        timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            coolTimeImage.fillAmount = timer / duration;
            yield return null;
        }
        coroutine = null;
        Destroy(this.gameObject);
    }
}
