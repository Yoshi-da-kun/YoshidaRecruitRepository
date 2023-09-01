
using UnityEngine;

/// --------------------------------------------------
/// #PlayerParameter.cs
/// �쐬��:�g�c�Y��
/// 
/// �v���C���[�̃p�����[�^���܂Ƃ߂�X�N���v�g
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/PlayerParameter", fileName = "NewPlayerParameter")]
public class PlayerParameter : AircraftParameter
{
    [field: SerializeField, Label("���[�v�X�L���̐ݒu�ォ��A�X�L����������܂ł̎���")]
    public float _warpSkillActivateTime { get; private set; } = 3;

    [field: SerializeField, Label("���[�v�ɂ����鎞��")]
    public float _warpingTime { get; private set; } = 1.3f;

    [field: SerializeField, Label("��ꂽ���̃G�t�F�N�g��Prefab")]
    public GameObject _breakedEffectPrefab { get; private set; }
}
