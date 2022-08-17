using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    private Slider hp;

    public override void Awake()
    {
        base.Awake();
        hp = GameObject.Find("HPBar").GetComponent<Slider>();
    }

    public void UpdateBar()
    {
        StartCoroutine(UpdateEffect());
    }
    public IEnumerator UpdateEffect() // Bar value Decrease Effect
    {
        while(true)
        {
            hp.value = Mathf.Lerp(hp.value, GameManager.Instance.MyPlayer.GetHP, 1f * Time.deltaTime);
            if(hp.value == GameManager.Instance.MyPlayer.GetHP)
            {
                StopCoroutine(UpdateEffect());
                break;
            }
            yield return null;
        }
    }
    public void InitUIManager()
    {
        GameManager.Instance.MyPlayer.hpChanged += UpdateBar;
        hp.maxValue = GameManager.Instance.MyPlayer.GetMaxHP;
        hp.value = GameManager.Instance.MyPlayer.GetHP;
    }
}
