using Assets.Frame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMove : MonoBehaviour
{
    [Header("飞行启动键")]
    public KeyCode FlyKeyCode;
    [Header("侧向旋转键")]
    public KeyCode CrossRotateKeyCode;
    [Header("上升加速度")]
    public float Force;
    [Header("旋转力")]
    public float RotationForce;
    [Header("最大音效")]
    public float MaxVolume;
    [Header("升音效速度")]
    public float VolumeIncreaseSpeed;
    [Header("降音效速度")]
    public float VolumeDecreaseSpeed;

    public bool Debug_IsOn;

    private AudioSourceController AudioController;
    private Rigidbody rb;
    private BoxCheckPoint checkPoint;
    private CharacterEventCenter eventCenter;
    private VelocitySystem velocitySystem;
    private GameUIManager gameUIManager;
    private Vector3 DirV=Vector3.zero;
    private float Mx = 0;
    private float My = 0;
    private bool IsForwardRotate = true;
    private AudioSource audioSource;

    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
        checkPoint=transform.Find("OnGroundCheckBox").GetComponent<BoxCheckPoint>();
        audioSource = GetComponent<AudioSource>();
        AudioController = GetComponent<AudioSourceController>();
    }
    private void Start()
    {
        eventCenter=CharacterEventCenter.GetInstance();
        velocitySystem=VelocitySystem.GetInstance();
        gameUIManager=GameUIManager.GetInstance();
        RegisterKeys();
    }
    private void Update()
    {

        if (eventCenter.CurrentState != CharacterState.OnSpaceShip && checkPoint.IsOnGround && velocitySystem.Center != transform.root.GetComponent<VelocityController>())
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }

        if (CanMove())
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            if (DirV.sqrMagnitude > 0.1)
            {
                AudioTools.ControllAudioSource(AudioController, true, MaxVolume, VolumeIncreaseSpeed, VolumeDecreaseSpeed);
            }
            else
            {
                AudioTools.ControllAudioSource(AudioController, false, MaxVolume, VolumeIncreaseSpeed, VolumeDecreaseSpeed);
            }
            rb.AddForce(Vector3.Normalize(transform.right*-DirV.x+transform.up*DirV.y*((IsForwardRotate)?1:-1)+transform.forward*-DirV.z) * Force, ForceMode.Acceleration);
        }
        if (CanRotate())
        {
            if(IsForwardRotate)
            {
                Vector3 Dir =Vector3.Normalize(Vector3.Cross(transform.forward, -transform.up * My + transform.right * Mx));
                rb.AddTorque(Dir*RotationForce, ForceMode.Acceleration);
            }
            else
            {
                Vector3 Dir = Vector3.Normalize(Vector3.Cross(transform.up, transform.forward * My - transform.right * Mx));
                rb.AddTorque(Dir * RotationForce, ForceMode.Acceleration);
            }
        }
        if (Debug_IsOn)
        {
            Vector3 Dir2 = Vector3.Normalize(Vector3.Cross(transform.up, transform.forward * 0 - transform.right * 1));
            rb.AddTorque(Dir2 * RotationForce, ForceMode.Acceleration);
        }

        if (eventCenter.CurrentState!=CharacterState.OnSpaceShip&&transform.parent!=null&&transform.parent.GetComponent<VelocityController>()!=null&& velocitySystem.Center != transform.parent.GetComponent<VelocityController>())
        {
            if (!rb.isKinematic && rb.velocity.magnitude > 1)
            {
                rb.isKinematic = true;
            }
        }
        else
        {
            rb.isKinematic=false;
        }
    }

    private bool CanMove()
    {
        return eventCenter.CurrentState==CharacterState.OnSpaceShip;
    }

    private bool CanRotate()
    {
        return eventCenter.CurrentState == CharacterState.OnSpaceShip;
    }
    private void RegisterKeys()
    {
        gameUIManager.AddEventToSurfaceUI((keys, u) =>
        {
            if (!u)
            {
                DirV.y = 0;
                return;
            }
            if (Input.GetKey(keys[0]))
            {
                DirV.y = 1;
            }
            else
            {
                DirV.y = 0;
            }
        }, FlyKeyCode);

        gameUIManager.AddEventToSurfaceUI((keys, u) =>
        {
            if (!u)
            {
                IsForwardRotate = false;
                return;
            }
            if (Input.GetKey(keys[0]))
            {
                IsForwardRotate = false;
            }
            else
            {
                IsForwardRotate= true;
            }
        }, CrossRotateKeyCode);

        gameUIManager.AddEventToSurfaceUI((keys, u) =>
        {
            if (!u)
            {
                DirV.x = 0;
                DirV.z = 0;
                return;
            }
            DirV.x = Input.GetAxisRaw(keys[0]);
            DirV.z = Input.GetAxisRaw(keys[1]);
        }, "Horizontal", "Vertical");

        gameUIManager.AddEventToSurfaceUI((keys,u) =>
        {
            if (!u)
            {
                Mx = 0;
                My = 0;
                return;
            }
            Mx = Input.GetAxisRaw(keys[0]);
            My = Input.GetAxisRaw(keys[1]);
        }, "Mouse X", "Mouse Y");
    }

   
}
