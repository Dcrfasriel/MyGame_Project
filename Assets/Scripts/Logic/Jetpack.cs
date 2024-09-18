using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Jetpack : MonoBehaviour
{
    [Header("��������")]
    public KeyCode EffectKeyCode;
    [Header("�����ٶȴ�С")]
    public float MAcceleration;
    [Header("ˮƽ���ٶȴ�С")]
    public float NAcceleration;
    [Header("˥���ָ��")]
    public float DecreasePoint;
    [Header("�����ٶ�(0-1)")]
    public float CostSpeed;
    [Header("�ָ��ٶ�(0-1)")]
    public float RecoverySpeed;
    [Header("�������������")]
    public int MaxParticleRate;
    [Header("��С����������")]
    public int NParticleRate;

    private float ParticleRate;
    private float Power = 1;
    private SurfaceUI surfaceUI;
    private CharacterEventCenter eventCenter;
    private GameDataCenter Data;
    private Rigidbody EffectRigidbody;
    private CharacterMove characterMove;
    private bool CanEffect = true;
    private ParticleSystem LeftPipeParticle;
    private ParticleSystem RightPipeParticle;
    private AudioSource Audio;
    private float XInput;
    private float YInput;

    private void Awake()
    {
        Audio= GetComponent<AudioSource>();
        LeftPipeParticle = transform.Find("FirePipe1").Find("FlameThrower").GetComponent<ParticleSystem>();
        RightPipeParticle = transform.Find("FirePipe2").Find("FlameThrower").GetComponent<ParticleSystem>();
        surfaceUI=GameUIManager.GetInstance().SurfaceUI;
    }

    private void Start()
    {
        Data=GameDataCenter.GetInstance();
        EffectRigidbody=Data.Character.GetComponent<Rigidbody>();
        characterMove= Data.Character.GetComponent<CharacterMove>();
        eventCenter=CharacterEventCenter.GetInstance();
        GameUIManager.GetInstance().AddEventToSurfaceUI((key, u) =>
        {
            if (!u)
            {
                XInput = 0;
                YInput = 0;
                return;
            }
            XInput = Input.GetAxisRaw(key[0]);
            YInput = Input.GetAxisRaw(key[1]);
        }, "Horizontal", "Vertical");

        GameUIManager.GetInstance().AddEventToSurfaceUI((key, u) =>
        {
            bool CanTrig;
            if (u && Input.GetKey(key[0]) && CanEffect && eventCenter.CurrentState == CharacterState.Normal)
            {
                characterMove.SetIfUnLockRigibody(true);
                Vector3 Direction = Data.Character.transform.localToWorldMatrix * Vector3.up;
                if (XInput * XInput + YInput * YInput >= 0.5)
                {
                    Direction = Vector3.Normalize(Data.Character.transform.localToWorldMatrix * new Vector3(XInput, 0, YInput));
                }
                if (!eventCenter.IsFloating)
                {
                    Direction = Vector3.Normalize(Data.Character.transform.localToWorldMatrix * new Vector3(XInput / 4, 1, YInput / 4));
                }
                float a = Trig(true, out CanTrig);
                if (!CanTrig)
                {
                    ParticleSystem.EmissionModule emissionModuleR = RightPipeParticle.emission;
                    ParticleSystem.EmissionModule emissionModuleL = LeftPipeParticle.emission;
                    emissionModuleR.enabled = false;
                    emissionModuleL.enabled = false;
                    if (Audio.isPlaying) Audio.Stop();
                }
                else
                {

                    EffectRigidbody.AddForce(Direction * a, ForceMode.Acceleration);
                    ParticleSystem.EmissionModule emissionModuleR = RightPipeParticle.emission;
                    ParticleSystem.EmissionModule emissionModuleL = LeftPipeParticle.emission;
                    emissionModuleL.rateOverTime = ParticleRate;
                    emissionModuleR.rateOverTime = ParticleRate;
                    emissionModuleR.enabled = true;
                    emissionModuleL.enabled = true;
                    if (!Audio.isPlaying) Audio.Play();
                }
            }
            else
            {
                Trig(false, out CanTrig);
                ParticleSystem.EmissionModule emissionModuleR = RightPipeParticle.emission;
                ParticleSystem.EmissionModule emissionModuleL = LeftPipeParticle.emission;
                emissionModuleR.enabled = false;
                emissionModuleL.enabled = false;
                if (Audio.isPlaying) Audio.Stop();
            }
        },EffectKeyCode);
    }
    private float Trig(bool isTrig,out bool CanTrig)
    {
        CanTrig = true;
        if (eventCenter.IsOnGround || eventCenter.IsFloating)
        {
            Power += RecoverySpeed * Time.deltaTime;
            Power = Mathf.Clamp(Power, 0, 1);
        }
        if (Power <= 0.01&&isTrig)
        {
            CanTrig = false;
            return 0;
        }
        if(isTrig)
        {
            Power -= CostSpeed * Time.deltaTime;
            Power = Mathf.Clamp(Power, 0, 1);
            if (Power>DecreasePoint)
            {
                ParticleRate=Mathf.Lerp(NParticleRate, MaxParticleRate, (Power - DecreasePoint) / (1 - DecreasePoint));
                return Mathf.Lerp(NAcceleration,MAcceleration,(Power-DecreasePoint)/(1-DecreasePoint));
            }
            else
            {
                ParticleRate = NParticleRate;
                return NAcceleration;
            }
        }
        else
        {
            return 0;
        }
    }

    private void OnGUI()
    {
        surfaceUI.SetPowerSliderValue(Power);
    }
}
