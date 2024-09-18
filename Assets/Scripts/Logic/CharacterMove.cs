using Assets.Frame.Tools;
using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMove : MonoBehaviour,IRecieveForce
{
    [Header("相机轴点")]
    public Transform AxisPoint;
    [Header("层遮罩")]
    public int RayLayerMask;
    [Header("走路速度")]
    public float WalkSpeed;
    [Header("跑步速度")]
    public float RunSpeed;
    [Header("后退跑步速度")]
    public float BackWardRunSpeed;
    [Header("后退走路速度")]
    public float BackWardWalkSpeed;
    [Header("最大上坡角度")]
    public float MaxAngle;
    [Header("最小下坡角度(负值)")]
    public float MinAngle;
    [Header("空中速度")]
    public float AirSpeed;
    [Header("跳跃高度")]
    public float JumpHeight;
    [Header("绑定")]
    public Transform bindTransform;
    [Header("重力")]
    public Vector3 G;
    [Header("视角调整速度")]
    public float AxisRotationSpeed;

    private BoxCheckPoint GroundCheckBox;
    private new Rigidbody rigidbody;
    private MoveState RunOrWalk;
    private bool IsLastMoveOnGround;
    private Vector3 LastGroundMoveSpeedVector;
    private KeyCode JumpKey;
    private bool WillLeaveGround=false;
    private bool IsLastMoveBlockBySlope=false;
    private Vector3 LastSpeed=new Vector3(0,0,0);
    private Vector3 LastPosition;
    private CharacterEventCenter characterEventCenter;
    private bool WillUnLockRigibody;
    private float XInput;
    private float YInput;
    private void Awake()
    {
        MaxAngle = Mathf.Clamp(MaxAngle, 0, 90);
        MinAngle = Mathf.Clamp(MinAngle, -90,0);
        GroundCheckBox = transform.Find("GroundCheckPoint").gameObject.GetComponent<BoxCheckPoint>();
        rigidbody = GetComponent<Rigidbody>();
        GroundCheckBox.OnEnter += TouchingGround;
        LastPosition=transform.position;
    }

    private void Start()
    {
        characterEventCenter=CharacterEventCenter.GetInstance();
        GameUIManager.GetInstance().AddEventToSurfaceUI((keys,u) =>
        {
            if (!u)
            {
                XInput = 0; YInput = 0;
                return;
            }
            XInput = Input.GetAxis(keys[0]);
            YInput = Input.GetAxis(keys[1]);
        }, "Horizontal", "Vertical");

        GameUIManager.GetInstance().AddEventToSurfaceUI((key, u) =>
        {
            if (u&&Input.GetKeyUp(JumpKey) && CheckIsOnGround())
            {
                WillLeaveGround = true;
                LastSpeed = Vector3.ProjectOnPlane(LastGroundMoveSpeedVector, transform.up) + (LastSpeed - LastGroundMoveSpeedVector) + transform.up * JumpHeight;
                if (IsLastMoveBlockBySlope)
                {
                    LastSpeed += LastGroundMoveSpeedVector;
                }
                Invoke("SetLeaveFlase", 0.5f);
            }
        }, JumpKey);
    }
    private void Update()
    {
        Move();
        LastSpeed=(transform.position-LastPosition)/Time.deltaTime;
        LastPosition=transform.position;
        rigidbody.AddForce(G, ForceMode.Acceleration);
        SetCharacterDirection(characterEventCenter.CurrentCenter);
        SetIfUnLockRigibody(false);
    }

    private void Move()
    {
        if (CheckIsOnGround()&&CheckIsDownFall()&&!WillLeaveGround&&rigidbody.velocity.magnitude<5f)
        {
            IsLastMoveBlockBySlope = false;
            LastGroundMoveSpeedVector = Vector3.zero;
            IsLastMoveOnGround = true;
            Vector3 Normal = GetGroundNormal();
            Vector3 MoveFaceDir = transform.forward * YInput + transform.right * XInput;
            Vector3 MoveDir = Vector3.Normalize(Vector3.ProjectOnPlane(MoveFaceDir, Normal));
            float Upslope= Vector3.Angle(Normal, transform.up);
            float slope = 90 - Vector3.Angle(MoveDir, transform.up);
            if ((XInput * XInput + YInput * YInput) <= 0.01 && Upslope <= MaxAngle && !WillUnLockRigibody) LockRigidbody(true);
            else LockRigidbody(false);
            if (slope <= MinAngle)
            {
                MoveDir = Vector3.Normalize(MoveFaceDir);
            }
            float speed = 0;
            if (RunOrWalk == MoveState.Walk)
            {
                speed = WalkSpeed;
                if (YInput < 0.4)
                {
                    speed = BackWardWalkSpeed;
                }
            }
            else if (RunOrWalk == MoveState.Run)
            {
                speed = RunSpeed;
                if (YInput < 0.4)
                {
                    speed = BackWardRunSpeed;
                }
            }
            float speedRate = 1;
            if (XInput > 0.1) speedRate = XInput / Vector3.Normalize(new Vector3(XInput, YInput, 0)).x;
            else if (YInput > 0.1) speedRate = YInput / Vector3.Normalize(new Vector3(XInput, YInput, 0)).y;
            LastGroundMoveSpeedVector = MoveDir * speed * speedRate;
            if (slope >= MaxAngle)
            {
                IsLastMoveBlockBySlope = true;
                return;
            }
            transform.position += LastGroundMoveSpeedVector * Time.deltaTime;
        }
        else
        {
            LockRigidbody(false);
            SetBind(null);
            if(IsLastMoveOnGround)rigidbody.velocity = LastSpeed;
            IsLastMoveOnGround = false;
        }
    }

    private Vector3 GetGroundNormal()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + transform.up, transform.up * -1, out hit,10,RayLayerMask))
        {
            return hit.normal;
        }
        else
        {
            return transform.up;
        }
    }

    private bool CheckIsOnGround()
    {
        return GroundCheckBox.IsOnGround;
    }

    private bool CheckIsDownFall()
    {
        if (!CheckIsOnGround())
        {
            Vector3 fall = rigidbody.velocity;
            if (Vector3.Angle(fall, transform.up) > 90)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    private void LockRigidbody(bool lockOrUnlock)
    {
        if (lockOrUnlock)
        {
            if (!rigidbody)
            {
                Debug.LogError("Rigidbody component is missing!");
                return;
            }

            // 检查并锁定刚体的某些约束
            //if (!rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionX))
            //{
            //    rigidbody.constraints |= RigidbodyConstraints.FreezePositionX;
            //}
            //if (!rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionY))
            //{
            //    rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
            //}
            //if (!rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionZ))
            //{
            //    rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
            //}
            rigidbody.isKinematic = true;
        }
        else
        {
            if (!rigidbody)
            {
                Debug.LogError("Rigidbody component is missing!");
                return;
            }

            // 解锁刚体的某些约束
            //if (rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionX))
            //{
            //    rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            //}
            //if (rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionY))
            //{
            //    rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
            //}
            //if (rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionZ))
            //{
            //    rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            //}

            rigidbody.isKinematic = false;
        }
    }

    public void SetRunOrWalk(MoveState state)
    {
        RunOrWalk= state;
    }

    public void SetJumpKey(KeyCode keyCode)
    {
        JumpKey= keyCode;
    }

    
    private void SetLeaveFlase()
    {
        WillLeaveGround= false;
    }

    public void SetBind(Transform transform)
    {
        //bindTransform = transform;
        //if (transform != null)
        //{
        //    //LastBindPosition = bindTransform.position;
        //    this.transform.parent = transform;
        //    AxisPoint.parent= transform;
        //}
        //else
        //{
        //    this.transform.parent = null;
        //    AxisPoint.parent = null;
        //}
    }

    public void SetForce(Vector3 Gravity)
    {
        this.G=Gravity;
    }

    private void TouchingGround(Collider other)
    {
        SetBind(other.transform);
    }
    private Vector3? PreviousCenter=null;
    private bool IsFirstEnter=false;
    public void SetCharacterDirection(Vector3? Center)
    {
        if (Center == null)
        {
            PreviousCenter = null;
            return;
        }
        Vector3 NCenter = (Vector3)Center;
        Vector3 Up=transform.position- NCenter;
        if(PreviousCenter!=NCenter)
        {
            PreviousCenter = NCenter;
            IsFirstEnter = true;
        }
        Up=Vector3.Normalize(Up);
        if(!IsFirstEnter)
        {
            AxisPoint.LookAt(AxisPoint.position+Vector3.Cross(AxisPoint.right, AxisPoint.position - NCenter),Up);
        }
        else
        {
            float angle = Vector3.Angle(AxisPoint.up, Up);
            Tools.StepRotateTowards(AxisPoint, Vector3.ProjectOnPlane(transform.forward, transform.position - NCenter), Up, AxisRotationSpeed);
            if (angle < 1)
            {
                IsFirstEnter = false;
            }
        }
    }

    public Vector3 GetLastSpeed()
    {
        return rigidbody.velocity;
    }

    public void SetIfUnLockRigibody(bool Bool)
    {
        WillUnLockRigibody = Bool;
    }

    
    
}
