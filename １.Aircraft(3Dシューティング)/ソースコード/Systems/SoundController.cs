
using UnityEngine;

/// --------------------------------------------------
/// #SoundController.cs
/// 作成者:吉田雄伍
/// 
/// 音を管理するスクリプト
/// --------------------------------------------------

public class SoundController : MonoBehaviour
{
    [SerializeField, Label("SEのオーディオソース")]
    private AudioSource _seAudioSource;


    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="playSound"> 再生するSE </param>
    public void PlaySeSound(AudioClip playSound)
    {
        _seAudioSource.PlayOneShot(playSound);
    }
}
