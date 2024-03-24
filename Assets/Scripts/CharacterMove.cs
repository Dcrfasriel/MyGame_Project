using Assets.Frame.Tools;
using Assets.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private CameraFol cameraFol;
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
    private void Awake()
    {
        cameraFol=Camera.main.GetComponent<CameraFol>();
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
    }
    private void Update()
    {
        CheckTrigJump();
        Move();
        LastSpeed=(transform.position-LastPosition)/Time.deltaTime;
        LastPosition=transform.position;
        rigidbody.AddForce(G, ForceMode.Acceleration);
        SetCharacterDirection(characterEventCenter.CurrentCenter);
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (CheckIsOnGround()&&CheckIsDownFall()&&!WillLeaveGround)
        {
            IsLastMoveBlockBySlope = false;
            LastGroundMoveSpeedVector = Vector3.zero;
            IsLastMoveOnGround = true;
            Vector3 Normal = GetGroundNormal();
            Vector3 MoveFaceDir = transform.forward * y + transform.right * x;
            Vector3 MoveDir = Vector3.Normalize(Vector3.ProjectOnPlane(MoveFaceDir, Normal));
            float Upslope= Vector3.Angle(Normal, transform.up);
            float slope = 90 - Vector3.Angle(MoveDir, transform.up);
            if ((x * x + y * y) <= 0.01 && Upslope <= MaxAngle) LockRigidbody(true);
            else LockRigidbody(false);
            if (slope <= MinAngle)
            {
                MoveDir = Vector3.Normalize(MoveFaceDir);
            }
            float speed = 0;
            if (RunOrWalk == MoveState.Walk)
            {
                speed = WalkSpeed;
                if (y < 0.4)
                {
                    speed = BackWardWalkSpeed;
                }
            }
            else if (RunOrWalk == MoveState.Run)
            {
                speed = RunSpeed;
                if (y < 0.4)
                {
                    speed = BackWardRunSpeed;
                }
            }
            float speedRate = 1;
            if (x > 0.1) speedRate = x / Vector3.Normalize(new Vector3(x, y, 0)).x;
            else if (y > 0.1) speedRate = y / Vector3.Normalize(new Vector3(x, y, 0)).y;
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
            if (!rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionX))
            {
                rigidbody.constraints |= RigidbodyConstraints.FreezePositionX;
            }
            if (!rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionY))
            {
                rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
            }
            if (!rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionZ))
            {
                rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
            }
        }
        else
        {
            if (!rigidbody)
            {
                Debug.LogError("Rigidbody component is missing!");
                return;
            }

            // 解锁刚体的某些约束
            if (rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionX))
            {
                rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            }
            if (rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionY))
            {
                rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
            if (rigidbody.constraints.HasFlag(RigidbodyConstraints.FreezePositionZ))
            {
                rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            }
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

    private void CheckTrigJump()
    {
        if(Input.GetKeyUp(JumpKey)&&CheckIsOnGround())
        {
            WillLeaveGround = true;
            LastSpeed =Vector3.ProjectOnPlane(LastGroundMoveSpeedVector,transform.up)+(LastSpeed- LastGroundMoveSpeedVector) +transform.up * JumpHeight;
            if (IsLastMoveBlockBySlope)
            {
                LastSpeed += LastGroundMoveSpeedVector;
            }
            Invoke("SetLeaveFlase", 0.5f);
        }
    }
    private void SetLeaveFlase()
    {
        WillLeaveGround= false;
    }

    public void SetBind(Transform transform)
    {
        bindTransform = transform;
        if (transform != null)
        {
            //LastBindPosition = bindTransform.position;
            this.transform.parent = transform;
            AxisPoint.parent= transform;
        }
        else
        {
            this.transform.parent = null;
            AxisPoint.parent = null;
        }
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

    
}
