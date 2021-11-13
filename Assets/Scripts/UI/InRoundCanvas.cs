using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
        
        [SerializeField] private GameObject[] gameUIs;

        public void OnInitRound(int roundCount)
        {
            // backGroundImage.sprite = backGrounds[roundCount - 1];
            roundText.text = $"{roundCount} Round";
            
            StartCoroutine(FadeIn());
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
            // 대충 시작하는 애니메이션
        }

        public void OnClickTimingGameButton()
        {
            GameManager.Instance.isPlayerUpInTimingGame = true;
        }
    }
}
