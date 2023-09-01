
using UnityEngine;

/// --------------------------------------------------
/// #AircraftParameter.cs
/// �쐬��:�g�c�Y��
/// 
/// �@�̂̃p�����[�^���܂Ƃ߂��X�N���v�g
/// --------------------------------------------------

public class AircraftParameter : CharacterBaseParameter
{
    [field: SerializeField, Label("�ړ����x�̏����l"), Range(0.001f, 100)]
    public float _initialMovementSpeed { get; private set; } = 4;

    [field: SerializeField, Label("�u�[�X�g���̈ړ����x"), Range(0.001f, 100)]
    public float _boostMovementSpeed { get; private set; } = 7;

    [field: SerializeField, Label("�������̈ړ����x"), Range(0.001f, 100)]
    public float _slowMovementSpeed { get; private set; } = 1;

    [field: SerializeField, Label("�����x"), Range(0.0001f, 20)]
    public float _movementAcceleration { get; private set; } = 1;
    
    [field: SerializeField, Label("�����x"), Range(0.0001f, 20)]
    public float _movementDeceleration { get; private set; } = 1;

    [field: SerializeField, Label("�@�̂̃��[�����x")]
    public float _rollingSpeed { get; private set; }

    [field: SerializeField, Label("�@�̂̐��񑬓x")]
    public float _turningSpeed { get; private set; }


    private void OnEnable()
    {
        // �ړ����x�̐ݒ肪�z��ƍ����Ă��邩���`�F�b�N����
        if (_boostMovementSpeed <= _slowMovementSpeed)
        {
            Debug.LogError("�@�̂̃u�[�X�g���̑��x���A�������̑��x��菬������I");
        }

        // �ړ����x�̐ݒ肪�z��ƍ����Ă��邩���`�F�b�N����
        if (_boostMovementSpeed <= _initialMovementSpeed || _initialMovementSpeed <= _slowMovementSpeed)
        {
            Debug.LogError("�@�̂̏����ړ����x���ő�l�𒴂��Ă��邩�A�������̒l��菬������I");
        }
    }
}
