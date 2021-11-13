using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InRoundCanvas : MonoBehaviour
    {
        [SerializeField] private Sprite[] backGrounds;
        
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

        public void OnInitGame(GameType gameType)
        {
            gameUIs[(int)gameType].SetActive(true);
            // 대충 시작하는 이미지
        }
    }
}
