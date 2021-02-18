using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Kudan 사용을 위한 using 선언
using Kudan.AR;

public class SasinMon_Go_Main : MonoBehaviour
{

    // KudanTracker을 가리키는 레퍼런스
    public KudanTracker kudan_tracker_;

    // 사신과 몬스터볼을 가리킬 레퍼런스
    public SasinMon obj_sasin_mon_;
    public CatchBall obj_catchball_;

    // Use this for initialization
    IEnumerator Start()
    {

        // Kudan AR 초기화를 위한 대기
        yield return new WaitForSeconds(1.0f);

        // 사신의 출현을 위한 바닥 정보
        Vector3 floor_position;
        Quaternion floor_orientation;

        // 카메라가 바라보는 화면상에 바닥 정보를 저장.(out 키워드로 외부변수에 값 저장)
        kudan_tracker_.FloorPlaceGetPose(out floor_position, out floor_orientation);
        // ArbiTrackStart 함수로 트래킹 위치 정보를 KudanTracker 컴포넌트에 전달. 따라서
        // 카메라가 해당 위치를 바라보면 트래킹 됨~ (이 내용을 실제 개발시 실시간 업뎃 되야함)
        kudan_tracker_.ArbiTrackStart(floor_position, floor_orientation);

    }

    // GUI 콜백 함수
    void OnGUI()
    {
        // 사신 생성 버튼
        if (GUI.Button(new Rect(0, 100, 100, 50), "사신 출몰"))
        {
            Debug.Log("사신 출몰");
            SpawnSasin(); // 사신 다시 생성
        }

        // 캐치볼 생성 버튼
        if (GUI.Button(new Rect(0, 200, 100, 50), "캐치볼 리셋"))
        {
            Debug.Log("캐치볼 리셋");
            obj_catchball_.ResetCatchBall(); // 캐치볼 리셋
        }
    }

    void SpawnSasin()
    {
        // 사신의 출현을 위한 바닥 정보
        Vector3 floor_position;
        Quaternion floor_orientation;

        // 카메라가 바라보는 화면상에 바닥 정보를 저장.(out 키워드로 외부변수에 값 저장)
        kudan_tracker_.FloorPlaceGetPose(out floor_position, out floor_orientation);
        // ArbiTrackStart 함수로 트래킹 위치 정보를 KudanTracker 컴포넌트에 전달. 따라서
        // 카메라가 해당 위치를 바라보면 트래킹 됨~ (이 내용을 실제 개발시 실시간 업뎃 되야함)
        kudan_tracker_.ArbiTrackStart(floor_position, floor_orientation);
    }

}

/*
   =1=
    Kudan의 AR 클래스를 이용하기 위해 클래스 밖 상단에 using Kudan.AR을 꼭 추가해야 한다.
    그리고 Kudan Camera - Markerless Only 오브젝트에 붙어있는 Kudan Tracker를 사용하기 위해
    public으로 멤버 변수에 추가한다.

    유니티로 이동해 
    ( (Kudan)사신몬 고 설정 폴더에 (그림) 36. SasinMon_Go_Main 스크립트에서 제어할 수 있게 된 Kudan 카메라 그림 처럼,
    Kudan Camera 오브젝트를 kudan_tracker_의 대상으로 지정 한다.

    MonoBehaviour를 생성할 때 기본으로 포함돼 있는 Start와 Update 메서드를 제거한 뒤(최적화, Start는 코루틴으로...)
    다음과 같이 코드를 작성한다.

    public KudanTracker kudan_tracker_;

	IEnumerator Start () {

        yield return new WaitForSeconds(1.0f);

        Vector3 floor_position;
        Quaternion floor_orientation;

        kudan_tracker_.FloorPlaceGetPose(out floor_position, out floor_orientation);
        kudan_tracker_.ArbiTrackStart(floor_position, floor_orientation);

	}

    첫째 줄에서 다음과 같은 코드로 1초간 대기시키는 것을 볼 수 있다.

    yield return new WaitForSeconds(1.0f);

    Kudan AR 초기화가 완료됐는지를 구분하는 방법이 아직 없으므로 Kudan AR의 초기화를 기다리기
    위해서 사용하는 것인데, 실제로 사용하면 상당히 위험한 코드지만 시작하자마자 특정 로직을
    실행하기 위해 이렇게 작성한 것이다.

    그 다음 두 줄을 보면 변수가 선언된 것을 볼 수 있다. 이 변수는 바닥의 위칫값을 Vector로 나타낸 것이고
    아래 변수는 바닥의 회전값을 Quaternion(사원수)로 나타낸 것이다. 이 값들을 이해하려면 수학 책과
    3D 게임 프로그래밍 책의 예제를 들면서 설명해야 하니 이런 것들을 의미하는 변수라고만 생각하고
    넘어가면 된다.(우린 다 했음)

    Vector3 floor_position;
    Quaternion floor_orientation;

    다음 메서드는 스마트폰 카메라로 현재 바닥의 위치와 방위 값을 얻어온다.

    kudan_tracker_.FloorPlaceGetPose(out floor_position, out floor_orientation);

    FloorPlaceGetPose로 얻어온 값들로 ArbiTrackStart 함수를 호출해 해당 값을 이용한 트래킹을 시작하는 것이다.
    FloorPlaceGetPose로 현재 화면 바닥의 위치 정보를 얻어온 뒤 그곳을 계속해서 트래킹한다.

    kudan_tracker_.ArbiTrackStart(floor_position, floor_orientation);

    빌드한 뒤 실행해서 어떻게 동작하는지 확인해보자.

    // (참고) https://wiki.kudan.eu/ArbiTrack_Basics#Stopping_ArbiTrack 

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    (참고)
    만약 유니티에서 실행하면 ArbiTrack is not supported on this platform 이러한 경고가 뜬다...즉 이 함수는 
    유니티 에디터에서는 동작안한다. 따라서 빌드해서 실행하자.

    모바일에서 빌드에서 실행했는데...사신이 나오긴하지만 아마 검은 화면이 나올거다 ....
     ( (Kudan)사신몬 고 설정 폴더에 (그림) 37. Multithreaded Rendering 해제 ) 그림 처럼,
    [PlayerSettings] => 안드로이드 [Other Settings]로 이동해 Multithreaded Rendering의 체크를 해제하자.(나두 한참 고생함 ㅜ)
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 38. 책상 위에 올라가 있는 사신 모델 ) 그림 처럼,
    빌드하면 화면에 사신이 잡히고 그것을 중점으로 위치를 이동해도 사신이 그 자리에 있다.
    특정 환경이나 카메라에서는 해당 값이 튀어서 모델이 화면 밖을 벗어날 가능성도 있다.
    실제 제품을 개발할 때는 이런 것들은 예외 처리를 따로 해야 한다.

    Kudan으로 ( (Kudan)사신몬 고 설정 폴더에 (그림) 38. 책상 위에 올라가 있는 사신 모델 ) 그림 처럼,
    이미지 마크 없이 화면에 증강 현실로 사신을 띄워보았다. 다음으로는 포켓몬 고처럼 사신을 
    몬스터볼로 잡을 수 있는 작업을 진행하겠다.

    3. 사신을 잡아라!!! :

    화면에 사신을 띄워줬으니 이제 포켓몬 고의 몬스터볼처럼 무언가 던져서 사신을 잡는 것을
    개발해보자.

    이제부터 코드 입력이 많다. 이해하기 힘든 부분이 있어도 따라하면서 진행하자. 

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 39. Sphere 오브젝트 생성 ) 그림 처럼,
    [Sphere]를 하나 생성하자.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 40. 카메라의 위치 설정 ) 그림 처럼,
    카메라의 위치를 조금 뒤쪽으로 변경해 사신에게 캐치볼을 던져 맞추기 적당한 위치로
    옮겨주자.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 41. Sphere(캐치볼)의 위치 설정 ) 그림 처럼,
    캐치볼의 위치도 던지기 적당하게 설정하겠다.

    캐치볼을 던지는 테스트를 위해 Kudan SDK의 기능을 끄겠다. 
    ( (Kudan)사신몬 고 설정 폴더에 (그림) 42. Markerless Transform Driver 비활성화 ) 그림 처럼,
    사신 모델의 상위 오브젝트인 Markless_Driver에서 Markerless Transform Driver의 enable을 false로 설정해 비활성화하자.
    이렇게 설정하면 트래킹과 별개로 작동한다.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 43. 포켓몬 고와 비슷하게 구성된 씬 ) 그림 참고.

    
    ( (Kudan)사신몬 고 설정 폴더에 (그림) 44. 캐치볼 테스트 카메라 생성 ) 그림 처럼,
    실행했을 때 정상적으로 작동하면 좋겠지만 Kudan 카메라에서는 Kudan에서 제공하는 Transform을 사용하지 않으면
    정상적으로 출력이 안 되므로 카메라를 새로 생성해 기존의 위치로 이동시킨 뒤 실행하자.
    (빌드시 그렇다는거고 웹캠 + 에디터는 상관 없다.)

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 45. CatchBall 스크립트 생성 ) 그림 처럼,
    포켓몬 고의 몬스터볼처럼 사용될 캐치볼 스크립트를 생성하자.

    CatchBall 스크립트 파일에 코드를 작성하자.

    (CatchBall =2=로 이동)

    =4=

    아래와 같이 해당 코드를 작성 후, 실행해서 버튼의 위치를 확인하자.

    void OnGUI()
    {
        // 사신 생성 버튼
        if (GUI.Button(new Rect(0, 100, 100, 50), "사신 출몰"))
        {
            Debug.Log("사신 출몰");
        }

        // 캐치볼 생성 버튼
        if (GUI.Button(new Rect(0, 200, 100, 50), "캐치볼 리셋"))
        {
            Debug.Log("캐치볼 리셋");
        }
    }

    아래와 같이 사신과 캐치볼을 제어하기 위해 각각을 멤버 변수로 추가하자.

    public SasinMon obj_sasin_mon_;
    public CatchBall obj_catchball_;

    사신 위치를 초기화하는 코드를 다음과 같이 작성하자.

    void SpawnSasin()
    {
        Vector3 floor_position;
        Quaternion floor_orientation;

        kudan_tracker_.FloorPlaceGetPose(out floor_position, out floor_orientation);
        kudan_tracker_.ArbiTrackStart(floor_position, floor_orientation);
    }

    (CatchBall =5=로 이동)

    =6=

    다시 배치하는 두 개의 메서드를 작성했다면 이제 두 버튼으로 이 메서드를 실행하도록 코드를 작성하자.

    void OnGUI()
    {
        // 사신 생성 버튼
        if (GUI.Button(new Rect(0, 100, 100, 50), "사신 출몰"))
        {
            Debug.Log("사신 출몰");
            SpawnSasin(); // 사신 다시 생성
        }

        // 캐치볼 생성 버튼
        if (GUI.Button(new Rect(0, 200, 100, 50), "캐치볼 리셋"))
        {
            Debug.Log("캐치볼 리셋");
            obj_catchball_.ResetCatchBall(); // 캐치볼 리셋
        }
    }


    ( (Kudan)사신몬 고 설정 폴더에 (그림) 63. 사신과 캐치볼을 추가 ) 그림 처럼,
    SasinMon_Go_Main 스크립트가 사신과 캐치볼을 제어할 수 있게 지정하자.

    수고 많았음. 사신몬 고가 완성되었음.
    ( (Kudan)사신몬 고 설정 폴더에 (그림) 64. 완성된 사신몬 고 ) 그림 참고.



    스킬을 더 올릴 수 있는 도전 과제가 존재하지만 수업에서 알려드리는 가상 * 증강 현실 개발 수업은
    여기서 마친다. 여기까지 따라와줘서 정말로 고생많았음.








    도전 과제 1. 돌맹이를 이쁜 캐치볼로 바꾸자
    현재 캐치볼은 유니티에서 기본으로 제공하는 구로 만든 것이다. 제가 제공하는 파일들 중 캐치볼 모델을 불러와 구 대신
    캐치볼로 사용하자.

    Hint (배틀 카드 대전 만들기에서 FBX 파일을 불러오는 방법)

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 65. 몬스터볼로 디자인이 교체된 캐치볼 ) 그림 참고.


    도전 과제 2. 자연스러운 반복 만들어주기
    현재 좌측에 존재하는 사신 리셋과 캐치볼 리셋 버튼은 게임이라고 하기엔 자연스럽지 못한 것 같다.
    그렇다면 다음 조건을 만족시켜 자연스러운 반복을 유도하자.

    1. 사신을 맞출 경우 사신이 사라졌다가 일정 시간 뒤에 캐치볼과 사신 재생성
    2. 사신을 못 맞추고 바닥에 떨어졌을 때 캐치볼을 일정 시간 뒤에 재생성

    Hint (Coroutine 메서드 사용과 Plane으로 바닥 만들기, Box Collider)

    
    도전 과제 3. 추가적인 몬스터 등장하기
    지금은 사신만 등장하지만 임의로 진달래나 검을 등장시키자! 검은 정말 잡기 어려울 것 같다(게임성).

    Hint (배틀 카드 대전 만들기에서 FBX 파일을 불러오는 방법, Random 클래스)

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 66. 진달래가 사신군 고에 등장!!! ) 그림 참고.

    도전 과제 4. 게임 상태 만들어주기
    현재의 사신몬 고는 시작하자마자 바로 사신이 등장하고 잡게 돼 있다.
    타이틀 화면이나 다른 화면을 만들면 좋을 것 같다.(즉, 게임 처럼 만들라~)

    Hint (배틀 카드 대전 만들기에서 만든 게임 상태)

    도전 과제 5. 캐치볼 연출 추가하기 & 포획 확률 추가하기
    캐치볼을 맞추면 사신과 캐치볼이 화면 속에서 사라진다. 조금 더 완성도를 높이기 위해 캐치볼을 맞췄을 때의
    연출을 추가하고 일정 확률로 사신을 포획하는 데 실패하도록 만들어보자.
    다음의 조건을 만족하도록 도전해보자.

    1. 캐치볼과 사신이 충돌했을 때 사신이 서서히 캐치볼만큼 작아져서 안 보이게 한다.
     Hint (Transform Scale과 Coroutine 메서드)

    2. 작아진 후, 캐치볼이 좌우로 2~3회 반복해서 흔들린다.
    (좀더 실감나게 만들려면 내 수업자료보다 캐치볼과 사신을 좀더 크게 작업해주자~ 공과 거리도 좁히고~)

    3. 50% 확률로 사신이 풀려날 경우 작아서 안보였던 사신이 원래 사이즈로 커진다.
    Hint (Transform Scale과 Coroutine 메서드와 Random 클래스)

    4. 50% 확률로 사신을 잡을 경우 포획이라는 글씨를 출력한다.
    Hint (UI Canvas)

    = 고급 도전 과제 =
    이번 도전 과제는 조금 어렵다. 증강 현실 응용 분야에서 GPS를 이용하는 방법을 다뤄보는것도
    상당히 좋은 경험이 될 수 있으니 시간적인 여유가 있다면 한번 도전하길 바란다.

    모바일 기기에는 대부분 현재 위치를 알 수 있는 GPS가 장착돼 있다.

    포켓몬 고의 경우 이 GPS로 현재 위칫값을 서버와 통신해 해당 지역에 근접하면 특정 몬스터가
    나타나게 구현돼 있다.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 67. 위치 기반으로 등장하는 포켓몬들이 다르다 ) 그림 참고.

    서버와 통신 없이 GPS를 이용해서 특정 지역 근처일 때 특별한 몬스터를 띄워보자!!!!!!!!!! (GPS_Study.cs 참고)

    수고많았어요!!!

*/
