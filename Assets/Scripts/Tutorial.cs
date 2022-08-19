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
        textBuffer = "지금부터 튜토리얼을 시작하겠습니다!";
        animator.SetBool("IsTalk", true);
        myCoroutine = StartCoroutine(TextView());
    }
    private void MouseControl()
    {
        talking = true;
        textBuffer = "먼저, 마우스 조작에 대해 알아볼게요.\n마우스 휠을 클릭하고 마우스를 움직여보세요.";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", true);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckMouseRotate());
    }
    private void Clear()
    {
        talking = true;
        textBuffer = "잘했습니다!";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", true);
        myCoroutine = StartCoroutine(TextView());
    }
    private void MoveTuto()
    {
        talking = true;
        textBuffer = "이제 우클릭을 해서 이동해 보세요!";
        animator.SetBool("IsTalk", true);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", false);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckMove());
    }
    private void ClickTuto()
    {
        talking = true;
        textBuffer = "잘했습니다! 이제, 좌클릭으로 땅을 파보세요!";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", true);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckClick());
    }
    private void FocusTuto()
    {
        talking = true;
        textBuffer = "잘했습니다! Q를 누르면 특수능력을 사용할 수 있습니다.\n1초마다 숨겨진 보물의 방향을 1개씩 화살표로 알 수 있어요!\n대신 체력을 소모하니 주의하세요!";
        animator.SetBool("IsTalk", false);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", true);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckQ());
    }
    private void ItemTuto()
    {
        talking = true;
        textBuffer = "별 아이템을 먹고, R키를 눌러 사용하면 10초 동안 체력소모가 없습니다!";
        animator.SetBool("IsTalk", true);
        animator.SetBool("IsTalk2", false);
        animator.SetBool("IsClear", false);
        myCoroutine = StartCoroutine(TextView());
        StartCoroutine(CheckEnter());
    }
    private void TutoClear()
    {
        talking = true;
        textBuffer = "이제 엔터를 누르면 게임을 시작할게요.";
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
