
using UnityEngine;

/// --------------------------------------------------
/// #SummarizeScriptableObjects.cs
/// �쐬��:�g�c�Y��
/// 
/// �e�p�����[�^��ScriptableObject���܂Ƃ߂��X�N���v�g�ł�
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Others/SummarizeScriptableObjects", fileName = "NewSummarizeScriptableObjects")]
public class SummarizeScriptableObjects : ScriptableObject
{
    [field: SerializeField, Header("�t�B�[���h�̃p�����[�^")]
    public FieldParameter _fieldParameter { get; private set; }

    [field: SerializeField, Header("�v���C���[�̃p�����[�^")]
    public PlayerParameter _playerParameter { get; private set; }

    [field: SerializeField, Header("���o�̃p�����[�^")]
    public PresentationParameter _prensentationParameter { get; private set; }

    [field: SerializeField, Header("�T�E���h���܂Ƃ߂��X�N���v�g")]
    public InGameSounds _inGameSounds { get; private set; }
}