
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #WarpSkill.cs
/// �쐬��:�g�c�Y��
/// 
/// ���[�v�̃X�L���𐧌䂷��X�N���v�g
/// --------------------------------------------------

public class WarpSkill : MonoBehaviour
{
    [SerializeField]
    private PlayerParameter _playerParameter = default;

    // ���[�v�X�L������(�ݒu���ꂽ��)�������t���O
    public bool _inWarpSkill { get; private set; }

    // ���[�v�����������t���O
    public bool _isWarping { get; private set; }

    // ���[�v�X�L���ݒu��̈ړ����W��p���Ȃǂ�ۑ����郊�X�g
    private List<Vector3> _cordinatesDuringWarpSkill = new List<Vector3>();
    private List<Quaternion> _attributesDuringWarpSkill = new List<Quaternion>();

    // ���[�v�Ō��̈ʒu�ɖ߂��Ă����Ƃ��́A�ړ����W�p�����X�g�̂P�v�f������̌o�ߕb��
    private float _warpElapsedTimePerIndex = default;

    // ���[�v�X�L���Ɋւ���v������
    private float _elapsedTimeDuringWarpSkill = default;


    /// <summary>
    /// ���[�v�X�L���g�p���̂Ƃ��̏���
    /// </summary>
    public void WarpSkillProcessing(Vector3 _aircraftPosition, Quaternion _aircraftRotation)
    {
        // ���[�v���̋@�̂̍��W�Ǝp�����L�^����
        _cordinatesDuringWarpSkill.Add(_aircraftPosition);
        _attributesDuringWarpSkill.Add(_aircraftRotation);

        // ���[�v�X�L���ݒu�ォ��̌o�ߎ��Ԃ����Z
        _elapsedTimeDuringWarpSkill += Time.fixedDeltaTime;

        // �o�ߎ��Ԃ��������Ԃ𒴂����Ƃ��̏���
        if (_elapsedTimeDuringWarpSkill >= _playerParameter._warpSkillActivateTime)
        {
            // ���[�v���̃t���O���Z�b�g���A�v�����Ԃ�������
            _isWarping = true;
            _elapsedTimeDuringWarpSkill = 0;

            // �ړ����W�p�����X�g�̂P�v�f������̌o�ߕb�������߂�
            _warpElapsedTimePerIndex = _playerParameter._warpingTime / _cordinatesDuringWarpSkill.Count;
        }
    }


    /// <summary>
    /// ���[�v���̏���(���[�v�X�L���ݒu�ʒu�ɖ߂鏈��)
    /// </summary>
    public void WarpingProcessing(Transform _aircraftTransform)
    {
        // ���[�v���Ă��鎞�Ԃ��v������
        _elapsedTimeDuringWarpSkill += Time.fixedDeltaTime;

        // ���݂̋@�̂̍��W�Ǝp��������v�f�ԍ������߂�
        int currentListIndex = _cordinatesDuringWarpSkill.Count - 1 - (int)(_elapsedTimeDuringWarpSkill / _warpElapsedTimePerIndex);

        // �v�f�ԍ����ԊO�ɂȂ�Ȃ��悤�ɂ���
        if (currentListIndex < 0)
        {
            currentListIndex = 0;
        }

        // ���[�v�����݂̋@�̂̈ʒu�Ǝp���ɂ���
        _aircraftTransform.position = _cordinatesDuringWarpSkill[currentListIndex];
        _aircraftTransform.rotation = _attributesDuringWarpSkill[currentListIndex];

        // �v�����Ԃ����[�v�ɂ����鎞�Ԃ𒴂����烏�[�v���I������
        if (_elapsedTimeDuringWarpSkill >= _playerParameter._warpingTime)
        {
            // ���[�v�Ɋւ���t���O��������
            _isWarping = false;
            _inWarpSkill = false;

            // ���[�v�Ɋւ��郊�X�g��������
            _cordinatesDuringWarpSkill.Clear();
            _attributesDuringWarpSkill.Clear();
        }
    }


    /// <summary>
    /// ���[�v�X�L���̔���������
    /// </summary>
    public void WarpSkillStart()
    {
        // ���[�v�X�L�����ݒu����Ă��Ȃ��Ȃ�X�L������
        if (!_inWarpSkill)
        {
            Debug.Log("�X�L�������I");

            // �X�L���ݒu�t���O���Z�b�g
            _inWarpSkill = true;
        }
    }
}
