using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public void PlaySFX(SFXType sfxType)
    {
        AudioManager.Instance.PlaySFX(sfxType);
    }
}
