
using UnityEngine;

/// --------------------------------------------------
/// #FieldParameter.cs
/// �쐬��:�g�c�Y��
/// 
/// �t�B�[���h�̑傫�����A�t�B�[���h��Ղ�̏��B�Ղ�̏�Ԃ��܂Ƃ߂��X�N���v�g�ł�
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/PresentationParamete", fileName = "NewPresentationParameter")]
public class PresentationParameter : ScriptableObject
{
    [field: SerializeField, Label("�Ղ�̏����鉉�o���s������")]
    public float _eracePresentationTime { get; private set; } = 1;

    [field: SerializeField, Label("�Ղ��ݒu���鉉�o���s������")]
    public float _installPresentationTime { get; private set; } = 1;

}
