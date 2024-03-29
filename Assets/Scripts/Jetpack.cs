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
    }
    private void Update()
    {
        bool CanTrig;
        if (Input.GetKey(EffectKeyCode)&&CanEffect)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            characterMove.SetIfUnLockRigibody(true);
            Vector3 Direction = Data.Character.transform.localToWorldMatrix * Vector3.up;
            if ( x*x + y*y >= 0.5)
            {
                Direction= Vector3.Normalize(Data.Character.transform.localToWorldMatrix * new Vector3(x, 0, y));
            }
            if (!eventCenter.IsFloating)
            {
                Direction =Vector3.Normalize( Data.Character.transform.localToWorldMatrix * new Vector3(x/4, 1, y/4));
            }
            float a = Trig(true,out CanTrig);
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
            Trig(false,out CanTrig);
            ParticleSystem.EmissionModule emissionModuleR = RightPipeParticle.emission;
            ParticleSystem.EmissionModule emissionModuleL = LeftPipeParticle.emission;
            emissionModuleR.enabled = false;
            emissionModuleL.enabled = false;
            if (Audio.isPlaying) Audio.Stop();
        }
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
