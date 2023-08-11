using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �� ������ BGM�� �ν����Ϳ��� �Ҵ�
// �����̴��� ���� ����� �� ���� ���� ���� �� ����
public class SoundManager : MonoBehaviour
{   
    private SeasonDateCalc mSeason; // ����

    // BGM, ȿ���� ����� �ҽ�
    private AudioSource mBGM;
    private AudioSource mSFx; // �⺻ Ŭ����
    private AudioSource mSubSFx; // ���� Ŭ����

    public AudioClip[] mSeasonBGMArray = new AudioClip[4]; // 4�������� �޶����� BGM
    public AudioClip[] mBGMArray = new AudioClip[2]; // [0] �׸� [1] ������
    private enum SFx {
        SFx_CloudeDecorEnd,
        SFx_CloudMaking,
        SFx_CloudMonitor,
        SFx_HarvestDone,
        SFx_Hint,
        SFx_MonitorLoading,
        SFx_StickerPeel,
        SFx_StickerStick,
        SFx_TalkFemale,
        SFx_TalkMale,
        SFx_TalkNarr,
        SFx_UseIngredient,
        SFx_END,
    };
    public AudioClip[] mSubSFxArray = new AudioClip[(int)SFx.SFx_END];

    public bool[] isOneTime = new bool[4]; // �뷡 �ѹ��� Ʋ��

    // To Use Audio Fade In Effect
    public bool         fadeIn = true;
    private float       timer = 0.0f;

    const int MAX_VALUE = 2;
    private bool[] bFirstPlay = new bool[MAX_VALUE];

    void Awake()
    {   
        // ����� �ҽ� ã��
        mBGM = GameObject.Find("mBGM").GetComponent<AudioSource>();
        mSFx = GameObject.Find("mSFx").GetComponent<AudioSource>();
        mSubSFx = GameObject.Find("mSubSFx").GetComponent<AudioSource>();

        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
    }
    // ����Ƽ �����ֱ� Ȱ��
    void Start()
    {
        if (SceneData.Instance) // null check
        {
            // ���� ����� �� ����� ������ ���� ������Ʈ
            mBGM.volume = SceneData.Instance.BGMValue;
            mSFx.volume = SceneData.Instance.SFxValue;
            mSubSFx.volume = SceneData.Instance.SFxValue; 
        }

        if ((SceneManager.GetActiveScene().name == "Lobby"))
        {
            mBGM.clip = mBGMArray[0];
            mBGM.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Drawing Room" )
        {
            mBGM.clip = mBGMArray[1];
            mBGM.Play();
        }
        
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Space Of Weather")
        { // ������ ���� ���� ���� �Ұ����� �ߺ� �÷��� ����
            switch (mSeason.mSeason)
            {
                case 1:
                    UpdateAudio(0);
                    break;
                case 2:
                    UpdateAudio(1);
                    break;
                case 3:
                    UpdateAudio(2);
                    break;
                case 4:
                    UpdateAudio(3);
                    break;
                default:
                    break;
            }           
        }

        if(fadeIn == true)
        {
            AudioFadeIn(1.0f);
        }
    }

    void AudioFadeIn(float duration)
    {
        float bgmValue = SceneData.Instance.BGMValue;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            timer = 0.0f;
            fadeIn = false;
            return;
        }
        mBGM.volume = bgmValue * timer / duration;

    }

    void UpdateAudio(int _iIndex)
    {
        if (!isOneTime[_iIndex])
        {
            mBGM.clip = mSeasonBGMArray[_iIndex];
            mBGM.Play();
            for (int i = 0; i < 4; i++)
            {
                if (_iIndex == i) continue;

                isOneTime[i] = false;
            }
            isOneTime[_iIndex] = true;
        }
    }

    // BGM ���� ����
    public void SetBGMVolume(float volume)
    {
        mBGM.volume = volume * 0.5f;
        // ���� ������ �� ���� ����
        SceneData.Instance.BGMValue = volume;
        if (false == bFirstPlay[0])
        {
            bFirstPlay[0] = true;
        }
        else
        {
            mSFx.Play();
        }
        
    }

    // SFx ���� ����
    public void SetSFxVolume(float volume)
    {
        mSFx.volume = volume;
        mSubSFx.volume = volume;
        // ���� ������ �� ���� ����
        SceneData.Instance.SFxValue = volume;
        if (false == bFirstPlay[1])
        {
            bFirstPlay[1] = true;
        }
        else
        {
            mSFx.Play();
        }
    }
}
