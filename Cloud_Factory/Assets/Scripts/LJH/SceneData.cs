using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ������ ���Ǵ� �ӽ� ������ ����
// �� �̵� �� ������� ������
// ��) ���� ũ��, �� �ε���
public class SceneData : MonoBehaviour
{
    // SceneData �ν��Ͻ��� ��� ���� ����
    private static SceneData instance = null;
    public  static SceneData Instance
    {
        get
        {
            if (null == instance) return null;

            return instance;
        }
    }

    [SerializeField]
    private int mCurrentSceneIndex;    // ���� �� �ε���
    public int currentSceneIndex
    {
        get { return mCurrentSceneIndex; }
        set { mCurrentSceneIndex = value; }
    }

    [SerializeField]
    private int     mPrevSceneIndex;    // ���� �� �ε���
    public  int     prevSceneIndex
    {
        get { return mPrevSceneIndex; }
        set { mPrevSceneIndex = value; }
    }

    private float   mBGMValue;          // BGM �Ҹ� ũ�� 
    public  float   BGMValue
    {
        get { return mBGMValue; }
        set { mBGMValue = value; }
    }
    private float   mSFxValue;          // SFx �Ҹ� ũ�� 
    public  float   SFxValue
    {
        get { return mSFxValue; }
        set { mSFxValue = value; }
    }

    public bool mContinueGmae = false;

    void Awake()
    {
        // �ν��Ͻ� �Ҵ�
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // �̹� �����ϸ� �������� ����ϴ� ���� �����
            Destroy(this.gameObject);
        }

        // �ʱⰪ �Ҵ�
        mBGMValue = 0.5f;
        mSFxValue = 1.0f;
    }

    void Start()
    {
        
    }
}