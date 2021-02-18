using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBall : MonoBehaviour
{

    // 공을 던지기 위한 각종 변수
    bool is_drag_now_ = false;
    float catch_ball_distance_;
    public float catch_ball_throw_speed_ = 120;
    public float catch_ball_arch_speed_ = 100;
    public float catch_ball_speed_ = 1000;

    // 마우스 클릭시(모바일은 손가락으로 터치시)
    void OnMouseDown()
    {
        // 터치볼과 카메라의 거리를 계산
        catch_ball_distance_ = Vector3.Distance(transform.position, Camera.main.transform.position);
        // 드래그 중이다!!!
        is_drag_now_ = true;
    }

    // 마우스를 놨을때(모바일은 손가락을 땠을 때)
    void OnMouseUp()
    {
        // 화면의 터치볼에 중력을 적용
        GetComponent<Rigidbody>().useGravity = true;
        // 화면의 터치볼의 속도를 (공의 전면(Z축방향) * 공의 던지는 속도로)로 증가 시킴
        GetComponent<Rigidbody>().velocity += transform.forward * catch_ball_throw_speed_;
        // 화면의 터치볼의 속도를 (공의 윗 방향(Y축방향) * 공의 수직 속도로)로 증가 시킴
        GetComponent<Rigidbody>().velocity += transform.up * catch_ball_arch_speed_;
        // 드래그 중지다!!!
        is_drag_now_ = false;
    }


    // Update is called once per frame
    void Update()
    {

        // 터치볼이 드래그 중일때~(드래그 하는 로직)
        if (is_drag_now_)
        {
            // 카메라로 부터의 스크린의 점을 통해 레이를 반환한다.
            // 픽셀단위: 1번 => 왼쪽 하단의 화면이(0, 0) 2번 => 오른쪽 상단이(pixelWidth, pixelHeight)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // GetPoint 함수로 ray 정보와 * 해당 거리(유닛)의 위치을 반환 받는다.(여기선 터치볼의 이동 할 위치)
            Vector3 ray_point = ray.GetPoint(catch_ball_distance_);
            // 터치볼의 위치를 Lep 함수로 ray_point까지 계속 이동 시킨다.(공이 드래그 됨)
            transform.position = Vector3.Lerp(transform.position, ray_point, catch_ball_speed_ * Time.deltaTime);
        }

    }

    // 몬스터볼을 리셋 시키는 함수
    public void ResetCatchBall()
    {
        // 해당 프로퍼티 초기화
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Landscape 모드
        // transform.position = new Vector3(0, -60, -150);
        // Portrait 모드
        transform.position = new Vector3(123, 47, -150);
    }
}




/*
 
    =2=

    지금 작성한 코드는 캐치볼을 드래그해 던지는 내용으로 설명은 따로 하지 않겠다.(주석에 다 있음)

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    (참고)
    3D 프로그래밍이나 유니티로 프로그래밍을 시작한 학생이면 다 이해할것임. (우리반은 다 이해할것임...)
    ///////////////////////////////////////////////////////////////////////////////////////////////////

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 46. Sphere 오브젝트 설정 ) 그림 처럼,
    Sphere 오브젝트에서 [Add Component]를 클릭해 [Rigidbody], [CatchBall] 컴포넌트를 추가한다.
    [Rigidbody] 의 Use Gravity 는 해제하자 그래야 시작과 동시에 밑으로 안 떨어진다.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 47. 유니티에서 움직이는 캐치볼 ) 그림 처럼,
    실행한 뒤 캐치볼을 클릭해 위/아래/왼쪽/오른쪽으로 드래그하면 캐치볼이 움직인다. 
    이때, 놓으면 앞으로 날라간다.~

    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    (참고)
    유니티에서 실행시 Kudan.AR.KudanTracker 에서....
    NullReferenceException: Object reference not set to an instance of an object 이러한 에러가 난다.
    일단 무시해도 된다. (KudanTracker 컴포넌트를 가진 Kudan 카메라를 비활성화 해서 그런것임...)
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    이제 스마트폰에서 실행하기 위해 앞서 했었던 일련의 과정들을 다시 되돌리자.

    (1). Markerless Transform Driver On
    (2). Kudan 카메라 On
    (3). 테스트용 카메라 Off

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    (참고)
    실행해보면....
    엄청난 문제가 발생한다.;;
    Landscape 에서는 문제가 없다... (이 용도로 만들어 진듯...)
    하지만, 우린 Portrait 모드인데..엄청난 문제가 생긴다...직접 경험해보자...ㅜㅜ (화면은 Portrait 인데 카메라는 Landscape 로 동작;;;)
    (해결법) 이거 때문에, 나도 엄청 이것저것 해보니라 고생했는데 최적의 해결방안은...

    1. ( (Kudan)사신몬 고 설정 폴더에 (그림) 48. Portrait 해결방안 1 ) 그림 처럼,
    케릭터를 y 축으로 -90 도 돌린다. (또한 현재 라이트가 없으므로 캐치볼 색상이 어둡다..라이트를 추가해줬다...)

    2. ( (Kudan)사신몬 고 설정 폴더에 (그림) 48. Portrait 해결방안 2 ) 그림 처럼,
    Kudan Camera - Markerless Only 카메라를 일단 비 활성화 하자.. 이유는 게임 실행시 어짜피 유니티 에디터상에서 보이는 카메라 회전값이 아닌
    Landscape 모드로 변경되기 때문에 헷갈린다. (우린 테스트용 Camera로  Landscape 모드를 임시로 구현해 볼것임)

    3. 2. ( (Kudan)사신몬 고 설정 폴더에 (그림) 48. Portrait 해결방안 3 ) 그림 처럼,
    테스트용 Camera를 활성화하고 Z축으로 90도 회전 시켜서 작업용 Landscape 모드 카메라를 만들자.

    3. 2. ( (Kudan)사신몬 고 설정 폴더에 (그림) 48. Portrait 해결방안 4 ) 그림 처럼,
    작업을 진행할때 실제 스마트폰에서 표현되는 화면은 Game 뷰 이다. 따라서 Game 뷰를 기준으로 
    작업을 진행하면 된다. 주의 할 점은 우리가 작업한 캐치볼 또한 Landscape 모드에 맞춰서 Z 축으로 90도 회전 시켜야 한다.
    (안그러면, 공이 이상하게 나라간다...)

    4. 일련의 과정 이후,
       (1). Markerless Transform Driver On
       (2). Kudan 카메라 On
       (3). 테스트용 카메라 Off

       게임을 빌드해서 실행한다...

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    게임을 빌드해서 실행해 보면~

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 49. 실제 스마트폰에서 실행한 화면, 50. 실제 스마트폰에서 캐치볼을 던지는 화면) 그림 처럼,
    하단의 캐치볼을 포켓몬 고의 몬스터볼이라 생각하고 드래그해 사신을 향해 던져보자. 맞추기도 어렵고
    운 좋게 사신을 맞추더라도 스쳐 지나간다.

    그 이유는 현재 사신의 충돌 처리가 안됐기 때문이다.

    다시 유니티에서 테스트할 수 있게 다음과 같이 설정하자.

    (1). Markerless Transform Driver Off
    (2). Kudan 카메라 Off
    (3). 테스트용 카메라 On

    그 다음 ( (Kudan)사신몬 고 설정 폴더에 (그림) 51. Box Collider의 Size와 Center 설정 ) 그림 처럼,
    1. wizard_weapon_legacy DEMO을 선택하고 [Add Component] 버튼을 클릭해 [Box Collider]를 추가하자.
    2. 그림 처럼 데이터를 입력해 충돌 범위를 설정한다.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 53. Scene 뷰에서 본 Box Collider의 크기 ) 그림 처럼,
    에디터로 초록색 선이 적당히 사신 모델링을 둘러싸게 하자.

    우리가 추가한 Box Collider는 충돌되는 물체의 크기를 지정한 것이다. 이렇게 되면 이제 캐치볼과
    사신의 충돌 처리가 완료된 것이다.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 54. Box Collider 간의 충돌 ) 그림 참고.

    이제 실행해서 사신을 맞추면 이전처럼 사신을 통과하지 않고 사신 앞에 멈추거나 사신을
    맞고 튕겨져 나가는 것을 확인할 수 있다.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 55. 사신을 통과하지 않는다 ) 그림 참고.

    이제 두 물체가 부딪쳤을 때 처리를 코드로 작성하자.
    몬스터볼이 포켓몬에게 닿으면 몬스터볼로 빨려 들어가는 것처럼 하겠다.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 56. SasinMon 스크립트 생성 ) 그림 처럼,
    C# 스크립트를 SasinMon이라는 이름으로 하나 생성하자.

    (SasinMon =3=로 이동)

    =5=

    사신을 다시 배치한 것처럼 캐치볼도 다시 배치하는 코드를 작성하자.

    public void ResetCatchBall()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(123, 47, -150);
    }

    (SasinMon_Go_Main =6=로 이동)
   
*/
