using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private void Start()
    {
        switch (SceneManagerEx.Instance.CurrentSceneType)
        {
            case SceneType.InRound:
                AudioManager.Instance.PlayBGM((BGMType)GameManager.Instance.roundCount);
                break;
            case SceneType.Ending:
                AudioManager.Instance.PlayBGM(SceneManagerEx.Instance.isHappyEnding ? BGMType.Success : BGMType.Fail);
                break;
            case SceneType.Title:
                AudioManager.Instance.PlayBGM(BGMType.Title);
                break;
        }
    }
}
