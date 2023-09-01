using UnityEngine;

/// --------------------------------------------------
/// #InGameSounds.cs
/// �쐬��:�g�c�Y��
/// 
/// �Q�[�����̉����܂Ƃ߂�X�N���v�g
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Others/InGameSounds", fileName = "NewInGameSouds")]
public class InGameSounds : ScriptableObject
{
    [field: SerializeField, Label("�Ղ悪�������Ƃ��̉�")]
    public AudioClip _puyoEraceSE { get; private set; }

    [field: SerializeField, Label("�Ղ悪�ݒu�������̉�")]
    public AudioClip _puyoInstallSE { get; private set; }

    [field: SerializeField, Label("�Ղ����]���������̉�")]
    public AudioClip _puyoRotationSE { get; private set; }
}
