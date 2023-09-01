
using UnityEngine;

/// --------------------------------------------------
/// #PlayerParameter.cs
/// �쐬��:�g�c�Y��
/// 
/// �ړ����x�◎�����x�Ȃǃv���C���[�̑��슴�Ɋւ���
/// �p�����[�^���܂Ƃ߂��X�N���v�g�ł�
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/PlayerParameter", fileName = "NewPlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [field: Header("���E�̈ړ��֘A")]

    [field: SerializeField, Label("���E�̍����ړ��ɓ���܂ł̎���(�b)"), Range(0.01f, 1)]
    public float _intoHighSpeedTime { get; private set; } = 1;

    [field: SerializeField, Label("���E�̍����ړ����Ƃ̍d��(�b)"), Range(0.01f, 1)]
    public float _moveStoppingTime { get; private set; } = 1;

    [field: SerializeField, Label("���E�̈ړ��ɂ�����ړ���")]
    public sbyte _horizontalMoveCounts { get; private set; } = 1;


    [field: Header("�����֘A")]

    [field: SerializeField, Label("�����̂Ȃ߂炩��"), Range(1, 10)]//////////////////////////�v����
    public int _fallSmoothness { get; private set; } = 2;

    [SerializeField, Label("�������x(�ő�500���炢)")]
    private float _fallNormalSpeed = 1;

    // �ʏ헎�����A��񗎉�����܂ł̎���
    public float _normalSpeedFallTime { get; private set; }

    [SerializeField, Label("�����������x(�ő�500���炢)")]
    private float _fallHighSpeed = 1;

    // �����������A��񗎉�����܂ł̎���
    public float _highSpeedFallTime { get; private set; } = 1;


    [field: Header("�Ղ�̉�]�Ɋւ���l")]

    [field: SerializeField, Label("�Ղ����]�����鑬�x")]
    private float _puyoRotationSpeed = 1; 

    // ���K�����ꂽ�Ղ�̉�]���x
    public float _normalizedRotationSpeed { get; private set; }

    // ����]�A�S���̂P��]����̂ɂ����鎞��
    public float _quarterRotationTime { get; private set; }
    public float _halfRotationTime { get; private set; }




    [field: Header("���͔͈͂Ɋւ���l")]

    [field: SerializeField, Label("�X�e�B�b�N���E���͂̃f�b�h�]�[��"), Range(0, 1)]
    public float _horizontalDeadZone { get; private set; }

    [field: SerializeField, Label("�X�e�B�b�N�㉺���͂̃f�b�h�]�[��"), Range(0, 1)]
    public float _verticalDeadZone { get; private set; }



    [field: Header("���o��d�����ԂȂǂ̒l")]

    [field: SerializeField, Label("�l�N�X�g���̂Ղ�𓮂�������"), Range(0, 10)]
    public float _nextPuyoMoveTime { get; private set; }

    [field: SerializeField, Label("�Ղ悪���n���Ă���A�ݒu�����܂ł̗P�\����")]
    public float _installGraceTime { get; private set; }


    [field: SerializeField, Label("�����琔�����Ƃ��̂Ղ�̐����s(���[�͂O�s��)")]
    public sbyte _puyoInstantRow { get; private set; }



    private void OnEnable()
    {
        // �ʏ�A�������ꂼ��̗����܂ł̎��Ԃ����߂�( 1�b / ���x )
        _normalSpeedFallTime = 1 / _fallNormalSpeed;
        _highSpeedFallTime = 1 / _fallHighSpeed;

        // ����]�A�S���̂P��]����̂ɂ����鎞�Ԃ����߂�( 1 �� 0.5f �͉�]���x�� 1 �̂Ƃ��A�e��]�ɂ����鎞��)
        _halfRotationTime = 1 / _puyoRotationSpeed;
        _quarterRotationTime = 0.5f / _puyoRotationSpeed;

        // ��]���x�𐳋K������(Sin,Cos���g���ĉ�]���邽��)
        _normalizedRotationSpeed = _puyoRotationSpeed * Mathf.PI;
    }
}