using System;
using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class InRoundCanvas : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private Sprite[] backGrounds;
        
        [Header("UI")]
        [SerializeField] private Image backGroundImage;
        [SerializeField] private Image fadeImage;
        [SerializeField] private Text roundText;
        [SerializeField] private Image[] gameTitles;
        
        [SerializeField] private GameObject[] gameUIs;

        [Header("Game UI")] 
        [SerializeField] private Text[] clap369ButtonTexts;
        [SerializeField] private Button[] clap369Buttons;
        [SerializeField] private Text[] baskinRobbins31ButtonTexts;
        [SerializeField] private Slider cleopatraSlider;
        [SerializeField] private Text cleopatraText;
        [SerializeField] private Text spinningBombQuestionText;
        [SerializeField] private Text[] spinningBombButtonTexts;
        [SerializeField] private Button[] spinningBombButtons;

        [SerializeField] private Text subtitleText;
        
        [Header("Animations")] 
        [SerializeField] private Animator[] playerAnimators;
        [SerializeField] private Animator bombAnimator;
        private static readonly int StandUp = Animator.StringToHash("StandUp");
        private static readonly int Clap = Animator.StringToHash("Clap");
        private static readonly int Shout = Animator.StringToHash("Shout");
        private static readonly int Drink = Animator.StringToHash("Drink");
        private static readonly int IsFail = Animator.StringToHash("IsFail");
        
        public void OnInitRound(int roundCount)
        {
            backGroundImage.sprite = backGrounds[roundCount - 1];
            roundText.text = $"{roundCount} Round";
            
            //StartCoroutine(FadeIn());
        }

        public IEnumerator FadeIn()
        {
            yield return new WaitForSeconds(2f);
            fadeImage.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
            fadeImage.gameObject.SetActive(false);
        }

        public IEnumerator FadeOut()
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.DOFade(1f, 1f);
            yield return new WaitForSeconds(3f);
        }

        public void EnableGameUI(GameType gameType)
        {
            gameUIs[(int)gameType].SetActive(true);
            gameTitles[(int) gameType].DOFade(1f, 0f);
            gameTitles[(int) gameType].rectTransform.DOScale(0f, 0f);
            // 대충 시작하는 애니메이션
        }

        public void EnableGameTitle(int gameType)
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => gameTitles[gameType].gameObject.SetActive(true))
                .Append(gameTitles[gameType].rectTransform.DOScale(1f, 1f))
                .AppendInterval(1f)
                .Append(gameTitles[gameType].DOFade(0f, 1f))
                .AppendCallback(() => gameTitles[gameType].gameObject.SetActive(false));
        }

        public void DisableGameUI(GameType gameType)
        {
            gameUIs[(int)gameType].SetActive(false);
        }

        public void OnClickTimingGameButton()
        {
            GameManager.Instance.isPlayerUpInTimingGame = true;
        }

        public void SetClap369UI(int number)
        {
            foreach (var button in clap369Buttons)
            {
                button.onClick.RemoveAllListeners();
            }

            var index = Random.Range(0, clap369Buttons.Length);
            for (int i = 0; i < clap369Buttons.Length; i++)
            {
                var answer = (i == index) ? number : number + Random.Range(-2, 3);
                clap369Buttons[i].onClick.AddListener(() => OnClickClap369Button(answer));
                clap369ButtonTexts[i].text = answer.ToString();
            }
        }

        public void OnClickClap369Button(int answer)
        {
            
            GameManager.Instance.answerOfPlayerInClap369 = answer;
        }

        public void SetBaskinRobbins31UI(int number)
        {
            var str = "";
            foreach (var text in baskinRobbins31ButtonTexts)
            {
                number++;
                str += (number + " ");
                text.text = str;
            }
        }

        public void OnClickBaskinRobbins31Button(int answer)
        {
            GameManager.Instance.answerOfPlayerInBaskinRobbins31 = answer;
        }

        public void SetCleopatraUI(int goal)
        {
            cleopatraSlider.maxValue = goal;
            cleopatraSlider.value = 0;
            cleopatraText.text = "0";
        }

        public void OnClickCleopatraButtonClick()
        {
            GameManager.Instance.clickCountInCleopatra++;
            cleopatraSlider.value = GameManager.Instance.clickCountInCleopatra;
            cleopatraText.text = GameManager.Instance.clickCountInCleopatra.ToString();
        }

        private int _rightAnswerInSpinningBomb;
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Index = Animator.StringToHash("Index");

        public void SetSpinningBombUI(Tuple<string, int> question)
        {
            spinningBombQuestionText.text = question.Item1;
            _rightAnswerInSpinningBomb = question.Item2;
            foreach (var button in spinningBombButtons)
            {
                button.onClick.RemoveAllListeners();
            }

            var index = Random.Range(0, spinningBombButtons.Length);
            for (int i = 0; i < spinningBombButtons.Length; i++)
            {
                var answer = (i == index) ? question.Item2 : question.Item2 + Random.Range(-50, 50);
                spinningBombButtons[i].onClick.AddListener(() => OnClickSpinningBomb(answer));
                spinningBombButtonTexts[i].text = answer.ToString();
            }
        }

        public void OnClickSpinningBomb(int answer)
        {
            if (_rightAnswerInSpinningBomb != answer) return;

            GameManager.Instance.isPlayerCorrectInSpinningBomb = true;
        }
        
        public void StandUpAnim(int playerIndex)
        {
            playerAnimators[playerIndex].SetTrigger(StandUp);
        }

        public void ClapAnim(int playerIndex)
        {
            playerAnimators[playerIndex].SetTrigger(Clap);
        }
    
        public void ShoutAnim(int playerIndex)
        {
            playerAnimators[playerIndex].SetTrigger(Shout);
        }
    
        public void DrinkAnim(int playerIndex)
        {
            playerAnimators[playerIndex].SetTrigger(Drink);
        }
    
        public void TrumbleAnim(int playerIndex)
        {
            playerAnimators[playerIndex].SetBool(IsFail, true);
        }

        public void BombAnim(int playerIndex)
        {
            bombAnimator.SetTrigger(Move);
            bombAnimator.SetInteger(Index,playerIndex);
        }

        public void Subtitle(string str)
        {
            subtitleText.text = str;
        }
    }
}
