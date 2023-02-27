using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GuestObject : MonoBehaviour
{
    // ������Ʈ ������ �ʿ��� ����
    [Header("[�մ� ����]")]
    public float        mLimitTime;         // �մ��� ����� �ð�
    public float        mMaxLimitTime;      // �մ��� ����ϴ� �ð��� �ִ밪
    public int          mGuestNum;          // �ش� ������Ʈ�� �մԹ�ȣ
    private Transform   mTransform;         // ��ġ���� ���ϴ��� Ȯ���ϱ� ���� ����
    public Transform    mTargetChair;       // ��ǥ�� �ϴ� ������ ��ġ
    public int          mTargetChiarIndex;

    [Header("[FSM ����]")]
    public bool isSit;                      // �ڸ��� �ɾ��ִ°�?
    public bool isUsing;                    // ���� ġ�Ḧ �޴����ΰ�?
    public bool isMove;                     // �̵����ΰ�?   
    public bool isGotoEntrance;             // �ⱸ�� ������ ���ΰ�?
    public bool isEndUsingCloud;            // ���� ����� �����ƴ°�?

    [Header("[����ǥ�� ����]")]
    public int   dialogEmotion;             // ���� ǥ����, ��ǳ������ ������ ������ ��ȣ 
    public int[] faceValue;                 // ���� ǥ����, ����Ʈ�� ������ ������ ��ȣ       
    public GameObject SpeechBubble;         // ���� ǥ����, ��ǳ�� ������ ä��� �ؽ�Ʈ ĭ
    public bool isSpeakEmotion;             // �մ��� ����ǥ�� �������� ��Ÿ���� ������      

    [Header("[��͵� 4 ��� ���� ��� ����]")]
    private RLHReader   textReader;
    private bool        isHintTextPrinted;
    public bool         isUseRarity4;
    private bool        isUsingHint;

    [Header("[��Ÿ]")]
    public Animator     mGuestAnim;         // �մ��� �ִϸ��̼� ����
    private Guest       mGuestManager;
    public SOWManager   mSOWManager;

    // ����� ����
    const int MAX_GUEST_NUM = 20;
    const int MAX_EMOTION_NUM = 20;
    const float DELAY_OF_SPEECH_BUBBLE = 5.0f;
    const float CHAR_SIZE = 0.9f;

    List<List<int>> EmotionList = new List<List<int>>
    {
        new List<int> {0,8,15,7}, // JOY
        new List<int> {1,2,16,13}, // SAD
        new List<int> {4,9}, // CALM
        new List<int> {3,6,14,12,11}, // ANGRY
        new List<int> {18,19,5,17,10}  // SURPRISE
    };

    // ����ǥ�� ��, ������ ���� ǥ���� ��ǳ�� ���� ����Ʈ -> ���� ��ȣ�� �̿��Ͽ� ������ �����ͼ� ä���.
    string[] EmotionDialogList = new string[]
    {
        "���",
        "�Ҿ�",
        "����",
        "¥��",
        "����",
        "���&ȥ��",
        "����",
        "����&���",
        "���",
        "����",
        "��ܽ�",
        "�ݴ�",
        "��å",
        "���",
        "���ݼ�",
        "��õ",
        "������",
        "����",
        "������",
        "ȥ��������",
    };

    // �մ԰� ��ȣ�ۿ��� ���� �ʿ��� �ݶ��̴� 
    private Collider2D sitCollider;
    private Collider2D walkCollider;

    // ����� ������� ������ ����
    private int enterSat;
    private int outSat;

    // �� �մ��� ��ȣ�� ���� �ִϸ����͸� ���� �����Ѵ�.
    public RuntimeAnimatorController[] animators = new RuntimeAnimatorController[MAX_GUEST_NUM];

    // �� ������ ����Ʈ�� �����صΰ� �ش� ��Ȳ�� ���� �������־� ����Ѵ�.
    public Animation[] EffectAnimations = new Animation[MAX_EMOTION_NUM];

    // ����ǥ�� ����Ʈ�� Front/Back���� ����� �����Ѵ�.
    public Animator FrontEffect;
    public Animator BackEffect;

    // �մ� ��ȣ�� �������ش�.
    public void setGuestNum(int guestNum = 0)
    {
        mGuestNum = guestNum;      
    }


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void init()
    {
        // ���ð� �ʱ�ȭ
        mLimitTime = 0.0f;
        //mMaxLimitTime = 50.0f;

        isSit = false;
        isUsing = false;
        isMove = false;
        isGotoEntrance = false;
        isEndUsingCloud = false;
        mTransform = this.transform;
        mTargetChiarIndex = -1;
        mTargetChair = null;
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        mGuestAnim = GetComponent<Animator>();

        sitCollider = this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>();
        walkCollider = this.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<CircleCollider2D>();

        enterSat = mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction;

        faceValue = mGuestManager.SpeakEmotionEffect(mGuestNum);
        dialogEmotion = mGuestManager.SpeakEmotionDialog(mGuestNum);
        SpeechBubble = this.transform.GetChild(1).gameObject;
        isSpeakEmotion = false;

        BackEffect = this.transform.GetChild(3).transform.GetChild(1).gameObject.GetComponent<Animator>();

        textReader = this.gameObject.GetComponent<RLHReader>();
        isHintTextPrinted = false;
        isUseRarity4 = true; // Test�� ���ؼ� true�� �ӽ� ����
        isUsingHint = false;
    }

// �ȴ� �ִϸ��̼� ���
// �ȴ� �ִϸ��̼��� ����Ʈ �ִϸ��̼����� ����

private void Update()
    {
        // �Ҵ�޴� ���� ����
        if (mTargetChiarIndex != -1 && isGotoEntrance == false)
        {
            mTargetChair = mSOWManager.mChairPos[mTargetChiarIndex].transform;
            mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = false;
            this.GetComponent<AIDestinationSetter>().enabled = true;
            this.GetComponent<AIDestinationSetter>().target = mTargetChair.transform;

            // ���ڿ� �������� �ʾҴٸ� AIPATH�� Ȱ��ȭ�Ѵ�.
            if (this.transform != mTargetChair.transform)
            {
                this.GetComponent<WayPoint>().isMove = false;
                this.GetComponent<AIPath>().enabled = true;
            }
            else
            {
                this.GetComponent<AIPath>().enabled = false;
            }

            mGuestAnim.SetBool("isStand", false);
        }

        // ������ �����޴� ���°� �ƴ϶�� ���ð��� ���Ž�Ų��.
        if (isUsing == false)
        {
            TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
            if (SceneManager.GetActiveScene().name != "Lobby"
                && SceneManager.GetActiveScene().name != "Cloud Storage"
                && SceneManager.GetActiveScene().name != "Give Cloud"
                && mTutorialManager.isTutorial == false)
            {
                mLimitTime += Time.deltaTime;
            }
        }

        // ���ð��� �����ų� �Ҹ���Ƽ�� �� ��쿡 (ġ�Ḧ ��ġ�� ���� ���� ����)
        if ((mLimitTime > mMaxLimitTime || mGuestManager.mGuestInfo[mGuestNum].isDisSat == true) && !isGotoEntrance)
        {
            // ����� ����Ʈ���� ���ְ�, �ش� ���ڸ� �ٽ� true�� �ٲ��־�� �Ѵ�.
            mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = true;
            mTargetChair = null;
            isSit = false;


            // ���� ��밡�� ����Ʈ���� ����
            int count = mSOWManager.mUsingGuestList.Count;
            for (int i = 0; i < count; i++)
            {
                if (mSOWManager.mUsingGuestList[i] == mGuestNum)
                {
                    mSOWManager.mUsingGuestList.RemoveAt(i);
                    mSOWManager.mUsingGuestObjectList.RemoveAt(i);
                }
            }

            // �Ҹ� �մ����� ��ȯ ��, �Ͱ�
            mGuestManager.mGuestInfo[mGuestNum].isDisSat = true;

            Debug.Log("Time");

            MoveToEntrance();
        }

        // �Ա��� ������ ���
        if (isGotoEntrance == true && transform.position.x - mSOWManager.mWayPoint[0].transform.position.x <= 0.2f)
        {
            Debug.Log("Destroy");
            Destroy(this.gameObject);
           
        }

        // ���ڿ� ������ ���
        if (mTargetChiarIndex != -1)
        {
            if (isGotoEntrance == false && Mathf.Abs(transform.position.x - mTargetChair.transform.position.x) 
                <= 0.1f && Mathf.Abs(transform.position.y - mTargetChair.transform.position.y) <= 0.1f)
            {
                // ���� ��ġ�� �̵� , ���⿡ ���� LocalScale ����
                if(mSOWManager.mSitDir[mTargetChiarIndex] == 1)
                    transform.localScale = new Vector3(CHAR_SIZE, CHAR_SIZE, 1f);
                else
                    transform.localScale = new Vector3(-CHAR_SIZE, CHAR_SIZE, 1f);

                mGuestAnim.SetBool("isSit", true);
                ChangeLayerToSit();

                // TODO : �ݶ��̴� ���� Walking ->Sitting
                sitCollider.enabled = true;
                walkCollider.enabled = false;

                this.transform.position = mTargetChair.transform.position;
                isSit = true;
            }
        }
        // ���¿� ���� �ִϸ��̼� ����
        if (isSit)
        {
            if(mGuestManager.mGuestInfo[mGuestNum].isUsing == true)
            {
                isUsing = true;
                Debug.Log("isUsing : true");
                mGuestManager.mGuestInfo[mGuestNum].isUsing = false;
            }
            // ġ�� ���� ��� ġ��ȿ���� ���� �ֱ������� �ִϸ��̼��� ����
            if (isUsing)
            {
                // ���� ���� ������ ���⿡ ���� �ɾ��ִ� ����� ������/������ �� �ϳ��� ���´�.
                mGuestAnim.SetBool("isUsing", true);

                // ���ð��� ������ ���� ������Ʈ���� ����� �ڷ�ƾ�� ���� isEndUsingCloud�� true�� �Ǿ� �Ͱ��Ѵ�.
                if (isEndUsingCloud)
                {
                    // ��͵� 4��Ḧ ����ߴ��� üũ
                    if (isUseRarity4)
                    {
                        // ����Ͽ��� ���� ��Ʈ�� ������� �ʾҴٸ� ��Ʈ ���
                        if (!isHintTextPrinted && !isUsingHint)
                        {
                            Hint();
                        }
                        // ��Ʈ ����� �Ϸ��ߴٸ� �Ͱ�
                        else if (isHintTextPrinted)
                            MoveToEntrance();
                    }
                    else
                    {
                        // ������� �ʾҴٸ� �ٷ� �Ͱ�
                        MoveToEntrance();
                    }
                }
            }
        }
        // ���� �̿��� ������ ��         TODO: ��͵� 4��ᰡ ������ üũ�ؾ� ��
        if (isEndUsingCloud && !isHintTextPrinted)
        {
            

            //isHintTextPrinted = true;
			//TextMeshPro Text = SpeechBubble.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
			//Text.text = textReader.PrintHintText();
			//SpeechBubble.transform.GetChild(0).gameObject.SetActive(true);                                  // ��ǳ�� Ȱ��ȭ
			//SpeechBubble.transform.GetChild(1).gameObject.SetActive(true);                                  // �ؽ�Ʈ Ȱ��ȭ
			//Invoke("EndBubble", 5.0f);
		}

        // �ȴ� ���⿡ ���� �ִϸ��̼��� ������ �ٸ��� �����Ѵ�.
        if (GetComponent<AIPath>().desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(CHAR_SIZE, CHAR_SIZE, 1f);
        }
        else if (GetComponent<AIPath>().desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-CHAR_SIZE, CHAR_SIZE, 1f);
        }
        // ���� ��ġ�� �����Ѵ�.
        mTransform = transform;
    }

    public void SpeakEmotion()
    { 
        // �ɾ��ִ� ��쿡�� Ŭ�� �� ��ȣ�ۿ��� ���� ������ ǥ���Ѵ�.
        if (!mGuestAnim.GetBool("isSit")) return;

        // �̹� ��ȣ�ۿ� ���� ��쿡�� Ŭ���� �� ���� �����Ѵ�.
        if (isSpeakEmotion)
        {
            Debug.Log("Already Speaking");
            return;         
        }


        // ��Ʈ�� ������� ��쿡�� ����ǥ���� �� �� ����.
        // TODO : ��Ʈ�� ������� ��� return�ϰԲ� ����

        Debug.Log("���� ����� ����մϴ�");
        isSpeakEmotion = true;

        // ���� ����, ���� ������ ���� ����� ������ ���� ��Ʈ(����Ʈ)
        for (int i = 0; i< faceValue.Length; i++)
        {
            StartCoroutine(Emotion(3.0f * (i), faceValue[i]));
        }
        // ������ �ݿ� �������� ���� �� ������ �˷��ִ� ��ǳ��  -> �մ��� ��ġ���� ���� ��/�� ���� ����
        StartCoroutine("DialogEmotion");

        // ��ȣ�ۿ� ������ ��Ÿ���� bool���� ��ȣ�ۿ��� ���� ���Ŀ� false�� �����Ѵ�.
        if (faceValue.Length > 1) Invoke("EndSpeakEmotion", faceValue.Length * 3.0f + 1.0f);
        else Invoke("EndSpeakEmotion", 6.0f);
    }

    // Hint�� ����ؾ� �ϴ� ��� 
    public void Hint()
    {
        // �ɾ��ִ� ��쿡�� Ŭ�� �� ��ȣ�ۿ��� ���� ������ ǥ���Ѵ�.
        if (!mGuestAnim.GetBool("isSit")) return;

        // �̹� ��ȣ�ۿ� ���� ��쿡�� Ŭ���� �� ���� �����Ѵ�.
        if (isSpeakEmotion)
        {
            Debug.Log("Already Speaking");
            return;
        }

        isUsingHint = true;
        isSpeakEmotion = true;

        // ��ǳ���� ����� ���� �ҷ����� -> ����Ʈ���� �������� ���� �ҷ�����
        string temp = textReader.PrintHintText();
        TextMeshPro Text = SpeechBubble.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
        Animator Anim = SpeechBubble.transform.GetChild(0).gameObject.GetComponent<Animator>();
        Text.text = temp;

        SpeechBubble.transform.GetChild(1).gameObject.transform.localScale = this.transform.localScale;

        // ��ǳ�� ����
        SpeechBubble.transform.GetChild(0).gameObject.SetActive(true);
        Anim.SetTrigger("Start");
        mGuestAnim.SetTrigger("Hint");

        // �����ð� ���� ��ǳ�� ����
        Invoke("EndBubble", 5.0f);
    }
    private void EndBubble()
    {
        Animator Anim = SpeechBubble.transform.GetChild(0).gameObject.GetComponent<Animator>();
        Anim.SetTrigger("EndBubble");
        mGuestAnim.SetTrigger("EndHint");
        isHintTextPrinted = true;
        EndSpeakEmotion();
    }


    private void EndSpeakEmotion()
    {
        isSpeakEmotion = false;
    }

    IEnumerator DialogEmotion()
    {
        // ��ǳ�� ���� ä���
        string temp = EmotionDialogList[dialogEmotion];
        TextMeshPro Text = SpeechBubble.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
        Animator Anim = SpeechBubble.transform.GetChild(0).gameObject.GetComponent<Animator>();

        Debug.Log("DialogEmotion : " + dialogEmotion);

        // Text�� NULL�� �ƴѰ�� ���� ä���ֱ�
        if (Text != null)
            Text.text = temp;

        SpeechBubble.transform.GetChild(1).gameObject.transform.localScale = this.transform.localScale;

        // ��ǳ�� ����
        SpeechBubble.transform.GetChild(0).gameObject.SetActive(true);
        Anim.SetTrigger("Start");

        // ������
        yield return new WaitForSeconds(DELAY_OF_SPEECH_BUBBLE);

        // ��ǳ�� �����
        Anim.SetTrigger("EndBubble");
    }

    IEnumerator Emotion(float delay, int emotionNum)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log(emotionNum + "Emotion ���");

        // Interaction Ʈ���� �ߵ� -> emotionNum�� ���� FaceValue���� ������Ų��.
        mGuestAnim.SetInteger("FaceValue", ChangeFaceValue(emotionNum));

        // �ش� emotionNum�� �ش��ϴ� ����Ʈ�� �����Ų��.
        BackEffect.SetInteger("EmotionValue", emotionNum);
        Debug.Log("Emotion Value : " + emotionNum);

        mGuestAnim.SetTrigger("Interaction");
        Invoke("EndInteraction", 2.9f);
    }

    int ChangeFaceValue(int emotionNum)
    {
        for(int i = 0; i< EmotionList.Count; i++)
        {
            foreach(int index in EmotionList[i])
            {
                if (index == emotionNum)
                    return i;
            }
        }
        return -1;
    }

    void EndInteraction()
    {
        BackEffect.SetInteger("EmotionValue", -1);
        mGuestAnim.SetTrigger("InteractionEnd");
        Debug.Log("Emotion ��� ������");
    }

    void EndHint()
    {
        mGuestAnim.SetTrigger("EndHint");
    }


    // �ִϸ��̼� Ŭ������ �մԿ� �°� �ʱ�ȭ�Ѵ�.
    public void initAnimator()
    {
        GetComponent<Animator>().runtimeAnimatorController = animators[mGuestNum];
        Debug.Log("init Guest Anim");
    }

    // �Ա��� �����ϴ� �Լ��̴�.
    private void MoveToEntrance()
    {
        //��� �ð��� �����ų�, ������ �����޾��� ��, ������ ������ ���
        outSat = mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction;
        CalcSatVariation(enterSat, outSat);

        isSit = false;
        isUsing = false;
        mGuestAnim.SetBool("isUsing", false);

        isGotoEntrance = true;
        mGuestAnim.SetBool("isSit", false);

        if (mGuestManager.mGuestInfo[mGuestNum].isDisSat == true)
        {
            mGuestAnim.SetBool("isDisSat", true);
            Invoke("ChangeTarget", 3.0f);
        }
        else if(mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction >= 5)
        {
            mGuestAnim.SetBool("isFullSat", true);
            Invoke("ChangeTarget", 4.0f);
        }
        else
        {
            Invoke("ChangeTarget", 3.0f);
        }
        ChangeLayerToDefault();

        // TODO : �ݶ��̴� ���� Sitting -> Walking
        sitCollider.enabled = false;
        walkCollider.enabled = true;

        // �ο����� ���� �ε����� �ʱ�ȭ
        mGuestManager.mGuestInfo[mGuestNum].mSitChairIndex = -1;
    }
    private void CalcSatVariation(int enterSat, int outSat)
    {
        if (enterSat > outSat) { mGuestManager.mGuestInfo[mGuestNum].mSatVariation = -1; }           // ������ ����
        else if (enterSat == outSat) { mGuestManager.mGuestInfo[mGuestNum].mSatVariation = 0; }     // ������ ����
        else { mGuestManager.mGuestInfo[mGuestNum].mSatVariation = 1; }                             // ������ ����
    }


    private void ChangeTarget()
    {
        this.GetComponent<AIDestinationSetter>().target = mSOWManager.mWayPoint[0].transform;
    }

    public void ChangeLayerToSit()
    {
        this.GetComponent<SortingGroup>().sortingLayerName = "SittingGuest";
    }

    public void ChangeLayerToDefault()
    {
        this.GetComponent<SortingGroup>().sortingLayerName = "Guest";
    }
}
