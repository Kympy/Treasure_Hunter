using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    private Slider hp;
    private TextMeshProUGUI CoinText;
    private Coroutine my;
    private Image adre;
    private Sprite off;
    private Sprite on;
    public void Awake()
    {
        hp = GameObject.Find("HPBar").GetComponent<Slider>();
        CoinText = GameObject.Find("Coin").GetComponent<TextMeshProUGUI>();
        adre = GameObject.Find("Adr").GetComponent<Image>();
        on = Resources.Load<Sprite>("On");
        off = Resources.Load<Sprite>("Off");
        adre.sprite = off;
    }
    public void UpdateBar()
    {
        my = StartCoroutine(UpdateEffect());
    }
    public void UpdateCoin()
    {
        CoinText.text = "LEFT COINS : " + GameManager.Instance.TotalCoinCount;
    }
    public void ChangeAdr(bool IsOn)
    {
        if(IsOn)
        {
            adre.sprite = on;
        }
        else
        {
            adre.sprite = off;
        }
    }
    public IEnumerator UpdateEffect() // Bar value Decrease Effect
    {
        while(true)
        {
            hp.value = Mathf.Lerp(hp.value, GameManager.Instance.MyPlayer.GetHP, 1f * Time.deltaTime);
            if(hp.value == GameManager.Instance.MyPlayer.GetHP)
            {
                StopCoroutine(my);
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
