
using UnityEngine;

/// --------------------------------------------------------
/// #PlayerParameter.cs
/// 
/// �v���C���[�Ɋւ���p�����[�^���܂Ƃ߂��X�N���v�g
/// --------------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/PlayerParameter", fileName = "NewPlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [field: SerializeField, Label("�v���C���[�̈ړ����x"), Range(0.01f, 10)]
    public float _moveSpeed { get; private set; } = 1;

    [field: SerializeField, Label("�v���C���[�̍U����(�P��z��)"), Range(1, 10)]
    public int _attackVolume { get; private set; } = 1;


    [field: SerializeField, Label("���̂�j�󂷂�Ԋu"), Range(0.01f, 10)]
    public float _breakIntervalTime { get; private set; } = 1;

    [field: SerializeField, Label("���̂�j��ł��鋗��"), Range(0.01f, 10)]
    public float _breakingDistance { get; private set; } = 1;

}
