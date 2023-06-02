using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBubbleScript : MonoBehaviour
{
	private Text tGuideText;

	private string[,] mDialog;

	[HideInInspector]
	private int mDialogIndex;		// 몇 번째 Dialog를 불러올 것인지 선정(외부에서 입력 받음)
	private int currentDialogNum;   // 최근 텍스트 넘버
	[HideInInspector]
	private int presentDialogNum;    // 현재 텍스트 넘버, currentDialogNum != presentDialogNum일 때, currentDialogNum <= presentDialogNum && Update Text

	private TutorialManager mTutorialManager;
	private	SOWManager		mSOWManager;

	void Awake()
	{
		tGuideText = transform.Find("Button").gameObject.transform.Find("Text").GetComponent<Text>();
		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
		mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

		InitDialog();
	}

	void Update()
	{
		ReadDialog();
	}

	public void SetDialogIndex(int idx = 0) { mDialogIndex = idx; }

	// Dialog 초기화, 비어있는 string은 " "으로 관리
	private void InitDialog()
	{
		mDialogIndex = 0;
		currentDialogNum = -1;
		presentDialogNum = 0;

		mDialog = new string[9, 10];

		//  mDialog 초기화
		for (int num1 = 0; num1 < 9; num1++)
		{ for(int num2 = 0; num2 < 10; num2++)
			{
				mDialog[num1, num2] = " ";
			}
		}

		LoadText();
	}


	private void ReadDialog()
	{
		if (currentDialogNum == presentDialogNum) { return; }
		else { currentDialogNum = presentDialogNum; }

		if(mDialog[mDialogIndex, currentDialogNum] == " ") {
			this.gameObject.SetActive(false);
			return;
		}

		// 첫 번째 날씨의 공간 화면 페이드 아웃
		// 튜토리얼 종료 확인 여부는 응접실 버튼에서 처리
		ProcessSpecialText();

		tGuideText.text = mDialog[mDialogIndex, currentDialogNum];
	}

	public void LoadText()
	{
		mDialog[0, 0] = "이곳 클라우드 팩토리는 고민을 가진 \"뭉티\"들이 방문하는 장소입니다.";
		mDialog[0, 1] = "뭉티는 사람들의 고민 속에서 태어난 존재입니다.";
		mDialog[0, 2] = "뭉티들은 사람들이 잠든 사이에 깨어나 사람들을 대신해 고민하고 감정을 표출해줍니다.";
		mDialog[0, 3] = "뭉티들을 맞이할 준비를 시작합시다.";
		mDialog[0, 4] = "응접실에 느낌표 표시가 있으면 손님이 왔다는 걸 의미합니다.";
		mDialog[0, 5] = "응접실에 가서 누가 왔는지 확인해봅시다.";
		// 화살표 UI
		mDialog[0, 6] = "FadeOut00";

		mDialog[1, 0] = "이곳은 응접실 입니다. 클라우드 팩토리를 찾아온 손님들의 이야기를 들을 수 있는 장소입니다.";
		mDialog[1, 1] = "마침 클라우드 팩토리의 첫 손님이 오셨네요. 손님의 이야기를 귀담아 들어볼까요?";
		mDialog[1, 2] = "DialogSpace1";
		mDialog[1, 3] = "색이 있는 단어들은 손님이 필요로 하는 감정입니다.";
		mDialog[1, 4] = "<color=\"red\">붉은색</color> 글씨는 현재 손님에게 불필요하게 과다한 감정을 나타냅니다.";
		mDialog[1, 5] = "<color=\"green\">초록색</color> 글씨는 현재 손님이 부족하다고 느끼는 감정을 나타냅니다.";
		mDialog[1, 6] = "DialogSpace2";
		mDialog[1, 7] = "클라우드 팩토리의 첫번째 손님을 받아봅시다.";
		// 화살표 UI
		mDialog[1, 8] = "FadeOut10";

		mDialog[2, 0] = "받은 손님은 날씨의 공간에서 구름을 기다립니다.";
		// Demo Version
		/*
        mDialog[2, 1] = "손님이 자리에 앉아있을 때 클릭하면, 손님의 감정을 한 번 더 확인할 수 있습니다.";
        // 화살표 UI
        mDialog[2, 2] = "FadeOut20";
        */
		mDialog[2, 1] = "구름을 제작할 때 필요한 재료를 채집해봅시다.";
		mDialog[2, 2] = "Gathering";
		mDialog[2, 3] = "이제 구름을 만들러 가볼까요?";
		// 화살표 UI
		mDialog[2, 4] = "FadeOut21";

		mDialog[3, 0] = "구름은 구름 제작 기계를 통해 제작할 수 있습니다.";
		// 화살표 UI로 수정
		mDialog[3, 1] = "FadeOut30";

		mDialog[4, 0] = "채집한 재료들을 모두 사용해 구름을 만들어봅시다.";
		// 재료 모두 선택 시 화살표 UI로 수정
		mDialog[4, 1] = "FadeOut40";
		mDialog[4, 2] = "구름이 만들어졌으니 다시 공장으로 돌아가봅시다.";
		// 돌아가기 버튼 화살표 UI로 수정
		mDialog[4, 3] = "FadeOut41";

		mDialog[5, 0] = "방금 만든 구름이 기계에서 나오고 있습니다. 구름이 성공적으로 나왔을 때 구름을 눌러 장식해봅시다.";
		// 구름이 모두 나왔을 때, 화살표 UI
		mDialog[5, 1] = "FadeOut50";

		// fade out emotion list
		mDialog[6, 0] = "위쪽의 장식은 클릭하면 선택되고, 원하는 위치에 가져다 놓을 수 있습니다.";
		mDialog[6, 1] = "FadeOut60";
		mDialog[6, 2] = "구름과 장식으로 표현된 각 감정은 손님에게 긍정적인 영향을 미칠 수도, 부정적인 영향을 미칠 수도 있습니다.";
		mDialog[6, 3] = "이곳에서는 각 감정이 손님에게 어떤 영향을 미칠지 선택할 수 있습니다.";
		mDialog[6, 4] = "손님은 슬픔을 경험하고 싶어 했으니, 슬픔에 '+'가 되도록 해봅시다.";
		mDialog[6, 5] = "GuideSpeechOut60";
		// 화살표 UI
		mDialog[6, 6] = "다른 감정도 모두 결정하고 장식을 배치한 뒤, 꾸미기를 마쳐봅시다.";
		mDialog[6, 7] = "FadeOut61";
		// 돌아가기 버튼 막기

		// 화살표 UI
		mDialog[7, 0] = "완성된 구름은 구름 보관함에 저장됩니다.";
		mDialog[7, 1] = "각 구름은 보관 기간이 정해져 있습니다. 보관 기간이 지나면 구름이 사라집니다.";
		mDialog[7, 2] = "구름을 우클릭 하는 동안 어떤 재료를 사용하였는지 확인할 수 있습니다.";
		mDialog[7, 3] = "FadeOut70";
		// 우클릭 이벤트 추가 필요
		mDialog[7, 4] = "구름을 선택 해 제공해봅시다.";
		// 화살표 UI
		mDialog[7, 5] = "FadeOut71";
		mDialog[7, 6] = "제공하기 버튼을 눌러 뭉티에게 구름을 제공합시다.";
		mDialog[7, 7] = "FadeOut72";

		mDialog[8, 0] = "FadeOut80";
		mDialog[8, 1] = "구름을 받고 감정에 변화가 생긴 손님은 집으로 돌아갑니다. 조만간 다시 방문할지도 모르겠네요.";
		mDialog[8, 2] = "FadeOut81";
		mDialog[8, 3] = "모든 가이드를 마쳤습니다. 여러 뭉티의 안식처가 될 수 있는 클라우드 팩토리가 되길 바랍니다.";
		mDialog[8, 4] = "End";
	}

	public void UpdateText()
	{
		// Demo Version
		/*
		// 응접실 튜토리얼 후, 날씨의 공간에서 뭉티가 자리에 앉기 전까지 텍스트 넘김을 막는다.
		if (mTutorialManager.isFinishedTutorial[1] == true
			&& mTutorialManager.isFinishedTutorial[2] == false
			&& mSOWManager.mUsingGuestObjectList.Count > 0
			&& mSOWManager.mUsingGuestObjectList[0].GetComponent<GuestObject>().isSit == false
			&& presentDialogNum >= 2)
		{ return; }
		*/
		presentDialogNum++;
	}

	public void ProcessSpecialText()
	{
		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut00")
		{
			GameObject.Find("GuestManager").GetComponent<Guest>().isTimeToTakeGuest = true;
			GameObject.Find("GuestManager").GetComponent<Guest>().TakeGuest();
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Drawing Room").transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_Drawing Room").transform.localPosition, -200f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "DialogSpace1")
		{
			GameObject.Find("DialogManager").GetComponent<DialogManager>().ReadDialog();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}
		if (mDialog[mDialogIndex, currentDialogNum] == "DialogSpace2")
		{
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut10")
		{
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_OK").transform.localPosition, 300f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "Gathering")
		{
			mTutorialManager.FadeOutSpaceOfWeather();
			GameObject.Find("B_GardenSpring").transform.SetAsLastSibling();
			GameObject.Find("GatherGroup").transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut20")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut21")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Cloud Factory").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut30")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_GiveCloud").transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_GiveCloud").transform.localPosition, 300f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut40")
		{
			presentDialogNum++;
			currentDialogNum++;
			mTutorialManager.InstantiateArrowUIObject(new Vector3(410f, -360f, 0f), 0f);
			mTutorialManager.SetActiveArrowUIObject(false);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut41")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Back").transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_Back").transform.localPosition, -215f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut50")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_GiveCloud").transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_GiveCloud").transform.localPosition + new Vector3(180f, 32f, 0f), 150f);
			mTutorialManager.SetActiveArrowUIObject(false);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut60")
		{
			presentDialogNum++;
			currentDialogNum++;
			mTutorialManager.FadeOutDecoCloud();
			this.gameObject.transform.SetAsLastSibling();
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "GuideSpeechOut60")
		{
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("ButtonGroup(1)").transform.Find("PosButton").transform.localPosition, -200f, -260f);
			Debug.Log(GameObject.Find("ButtonGroup(1)").transform.Find("PosButton").transform.localPosition);
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut61")
		{
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut70")
		{
			mTutorialManager.FadeOutCloudStorage0();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut71")
		{
			mTutorialManager.FadeOutCloudStorage();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut72")
		{
			mTutorialManager.FadeOutScreen();
			GameObject tempButton = GameObject.Find("I_ProfileBG1").transform.Find("B_CloudGIve").gameObject;
			tempButton.transform.SetParent(GameObject.Find("Canvas").transform);
			tempButton.transform.SetAsLastSibling();
			mTutorialManager.InstantiateArrowUIObject(GameObject.Find("Canvas").transform.Find("B_CloudGIve").gameObject.transform.localPosition, -200f);
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut80")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut81")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "End")
		{
			mTutorialManager.isFinishedTutorial[8] = true;
			mTutorialManager.isTutorial = false;
			mTutorialManager.DestroyAllObject();

			// 튜토리얼 데이터 저장함수
			SaveUnitManager mSaveUnitManager = GameObject.Find("SaveUnitManager").GetComponent<SaveUnitManager>();
			if (null == mSaveUnitManager)
				return;
			mSaveUnitManager.Save_Tutorial();

			return;
		}
	}
}
