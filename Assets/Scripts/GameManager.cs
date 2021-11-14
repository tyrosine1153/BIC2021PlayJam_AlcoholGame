using System;
using System.Collections;
using System.Linq;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Player
{
    public int AlcoholCapacity;
    public int CurrentAlcoholCount;
    public bool IsFail;

    public Player(int alcoholCapacity)
    {
        AlcoholCapacity = alcoholCapacity;
        CurrentAlcoholCount = 0;
        IsFail = false;
    }
}

public enum GameType
{
    TimingGame = 0,
    Clap369,
    BeskinLavins31,
    Cleopatra,
    SpinningBomb,
}

public struct GameProbabilityInRound
{
    public float TimingGame;
    public float Clap369;
    public float BaskinRobbins31;
    public int[] Cleopatra;
    public float[] SpinningBomb;
    
    public GameProbabilityInRound(float timingGame, float clap369, float baskinRobbins31, int[] cleopatra, float[] spinningBomb)
    {
        TimingGame = timingGame;
        Clap369 = clap369;
        BaskinRobbins31 = baskinRobbins31;
        Cleopatra = cleopatra;
        SpinningBomb = spinningBomb;
    }
}

public class GameManager : Singleton<GameManager>
{
    private static readonly Player DefaultPlayer = new Player(3);

    private static readonly GameProbabilityInRound[] gameProb =
    {
        new GameProbabilityInRound(30, 90, 50, new[] {6, 12}, new[] {1f, 5f}),
        new GameProbabilityInRound(20, 93, 70, new[] {7, 13}, new[] {0.5f, 3f}),
        new GameProbabilityInRound(10, 97, 90, new[] {8, 14}, new[] {0.5f, 2f})
    };

    private static readonly WaitForSeconds Seconds3 = new WaitForSeconds(3f);

    public int roundCount = 1;

    public int playerCount;
    public Player[] players;
    public GameType currentGameType;

    private InRoundCanvas _inRoundCanvas;
    private int _nextFirstPlayer = 0;

    #region RoundSystem
    public void StartRound()
    {
        playerCount = 8;
        players = new Player[8];
        players[0] = DefaultPlayer;

        for (var i = 1; i < 8; i++)
        {
            players[i] = new Player(Random.Range(roundCount, roundCount + 2));
        }

        Invoke(nameof(DelayedStartRound), 1f);
    }

    private void DelayedStartRound()
    {
        _inRoundCanvas = FindObjectOfType<InRoundCanvas>();
        _inRoundCanvas.OnInitRound(roundCount);

        Invoke(nameof(StartGame), 3f);
    }

    private void FailRound()
    {
        SceneManagerEx.Instance.isHappyEnding = false;
        SceneManagerEx.Instance.LoadScene(SceneType.Ending);
    }

    private void EndRound()
    {
        // 대충 신나하는 애니메이션
        // 효과음

        if (roundCount < 3)
        {
            SceneManagerEx.Instance.LoadScene(SceneType.InRound);
            roundCount++;
        }
        else
        {
            SceneManagerEx.Instance.isHappyEnding = true;
            SceneManagerEx.Instance.LoadScene(SceneType.Ending);
        }
    }

    private void StartGame()
    {
        // 인원이 2명 이하일 경우 눈치게임은 제외
        var gameTypeIndex 
            = Random.Range(playerCount > 2 ? 0 : 1, Enum.GetValues(typeof(GameType)).Length);
        currentGameType = (GameType) gameTypeIndex;
        
        // _inRoundCanvas.EnableGameUI(currentGameType);

        //StartCoroutine(_inRoundCanvas.FadeIn());
        AudioManager.Instance.PlaySFX(SFXType.Drum);
        _inRoundCanvas.EnableGameTitle(gameTypeIndex);

        switch ((GameType) gameTypeIndex)
        {
            case GameType.TimingGame:
                StartCoroutine(TimingGame());
                break;
            case GameType.Clap369:
                StartCoroutine(Clap369());
                break;

            case GameType.BeskinLavins31:
                StartCoroutine(BaskinRobbins31());
                break;

            case GameType.Cleopatra:
                StartCoroutine(Cleopatra());
                break;

            case GameType.SpinningBomb:
                StartCoroutine(SpinningBomb());
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    // 입력 : 게임이 끝나고 벌칙을 받을 플레이어들 목록
    private void EndGame(int[] failPlayers)
    {
        Debug.Log("Game end!");

        _inRoundCanvas.DisableGameUI(currentGameType);

        foreach (var index in failPlayers)
        {
            players[index].CurrentAlcoholCount++;
            // **벌칙 애니메이션
            AudioManager.Instance.PlaySFX(SFXType.Drink);

            Debug.Log($"FailPlayer : {index}, Alcohol : {players[index].CurrentAlcoholCount} / {players[index].AlcoholCapacity}");
            if (players[index].AlcoholCapacity <= players[index].CurrentAlcoholCount)  // 도전자 탈락
            {
                players[index].IsFail = true;
                playerCount--;
                // **탈락 애니메이션

                if (index == 0)
                {
                    FailRound();
                }
                else if (playerCount <= 1)
                {
                    EndRound();
                }
            }
            else if (index == 0)
            {
                // **플레이어 주량 패널티
            }
        }

        // 잠시 후 새 게임 시작, 페이드 아웃
        _nextFirstPlayer = failPlayers[Random.Range(0, failPlayers.Length)];
        Invoke(nameof(StartGame), 3);
        //StartCoroutine(_inRoundCanvas.FadeOut());
    }

    private void EndGame(int failPlayer)
    {
        EndGame(new[] {failPlayer});
    }
    #endregion

    #region Game

    private const float FailTimeInterval = 0.5f;
    [HideInInspector] public bool isPlayerUpInTimingGame;
    private IEnumerator TimingGame()
    {
        Debug.Log("TimingGame Start");
        yield return new WaitForSeconds(5f); // 기존엔 2초였으나 모든 게임에서 게임 이름을 3초간 알려주고 시작하기 때문에 예외적으로 5초로 늘렸음.

        var upSeq = new int[playerCount - 1];
        var idx = 0;
        for (int i = 1; i < playerCount; i++)
        {
            if (!players[i].IsFail)
            {
                upSeq[idx++] = i;
            }
        }

        upSeq = upSeq.OrderBy(x => Random.Range(0, playerCount-1)).ToArray(); // 일어나는 순서 ex) 3, 2, 5, 7, 6, 4, 1 
        var upTime = new float[playerCount]; // 일어나는 시간 (오름차순)
        upTime[0] = -1f;
        upTime[1] = RandomUtility.CalculateProbability(50f) ? 0.1f : Random.Range(FailTimeInterval, 2f);
        for (int i = 2; i < playerCount; i++)
        {
            upTime[i] = RandomUtility.CalculateProbability(gameProb[roundCount - 1].TimingGame)
                ? upTime[i - 1] + Random.Range(0.1f, FailTimeInterval)
                : upTime[i - 1] + Random.Range(FailTimeInterval, 2f);
        }

        var curUpSeq = 1; // 현재 일어나야 하는 순서

        var curTime = 0f;
        isPlayerUpInTimingGame = false;
        var wasPlayerUp = false;

        // 현재 npc : upSeq[curUpSeq-1]
        // 현재 시간 : upTime[curUpSeq]

        _inRoundCanvas.EnableGameUI(currentGameType);
        while (true)
        {
            curTime += Time.deltaTime;
            if (isPlayerUpInTimingGame) // 플레이어가 일어남
            {
                AudioManager.Instance.PlaySFX(SFXType.Correct);
                Debug.Log($"Player Up : {curTime}");
                // **애니메이션

                if (curTime - upTime[curUpSeq - 1] < FailTimeInterval) // 시간이 겹침
                {
                    AudioManager.Instance.PlaySFX(SFXType.Wrong);
                    // **애니메이션
                    Debug.Log($"겹칩! {curTime} {upTime[curUpSeq - 1]}");
                    yield return new WaitForSeconds(3f);
                    EndGame(new[] {0, upSeq[curUpSeq - 1]});
                    yield break;
                }

                isPlayerUpInTimingGame = false;
                wasPlayerUp = true;
            }
            else if (upTime[curUpSeq] < curTime) // NPC가 일어남
            {
                AudioManager.Instance.PlaySFX(SFXType.Correct);
                Debug.Log($"NPC {upSeq[curUpSeq - 1]} Up : {curTime} -> curUpSeq : {curUpSeq}");
                // **애니메이션

                if (upTime[curUpSeq] - upTime[curUpSeq - 1] < FailTimeInterval && curUpSeq != 1) // 시간이 겹침
                {
                    AudioManager.Instance.PlaySFX(SFXType.Wrong);
                    // **애니메이션
                    Debug.Log($"겹칩! {curTime} {upTime[curUpSeq - 1]}");
                    yield return new WaitForSeconds(3f);
                    EndGame(new[] {upSeq[curUpSeq - 1], upSeq[curUpSeq - 2]});
                    yield break;
                }

                curUpSeq++;
                if (curUpSeq == playerCount - 1 && wasPlayerUp) // npc 걸림
                {
                    AudioManager.Instance.PlaySFX(SFXType.Wrong);
                    // ** 애니메이션
                    Debug.Log($"마지막! {upSeq[curUpSeq - 1]}");
                    yield return new WaitForSeconds(3f);
                    EndGame(upSeq[curUpSeq - 1]); // 마지막 인덱스 +1번째 참조해서 인덱스 아웃 오브 레인지 떠서 고쳤음
                    yield break;
                }

                if (curUpSeq == playerCount && !wasPlayerUp) // player 걸림
                {
                    AudioManager.Instance.PlaySFX(SFXType.Wrong);
                    // ** 애니메이션
                    Debug.Log("마지막! Player");
                    yield return new WaitForSeconds(3f);
                    EndGame(0);
                    yield break;
                }
            }

            yield return null;
        }
    }

    private const int DefaultValue = Int32.MinValue;
    [HideInInspector] public int answerOfPlayerInClap369 = DefaultValue;
    private IEnumerator Clap369()
    {
        Debug.Log("Clap369 Start");
        yield return Seconds3;
        _inRoundCanvas.DisableGameUI(GameType.Clap369);
        var turn = _nextFirstPlayer;
        var number = 0;

        while (true)
        {
            if (players[turn].IsFail)
            {
                turn = (++turn) % 8;
                continue;
            }
            
            var isCorrect = false;

            ++number;
            if (turn == 0) // 플레이어
            {
                // -- 버튼 생성 -- 클릭까지 대기 -- 클릭 값에 따라 정오답 여부 판별
                _inRoundCanvas.EnableGameUI(GameType.Clap369);
                _inRoundCanvas.SetClap369UI(number);
                answerOfPlayerInClap369 = DefaultValue;
                yield return new WaitUntil(() => answerOfPlayerInClap369 != DefaultValue);
                
                if (answerOfPlayerInClap369 != DefaultValue) isCorrect = true;  // 369룰 제대로 모름!

                _inRoundCanvas.DisableGameUI(GameType.Clap369);
            }
            else // npc
            {
                // ** 애니메이션
                if (RandomUtility.CalculateProbability(gameProb[roundCount - 1].Clap369))
                {
                    isCorrect = true;
                }
                AudioManager.Instance.PlaySFX(SFXType.Clap369);
            }
            Debug.Log($"number : {number} -> turn : {turn} isCorrect : {isCorrect}");

            if (!isCorrect)
            {
                AudioManager.Instance.PlaySFX(SFXType.Wrong);
                yield return new WaitForSeconds(3f);
                EndGame(turn);
                yield break;
            }

            turn = (++turn) % 8;
            yield return new WaitForSeconds(3f);
        }
    }

    [HideInInspector] public int answerOfPlayerInBaskinRobbins31 = DefaultValue;
    private IEnumerator BaskinRobbins31()
    {
        Debug.Log("BaskinRobbins31 Start");
        yield return Seconds3;
        _inRoundCanvas.DisableGameUI(GameType.BeskinLavins31);
        var turn = _nextFirstPlayer;
        var number = 0;

        while (true)
        {
            if (players[turn].IsFail)
            {
                turn = (++turn) % 8;
                continue;
            }
            
            var add = 0;

            if (turn == 0) // 플레이어
            {
                // -- 버튼 생성 -- 클릭까지 대기 -- 클릭 값에 따라 정오답 여부 판별
                _inRoundCanvas.EnableGameUI(GameType.BeskinLavins31);
                _inRoundCanvas.SetBaskinRobbins31UI(number);
                answerOfPlayerInBaskinRobbins31 = DefaultValue;
                yield return new WaitUntil(() => answerOfPlayerInBaskinRobbins31 != DefaultValue);

                add = answerOfPlayerInBaskinRobbins31;

                _inRoundCanvas.DisableGameUI(GameType.BeskinLavins31);
            }
            else // npc
            {
                var leftNumber = 31 - number;

                if (leftNumber == 1)
                {
                    add = 1;
                }
                else if (leftNumber < playerCount + 3 &&
                         RandomUtility.CalculateProbability(gameProb[roundCount - 1].BaskinRobbins31))
                {
                    add = 3;
                }
                else if (leftNumber <= 3)
                {
                    add = Random.Range(1, leftNumber);
                }
                else
                {
                    add = Random.Range(1, 4);
                }

                // ** 애니메이션
            }
            Debug.Log($"number : {number} -> turn : {turn} add : {add}");


            number += add;
            if (number >= 31)
            {
                AudioManager.Instance.PlaySFX(SFXType.Wrong);
                yield return new WaitForSeconds(3f);
                EndGame(turn);
                yield break;
            }

            turn = (++turn) % 8;
            yield return new WaitForSeconds(3f);
        }
    }

    [HideInInspector] public int clickCountInCleopatra = 0;
    private IEnumerator Cleopatra()
    {
        Debug.Log("Cleopatra Start");
        yield return Seconds3;
        _inRoundCanvas.DisableGameUI(GameType.Cleopatra);
        var turn = _nextFirstPlayer;
        var goal = 5;

        var soundLength = 3f;
        while (true)
        {
            if (players[turn].IsFail)
            {
                turn = (++turn) % 8;
                continue;
            }
            
            var pitch = 0;

            AudioManager.Instance.PlaySFX(SFXType.Cleopatra);
            if (turn == 0) // 플레이어
            {
                // -- 버튼 생성 -- 클릭까지 대기 -- 클릭 값에 따라 정오답 여부 판별
                _inRoundCanvas.EnableGameUI(GameType.Cleopatra);
                _inRoundCanvas.SetCleopatraUI(goal);
                clickCountInCleopatra = 0;
                var curTime = Time.time;
                yield return new WaitUntil(() => Time.time - curTime > soundLength || clickCountInCleopatra >= goal);

                pitch = clickCountInCleopatra;
                _inRoundCanvas.DisableGameUI(GameType.Cleopatra);
            }
            else // npc
            {
                var prob = gameProb[roundCount - 1].Cleopatra;
                pitch = (int) Mathf.Round(soundLength * Random.Range(prob[0], prob[1]));

                // ** 애니메이션
                yield return new WaitForSeconds(soundLength);
            }
            
            Debug.Log($"goal : {goal} -> turn : {turn} pitch : {pitch}");

            if (goal <= pitch)
            {
                goal += 3;
                AudioManager.Instance.PlaySFX(SFXType.Correct);
            }
            else
            {
                AudioManager.Instance.PlaySFX(SFXType.Wrong);

                yield return new WaitForSeconds(3f);
                EndGame(turn);
                yield break;
            }

            turn = (++turn) % 8;
            yield return new WaitForSeconds(3f);
        }
    }

    [HideInInspector] public bool isPlayerCorrectInSpinningBomb = false;
    private IEnumerator SpinningBomb()
    {
        Debug.Log("SpinningBomb Start");
        yield return Seconds3;
        _inRoundCanvas.DisableGameUI(GameType.SpinningBomb);
        var turn = _nextFirstPlayer;

        var oldTime = Time.time;
        var limitTime = Random.Range(5f, 30f);
        while (Time.time - oldTime < limitTime)
        {
            if (players[turn].IsFail)
            {
                turn = (++turn) % 8;
                continue;
            }
            AudioManager.Instance.PlaySFX(SFXType.Bomb);

            if (turn == 0) // 플레이어
            {
                // -- 버튼 생성 -- 클릭까지 대기 -- 클릭 값에 따라 정오답 여부 판별
                _inRoundCanvas.EnableGameUI(GameType.SpinningBomb);
                _inRoundCanvas.SetSpinningBombUI(RandomUtility.CreateMathQuestion());
                isPlayerCorrectInSpinningBomb = false;
                yield return new WaitUntil(() => Time.time - oldTime > limitTime || isPlayerCorrectInSpinningBomb);

                _inRoundCanvas.DisableGameUI(GameType.SpinningBomb);
            }
            else // npc
            {
                var prob = gameProb[roundCount - 1].SpinningBomb;
                yield return new WaitForSeconds(Random.Range(prob[0], prob[1]));
                // ** 애니메이션
            }

            Debug.Log($"turn : {turn}");

            turn = (++turn) % 8;
        }

        AudioManager.Instance.PlaySFX(SFXType.Wrong);
        yield return new WaitForSeconds(3f);
        EndGame((--turn + 8) % 8);
    }
    #endregion
}