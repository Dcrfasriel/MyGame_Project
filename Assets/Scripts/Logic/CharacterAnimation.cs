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
    private bool IsLastOnGround;

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
        GameUIManager.GetInstance().AddEventToSurfaceUI((key,u) =>
        {
            if (!u)
            {
                return;
            }
            if (Input.GetKeyDown(key[0]))
            {
                if (RunOrWalk == MoveState.Run)
                {
                    RunOrWalk = MoveState.Walk;
                }
                else
                {
                    RunOrWalk = MoveState.Run;
                }
            }
        }, SwitchRunWalkKey);
        GameUIManager.GetInstance().AddEventToSurfaceUI((key,u) =>
        {
            if (!u)
            {
                return;
            }
            if (Input.GetKeyUp(key[0]))
            {
                animator.SetTrigger("Jump");
            }
            else
            {
                animator.ResetTrigger("Jump");
            }
        }, JumpKey);
        GameUIManager.GetInstance().AddEventToSurfaceUI((key,u) =>
        {
            if (!u)
            {
                XInput = 0;
                YInput = 0;
                return;
            }
            XInput = Input.GetAxis(key[0]);
            YInput= Input.GetAxis(key[1]);
        }, "Horizontal", "Vertical");
    }
    private void Update()
    {
        CheckAndSetAnimatorParameter();
        AdjustFaceDirection_SetRelativePos();
        characterMove.SetRunOrWalk(RunOrWalk);
        characterMove.SetJumpKey(JumpKey);
        CheckDownFall();
        if (!IsLastOnGround && CheckIsOnGround())
        {
            animator.Play("FallOnGround");
        }
        IsLastOnGround=CheckIsOnGround();
    }
    private float XInput=0;
    private float YInput=0;
    private void CheckAndSetAnimatorParameter()
    {
        animator.SetBool("IsOnGround", CheckIsOnGround());
        animator.SetBool("IsMoving", XInput * XInput + YInput * YInput > 0.1);
        animator.SetBool("IsRun", RunOrWalk == MoveState.Run);
        animator.SetFloat("ForwardDir", YInput);
        animator.SetFloat("RightDir", XInput);
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
