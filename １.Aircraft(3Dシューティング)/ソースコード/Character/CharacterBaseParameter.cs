
using UnityEngine;

/// --------------------------------------------------
/// #CharacterBaseParameter.cs
/// �쐬��:�g�c�Y��
/// 
/// �e�L�����N�^�[�̃p�����[�^���܂Ƃ߂�X�N���v�g
/// �L�����N�^�[�Ƃ́AHP��������
/// --------------------------------------------------

public class CharacterBaseParameter : ScriptableObject
{
    [field: SerializeField, Label("�L�����N�^�[�̍ő�HP")]
    public int _maxHp { get; private set; }
}