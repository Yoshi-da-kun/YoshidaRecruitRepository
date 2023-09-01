
using UnityEngine;

/// --------------------------------------------------
/// #TargetBombParameter.cs
/// �쐬��:�g�c�Y��
/// 
/// ���e�^�̓I�̃p�����[�^���܂Ƃ߂�X�N���v�g
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/TargetBombParameter", fileName = "NewTargetBombParameter")]
public class TargetBombParameter : CharacterBaseParameter
{
    [field: SerializeField, Label("��ꂽ���̃G�t�F�N�g��Prefab")]
    public GameObject _breakedEffectPrefab { get; private set; }

    [field: SerializeField, Label("�j�󂳂ꂽ����SE")]
    public AudioClip _breakedSound { get; private set; }
}