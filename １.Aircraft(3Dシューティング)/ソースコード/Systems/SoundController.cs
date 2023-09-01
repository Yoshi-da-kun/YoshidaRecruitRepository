
using UnityEngine;

/// --------------------------------------------------
/// #SoundController.cs
/// �쐬��:�g�c�Y��
/// 
/// �����Ǘ�����X�N���v�g
/// --------------------------------------------------

public class SoundController : MonoBehaviour
{
    [SerializeField, Label("SE�̃I�[�f�B�I�\�[�X")]
    private AudioSource _seAudioSource;


    /// <summary>
    /// SE���Đ�����
    /// </summary>
    /// <param name="playSound"> �Đ�����SE </param>
    public void PlaySeSound(AudioClip playSound)
    {
        _seAudioSource.PlayOneShot(playSound);
    }
}
