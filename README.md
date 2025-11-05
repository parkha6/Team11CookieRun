![로고 이미지](https://raw.githubusercontent.com/parkha6/Team11CookieRun/41dd80ffef3e717ecd460197a63c5a07bd8ef510/Capture/Logo.jpg)
# Last Jump : 최후의 도약
## 목차
1. [프로젝트 개요 및 목표]
2. [주요기능]
3. [개발기간]
4. [역할분담]
5. [기술스택]
6. [프레임워크]
7. [사용에셋 목록]
## 프로젝트 개요 및 목표
* 장르: 밀리터리 테마의 횡스크롤 자동 러너 (Endless Runner)
* 스타일: 쿠키런 스타일의 간단한 조작 메커니즘을 기반으로, 긴박하고 어두운 분위기의 맵을 배경으로 함.
## 주요기능
### 게임플레이
- 단순하고 직관적인 조작 방식으로, 남녀노소 누구나 쉽게 즐길 수 있는 러너형 게임.
- 예측 불가능한 패턴의 장애물들이 실시간으로 등장, 동적으로 생성되는 맵으로 플레이할 때마다 새로운 경험을 제공.
- 게임 진행 중 획득 가능한 다양한 아이템을 통해 플레이에 이점을 얻을 수 있음.
- 코인 획득으로 인한 스코어 시스템으로 누적 점수 및 최고 점수 기록 갱신
- 체력이 0이되거나 추락 시 게임 오버되며, 결과 창의 재시작 기능을 통해 재도전 가능.
### 핵심기술
- GamaManager를 통한 게임 시작, 진행, 일시정지, 종료 등의 게임 상태 관리
- ScoreManager를 이용한 점수 기록 및 저장 시스템
- 이외 각 담당의 Manager를 사용한 싱글턴 및 모듈화
- PlayerSpawner를 이용해 Player오브젝트를 생성, 사용자의 키 입력에 따른 움직임을 PlayerInputManager에서 처리. Player의 상태를 세분화하여 구현.
- 각 아이템 별 구현 된 효과 및 사운드와 이미지, 이를 활용한 UI
- 오브젝트 풀링 방식을 이용한 맵, 장애물, 아이템 오브젝트의 성능 최적화
- 오브젝트에 따른 다른 충돌 처리(데미지, 점수, 이로운 효과)
- Player 상태와 게임 상태에 따른 차별화 된 UI
## 개발기간
- 총 8일 { 2025.10.29 ~ 2025.11.05 }
## 역할분담
|캐릭터 & 저장|
|:---:|
|<img src="https://avatars.githubusercontent.com/u/151013695?v=4" width="100"/>|
|[정유찬](https://github.com/youchan97)|

|장애물 & 맵|
|:---:|
|<img src="https://avatars.githubusercontent.com/u/101345563?v=4" width="100">|
|[김하늘](https://github.com/Hagill)|

|아이템|
|:---:|
|<img src="https://avatars.githubusercontent.com/u/233911634?v=4" width="100">|
|[백성현](https://github.com/tjdgus76)|

|UI|
|:---:|:---:|
|<img src="https://avatars.githubusercontent.com/u/233683093?v=4" width="100">|<img src="https://avatars.githubusercontent.com/u/115542242?v=4" width="100">|
|[김태환](https://github.com/kimdf)|[곽민진](https://github.com/parkha6)|


|게임매니저|
|:---:|
|<img src="https://avatars.githubusercontent.com/u/115542242?v=4" width="100">|
|[곽민진](https://github.com/parkha6)|

## 기술스택
### Language
[![My Skills](https://skillicons.dev/icons?i=cs&perline=1)](https://skillicons.dev)
### Engine
[![My Skills](https://skillicons.dev/icons?i=unity&perline=1)](https://skillicons.dev)
### Version Control
[![My Skills](https://skillicons.dev/icons?i=git,github&perline=1)](https://skillicons.dev)
### IDE
[![My Skills](https://skillicons.dev/icons?i=visualstudio&perline=1)](https://skillicons.dev)

## 프레임 워크
 * [프레임 워크 & 초안 구성 (Figma)](https://www.figma.com/design/er4BxLbWAEO6Qd4Cq40ifw/11%EC%A1%B0-%ED%8C%80-%EC%8A%A4%ED%81%AC%EB%9F%BC?node-id=0-1&p=f&t=G4Sh1JYKRJVKQYyJ-0)
   
![프레임 워크 스샷](https://github.com/parkha6/Team11CookieRun/blob/main/Capture/FrameWork.jpg?raw=true)


## 사용에셋 목록
플레이어 : [Platformer Hero Pack](https://drasnus.itch.io/platformer-hero-pack)  
폰트 : [던파 비트비트체 v2](https://df.nexon.com/data/font/dnfbitbitv2)  
폰트2 : [ThaleahFat by Rick Hoppmann](https://tinyworlds.itch.io/free-pixel-font-thaleah)  
UI : [Free TDS Modern: GUI Pixel Art](https://craftpix.net/freebies/free-tds-modern-gui-pixel-art/?num=1&count=891&sq=pixel%20ui&pos=2)  
UI2 : [Fantasy Minimal Pixel Art GUI by etahoshi](https://etahoshi.itch.io/minimal-fantasy-gui-by-eta)  
맵타일셋 : [Free Platformer Game Tileset Pixel Art](https://craftpix.net/freebies/free-platformer-game-tileset-pixel-art/)  
체력바: [Basic Pixel Health bar and Scroll bar](https://bdragon1727.itch.io/basic-pixel-health-bar-and-scroll-bar)  
아이템 이미지 : [Survivalist 2D - The Ultimate Pack by ChahatKhandelwal](https://chahatkhandelwal.itch.io/survivalist-2d-the-ultimate-pack)    
돌 에셋 : [Free Rocks Pixel Art Asset Pack](https://craftpix.net/freebies/free-rocks-pixel-art-asset-pack/)
메인화면 BGM : [Don’t Give Up The Fight - 김성원](https://gongu.copyright.or.kr/gongu/wrt/wrt/view.do?wrtSn=13073685&menuNo=200020)  

