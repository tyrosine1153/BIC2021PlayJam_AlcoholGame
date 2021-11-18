# 끝까지 간다

BIC Make Play Jam 2021, Alcohol Free팀의 <끝까지 간다>입니다.

### 개발 기간

2021년 11월 12일 ~ 2021년 11월 14일 (3일)

### 개발 인원

Alcohol Free(알콜 프리) 팀 (총 3명)

- **기획** : 김기환
- **아트** : 윤혜진
- **프로그래밍** : 유정민

### 개발 환경

- Unity Engine (2020.3.20f1, 2D)
  - Asset(레포지토리에 미포함) : DOTween

### 게임 소개

- 제목 : 끝까지 간다

- 장르 : 캐주얼, 시뮬레이션

- 플랫폼 : 윈도우, 안드로이드

- 조작 : 마우스(윈도우), 터치(안드로이드)

- 다양한 술 게임을 바탕으로 진행되는 토너먼트! 마지막까지 살아남아, 술 게임의 일인자가 되어라!

  - “술 게임 최강자전” 대학교에 입학하고 많은 사람들을 만날 수 있는 곳, ‘술자리’. 그리고 그곳에서 펼쳐지는 ‘술 게임’. 그러나 그것은 단순한 게임이 아닌, ‘전쟁’이다. 어설픈 자는 살아남지 못한다. 세계 각지에서 인종, 성별, 종족을 불문하고 오직 ‘승리‘ 하기 위해 모두 한자리에 모인다.

  - 한 라운드당 플레이어 포함 8명의 사람이 참가합니다. 한 라운드에서 플레이어 한 명만 남을 때까지 계속해서 술게임을 하면서 최종 라운드까지 도달하세요!

    ![image-20211118200043631](C:\Users\dkssu\AppData\Roaming\Typora\typora-user-images\image-20211118200043631.png)

  - 참가자들은 술게임을 하고 게임에 진 사람은 벌칙으로 벌칙주를 마십니다. 

    ![drink](https://user-images.githubusercontent.com/63496908/142425852-dd5b3077-a0aa-4c67-8d98-d6affabfeb30.gif)

  - 라운드 내 게임 종류는 랜덤으로 정해집니다.   
    참가자끼리 **턴을 돌아가며 하는 게임**일 경우, 전 게임에서 벌칙을 받았던 참가자 중 한 사람부터 시작해 **시계 반대방향**으로 돌아갑니다. 전 게임이 없었을 경우 1번부터 시작합니다.

    - 눈치게임 : 게임 시작 후 마지막 순서가 되기 전에 일어나야 합니다. 마지막 순서가 될때까지 일어나지 않으면 벌칙을 받으며, 일어나다가 다른 사람과 순서가 겹쳐도 벌칙을 받습니다.
    - 369 : 턴을 돌아가며 숫자를 1부터 차례대로 부르고, 숫자에 3,6,9 중 하나 이상이 들어가면 숫자를 부르지 않고 3,6,9가 들어가는 수만큼 박수를 쳐야 합니다. `ex) 13 -> 박수 한 번, 293 -> 박수 두 번, 12 -> 12`
      규칙에 맞지 않게 숫자를 세면 벌칙을 받습니다.
    - 베스킨라빈스 31 : 턴을 돌아가며 숫자를 1부터 차례대로 부르는데, 숫자를 한 번에서 세 번까지 부를 수 있습니다. `ex) A: 1, 2 / B: 3 / C : 4, 5, 6...`
      그러다가 31번 째 수를 부르는 사람은 벌칙을 받습니다. 
    - 안녕 클레오파트라 : 턴을 돌아가며 앞사람보다 높은 소리를 내야 합니다. 화면을 터치해 소리를 높게 낼 수 있으며, 5회부터 시작해 턴마다 3회씩 더해가는 수만큼 화면을 터치해야 충분히 높은 소리를 낼 수 있습니다.
    - 폭탄돌리기 : 게임 시작 부터 정해진 제한 시간까지 옆사람에게 폭탄을 넘겨야 합니다. 주어진 간단한 사칙연산 문제를 풀면 옆사람에게 폭탄을 넘길 수 있습니다. 제한시간이 끝났을 때 폭탄을 가지고 있는 사람은 벌칙을 받습니다.


  - 플레이어의 주량은 벌칙주 세 잔 입니다. 다른 플레이어는 라운드마다 랜덤으로 정해지며, 라운드가 올라갈 수록 주량이 세집니다. 참가자들이 주량을 넘기면 탈락합니다. 

    ![image](https://user-images.githubusercontent.com/63496908/142404619-1fc45f53-63c9-4af4-8482-d7d87c998fd7.png)

### 실행 방법

이 저장소의 Release에 개시된 가장 최근 버전 파일을 다운받아 PC에서 실행합니다.