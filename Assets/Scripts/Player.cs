using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void HpChanged();
    public HpChanged hpChanged;

    private float HP;
    private float MaxHP = 100f;
    [SerializeField]
    private float speed = 2f;
    private float jumpPower = 3500f;

    private float recoverTimer = 0f;
    private float focusTimer = 0f;
    private int arrowCount = 0;

    private bool isGround = true;
    private bool isJumping = false;
    private bool isDigging = false;

    private RaycastHit hit;
    private RaycastHit groundHit;
    private Ray mouseRay;
    private Vector3 desiredPos;
    private Vector3 desiredDir;
    private Rigidbody rigid;
    private Animator animator;
    private GameObject FocusAnimation;
    private GameObject Arrow;
    private GameObject temp;
    private Vector3 coinPos;

    private Coroutine DigCoroutine;
    public float GetHP { get { return HP; } }
    public float GetMaxHP { get { return MaxHP; } }

    private void Awake()
    {
        InitStat();

        InputManager.Instance.keyInput -= OnMouse;
        InputManager.Instance.keyInput -= OnKeyBoard;

        InputManager.Instance.keyInput += OnMouse;
        InputManager.Instance.keyInput += OnKeyBoard;

        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        desiredPos = GameManager.Instance.GetStartPos;
        FocusAnimation = GameObject.Find("Focus");
        FocusAnimation.SetActive(false);
        Arrow = Resources.Load<GameObject>("Arrow");
    }
    private void FixedUpdate()
    {
        CheckGround();
        Movement();
        RecoverHP();
    }
    private void Movement()
    {
        if (desiredPos.x != transform.position.x && desiredPos.z != transform.position.z && isDigging == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPos, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDir), Time.deltaTime * 8f);
            animator.SetBool("IsWalk", true);
        }
        else animator.SetBool("IsWalk", false);
    }
    private void OnMouse()
    {
        if(Input.GetMouseButton(1) || Input.GetMouseButtonDown(0))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit))
            {
                if(Input.GetMouseButton(1))
                {
                    desiredPos = hit.point;
                    desiredPos.y = transform.position.y;
                    desiredDir = desiredPos - transform.position;
                }
                else if(isDigging == false && Input.GetMouseButtonDown(0) && isGround)
                {
                    if(hit.transform.tag == "Ground")
                    {
                        desiredPos = hit.point;
                        desiredPos.y = transform.position.y;
                        desiredDir = desiredPos - transform.position;
                        DigCoroutine = StartCoroutine(Dig(hit.transform.gameObject));
                    }
                }
            }
        }
    }
    private void OnKeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false && isGround)
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Force);
            animator.SetBool("IsJump", true);
            isJumping = true;
        }
        if(Input.GetKey(KeyCode.Q))
        {
            Debug.Log("Focusing!!");
            animator.SetBool("IsFocus", true);
            FindCoinFocus();
            FocusAnimation.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Q))
        {
            animator.SetBool("IsFocus", false);
            FocusAnimation.SetActive(false);
            ReleaseFocus();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetHP(-10);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.LoadMainScene();
        }
    }
    private void FindCoinFocus()
    {
        focusTimer += Time.deltaTime;
        if(focusTimer > 1f)
        {
            SetHP(-20);
            arrowCount++;
            if(arrowCount > GameManager.Instance.TotalCoinCount)
            {
                arrowCount = GameManager.Instance.TotalCoinCount;
            }
            if (GameManager.Instance.TotalCoinCount > 0)
            {
                temp = Instantiate(Arrow, transform);
                coinPos = GameManager.Instance.GetCoinList[arrowCount - 1].transform.position;
                coinPos.y = transform.position.y;
                temp.transform.LookAt(coinPos);
                temp.SetActive(true);
            }
            focusTimer = 0f;
        }

    }
    private void ReleaseFocus()
    {
        for(int i = 4; i < arrowCount + 4; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        focusTimer = 0f;
        arrowCount = 0;
    }
    private void CheckGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.3f, Color.red);
        if(Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.3f))
        {
            if (groundHit.transform.tag == "Ground" || groundHit.transform.tag == "Rock")
            {
                animator.SetBool("IsJump", false);
                isJumping = false;
                isGround = true;
            }
        }
        else
        {
            isGround = false;
        }
    }
    private IEnumerator Dig(GameObject block)
    {
        if(isDigging)
        {
            StopCoroutine(DigCoroutine);
        }
        transform.rotation = Quaternion.LookRotation(desiredDir);
        while (true)
        {
            if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                isDigging = false;

                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Dig"))
                {
                    animator.Play("Idle", 0);
                }
                StopCoroutine(DigCoroutine);
                break;
            }
            if(Mathf.Abs(transform.position.x - desiredPos.x) <= 1  && Mathf.Abs(transform.position.z - desiredPos.z) <= 1)
            {
                desiredPos = transform.position;
                if(isDigging == false)
                {
                    animator.SetTrigger("IsDigging");
                    isDigging = true;
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dig") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    block.GetComponent<CubeDestroy>().OpenAndDestroy();
                    SetHP(-5);
                    isDigging = false;
                    StopCoroutine(DigCoroutine);
                    break;
                }
            }
            yield return null;
        }
    }
    private void InitStat()
    {
        HP = MaxHP;
    }
    public void SetHP(int value)
    {
        if(HP <= 100 && HP >= 0)
        {
            HP += value;
            hpChanged();
        }
    }
    private void RecoverHP()
    {
        if (rigid.velocity.magnitude == 0f && HP < 100 && isDigging == false)
        {
            recoverTimer += Time.deltaTime;
            if (recoverTimer > 1f)
            {
                SetHP(10);
                recoverTimer = 0f;
                Debug.Log("Recover");
            }
        }
        else recoverTimer = 0f;
    }
}
