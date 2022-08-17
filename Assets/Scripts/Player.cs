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
    private float jumpPower = 2000f;

    private float recoverTimer = 0f;

    private bool isGround = true;
    private bool isJumping = false;

    private RaycastHit hit;
    private RaycastHit groundHit;
    private Ray mouseRay;
    private Vector3 desiredPos;
    private Vector3 desiredDir;
    private Rigidbody rigid;
    private Animator animator;
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
    }
    private void Update()
    {
        CheckGround();
        //AnimationPlay();
        RecoverHP();
        
    }
    private void Movement()
    {
        if (desiredPos.x != transform.position.x && desiredPos.z != transform.position.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredPos, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDir), Time.deltaTime * 3f);
            animator.SetBool("IsWalk", true);
        }
        else animator.SetBool("IsWalk", false);
    }
    private void OnMouse()
    {
        if(Input.GetMouseButton(1))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit))
            {
                desiredPos = hit.point;
                desiredPos.y = transform.position.y;
                desiredDir = desiredPos - transform.position;
            }
        }
        Movement();
    }
    private void OnKeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false && isGround)
        {
            Debug.Log("Jump");
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Force);

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Cheat HP");
            SetHP(-10);
        }
    }
    private void CheckGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.15f, Color.red);
        if(Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.15f))
        {
            if (groundHit.transform.tag == "Ground")
            {
                animator.SetBool("IsJump", false);
                isGround = true;
            }
        }
        else
        {
            isGround = false;
            animator.SetBool("IsJump", true);
        }
    }
    private void AnimationPlay()
    {
        if(isJumping)
        {
            animator.SetBool("IsJump", false);
        }
        else animator.SetBool("IsJump", true);
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
        if (rigid.velocity.magnitude == 0f && HP < 100)
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
