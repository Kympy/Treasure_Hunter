using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private string textBuffer;
    private Text message;
    private GameObject tutorialUI;
    private WaitForSeconds textTime = new WaitForSeconds(0.05f);
    private Coroutine myCoroutine;
    private Animator animator;
    private bool talking = false;
    private int order = 0;
    private bool missionClear = false;
    private void Awake()
    {
        tutorialUI = GameObject.Find("TBack");
        message = GameObject.Find("Msg").GetComponent<Text>();
        animator = GameObject.Find("Guide").GetComponent<Animator>();
        message.text = "";
    }
    private void Start()
    {
        SayHi();
    }
    private void Update()
    {
        if (missionClear && message.text == textBuffer && Input.GetKeyDown(KeyCode.Return))
        {
            tutorialUI.SetActive(false);
            Destroy(this.gameObject);
            return;
        }
        if (talking && Input.GetKeyDown(KeyCode.Return))
        {
            message.text = textBuffer;
            StopCoroutine(myCoroutine);
            talking = false;
            order++;
        }
        if(talking == false)
        {
            switch (order)
            {
                case 0: { SayHi(); break; }
                case 1: { MouseControl(); break; }
                case 2: { Clear(); break; }
                case 3: { MoveTuto(); break; }
                case 4: { ClickTuto(); break; }
                case 5: { FocusTuto(); break; }
                case 6: { ItemTuto(); break; }
                case 7: { TutoClear(); break; }
            }
        }
    }
    private void SayHi()
    {
        talking = true;
        textBuffer = "���ݺ��� Ʃ�丮���� �����ϰڽ��ϴ�!";
        animator.SetBool("IsTalk", true);
        myCoroutine = StartCoroutine(TextView());
    }
    private void MouseControl()
    {
        talking = true;
        textBuffer = "����, ���콺 ���ۿ� ���� �˾ƺ��Կ�.\n���콺 ���� Ŭ���ϰ� ���콺�� ������������.";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", true);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckMouseRotate());
    }
    private void Clear()
    {
        talking = true;
        textBuffer = "���߽��ϴ�!";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", true);
        myCoroutine = StartCoroutine(TextView());
    }
    private void MoveTuto()
    {
        talking = true;
        textBuffer = "���� ��Ŭ���� �ؼ� �̵��� ������!";
        animator.SetBool("IsTalk", true);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", false);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckMove());
    }
    private void ClickTuto()
    {
        talking = true;
        textBuffer = "���߽��ϴ�! ����, ��Ŭ������ ���� �ĺ�����!";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", true);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckClick());
    }
    private void FocusTuto()
    {
        talking = true;
        textBuffer = "���߽��ϴ�! Q�� ������ Ư���ɷ��� ����� �� �ֽ��ϴ�.\n1�ʸ��� ������ ������ ������ 1���� ȭ��ǥ�� �� �� �־��!\n��� ü���� �Ҹ��ϴ� �����ϼ���!";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", true);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckQ());
    }
    private void ItemTuto()
    {
        talking = true;
        textBuffer = "�� �������� �԰�, RŰ�� ���� ����ϸ� 10�� ���� ü�¼Ҹ� �����ϴ�!";
        animator.SetBool("IsTalk", true);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", false);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckEnter());
    }
    private void TutoClear()
    {
        talking = true;
        textBuffer = "���� ���͸� ������ ������ �����ҰԿ�.";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", true);
        animator.SetBool("IsClear", false);
        myCoroutine = StartCoroutine(TextView());
        missionClear = true;
    }
    private IEnumerator CheckMouseRotate()
    {
        Vector3 trans = Camera.main.transform.position;
        while (true)
        {
            if(message.text == textBuffer)
            {
                if (Input.GetMouseButton(2) && trans != Camera.main.transform.position)
                {
                    talking = false;
                    order++;
                    break;
                }
            }
            yield return null;
        }
        StopAllCoroutines();
    }
    private IEnumerator CheckMove()
    {
        Vector3 trans = GameManager.Instance.MyPlayer.transform.position;
        while (true)
        {
            if (message.text == textBuffer)
            {
                if (Input.GetMouseButton(1) && trans != GameManager.Instance.MyPlayer.transform.position)
                {
                    talking = false;
                    order++;
                    break;
                }
            }
            yield return null;
        }
        StopAllCoroutines();
    }
    private IEnumerator CheckClick()
    {
        while (true)
        {
            if (message.text == textBuffer)
            {
                if (GameManager.Instance.MyPlayer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dig"))
                {
                    talking = false;
                    order++;
                    break;
                }
            }
            yield return null;
        }
        StopAllCoroutines();
    }
    private IEnumerator CheckQ()
    {
        while (true)
        {
            if (message.text == textBuffer)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    talking = false;
                    order++;
                    break;
                }
            }
            yield return null;
        }
        StopAllCoroutines();
    }
    private IEnumerator CheckEnter()
    {
        while(true)
        {
            if(message.text == textBuffer)
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    talking = false;
                    order++;
                    break;
                }
            }
            yield return null;
        }
        StopAllCoroutines();
    }
    private IEnumerator TextView()
    {
        int i = 0;
        message.text = "";
        while (i < textBuffer.Length)
        {
            message.text += textBuffer[i];
            i++;
            yield return textTime;
        }
        StopCoroutine(myCoroutine);
    }
}
