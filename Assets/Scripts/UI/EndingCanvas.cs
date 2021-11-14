using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EndingCanvas : MonoBehaviour
{
    [SerializeField] private Sprite[] endingImageSprites;
    [SerializeField] private Sprite[] endingTitleSprites;
    [SerializeField] private string[] scripts;
    
    [Header("UI")]
    [SerializeField] private Image endingImage;
    [SerializeField] private Image endingTitle;
    [SerializeField] private Text scriptText;
    [SerializeField] private GameObject buttons;

    private void OnEnable()
    {
        endingImage.sprite = SceneManagerEx.Instance.isHappyEnding ? endingImageSprites[0] : endingImageSprites[1];
        endingTitle.sprite = SceneManagerEx.Instance.isHappyEnding ? endingTitleSprites[0] : endingTitleSprites[1];
        
        Invoke(nameof(ShowEnding), 1f);
    }

    private void ShowEnding()
    {
        scriptText.DOText(SceneManagerEx.Instance.isHappyEnding ? scripts[0] : scripts[1], 10);
        buttons.SetActive(true);
    }

    public void OnClickTitleButton()
    {
        SceneManagerEx.Instance.LoadScene(SceneType.Title);
    }

    public void OnClickEndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
