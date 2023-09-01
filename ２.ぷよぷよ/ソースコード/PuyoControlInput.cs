
using UnityEngine;

/// --------------------------------------------------
/// #PuyoControlInput.cs
/// �쐬��:�g�c�Y��
/// 
/// �Ղ�𑀍삷��R���g���[���̓��͏������s���X�N���v�g
/// --------------------------------------------------

public class PuyoControlInput : MonoBehaviour
{
    [SerializeField]
    private SummarizeScriptableObjects _scriptableObjects;

    private PlayerParameter _playerParameter;

    void Start()
    {
        _playerParameter = _scriptableObjects._playerParameter;
    }


    /// <summary>
    /// �ړ������邽�߂ɕK�v�ȕϐ�
    /// </summary>
    public struct HorizontalMoveVariables
    {
        public bool isMoving;
        public bool isInput;
        public bool isHighSpeed;
        public float inputElapsedTime;
    }

    // ���E�ړ����ꂼ��̈ړ��̂��߂̕ϐ�
    private HorizontalMoveVariables _leftMoveVariables;
    private HorizontalMoveVariables _rightMoveVariables;

    // ���E��]�̓��͂������t���O
    public struct RightLeftInputFlag
    {
        public bool Right;
        public bool Left;
    }

    // ��]�̓��͂�����Ă��邩�̃t���O
    public RightLeftInputFlag _isRotationButtonInput;

    /// <summary>
    /// ��]�p�Ɋւ�����͂�Ԃ�
    /// </summary>
    public RightLeftInputFlag PuyoRotationInput(RightLeftInputFlag currentRotationButtonInput)
    {
        ///-- ��]�{�^���̓��͂ɉ����Ċe�t���O���Z�b�g���� --///

        // ���E��]�̓��͂������t���O
        RightLeftInputFlag isRotation = default;

        // �E��]���͂����������Ƃ��A�t���O���Z�b�g����
        if (currentRotationButtonInput.Right && !_isRotationButtonInput.Right)
        {
            isRotation.Right = true;
        }

        // ����]���͂����������Ƃ��A�t���O���Z�b�g����
        if (currentRotationButtonInput.Left && !_isRotationButtonInput.Left)
        {
            isRotation.Left = true;
        }

        // ���݂̍��E��]�̃t���O���i�[����
        _isRotationButtonInput.Right = currentRotationButtonInput.Right;
        _isRotationButtonInput.Left = currentRotationButtonInput.Left;

        return isRotation;
    }


    /// <summary>
    /// ���E�ړ��Ɋւ�����͂�Ԃ�
    /// </summary>
    /// <param name="horizontalInput"> ���E���� </param>
    public RightLeftInputFlag HorizontalMoveInput(float horizontalInput)
    {
        RightLeftInputFlag isMoveInput = default;

        // �ړ��t���O��������
        _rightMoveVariables.isMoving = false;
        _leftMoveVariables.isMoving = false;

        // �E���͂�����Ă��邩
        if (horizontalInput >= _playerParameter._horizontalDeadZone)
        {
            // �E�ړ����͗p�̏������s��
            _rightMoveVariables = MoveInput(horizontalInput, _rightMoveVariables);
        }
        // ���͂���Ă��Ȃ��Ƃ��̃t���O���Z�b�g
        else
        {
            _rightMoveVariables.isInput = false;
        }
        // �����͂�����Ă��邩
        if (horizontalInput <= -_playerParameter._horizontalDeadZone)
        {
            // ���ړ����͗p�̏������s��
            _leftMoveVariables = MoveInput(horizontalInput, _leftMoveVariables);
        }
        // ���͂���Ă��Ȃ��Ƃ��̃t���O���Z�b�g
        else
        {
            _leftMoveVariables.isInput = false;
        }

        // �E�ړ����͂��i�[����
        isMoveInput.Right = _rightMoveVariables.isMoving;

        // ���ړ����͂��i�[����
        isMoveInput.Left = _leftMoveVariables.isMoving;

        return isMoveInput;
    }


    /// <summary>
    /// ���E�ړ������邩�𔻒肵�A���ʂ�Ԃ�
    /// </summary>
    /// <param name="horizontalInput"> ���E�ړ��̓��͒l </param>
    /// <param name="moveVariables"> ���E�ړ����s�����߂̕ϐ� </param>
    private HorizontalMoveVariables MoveInput(float horizontalInput, HorizontalMoveVariables moveVariables)
    {
        // �O�t���[���œ��͂���Ă��邩
        if (moveVariables.isInput)
        {
            // ���͂������Ă��鎞�Ԃ��v������
            moveVariables.inputElapsedTime += Time.deltaTime;
        }
        else
        {
            // �v�����Ԃ����������A�ړ�����t���O���Z�b�g
            moveVariables.inputElapsedTime = 0;
            moveVariables.isMoving = true;

            // �����ړ���Ԃ�����
            moveVariables.isHighSpeed = false;
        }

        // �����ړ���Ԃɂ͂����Ă���Ƃ�
        if (moveVariables.isHighSpeed)
        {
            // �ړ��d�����Ԃ𒴂�����
            if (moveVariables.inputElapsedTime >= _playerParameter._moveStoppingTime)
            {
                // �v�����Ԃ����������A�ړ�����t���O���Z�b�g
                moveVariables.inputElapsedTime = 0;
                moveVariables.isMoving = true;
            }
        }
        // ���͂������Ă��鎞�Ԃ������ړ����͎��Ԃ𒴂�����
        if (moveVariables.inputElapsedTime >= _playerParameter._intoHighSpeedTime)
        {
            // �v�����Ԃ����������A�ړ�����t���O���Z�b�g
            moveVariables.inputElapsedTime = 0;
            moveVariables.isMoving = true;

            // �����ړ��t���O���Z�b�g
            moveVariables.isHighSpeed = true;
        }

        // ���̓t���O���Z�b�g
        moveVariables.isInput = true;

        return moveVariables;
    }


    /// <summary>
    /// �����͂�����Ă��邩��Ԃ�
    /// </summary>
    /// <param name="verticalInput"> �㉺�̓��͒l </param>
    public bool FallInput(float verticalInput)
    {
        // �����͂ɉ����ĉ����̓t���O���Z�b�g����
        if (verticalInput <= -_playerParameter._verticalDeadZone)
        {
            return true;
        }
        // �����͂���Ă��Ȃ��Ƃ�
        else
        {
            return false;
        }
    }
}
