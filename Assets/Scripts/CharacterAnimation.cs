using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterAnimation : MonoBehaviour
{
    [Header("地面移动式跑步或走路")]
    public MoveState RunOrWalk;
    [Header("切换走路跑步按键")]
    public KeyCode SwitchRunWalkKey;
    [Header("跳跃按键")]
    public KeyCode JumpKey;
    [Header("固定轴")]
    public Transform Axis;

    private CharacterEventCenter characterEventCenter;
    private Animator animator;
    private BoxCheckPoint GroundCheckBox;
    private CharacterMove characterMove;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GroundCheckBox=transform.Find("GroundCheckPoint").gameObject.GetComponent<BoxCheckPoint>();
        characterMove=gameObject.GetComponent<CharacterMove>();
    }
    private void Start()
    {
        characterEventCenter=CharacterEventCenter.GetInstance();
    }
    private void Update()
    {
        CheckAndSetAnimatorParameter();
        CheckKey();
        AdjustFaceDirection_SetRelativePos();
        characterMove.SetRunOrWalk(RunOrWalk);
        characterMove.SetJumpKey(JumpKey);
        CheckDownFall();
    }
    private void CheckAndSetAnimatorParameter()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        animator.SetBool("IsOnGround", CheckIsOnGround());
        animator.SetBool("IsMoving", x * x + y * y > 0.1);
        animator.SetBool("IsRun", RunOrWalk == MoveState.Run);
        animator.SetFloat("ForwardDir", y);
        animator.SetFloat("RightDir", x);
    }
    private bool CheckIsOnGround()
    {
        return GroundCheckBox.IsOnGround;
    }

    private void AdjustFaceDirection_SetRelativePos()
    {
        if (characterEventCenter.IsFloating)
        {
            Vector3 Relative=transform.position- Axis.position;
            transform.LookAt(transform.position+Axis.forward,Axis.up);
            transform.position = Axis.position + Relative;
        }
        else
        {
            Vector3 up = Axis.up;
            Vector3 dir = Vector3.ProjectOnPlane(Camera.main.transform.forward, up);
            Vector3 OrgCengterPos = Axis.position;
            transform.LookAt(transform.position + dir, up);
            transform.position += OrgCengterPos - Axis.position;
        }

    }
    
    private void CheckKey()
    {
        if(Input.GetKeyDown(SwitchRunWalkKey))
        {
            if(RunOrWalk == MoveState.Run)
            {
                RunOrWalk = MoveState.Walk;
            }
            else
            {
                RunOrWalk = MoveState.Run;
            }
        }
        if (Input.GetKeyUp(JumpKey))
        {
            animator.SetTrigger("Jump");
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }
    
    private void CheckDownFall()
    {
        if (!CheckIsOnGround())
        {
            Vector3 fall=rigidbody.velocity;
            if(Vector3.Angle(fall, transform.up) > 90)
            {
                animator.SetBool("IsDownFall", true);
            }
            else
            {
                animator.SetBool("IsDownFall", false);
            }
        }
    }
}

public enum MoveState
{
    Idle,
    Run,
    Walk
}
