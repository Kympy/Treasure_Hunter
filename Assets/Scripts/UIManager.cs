using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : SingleTon<UIManager>
{
    private Slider hp;
    private TextMeshProUGUI CoinText;
    public override void Awake()
    {
        base.Awake();
        hp = GameObject.Find("HPBar").GetComponent<Slider>();
        CoinText = GameObject.Find("Coin").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateBar()
    {
        StartCoroutine(UpdateEffect());
    }
    public void UpdateCoin()
    {
        CoinText.text = "LEFT COINS : " + GameManager.Instance.TotalCoinCount;
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
        GameManager.Instance.coinChanged += UpdateCoin;

        hp.maxValue = GameManager.Instance.MyPlayer.GetMaxHP;
        hp.value = GameManager.Instance.MyPlayer.GetHP;
        CoinText.text = "LEFT COINS : " + GameManager.Instance.TotalCoinCount;
    }
}
