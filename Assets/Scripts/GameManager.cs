using System;
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
    TimingGame,
    Clap369,
    BaskinRobbins31,
    Cleopatra,
    SpinningBomb,
}

public class GameManager : Singleton<GameManager>
{
    private static readonly Player DefaultPlayer = new Player(3);

    public int roundCount = 1;

    public int playerCount;
    public Player[] players;
    public GameType gameType;

    private InRoundCanvas _inRoundCanvas;

    public void InitRound()
    {
        Debug.Log("InitRound");
        playerCount = 8;
        players = new Player[8];
        players[0] = DefaultPlayer;

        for (var i = 1; i < 8; i++)
        {
            players[i] = new Player(Random.Range(roundCount, roundCount + 2));
        }

        Invoke(nameof(DelayedInitRound), 1f);
    }

    private void DelayedInitRound()
    {
        _inRoundCanvas = FindObjectOfType<InRoundCanvas>();
        Debug.Log(_inRoundCanvas == null);
        _inRoundCanvas.OnInitRound(roundCount);
        
        StartGame();
    }

    private void StartGame()
    {
        // 인원이 2명 이하일 경우 눈치게임은 제외
        var gameTypeIndex = 0;
            // = Random.Range(playerCount > 2 ? 0 : 1, Enum.GetValues(typeof(GameType)).Length);

        _inRoundCanvas.OnInitGame((GameType) gameTypeIndex);

        switch ((GameType) gameTypeIndex)
        {
            case GameType.TimingGame:
                TimingGame();
                break;
            case GameType.Clap369:
                break;

            case GameType.BaskinRobbins31:
                break;

            case GameType.Cleopatra:
                break;

            case GameType.SpinningBomb:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        StartCoroutine(_inRoundCanvas.FadeIn());
    }

    // 입력 : 게임이 끝나고 벌칙을 받을 플레이어들 목록
    public void EndGame(int[] failPlayers)
    {
        // **게임 UI 끄기
        
        foreach (var index in failPlayers)
        {
            players[index].CurrentAlcoholCount++;
            // **벌칙 애니메이션

            // 플레이어 탈락
            if (players[index].AlcoholCapacity <= players[index].CurrentAlcoholCount)
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

        // ** 잠시 후 새 게임 시작, 페이드 아웃
        Invoke(nameof(StartGame), 3);
        StartCoroutine(_inRoundCanvas.FadeOut());
    }

    private void FailRound()
    {
        // 게임오버 UI
        // main 으로 돌아가기
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
            // 우승
            SceneManagerEx.Instance.LoadScene(SceneType.Ending);
        }
    }

    #region Game

    private void TimingGame()
    {

    }

    private void Clap369()
    {

    }

    #endregion

}
