using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasinMon : MonoBehaviour
{

    // 몬스터볼과 충돌 처리
    void OnTriggerEnter(Collider _others)
    {
        Debug.Log(this.name + "과 " + _others.name + "이 충돌됨!!");
        Debug.Log("사신이 잡혔다!!");
        this.gameObject.SetActive(false);
    }

}


/*
 
    =3=

    아래와 같은 코드를 작성하자.

    // 몬스터볼과 충돌 처리
    void OnTriggerEnter( Collider _others )
    {
        Debug.Log( this.name + "과 " + _others.name + "이 충돌됨!!" );
    }

    코드를 작성 한 다음,
    ( (Kudan)사신몬 고 설정 폴더에 (그림) 57. wizard_weapon_legacy DEMO에 추가된 Sasin Mon 스크립트 ) 그림 처럼,
    wizard_weapon_legacy DEMO으로 돌아와,
    1. [Box Collider]에서 [Is Trigger]에 체크하자.
    2. [Add Component] 버튼을 클릭해 생성한 SasinMon 스크립트를 추가하자.

    플레이하고 캐치볼로 사신을 맞춘 뒤 콘솔을 확인하면,
    ( (Kudan)사신몬 고 설정 폴더에 (그림) 58. 두 물체가 충돌될 때마다 출력되는 로그 ) 그림과 같은
    로그가 출력된다.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 59. 캐치볼 이름 변경 ) 그림 처럼,
    캐치볼의 이름을 CatchBall이라 변경하자.

    SasinMon 스크립트 파일에서 사신이 잡혔다라고 출력되는 코드 밑에 GameObject를 비활성화 시키는 코드를
    다음과 같이 작성하자.

    void OnTriggerEnter( Collider _others )
    {
        Debug.Log( this.name + "과 " + _others.name + "이 충돌됨!!" );
        Debug.Log("사신이 잡혔다!!");
        this.gameObject.SetActive(false);
    }

    이제 플레이해 캐치볼과 사신이 만나는 즉시 사신이 사라지는 것을 확인하자.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 60. 충돌 시 없어지는 사신 ) 그림 참고.

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 61. 빌드모드 셋팅 ) 그림 처럼,
    테스트 모드를 비활성화한 뒤 빌드해 스마트폰에 넣고 실행해보자. 

    ( (Kudan)사신몬 고 설정 폴더에 (그림) 62. 학원 창가에서 만난 사신 ) 그림 참고.
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    (참고)
    위에 그림 처럼 Display Debug GUI 를 해지하면 테스트용 유저인터페이스가 사라진다. 
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    스마트폰에서도 정상적으로 실행되는 것을 확인했다. 다만 공을 한 번 던져서 사신을 못 맞추거나 혹은
    맞추더라도 다시 시작할 수 있는 기능이 없다. 앱 자체를 종료했다가 다시 실행해야 사신몬 고에서
    캐치볼을 다시 던질 수 있다.

    사신이 잡혔다면 새로 사신을 배치하는 코드와 캐치볼을 다시 던질 수 있는 코드가 필요하다.
    우선 SasinMon_Go_Main 스크립트 파일로 이동해 OnGUI 메서드에서 각각의 버튼을 생성하는 코드를 작성하자.

    (SasinMon_Go_Main =4=로 이동)




*/
