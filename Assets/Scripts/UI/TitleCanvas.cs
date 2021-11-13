using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class TitleCanvas : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject storyPanel;
        [SerializeField] private Transform storyTextTransform;
        [SerializeField] private Image fadeImage;

        [Header("Variable Value")] 
        [SerializeField] private float fadeSpeed = 2;
        [SerializeField] private float storyTextSpeed = 10;
        [SerializeField] private float midScreenY = 540;
        
        private Coroutine _storyCoroutine;

        public void OnClickStartButton()
        {
            _storyCoroutine = StartCoroutine(ShowStory());
        }

        private IEnumerator ShowStory()
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.DOFade(1f, fadeSpeed);
            yield return new WaitForSeconds(2f);
            storyPanel.SetActive(true);
            fadeImage.gameObject.SetActive(false);

            storyTextTransform.DOMoveY(midScreenY, storyTextSpeed);
            yield return new WaitForSeconds(storyTextSpeed + 2);

            fadeImage.DOFade(1f, fadeSpeed);
            SceneManagerEx.Instance.LoadScene(SceneType.InRound);
        }        

        public void OnClickSkipButton()
        {
            StopCoroutine(_storyCoroutine);
            SceneManagerEx.Instance.LoadScene(SceneType.InRound);
        }
    }
}
