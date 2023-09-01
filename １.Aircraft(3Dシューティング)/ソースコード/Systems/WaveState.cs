
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #WaveState.cs
/// �쐬��:�g�c�Y��
/// 
/// �E�F�[�u�̏�Ԃ��t���O���ŕێ�����X�N���v�g
/// --------------------------------------------------

public static class WaveState
{
    // �^�[�Q�b�g(�G)�I�u�W�F�N�g���i�[���Ă��郊�X�g
    private static List<GameObject> _targetObjects  = new List<GameObject>();

    // �c��̃^�[�Q�b�g(�G)�̐�
    public static int _remainingTargets { get; private set; } = default;

    // �^�[�Q�b�g���ׂĂ��󂳂�Ă��邩
    public static bool _isAllTargetBreaked { get; private set; } = default;


    /// <summary>
    /// �^�[�Q�b�g�̐����X�V���鏈��
    /// </summary>
    public static void TaragetCountUpdate()
    {
        // �c��̃^�[�Q�b�g�̐���������
        _remainingTargets = 0;

        // �^�[�Q�b�g�I�u�W�F�N�g�̐��J��Ԃ�
        for (int i = 0; i < _targetObjects.Count; i++)
        {
            // �^�[�Q�b�g�I�u�W�F�N�g���L����
            if (_targetObjects[i].activeSelf)
            {
                // �c��̃^�[�Q�b�g�������Z���Ď��̗v�f��
                _remainingTargets++;
                
                continue;
            }

            // �^�[�Q�b�g�I�u�W�F�N�g������������Ă�����A���X�g������폜����
            _targetObjects.RemoveAt(i);

            // �폜��̃|�C���^�̈ʒu���C������
            i--;
        }

        // �^�[�Q�b�g�̐����O�̂Ƃ�
        if (_remainingTargets == 0)
        {
            // �S�Ẵ^�[�Q�b�g���j�󂳂�Ă���t���O���Z�b�g
            _isAllTargetBreaked = true;
        }
        else
        {
            // �^�[�Q�b�g���܂��c���Ă���t���O���Z�b�g
            _isAllTargetBreaked = false;
        }
    }
    

    /// <summary>
    /// �^�[�Q�b�g�I�u�W�F�N�g��V�����ǉ�����
    /// </summary>
    public static void NewTargetAdd(GameObject targetToAdd)
    {
        _targetObjects.Add(targetToAdd);
    }
}
