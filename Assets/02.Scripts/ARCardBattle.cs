using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 뷰포리아 사용을 위함
using Vuforia;

// 게임 상태를 나타내는 열거형
public enum eGameState
{
    Ready = 0,
    Battle,
    Result
}

public class ARCardBattle : MonoBehaviour {

    // TrackingObject 컴포넌트를 레퍼런스 함 
    public TrackingObject obj_dal_;
    public TrackingObject obj_sasin_;

    // 게임 상태 변수
    public eGameState game_state_ = eGameState.Ready;

    // 시스템 메시지 변수
    public string system_message_ = "";

    // OnGUI를 이용한 유저인터페이스 구현
    private void OnGUI()
    {
        //// 진달래 타깃이 DETECTED 중 일때...
        //if (obj_dal_.is_detected_)
        //{
        //    // 버튼 생성
        //    GUI.Button(new Rect(15, 700, 120, 60), "Dal - Ready");
        //}
        //// 사신 타깃이 DETECTED 중 일때...
        //if (obj_sasin_.is_detected_)
        //{
        //    // 버튼 생성
        //    GUI.Button(new Rect(150, 700, 120, 60), "Sasin - Ready");
        //}

        // 다음 코드는 폰트의 사이즈와 컬러를 조정한다.
        GUIStyle gui_style = new GUIStyle();
        gui_style.fontSize = 60;
        gui_style.normal.textColor = Color.yellow;
        // 다음은 화면 위치 X : 250, Y : 55에 "State : " 라는 문자열 뒤에 game_state_.ToString()을 이용해
        // 해당 상태 값을 문자열로 바꿔 추가해서 정해준 스타일로 화면에 문자열을 출력하는 코드(함수)이다.
        //GUI.Label(new Rect(250, 55, 120, 20), "State : " + game_state_.ToString(), gui_style);

        // 다음 코드는 버튼의 스타일을 지정한다.
        GUIStyle gui_style_btn = new GUIStyle("Button");
        gui_style_btn.fontSize = 50;

        /*
            if 문과 그 안에 AND(&&) 조건식을 이용해 진달래와 사신이 탐지되고 게임 상태도 Ready일 때만 참의
            결과를 얻어 Start Battle이라는 버튼을 띄워준다.
        */ 
        if(obj_dal_.is_detected_ && obj_sasin_.is_detected_ && game_state_ == eGameState.Ready)
        {
            // Start Battle 버튼을 눌렀을 때...참이 되어
            if(GUI.Button(new Rect(640, 200, 250, 100), "Start Battle", gui_style_btn))
            {
                // 게임 상태를 Battle로 변경
                game_state_ = eGameState.Battle;

                // 시스템 메시지 설정
                system_message_ = "주사위로 선공정하기";

                // 연출 주사위 게임 실행
                StartCoroutine( RollTheDices() );
            }
        }

        // 게임이 대기 상태일때...
        if(game_state_ == eGameState.Ready)
        {
            system_message_ = "[게임 준비중]카드를 인식시켜주세요.";
        }

        // 게임이 결과 화면 상태일때...
        if(game_state_ == eGameState.Result)
        {
            // Refresh 버튼을 활성화 하고... 
            if(GUI.Button(new Rect(640, 200, 250, 100), "Refresh", gui_style_btn))
            {
                // 다시 게임을 준비 상태로...
                game_state_ = eGameState.Ready;

                // 초기화
                obj_dal_.obj_text_mesh_.text = obj_dal_.name_ + "\nHP: " + obj_dal_.hp_;
                obj_sasin_.obj_text_mesh_.text = obj_sasin_.name_ + "\nHP: " + obj_sasin_.hp_;
            }
        }

        // 다음은 화면 위치 X : 250, Y : 55에 system_message_ 문자열을  
        // 정해준 스타일로 화면에 출력하는 코드(함수)이다.
        GUI.Label(new Rect(250, 55, 120, 20), system_message_, gui_style);

    }

    // 연출 주사위 코루틴 함수
    IEnumerator RollTheDices()
    {
        //obj_dal_.obj_text_mesh_.text = "주사위 : " + 6;
        //obj_sasin_.obj_text_mesh_.text = "주사위 : " + 1;
        int last_dal_dice   = 0;
        int last_sasin_dice = 0;

        for(int i = 0; i < 30; i++)
        {
            last_dal_dice   = 1 + Random.Range(0, 6);
            last_sasin_dice = 1 + Random.Range(0, 6);

            obj_dal_.obj_text_mesh_.text   = "주사위 : " + last_dal_dice;
            obj_sasin_.obj_text_mesh_.text = "주사위 : " + last_sasin_dice;

            yield return new WaitForSeconds(0.1f);
        }

        // 누가 이겼을까?

        if(last_dal_dice > last_sasin_dice)
        {
            system_message_ = "진달래 선공";
            StartCoroutine(StartBattle(obj_dal_, obj_sasin_));
        }
        else if(last_dal_dice < last_sasin_dice)
        {
            system_message_ = "사신 선공";
            StartCoroutine(StartBattle(obj_sasin_, obj_dal_));
        }
        else if(last_dal_dice == last_sasin_dice)
        {
            system_message_ = "무승부 - 다시하기";
            StartCoroutine(RollTheDices());
        }

        yield return null;
    }

    // 카드 싸움 코루틴 함수
    IEnumerator StartBattle(TrackingObject _first_turn, TrackingObject _second_turn)
    {
        // 1초 대기 (연출 및 최적화)
        yield return new WaitForSeconds(1.0f);

        // 체력 정보 받기
        int first_hp  = _first_turn.hp_;
        int second_hp = _second_turn.hp_;

        // 체력 정보 갱신
        _first_turn.obj_text_mesh_.text = _first_turn.name_ + "\nHP: " + first_hp;
        _second_turn.obj_text_mesh_.text = _second_turn.name_ + "\nHP: " + second_hp;

        while(true)
        {
            // 선공의 턴
            _first_turn.obj_animation_.Play("Attack");
            yield return new WaitForSeconds(_first_turn.obj_animation_.GetClip("Attack").length);
            _first_turn.obj_animation_.Play("Idle");
            second_hp -= _first_turn.atk_;

            // 체력 정보 갱신
            _first_turn.obj_text_mesh_.text = _first_turn.name_ + "\nHP: " + first_hp;
            _second_turn.obj_text_mesh_.text =  _second_turn.name_ + "\nHP: " + second_hp;

            // 후공 죽음 처리
            if(second_hp <= 0)
            {
                // _second_turn 의 패배
                system_message_ = _first_turn.name_ + " 가 승리하였습니다.";
                break;
            }
            // 연출 및 최적화
            yield return new WaitForSeconds(1.0f);

            // 후공의 턴
            _second_turn.obj_animation_.Play("Attack");
            yield return new WaitForSeconds(_second_turn.obj_animation_.GetClip("Attack").length);
            _second_turn.obj_animation_.Play("Idle");
            first_hp -= _second_turn.atk_;

            // 체력 정보 갱신
            _first_turn.obj_text_mesh_.text = _first_turn.name_ + "\nHP: " + first_hp;
            _second_turn.obj_text_mesh_.text = _second_turn.name_ + "\nHP: " + second_hp;

            // 선공 죽음 처리
            if (first_hp <= 0)
            {
                // _first_turn 의 패배
                system_message_ = _second_turn.name_ + " 가 승리하였습니다.";
                break;
            }

            // 연출 및 최적화
            yield return new WaitForSeconds(1.0f);
        }

        // 게임 상태를 결과 상태로...
        game_state_ = eGameState.Result;
    }

}

/*
 
    ===================================작업 과정==============================================
    =2=
    참고로 나는 안드로이드를 이용해 예제를 만들었다. 안드로이드가 아닌 다른 디바이스로 실습하는 사람은
    버튼이나 라벨의 위치, 크기 등을 자신의 디바이스에 맞춰 조정하자.

    해당 코드는 생성한 TrackingObject를 이용해 진달래와 사신의 트래킹 상태값을 가져오고 제어하기
    위해 public으로 멤버 변수를 선언했다.

    그리고 OnGUI를 이용해 각각의 트래킹 상태를 나타내는 is_detected_가 참인 경우에는 아무런 동작을
    하지 않는 버튼을 등장시키고 해당 오브젝트들이 등장할 때마다(is_detected_가 참일때 즉 트래킹 상태가 true 일때...)
    각각 맞는 버튼이 등장하도록 작성한 코드이다.

    다음으로 각 이미지 타깃에 TrackingObject를 추가한다.
    (뷰포리아 설정 폴더에 (그림) 87. 각 이미지 타깃에 추가된 TrackingObject) 그림 참고

    (뷰포리아 설정 폴더에 (그림) 88. AR Card Battle에 등록된 2개의 TrackingObject) 그림 처럼,
    그 뒤로 1. 새로운 게임 오브젝트를 만들어서 GameManager라 정하고 2. ARCardBattle를 추가한 뒤
    3. 각 이미지 타깃을 끌어와 등록하자.

    설정 후 실행하면 (뷰포리아 설정 폴더에 (그림) 88. 진달래만 카메라에 잡힐 때 보이는 버튼, 89. 두 개 다 잡힐 때는 둘 다 출력이 된다) 그림 처럼,
    진달래만 트래킹될 때는 진달래의 버튼만 출력되고, 둘 다 트래킹될 때는 두 버튼이 다 등장하게 된다.

    =3=
    각각의 이미지 타깃이 트래킹되는지 아닌지를 확인하는 코드를 작성했으니 다음으로 카드 게임 각각의 상태를 만들었다.
    게임은 크게 3가지 게임 상태로 나눠 코드를 진행했다.

    * 준비 상태
    * 전투 상태
    * 종료 상태
 
    이 3가지 상태를 enum으로 추가했다.

    public enum eGameState
    {
      Ready = 0,
      Battle,
      Result
    }

    그리고 상태를 멤버 변수로 추가했다.

    public eGameState game_state_ = eGameState.Ready;

    OnGUI로 이동해 진달래와 사신을 트래킹해 버튼을 띄우는 코드를 제거하고 현재의 상태를 출력하는 코드를 작성했다.

     private void OnGUI()
    {
        //if (obj_dal_.is_detected_)
        //{
        //    // 버튼 생성
        //    GUI.Button(new Rect(15, 700, 120, 60), "Dal - Ready");
        //}
        //// 사신 타깃이 DETECTED 중 일때...
        //if (obj_sasin_.is_detected_)
        //{
        //    // 버튼 생성
        //    GUI.Button(new Rect(150, 700, 120, 60), "Sasin - Ready");
        //}

        GUIStyle gui_style = new GUIStyle();
        gui_style.fontSize = 60;
        gui_style.normal.textColor = Color.yellow;
        GUI.Label(new Rect(250, 55, 120, 20), "State : " + game_state_.ToString(), gui_style);
    }


    화면에 텍스트가 잘 출력되는지 확인해보자.(뷰포리아 설정 폴더에 (그림) 90. 게임 상태가 출력된 화면) 그림 참고

    =4=
    그 다음 Ready인 상태에서 진달래와 사신이 트래킹됐다면 Start Battle 버튼이 생성되게 코드를 작성했다.(코드 라인 59)

    그리고 Start Battle 버튼을 눌렀을 때 게임 상태를 Battle로 변경되도록 코드를 작성했음.(코드 라인 52, 62)

    game_state_ = eGameState.Battle;

    잘 동작하는지 확인해보자 (뷰포리아 설정 폴더에 (그림) 91. 두 개의 모델이 트래킹될 때 생성되는 Start Battle 버튼) 그림 참고

    여기 까지 했다면 두 개의 카드가 정상적으로 트래킹되면 자연스럽게 Start Battle 버튼이 생성된다.
    그리고 이 버튼을 누르면 (뷰포리아 설정 폴더에 (그림) 92. 게임 상태가 Battle로 변했다) 그림 처럼,
    State는 Battle로 변하고 두 카드를 트래킹하더라도 Start Battle 버튼이 더 이상 
    표시되지 않는다.(TrackingObject =5=로 이동)

    =6=
    다음으로 Start Battle 이후 선공을 정하기 위해서 랜덤 메서드로 주사위를 굴리는 것처럼 연출하고
    높은 수를 가진 사람이 선공하게 코드를 추가했다. (코드 라인 71, 103)

    Start Battle 버튼을 누르면 RollTheDices를 코루틴으로 실행.

    StartCoroutine( RollTheDices() );

    다음 코드는 코루틴을 이용하기 위해 작성했다. 메서드의 타입은 IEnumerator여야하고 반드시 yild 구문이
    들어가야 한다.

    IEnumerator RollTheDices()

    그리고 RollTheDices 메서드 내에서 진달래의 이름을 출력했던 곳에 "주사위 : 6"을, 사신의 이름을
    출력했던 곳에  "주사위 : 1"을 출력하도록 작성했다.

    obj_dal_.obj_text_mesh_.text = "주사위 : " + 6;
    obj_sasin_.obj_text_mesh_.text = "주사위 : " + 1;

    테스트 해보자, 실행한 뒤 두 개의 카드를 트래킹하고 Start Battle 버튼을 누르면
    (뷰포리아 설정 폴더에 (그림) 99. 주사위가 표시된 화면) 그림 처럼 "주사위 : 6" 과 "주사위 : 1" 이라는 
    텍스트가 출력된다.


    =7=
    다음으로 숫자를 임의로 변하게 한 뒤 멈추게 하는 연출을 해보았다.
    
    코드는 30번의 for 문을 돌면서 1~6까지의 랜덤한 숫자들을 0.1초 간격으로 보여주는 코드를 작성했다.(코드 라인 110)

    원하는 대로 실행되는지 확인해보자. 
    (뷰포리아 설정 폴더에 (그림) 99. 주사위 숫자가 임의로 돌아간 후의 화면) 그림 처럼
    잘 따라왔다면 숫자가 계속해서 변하다가 마지막에 멈춰서 고정된다.
    
    =8=
    다음으로 누가 이겼는지 비교하는 작업을 진행했다.

    우선 누가 이겼는지 화면에 출력하기 위해 OnGUI에 현재 게임 상태가 배틀 중일 때 시스템 메시지를 출력하도록
    코드를 추가했다.

    먼저 기존에 게임 상태를 출력하는 코드를 삭제한 뒤 시스템 메시지를 제어할 문자열을 다음과 같이 멤버 변수로 추가했다.

    1. //GUI.Label(new Rect(250, 55, 120, 20), "State : " + game_state_.ToString(), gui_style);

    2. public string system_message_ = "";

    이렇게 추가한 문자열을 게임의 상태가 준비 상태일 때는 "[게임 준비중]카드를 인식시켜주세요." 라는
    메시지가 출력되도록 대입한다. OnGUI() 함수 가장 마지막 줄에 이 시스템 메시지를 출력한다.

    if(game_state_ == eGameState.Ready)
    {
        system_message_ = "[게임 준비중]카드를 인식시켜주세요.";
    }

    카드가 인식되지 않았다면 "[게임 준비중]카드를 인식시켜주세요." 라는 메시지가 출력된다.
    (뷰포리아 설정 폴더에 (그림) 100. 카드가 인식 안 될 시 표시되는 화면) 그림 참고

    그리고 코루틴으로 RollTheDices를 호출하기 전에 시스템 메시지로 "주사위로 선공정하기"라는 문자열을
    대입해 주사위를 굴리는 것을 유저들에게 인식시켰다.

    system_message_ = "주사위로 선공정하기";
    StartCoroutine( RollTheDices() );

    자 위와 같이 작성되었으면 [Play] 버튼을 클릭해 두 개의 카드를 트래킹한 뒤 Start Battle 버튼을 눌러주자.

    (뷰포리아 설정 폴더에 (그림) 101. 전투 시작 후 출력되는 시스템 메시지) 그림 처럼
    주사위로 선공 정하기라는 메시지가 출력될것이다.

    =9=
    이렇게 되면 누가 이겼는지를 표시할 수 있는 시스템 메시지를 준비해야 한다. 이제 정말로 누가 이겼는지를
    처리하는 코드를 작성했다.

    코드에서 if ~ else if 문으로 누구의 주사위 숫자가 큰지 비교하는 구문을 만들었다.
    그리고 그 안에서 누가 이겼는지를 시스템 메시지로 출력하게 했다. 만약 무승부가 나면 코루틴으로
    다시 호출해 주사위를 굴리게 했다. (코드 라인 123)

    코드에서는 싸움을 처리할 StartBattle 코루틴을 생성했다. 파라미터로는 TrackingObject 타입을 두 개 받는데
    선공정하기 주사위 게임에서의 승자와 패자이다. 이 두 개의 파라미터를 받아 내부에서 선공이 누구인지 파라미터로
    넣으면 이 메서드 내에서 나머지를 처리한다. (코드 라인 143)

    RollTheDices 메서드에서 주사위 수를 비교하는 if ~ else if 문에 코드와 같이 StartBattle 코루틴을 실행하게 작성했다.

    이제 가장 핵심인 공격을 처리하는 StartBattle 메서드를 보자.
    선공이 먼저 공격을 하고 그 뒤로 번갈아 가면서 공격 한다. 이때 누군가의 HP가 0 이하가 되면 승패 메시지를 출력한다.

    코드를 자세히 살펴보자.

    StartBattle 메서드 내 첫 번째 줄에서 선공이 누구인지 표시하는 것에 약간 딜레이를 준다.

    yield return new WaitForSeconds(1.0f);

    그리고 선공과 후공, 두 TrackingObject에서 설정된 HP 값을 가져와서 저장한다. 따로 저장하는 이유는
    게임을 새로 시작했을 때도 설정한 HP 값이 동일해야 하므로 전투 중에 계산할 HP를 가져와 저장하는 것이다.

    int first_hp  = _first_turn.hp_;
    int second_hp = _second_turn.hp_;

    다음 코드는 진달래와 사신 모델 상단에 떠 있는 이름과 HP 정보를 갱신한다. 이 코드는 이 메서드가 끝날 때까지
    HP 데이터를 변경할 때마다 갱신하듯이 호출한다. 그래서 나중에 실제 게임을 만들 때는 따로 모듈화를 시켜주면 좋다^^.

    // 체력 정보 갱신
    _first_turn.obj_text_mesh_.text = _first_turn.name_ + "\nHP: " + first_hp;
    _second_turn.obj_text_mesh_.text = _second_turn.name_ + "\nHP: " + second_hp;

    while(true) 문으로 묶여 있는 것을 볼 수 있다. 이것은 내부에서 break가 되지 않는 이상 계속해서 반복하는 코드이다.
    한마디로 둘 중 하나가 죽을 때까지 계속 치고받는다는 의미이다.

    다음 코드는 후공의 체력에 선공의 공격력을 빼줘서 후공의 체력을 감소시키는 것이다.

    // 선공의 턴
    second_hp -= _first_turn.atk_;

    그리고 다음은 선공의 공격을 받은 후공의 체력이 0 이하인 경우 패배 처리를 하고 선공이 이겼다는
    메시지를 남겨주고 break 문을 통해 while(true) 문을 벗어나게 한다.

    if(second_hp <= 0)
    {
        // _second_turn 의 패배
        system_message_ = _first_turn.name_ + " 가 승리하였습니다.";
        break;
     }

    첫 줄과 같은 마지막 코드는 체력의 변화를 플레이어가 볼 수 있게 1초간 대기 시간을 줬다. 그리고
    선공의 공격과 후공의 체력에 대한 처리를 반대로 후공의 공격과 선공의 체력에 대한 처리로 똑같이 작성했다.

    빌드해서 전투가 정상적으로 진행되는지 확인해보자.
    (뷰포리아 설정 폴더에 (그림) 102. 전투의 결과가 출력된 화면) 그림 참고.

    현재는 진달래의 체력과 공격력이 사신보다 월등해 무조건 진달래가 이기지만,
    같은 체력과 공격력으로 두 TrackingObject를 설정한다면 주사위 게임에서 선공을 잡는 사람이 승리한다.
    물론 실제 게임에서는 랜덤한 변수를 추가해야겠지???

    =10=
    플레이해서 전투가 진행되는 것을 확인했다면 다시 게임을 시작할 수 있게 (코드 라인 82~94, 201)처럼 코드를 작성했다.

    전투가 끝난 뒤 다음 코드로 게임 상태를 종료 상태로 만들었다.

    game_state_ = eGameState.Result;

    OnGUI에서 게임 상태가 종료 상태인 경우 Refresh 버튼을 생성해 게임 상태를 다시 Ready로 돌리고 감소했던 체력들을
    모두 복구한다.

    // 게임이 결과 화면 상태일때...
    if(game_state_ == eGameState.Result)
    {
        // Refresh 버튼을 활성화 하고... 
        if(GUI.Button(new Rect(640, 200, 250, 100), "Refresh", gui_style_btn))
        {
            // 다시 게임을 준비 상태로...
            game_state_ = eGameState.Ready;

            // 초기화
            obj_dal_.obj_text_mesh_.text = obj_dal_.name_ + "\nHP: " + obj_dal_.hp_;
            obj_sasin_.obj_text_mesh_.text = obj_sasin_.name_ + "\nHP: " + obj_sasin_.hp_;
        }
    }

    (TrackingObject =11=로 이동)

    =12=

    (참고) 애니메이션 클립 이름을 소스처럼 바꿨다.

    다음과 같이 StartBattle 메소드에 코드를 추가했다.(코드 라인 159~161, 179~181)

    // 선공의 턴
    _first_turn.obj_animation_.Play("Attack");
    yield return new WaitForSeconds(_first_turn.obj_animation_.GetClip("Attack").length);
    _first_turn.obj_animation_.Play("Idle");

    // 후공의 턴
    _second_turn.obj_animation_.Play("Attack");
    yield return new WaitForSeconds(_second_turn.obj_animation_.GetClip("Attack").length);
    _second_turn.obj_animation_.Play("Idle");

    다음 코드는 공격(Attack) 애니메이션을 실행한다.

    _first_turn.obj_animation_.Play("Attack");

    다음은 공격 애니메이션의 길이만큼 대기한 뒤 Idle 애니메이션을 실행하는 코드이다.

    yield return new WaitForSeconds(_first_turn.obj_animation_.GetClip("Attack").length);
    _first_turn.obj_animation_.Play("Idle");

    후공도 방금 설명한 코드와 같은 맥락이다. 그럼 실행해보자~~
    (뷰포리아 설정 폴더에 (그림) 104. 공격 시 사신이 공격 애니메이션을 한다) 그림 참고.

    애니메이션을 추가하니 좀 더 게임다워졌다.

    여기까지 하면 증강 현실을 이용해서 TCG 게임을 만드는 기본적인 방법들을 모두 배웠다.

    이제 다음 챕터에서 포켓몬 고를 따라 사신 고를 만들어 볼 텐데, 만약 지금까지 만든 카드 게임을
    조금 더 업그레이드하고 싶다면 도전 과제를 하면서 증강 현실 게임 관련 능력을 키우고 다른 사람과
    함께 게임할 수 있게 만들어 보자. 초보 개발자라면 도전 과제 3까지만 도전하면 좋을 것 같다. 

    도전 과제 1. UI를 이미지로 예쁘게 포장하자
    지금 게임은 화면에 텍스트를 출력해 현재의 게임 상태를 알려주고 있다. 이를 이미지로 넣어 완성도를 높여보자.
    UI Canvas로 개발해서 추가적으로 업데이트 해보자. Hint (UI Canvas, Texture)

    도전 과제 2. 사운드를 추가해보자
    기본적인 카메라 오브젝트에는 [Audio Listener]라는 컴포넌트가 붙어있다. 이 컴포넌트는 한 씬에 한 개만 사용되며,
    씬 내의 사운드를 출력한다.

    (뷰포리아 설정 폴더에 (그림) 105. 카메라 오브젝트에 이미 존재하는 Audio Listener) 그림 처럼, ARCamera 도 기본적으로
    가지고 있다.

    유니티로 사운드를 출력하는 방법을 간단히 알려주겠다. 카메라에 [Audio Listener] 컴포넌트가 있는 것을 확인했다면
    다음으로 해당 사운드를 사용할 게임 오브젝트에서 [Add Component] 버튼을 클릭해 [Audio Source]라는 컴포넌트를 추가한다.
    그리고 컴포넌트 속 [Audio Clip]에는 출력하고 싶은 배경 음악이나 효과음 파일을 설정한다.

    (뷰포리아 설정 폴더에 (그림) 106. AudioSource 컴포넌트를 추가한다) 그림 참고.

    그 외에 도전 과제를 해결하기 위해 알아야 할 속성은 다음 두 개이다.

    (Tip) Audio Source의 주요 속성
    * Play On Awake : 오브젝트가 생성 시 사운드 실행
    * Loop          : 반복 여부(배경음이라면 반복된다.)
     
    이렇게 [Audio Source]를 설정했다면 다음과 같은 코드 형식으로 [Audio Source]에 설정된
    [Audio Clip]의 사운드를 출력한다.

    AudioSource source = (AudioSource가 존재하는 오브젝트).GetComponent<AudioSource>();
    if( source != null )
    {
        source.Play();
    }

    이렇게 사운드를 출력하는 방법으로 배경 음악이라든지, 공격할 때 혹은 맞았을 때, 승리했을 때의 사운드를 출력해
    완성도를 높여보자.
   
    이건 기본이고...(혹시나 해서..)

    도전 과제 3. 다른 카드를 추가하거나 밸런스를 조정해보자
    현재 만든 카드 게임은 사실 재미없다. 2개의 카드밖에 존재하지 않는데 진달래가 사신보다 압도적으로
    강해 승패가 이미 결정나는 게임이다. 그래서 이번에는 새로운 카드를 추가해 게임에 다양성을 주는 도전을 해보자.

    (뷰포리아 설정 폴더에 (그림) 107. 도검 카드를 만들어 보았다) 그림 참고.

    도전 과제 4. TCG의 시스템을 설계하고 개발해보자
    사실 도전 과제 3까지만 해도 정말 잘한 것이다. 이 외에 도전 과제 4는 수업의 방향성과 거리가 멀지만,
    증강 현실 게임 개발을 공부한다면 모든 챕터를 마스터 한 뒤에 도전하면 굉장히 좋을 것이다.

    기본적인 요구 조건은 다음과 같다.

    1. 게임판이 존재, 플레이어는 앞에 각각 3장의 카드를 놓을 수 있다.

    2. 각 카드는 어느 플레이어의 카드인지 구분된다.

    Hint (게임판-이미지 타기팅, Box Collider)

    3. 유닛형 카드 외에 다른 기능을 하는 카드가 존재한다.(아이템 카드)
    (예: 도검 카드는 필드에 나타나면 상대방 유닛의 HP 값을 100 줄이고 소멸한다)

    4. 특정 조건으로 승과 패가 나뉜다.

    이상의 4가지 기본 요구 조건들은 가상 * 증강 현실 예제를 따라하며 배운 것을 응용하면 충분히
    개발할 수 있을 것이다. 요구 조건들을 만족시킨다면 증강 현실을 이용한 어떠한 종류의 카드 게임도
    만들 수 있을 거라고 장담한다. 분명히 말하지만 도전 과제 4는 모든 챕터를 다 마스터 한 뒤에 도전하자!!!.

    (뷰포리아 설정 폴더에 (그림) 108. TCG 버전, 배틀 카드게임) 그림 참고.


 */
