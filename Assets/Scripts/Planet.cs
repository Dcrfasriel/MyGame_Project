using Assets.Frame.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("��������")]
    public float PG;
    [Header("��ɫ�������")]
    public float EnterDistance;
    [Header("�������л�Ϊ�˶���ľ���")]
    public float CenterEnterDistance;
    [Header("��ת�ٶ�")]
    public float SelfRotationSpeed;
    [Header("��ת��")]
    public Vector3 SelfRotationAxis;
    [Header("��ת��")]
    public Transform RotationPoint;
    [Header("��ת�ٶ�")]
    public float RotationSpeed;
    [Header("��ת��")]
    public Vector3 RotationAxis;
    //[Header("����λ")]
    //public float Offset;

    private CharacterEventCenter characterEventCenter;
    private GameDataCenter gameDataCenter;
    private float R;
    private VelocityController velocityController;
    private ForceSystem forceSystem;
    private CharacterMove characterMove;
    private Vector3 OrgPosition;
    private Vector3 OrgRotationPoint;

    private void Awake()
    {
        R = transform.localScale.y;
        OrgPosition = transform.position;
        velocityController = GetComponent<VelocityController>();
        Vector3 Line = RotationPoint.position + RotationAxis;
        OrgRotationPoint = RotationPoint.position;
        velocityController.GetPosFunc = (t) =>
        {
            return Tools.GetPosition(OrgPosition, OrgRotationPoint, RotationAxis, RotationSpeed, t);
        };
    }
    private void Start()
    {
        characterEventCenter=CharacterEventCenter.GetInstance();
        forceSystem = ForceSystem.GetInstance();
        gameDataCenter=GameDataCenter.GetInstance();
        characterMove =gameDataCenter.Character.GetComponent<CharacterMove>();
        if (gameDataCenter.Character.GetComponent<CharacterMove>()!=null) characterMove=gameDataCenter.Character.GetComponent<CharacterMove>();
    }

    private void Update()
    {
        forceSystem.SetGravityForce(gameObject.name + "Gravity", PG, transform.position);
        SetCharacterDirection();
        transform.Rotate(SelfRotationAxis, SelfRotationSpeed * Time.deltaTime);
    }

    private void SetCharacterDirection()
    {
        if ((gameDataCenter.AxisPoint.position - transform.position).magnitude <= CenterEnterDistance + R)
        {
            VelocitySystem.GetInstance().SetCenter(velocityController);
        }
        if ((gameDataCenter.AxisPoint.transform.position - transform.position).magnitude <= EnterDistance + R)
        {
            characterEventCenter.SetCurrentCenter(transform.position);
        }
    }

}
