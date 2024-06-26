# Tangreed
 Chapter 3-3 Unity 게임개발 심화 팀 프로젝트

<br>

![image](https://github.com/Yoonwoojoo/Raising-Archer-Public/assets/167274465/dabdb25b-3a39-4fbb-be81-0b381a2011ad)

플레이 영상 : https://youtu.be/bNdYqCRNILg
<br>
브레인 스토밍 및 다이어 그램 : https://excalidraw.com/#room=a48660670f01c5d292a2,ijpBwSFn2Wiyjle6cDgU2g
<br><br>

## :airplane: 프로젝트 소개

### 2D 횡스크롤 액션 로그라이크
횡스크롤 플랫폼 게임으로, 던전에 입장해서 무기를 얻고 능력치를 향상시키며, 보스를 처치하면 클리어 하는 방식


<br><br>

## :thought_balloon: 구현 기능

### 필수 요구 사항

#### 1. **주인공 캐릭터의 이동 및 기본 동작**
    - 키보드 또는 터치 입력을 통해 주인공 캐릭터를 움직이고, 점프가 가능
    - 이동 및 점프 애니메이션을 포함하여 부드러운 이동 효과를 구현

#### 2. **레벨 디자인 및 적절한 게임 오브젝트 배치**
    - 최소한 2개 이상의 게임 레벨을 디자인하고 구현
    - 각 레벨에는 적절한 플랫폼, 장애물, 보상 아이템 등이 포함

#### 3. **충돌 처리 및 피해량 계산**
    - 주인공 캐릭터와 환경 또는 적 캐릭터 사이의 충돌을 처리하고, 피해량을 계산하여 게임 내에서 적절한 게임 오브젝트를 활용

#### 4. **UI/UX 요소**
    - 플레이어와 보스의 HP바 및 인벤토리와 아이템 UI 등
      
    
<br><br>

### 추가 구현 사항

#### 1. **다양한 적 캐릭터와 그들의 행동 패턴 추가**
#### 2. **다양한 무기나 아이템 추가**
#### 3. **다양한 환경과 배경 설정**
#### 4. **다양한 엔딩 및 스토리 브랜치**
#### 5. **사운드 및 음악 효과 추가**
#### 6. **게임 패턴 분석 및 밸런싱**


<br><br>

## :timer_clock: 프로젝트 수행 절차

![image](https://github.com/Yoonwoojoo/Tangreed/assets/167274465/c931b189-4e02-4988-a9e5-5cc20cfcdd3b)
![image](https://github.com/Yoonwoojoo/Tangreed/assets/167274465/85e5f81d-677d-457a-bfe1-d1ae5e3f6757)
![image](https://github.com/Yoonwoojoo/Tangreed/assets/167274465/82cb2de1-e7fc-4a8e-9e22-6bcd3c25f1d0)
![image](https://github.com/Yoonwoojoo/Tangreed/assets/167274465/dc319bdc-83c6-4756-97cc-df547f4b69b8)


<br><br>

## :notebook: 기술 스택
#### Generic Singleton
제너릭 싱글톤을 활용하여 다양한 매니저들을 관리
<br>

#### Scriptable Object
스크립테이블을 활용하여 플레이어와 몬스터의 데이터를 관리
<br>

#### InputSystem
플레이어 움직임 설정
<br>

#### 런타임 프리팹 관리
Scene에 오브젝트들이 많아져 매니저 클래스를 이용하여 Prefab으로 관리
<br>
