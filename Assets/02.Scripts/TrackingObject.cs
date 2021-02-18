using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 뷰포리아 사용을 위함
using Vuforia;

// ITrackableEventHandler 인터페이스 사용
public class TrackingObject : MonoBehaviour, ITrackableEventHandler {

    // ImageTargetBehaviour를 레퍼런스
    private TrackableBehaviour mTrackableBehaviour;
    public bool is_detected_ = false;

    public string name_; // 이름
    public int atk_;     // 공격력
    public int def_;     // 방어력
    public int hp_;      // 체력

    // 3D Text의 카드 내용을 실시간으로 코드에서 변경할 수 있게...
    public TextMesh obj_text_mesh_;

    // 애니메이션 컴포넌트 레퍼런스
    public Animation obj_animation_;


    // Use this for initialization
    void Start () {

        // ImageTargetBehaviour 컴포넌트 연결
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        // 만약 mTrackableBehaviour 가 NULL 이 아닐경우...즉 연결 될 경우 
        if(mTrackableBehaviour)
        {
            // 해당 함수로 하여금 OnTrackableStateChanged 이벤트 발생
            // 즉, DefaultTrackableEventHandler 함수에 OnTrackableStateChanged 이 함수를 연결 함
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        // 현재 스크립트의 설정된 이름과 체력을 3D Text의 카드 내용으로 지정
        obj_text_mesh_.text = name_ + "\nHP: " + hp_;

	}
	

    // ITrackableEventHandler 인터페이스 구현
    // 타깃 트래킹에 대한 이벤트가 발생하면 DefaultTrackableEventHandler 컴포넌트가 해당 이벤트 함수를 호출 함.
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        //throw new System.NotImplementedException();

        // 만약에 해당 이미지 타깃이 DETECTED, TRACKED 일때...
        if(newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)
        {
            is_detected_ = true;
        }
        else
        {
            is_detected_ = false;
        }
    }
}

/*
    ===================================작업 과정==============================================
    =1=
    일단 MonoBehaviour 외에 ITrackableEventHandler를 상속한 것을 볼 수 있다. 이 이벤트 핸들러를 상속한 뒤
    ImageTarget 게임오브젝트에 있는 TrackableBehaviour(ImageTargetBehaviour)에 다음과 같은 코드를 통해 앞으로 상태가 변하는 것에
    대해 알려달라고 등록한다. 즉, 카메라에 의해서 트래킹 되고 DefaultTrackableEventHandler 컴포넌트에 의해서
    OnTrackableStateChanged 이 함수가 호출 됨.

    mTrackableBehaviour.RegisterTrackableEventHandler(this);

    그 뒤 OnTrackableStateChanged 메서드에서 이벤트를 전달받는다.

    새로운 상태가 Status.DETECTED거나 Status.TRACKED이면 is_detected_ 플래그를 true로 하고,
    그렇지 않으면 false로 해서 외부 오브젝트들이 현재의 트래킹 상태를 알 수 있게 한거다.

    (앞에서 생성한 ARCardBattle 스크립트 파일로 이동해 코드를 작성하자.) (ARCardBattle =2=로 이동)

    =5=
    다음으로 공격 처리를 하기 위해 TrackingObject 스크립트 파일에 다음과 같은 변수들을 추가했다.(3D Text 에서 사용)

    public string name_; // 이름
    public int atk_;     // 공격력
    public int def_;     // 방어력
    public int hp_;      // 체력

    변수를 추가했다면 에디터로 이동해 ImageTarget_Dal과 ImageTarget_Sasin에 카드 정보를 추가하자.
    (뷰포리아 설정 폴더에 (그림) 93. Tracking Object에 카드 정보를 입력) 그림 참고

    두 개의 Tracking Object에 원하는 데이터를 모두 추가했다면, 이제 이러한 카드 정보들을 화면에 출력하는 작업을 하자.

    (뷰포리아 설정 폴더에 (그림) 94. 3D Text를 추가) 그림 처럼,
    Hierarchy에서 [Create] => [3D Object] => [3D Text]를 클릭해  3D Text 오브젝트를 생성하자.
    빌보드 텍스트라고 생각하면 된다.~~~!!!

    (뷰포리아 설정 폴더에 (그림) 95. 이미지 타깃의 하위 오브젝트로 만들어준 New Text 오브젝트) 그림 처럼,
    이 오브젝트를 이미지 타깃의 하위 오브젝트로 옮기고 Inspector에서 [Text Mesh]의 [Text]에 카드명과 HP를 넣는다.

    이렇게 작성하고 Scene 뷰를 클릭하면 (뷰포리아 설정 폴더에 (그림) 96. 사신 모델 위에 출력되는 사신의 정보) 그림 처럼,
    사신 모델 위에 텍스트가 출력된다. 만약에 텍스트가 출력되지 않는 사람은 크기나 위치를 조정해봐라.

    이제 3D Text의 카드 내용을 실시간으로 코드에서 변경할 수 있게 다음과 같이 코드를 추가했다.

    public TextMesh obj_text_mesh_;

    (뷰포리아 설정 폴더에 (그림) 97. New Text가 등록된 화면) 그림 처럼,
    코드를 작성했으면 New Text를 obj_text_mesh_로 등록하자.

    사신의 설정을 모두 완료했다. 진달래 또한 위와 같이 3D Text 오브젝트를 생성하고 이미지 타깃의 하위 오브젝트로
    넣은 후 카드 정보와 함께 Tracking Object에 등록하자. 그 뒤 TrackingObject 스크립트 파일로 이동해
    Start 메서드에 다음과 같이 코드를 작성하자.

    obj_text_mesh_.text = name_ + "\nHP: " + hp_;

    이 코드는 오브젝트가 처음 시작될 때 설정된 이름과 체력을 Text로 지정하는 코드이다.

    정상적으로 작동하는지 확인해보자.
    (뷰포리아 설정 폴더에 (그림) 98. 각각의 카드 정보가 정상적으로 출력되는 화면) 그림 참고

     (ARCardBattle =6=로 이동)

    =11=

    초기에 계획한 카드 배틀 게임을 완성했다. 하지만 단순히 화면에 숫자만 변하니 재미가 없다.

    TrackingObject에 다음과 같이 Animation을 멤버 변수로 추가하자.

    public Animation obj_animation_;

    그리고 (뷰포리아 설정 폴더에 (그림) 103. 애니메이션 지정) 그림 처럼,
    Animation을 Tracking Object에서 사용하게 지정한다.

    ARCardBattle 스크립트의 StartBattle 메서드로 이동하자.(ARCardBattle =12=로 이동)
 
 */
