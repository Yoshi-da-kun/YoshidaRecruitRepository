
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ControllerInputFunction;

/// --------------------------------------------------
/// #PlayerInputAndOperation.cs
/// �쐬��:�g�c�Y��
/// 
/// �t�B�[���h�̃I�u�W�F�N�g�ɃA�^�b�`���Ă�������
/// �v���C���[�̓��͂��󂯎��A�Ղ�𑀍삷��X�N���v�g�ł�
/// --------------------------------------------------

public class PlayerOperation : MonoBehaviour
{
    [SerializeField]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    // �e�p�����[�^(ScriptableObject)
    private PlayerParameter _playerParameter;
    private FieldParameter _fieldParameter;

    // �T�E���h���܂Ƃ߂��X�N���v�g
    private InGameSounds _inGameSounds;

    // �t�B�[���h���Ǘ�����X�N���v�g
    private FieldControl _fieldControl;

    // �Ղ�𑀍삷��R���g���[���̓��͏������s���X�N���v�g
    private PuyoControlInput _puyoControlInput;

    // �Ղ�𑀍�\��
    private bool _canPlayerControl = default;

    // ���삷��Ղ�� �F, Transform, �s, ��(�Ղ�̃f�[�^)
    private List<PuyoStructs.ControlPuyoData> _controlPuyoDatas = new List<PuyoStructs.ControlPuyoData>();

    // �e�s�Ɨ�̒��S���W
    private float[] _eachRowFieldPositions = default;
    private float[] _eachColumnFieldPositions = default;

    // �e�s�ɐς�ł���Ղ�̍���
    private sbyte[] _fieldHeightData = default;


    // �Ղ�̐ݒu�P�\���Ԓ��̌v������
    private float _installGraceElapsedTime = default;

    // �Ղ�̌��ʉ���炷���߂̃I�[�f�B�I�\�[�X
    private AudioSource _puyoSeAudioSource;


    #region �Ղ�̑���Ɠ��͂Ɋւ���ϐ�

    // �����̂��߂̓��̓t���O
    private bool _isFallInput = default;

    // �ړ����̃t���O
    private PuyoControlInput.RightLeftInputFlag _isMoving = default;

    // ���ړ��̈ړ��񐔂��J�E���g����
    private int _horizontalMoveCounter = default;

    // �Ղ�����E�Ɉړ�����Ƃ��̈ړ���
    private float _horizontalMoveDistance = default;

    // �����̌v������
    private float _fallElapsedTime = default;

    // ����������t���O
    private bool _isFalling = default;

    // ���̗���������̗�����
    private float _fallQuantity = default;

    // ��]���̃t���O
    private PuyoControlInput.RightLeftInputFlag _isRotation = default;

    // ���݂̉�]��Ԕԍ�
    private int _rotationNumber = default;

    // ��]�J�n���̎q�Ղ��Sin,Cos�̐U�ꕝ�̒l
    List<float> _childOriginAmplitude = new List<float>();

    // ��]���Ԃ��v������
    private float _rotationElapsedTime = default;

    #endregion


    /// <summary>
    /// Scene���J�n����Ƃ��ɕϐ��̏����l��ݒ肷�鏈��
    /// </summary>
    private void Start()
    {
        // �e�p�����[�^���擾
        _playerParameter = _summarizeScriptableObjects._playerParameter;
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;

        // �T�E���h���܂Ƃ߂��X�N���v�g���擾
        _inGameSounds = _summarizeScriptableObjects._inGameSounds;

        // �t�B�[���h���Ǘ�����X�N���v�g���擾
        _fieldControl = this.GetComponent<FieldControl>();

        // �Ղ摀��̓��͂Ɋւ��鏈�����s���X�N���v�g���擾
        _puyoControlInput = this.GetComponent<PuyoControlInput>();

        // ���E�̈ړ��ʂ����߂�
        _horizontalMoveDistance = _fieldParameter._scaleOfOneMass.x / _playerParameter._horizontalMoveCounts;

        // �t�B�[���h�̍���(��)�f�[�^���擾
        _fieldHeightData = _fieldControl.GetFieldHeightData();

        // �Ղ�̗����ʂ��v�Z����
        _fallQuantity = _fieldParameter._scaleOfOneMass.y / _playerParameter._fallSmoothness;

        // �t�B�[���h���̃}�X���Ƃ̍��W���擾
        (_eachRowFieldPositions, _eachColumnFieldPositions) = _fieldControl.GetFieldMassPos();

        // ���ʉ����o�����߂̃I�[�f�B�I�\�[�X���擾
        _puyoSeAudioSource = this.GetComponent<AudioSource>();
    }



    /// <summary>
    /// �v���C���[�̓��͔�����擾���郁�\�b�h
    /// </summary>
    private void Update()
    {
        // ���X�^�[�g���鏈��
        if (ControllerInput.RestartInput())
        {
            // �V�[����ǂݒ���
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        // �v���C���[�̑��삪�ł��Ȃ���ԂȂ珈�����Ȃ�
        if (!_canPlayerControl)
        {
            return;
        }

        ///-- �e���͏����̌��ʂ��󂯎�鏈�� --///

        // ���E�ړ������Ă��Ȃ��Ƃ��A���E���͏����̌��ʂ��i�[����
        if (!_isMoving.Right && !_isMoving.Left)
        {
            _isMoving = _puyoControlInput.HorizontalMoveInput(ControllerInput.HorizontalInput());
        }
        // ���E�ړ������Ă���Ƃ��A���E���͏����݂̂����s��
        else
        {
            _puyoControlInput.HorizontalMoveInput(ControllerInput.HorizontalInput());
        }


        // �����̓��͂�����Ă��邩���󂯎�鏈��
        _isFallInput = _puyoControlInput.FallInput(ControllerInput.VerticalInput());


        // ��]�̃{�^���̓���
        PuyoControlInput.RightLeftInputFlag rotationButtonInput = default;

        // ��]�{�^���̓��͂��擾����
        rotationButtonInput.Right = ControllerInput.RightRotationInput();
        rotationButtonInput.Left = ControllerInput.LeftRotationInput();

        // ��]�����Ă��Ȃ��Ƃ��A��]���͏����̌��ʂ��i�[����
        if (!_isRotation.Right && !_isRotation.Left)
        {
            _isRotation = _puyoControlInput.PuyoRotationInput(rotationButtonInput);
        }
    }


    /// <summary>
    /// �Ղ�����ۂɓ������������s��
    /// </summary>
    private void FixedUpdate()
    {
        // ����ł���Ղ悪�Ֆʂɏo�Ă����Ԃ�
        if (!_canPlayerControl)
        {
            return;
        }

        // �Ղ����]����
        PuyoRotation();

        // �Ղ�����E�ړ�����
        PuyoHorizontalMove();

        // �Ղ悪���n�������𔻒肷��
        PuyoInstalCheck();

        // �Ղ�𗎉�����
        PuyoFall();
    }


    /// <summary>
    /// �Ղ�̍��E�ړ����s��
    /// </summary>
    private void PuyoHorizontalMove()
    {
        // �ړ����Ă��Ȃ��Ƃ��͏������Ȃ�
        if (!_isMoving.Right && !_isMoving.Left)
        {
            return;
        }

        // �ړ����J�n�����Ƃ�
        if (_horizontalMoveCounter == 0)
        {
            // �ړ���(�s��)�ƈړ������̍s�̒[
            sbyte compareRow = default;
            int endRowValue = default;

            // �E�ړ��̂Ƃ��̈ړ�����s���ƍs�̉E�[���i�[����
            if (_isMoving.Right)
            {
                compareRow = 1;
                endRowValue = _fieldParameter._fieldRowSize - 1;
            }
            // ���ړ��̂Ƃ��̈ړ�����s���ƍs�̍��[���i�[����
            else
            {
                compareRow = -1;
                endRowValue = 0;
            }

            // �ړ����\���𒲂ׂ�
            for (int i = 0; i < _controlPuyoDatas.Count; i++)
            {
                // ���ݒn���t�B�[���h�̒[�ł͂Ȃ��A�ړ���̒i�����������ȉ��Ȃ玟�̗v�f��
                if (_controlPuyoDatas[i].Row != endRowValue && _controlPuyoDatas[i].Column >= _fieldHeightData[_controlPuyoDatas[i].Row + compareRow])
                {
                    continue;
                }

                // �Ղ�̂ǂꂩ���ړ��ł��Ȃ��Ȃ�A�ړ�����߂�
                _isMoving.Right = false;
                _isMoving.Left = false;

                return;
            }
            
            // �Ղ�̈ʒu�����X�V(1�}�X�ړ�)����
            for (int i = 0; i < _controlPuyoDatas.Count; i++)
            {
                // ���삷��Ղ�̃f�[�^
                PuyoStructs.ControlPuyoData controlPuyoData = _controlPuyoDatas[i];

                // ���삷��Ղ�̒i����i�グ�āA�i�[����
                controlPuyoData.Row += compareRow;
                _controlPuyoDatas[i] = controlPuyoData;
            }
        }

        // �ړ����鋗��
        float horizontalMoveDistance = default;

        // �E�ړ��̂Ƃ��̋������i�[����
        if (_isMoving.Right)
        {
            horizontalMoveDistance = _horizontalMoveDistance;
        }
        // ���ړ��̂Ƃ��̋������i�[����
        else
        {
            horizontalMoveDistance = -_horizontalMoveDistance;
        }

        // Scene��̂Ղ���ړ�����
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            _controlPuyoDatas[i].Transform.position += new Vector3(horizontalMoveDistance, 0, 0);
        }

        // �ړ��J�E���g�𑝂₷
        _horizontalMoveCounter++;

        // �ړ��񐔂��ݒ�񐔂ɓ��B�܂��́A�ړI���W�ɓ��B������ړ����I������
        if (_horizontalMoveCounter == _playerParameter._horizontalMoveCounts)
        {
            // �ړ����I��
            _isMoving.Right = false;
            _isMoving.Left = false;

            // �ړ��J�E���g��������
            _horizontalMoveCounter = 0;
        }
    }


    /// <summary>
    /// �Ղ�̉�]���s��
    /// </summary>
    private void PuyoRotation()
    {
        // ��]�t���O���Z�b�g����Ă��Ȃ���Ώ������Ȃ�
        if (!_isRotation.Right && !_isRotation.Left)
        {
            return;
        }


        #region ��]���J�n����Ƃ��Ɉ�x�s������

        // ��]���n�߂����Ɉ�x�����s������
        if (_rotationElapsedTime == 0)
        {
            // ��]��̉�]�ԍ����i�[����
            int rotationDestinationNumber = _rotationNumber;

            // �E��]�̂Ƃ��́A��]�ԍ��ƕ�����ݒ�
            if (_isRotation.Right)
            {
                // ��]��̔ԍ���ݒ�
                rotationDestinationNumber++;

                // ��]�ԍ��̍ő�l�𒴂�����ԍ����O�ɂ���
                if (rotationDestinationNumber >= _fieldParameter._puyosEachRotationDirections[0].Length)
                {
                    rotationDestinationNumber = 0;
                }
            }
            // ����]�̂Ƃ��́A��]�ԍ��ƕ�����ݒ�
            else
            {
                // ��]��̔ԍ���ݒ�
                rotationDestinationNumber--;

                // ��]�ԍ����O�����Ȃ�ԍ����ő�l�ɂ���
                if (rotationDestinationNumber < 0)
                {
                    rotationDestinationNumber = _fieldParameter._puyosEachRotationDirections[0].Length - 1;
                }
            }

            // ��]���ł��邩�𔻒肷�鏈��
            for (int i = 1; i < _controlPuyoDatas.Count; i++)
            {
                // ��]��Ɉړ�����ʒu
                int destinationRow = _controlPuyoDatas[0].Row + _fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].x;
                int destinationColumn = _controlPuyoDatas[0].Column + _fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].y;

                // ��]��̎q�Ղ�̈ʒu���t�B�[���h�̒[�ł͂Ȃ��A�ݒu�ς݂̂Ղ悪�Ȃ��Ȃ玟�̗v�f��
                if (destinationRow < _fieldParameter._fieldRowSize && destinationRow >= 0 && destinationColumn >= 0
                    && destinationColumn >= _fieldHeightData[destinationRow])
                {
                    continue;
                }

                // ��]��̂Ղ悪���������̂��E���̂Ƃ�
                if (_fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].x > 0)
                {
                    // ���Α��Ƀt�B�[���h�̒[�܂��͂Ղ悪���������]���Ȃ�
                    if (_controlPuyoDatas[0].Row - 1 < 0 || _controlPuyoDatas[0].Column < _fieldHeightData[_controlPuyoDatas[0].Row - 1])
                    {
                        _isRotation.Right = false;
                        _isRotation.Left = false;

                        return;
                    }

                    // �E�ړ����I�����āA���ړ�������
                    _isMoving.Right = false;
                    _isMoving.Left = true;

                    PuyoHorizontalMove();

                    break;
                }
                // ��]��̂Ղ悪���������̂������̂Ƃ�
                if (_fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].x < 0)
                {
                    // ���Α��Ƀt�B�[���h�̒[�܂��͂Ղ悪���������]���Ȃ�
                    if (_controlPuyoDatas[0].Row + 1 >= _fieldParameter._fieldRowSize || _controlPuyoDatas[0].Column < _fieldHeightData[_controlPuyoDatas[0].Row + 1])
                    {
                        _isRotation.Right = false;
                        _isRotation.Left = false;

                        return;
                    }

                    // ���ړ����I�����āA�E�ړ�������
                    _isMoving.Right = true;
                    _isMoving.Left = false;

                    PuyoHorizontalMove();

                    break;
                }
                // ��]��̂Ղ悪���������̂������̂Ƃ�
                if (_fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].y < 0)
                {
                    // �Ղ����i��Ɉړ�����
                    for (int j = 0; j < _controlPuyoDatas.Count; j++)
                    {
                        // ���삷��Ղ�̃f�[�^
                        PuyoStructs.ControlPuyoData controlPuyoData = _controlPuyoDatas[j];

                        // ���삷��Ղ�̒i����i�グ��
                        controlPuyoData.Column += 1;
                        controlPuyoData.Transform.position += new Vector3(0, _fieldParameter._scaleOfOneMass.y, 0);

                        // �X�V�����Ղ�̃f�[�^���i�[����
                        _controlPuyoDatas[j] = controlPuyoData;
                    }

                    // ��������܂ł̌v�����Ԃ�����������
                    _fallElapsedTime = 0;

                    break;
                }
            }

            // �Ղ��ݒu�����Ƃ��̌��ʉ����Ȃ炷
            _puyoSeAudioSource.PlayOneShot(_inGameSounds._puyoRotationSE);

            // �q�Ղ����]�J�n�ʒu�̒l���i�[���A�ʒu�f�[�^���X�V���鏈��
            for (int i = 1; i < _controlPuyoDatas.Count; i++)
            {
                // �Ղ�̉�]�J�n�ʒu(Sin,Cos�̐U�ꕝ�̒l)
                float initialAmplitudeValue = 0;

                // ����]�̂Ƃ��̂Ղ�̉�]�J�n�ʒu��ݒ肷��
                if (_isRotation.Left)
                {
                    initialAmplitudeValue += 0.5f;
                }

                // �Ղ悪�S���̂P��]���Ă���Ƃ��̐U�ꕝ�̒l
                float quaterRotationAmplitudeValue = 0.5f;

                // �q�Ղ悪�e�Ղ�̍��E�ɂ���Ƃ��A���E�̐U�ꕝ�̒l��ݒ�
                if (_fieldParameter._puyosEachRotationDirections[_rotationNumber][i].x != 0)
                {
                    initialAmplitudeValue += quaterRotationAmplitudeValue * _fieldParameter._puyosEachRotationDirections[_rotationNumber][i].x;
                }

                // �q�Ղ悪�e�Ղ�̉������ɂ���Ȃ甼��]�̒l�𑫂����U�ꕝ�̒l�����߂�
                if (_fieldParameter._puyosEachRotationDirections[_rotationNumber][i].y != 1)
                {
                    initialAmplitudeValue = quaterRotationAmplitudeValue * 2 - initialAmplitudeValue;
                }

                // ��]�p�̒l�ɐ��K������
                initialAmplitudeValue = initialAmplitudeValue * Mathf.PI;

                // �q�Ղ�̉�]�J�n����Sin,Cos�ɑ΂���U�ꕝ�̒l���i�[����
                _childOriginAmplitude.Add(initialAmplitudeValue);

                // �q�Ղ�̃f�[�^
                PuyoStructs.ControlPuyoData controlPuyoData = _controlPuyoDatas[i];

                // �q�Ղ�̈ʒu�f�[�^���X�V���Ă���
                controlPuyoData.Row = _controlPuyoDatas[0].Row + _fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].x;
                controlPuyoData.Column = _controlPuyoDatas[0].Column + _fieldParameter._puyosEachRotationDirections[rotationDestinationNumber][i].y;

                // �X�V�����Ղ�f�[�^���i�[����
                _controlPuyoDatas[i] = controlPuyoData;
            }

            // ��]�ԍ����X�V����
            _rotationNumber = rotationDestinationNumber;
        }

        #endregion ��]���J�n����Ƃ��Ɉ�x�����s������


        // ��]���̎��Ԃ��v������
        _rotationElapsedTime += Time.fixedDeltaTime;

        // ��]���Ԃ��I������Ƃ��̏���
        if (_rotationElapsedTime >= _playerParameter._quarterRotationTime)
        {
            // �q�Ղ����]�I�����̈ʒu�Ɉړ�����
            for (int i = 1; i <= _childOriginAmplitude.Count; i++)
            {
                // �Ղ�̐e�Ղ�ɑ΂���Ղ�̑��΍��W�����߂�
                Vector2 puyoRelativePosition = _fieldParameter._puyosEachRotationDirections[_rotationNumber][i] * _fieldParameter._scaleOfOneMass;

                // �Ղ�̃��[���h���W�����߂�
                Vector3 childPuyoPosition = _controlPuyoDatas[0].Transform.position + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

                // �ړ�����
                _controlPuyoDatas[i].Transform.position = childPuyoPosition;
            }

            // ��]���I�����A�v�����Ԃ̏�����
            _isRotation.Right = false;
            _isRotation.Left = false;
            _rotationElapsedTime = 0;

            // �U�ꕝ�̒l��������
            _childOriginAmplitude.Clear();

            return;
        }

        // �e�Ղ�𒆐S�Ɏq�Ղ���E��]����
        if (_isRotation.Right)
        {
            // �U�ꕝ�̒l�����߂āA�e�̍��W�����]���̎q�Ղ�̍��W�����߂�
            for (int i = 0; i < _childOriginAmplitude.Count; i++)
            {
                // ��]�Ɏg��Sin,Cos�̌��݂̐U�ꕝ�̒l�����߂�
                float rotationAmplitude = _playerParameter._normalizedRotationSpeed * _rotationElapsedTime + _childOriginAmplitude[i];

                _controlPuyoDatas[i + 1].Transform.position = _controlPuyoDatas[0].Transform.position + new Vector3(
                    Mathf.Sin(rotationAmplitude) * _fieldParameter._scaleOfOneMass.x, Mathf.Cos(rotationAmplitude) * _fieldParameter._scaleOfOneMass.y, 0);
            }
        }
        // �e�Ղ�𒆐S�Ɏq�Ղ������]����
        else
        {
            // �U�ꕝ�̒l���g���A�e�̍��W�����]���̎q�Ղ�̍��W�����߂�
            for (int i = 0; i < _childOriginAmplitude.Count; i++)
            {
                // ��]�Ɏg��Sin,Cos�̌��݂̐U�ꕝ�̒l�����߂�
                float rotationAmplitude = _playerParameter._normalizedRotationSpeed * _rotationElapsedTime + _childOriginAmplitude[i];

                _controlPuyoDatas[i + 1].Transform.position = _controlPuyoDatas[0].Transform.position + new Vector3
                    (Mathf.Cos(rotationAmplitude) * _fieldParameter._scaleOfOneMass.x, Mathf.Sin(rotationAmplitude) * _fieldParameter._scaleOfOneMass.y, 0);
            }
        }
    }


    /// <summary>
    /// �Ղ悪�������鏈�����s��
    /// </summary>
    private void PuyoFall()
    {
        // �Ղ悪��������
        if(!_isFalling)
        {
            return;
        }

        // �������Ă��鎞�Ԃ��v��
        _fallElapsedTime += Time.fixedDeltaTime;

        // �Ղ悪���������邩�������t���O
        bool isPuyoFall = false;

        // �����͎��̍�����������
        if (_isFallInput)
        {
            // �������Ԍo�߂��Ă��Ȃ���Η������Ȃ�
            if (_fallElapsedTime < _playerParameter._highSpeedFallTime)
            {
                return;
            }

            // �Ղ旎���t���O���Z�b�g
            isPuyoFall = true;
        }
        // �����͎��̒ʏ헎������
        else
        {
            // �������Ԍo�߂��Ă��Ȃ���Η������Ȃ�
            if (_fallElapsedTime < _playerParameter._normalSpeedFallTime)
            {
                return;
            }

            // �Ղ旎���t���O���Z�b�g
            isPuyoFall = true;
        }
        
        // �Ղ悪�����^�C�~���O����Ȃ��Ȃ珈�����Ȃ�
        if (!isPuyoFall)
        {
            return;
        }

        ///-- �Ղ�𗎉������鏈�� --///

        // �Ղ�𗎉��l������������
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            _controlPuyoDatas[i].Transform.position -= new Vector3(0, _fallQuantity, 0);
        }

        // �v�����Ԃ�������
        _fallElapsedTime = 0;

        // �Ղ�̍��������݂���i�̍����ȏ�Ȃ�Ղ�̒i���X�V���Ȃ�
        if (_controlPuyoDatas[0].Transform.position.y >= _eachColumnFieldPositions[_controlPuyoDatas[0].Column])
        {
            return;
        }

        // �t�B�[���h�̍������Ⴂ�ʒu�ɂ���Ղ悪���邩
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            // �Ղ悪���݂��鍂�����t�B�[���h�̍����ȏ�Ȃ玟�̗v�f��
            if (_controlPuyoDatas[i].Transform.position.y > _eachColumnFieldPositions[_fieldHeightData[_controlPuyoDatas[i].Row]])
            {
                continue;
            }

            // �Ղ�𐳂������n�ʒu�ɏC�����鏈��
            for (int j = 0; j < _controlPuyoDatas.Count; j++)
            {
                _controlPuyoDatas[j].Transform.position = new Vector3(_eachRowFieldPositions[_controlPuyoDatas[j].Row], _eachColumnFieldPositions[_controlPuyoDatas[j].Column], 0);
            }
        
            return;
        }

        // �Ղ�̌��݂���i����i������
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            // ���삷��Ղ�̃f�[�^
            PuyoStructs.ControlPuyoData controlPuyoData = _controlPuyoDatas[i];

            // ���삷��Ղ�̒i����i�����āA�i�[����
            controlPuyoData.Column -= 1;
            _controlPuyoDatas[i] = controlPuyoData;
        }
    }


    /// <summary>
    /// �Ղ悪���n�����������m����
    /// </summary>
    private void PuyoInstalCheck()
    {
        // �ݒu�P�\���Ԓ����������t���O
        bool inInstallGraceTime = false;

        // �Ղ悪���n�ł��邩�𔻒f���鏈��
        for (int i = 0; i < _controlPuyoDatas.Count; i++)
        {
            // ����Ղ�̍������t�B�[���h�̍�����荂���A����Ղ�̍��W���s�̒��S���W������ɂ���Ȃ玟�̗v�f��
            if (_controlPuyoDatas[i].Column > _fieldHeightData[_controlPuyoDatas[i].Row] ||
                _controlPuyoDatas[i].Transform.position.y > _eachColumnFieldPositions[_fieldHeightData[_controlPuyoDatas[i].Row]])
            {
                continue;
            }

            // ���n�ł���Ƃ��̃t���O���Z�b�g
            inInstallGraceTime = true;

            // �������~����
            _isFalling = false;

            break;
        }

        // �Ղ悪���n�ł���Ƃ��̏���
        if (inInstallGraceTime)
        {
            _installGraceElapsedTime += Time.fixedDeltaTime;

            // ��]���A�ړ����Ȃ�ݒu���Ȃ�
            if (_isMoving.Left || _isMoving.Right ||
                _isRotation.Left || _isRotation.Right)
            {
                return;
            }

            // �ݒu�P�\���Ԃ𒴂���܂��́A�����͂������Ă�����Ղ��ݒu����
            if (_installGraceElapsedTime >= _playerParameter._installGraceTime || _isFallInput)
            {
                // �v���C���[�̑�����I������
                _canPlayerControl = false;

                // ��]�ԍ��������l�ɂ���
                _rotationNumber = 0;

                // �Ղ��ݒu�����Ƃ��̏���������
                _fieldControl.PuyoInstallProcess(_controlPuyoDatas);
            }
        }
        else
        {
            // �ݒu�P�\���Ԃ�������
            _installGraceElapsedTime = 0;

            // �Ղ�̗������ł����Ԃɂ���
            _isFalling = true;
        }
    }


    /// <summary>
    /// �v���C���[�̑�����J�n����Ƃ��̏���
    /// </summary>
    /// <param name="controlPuyoDatas"> ���삷��Ղ�� �F, Transform, �s, �� </param>
    public void PlayerControlStart(List<PuyoStructs.ControlPuyoData> controlPuyoDatas)
    {
        // ���삷��Ղ�̃f�[�^���i�[����
        _controlPuyoDatas = controlPuyoDatas;

        // �Ղ�̍����f�[�^���X�V����
        _fieldHeightData = _fieldControl.GetFieldHeightData();

        // �Ղ�̑�����J�n����
        _canPlayerControl = true;

        // �Ղ悪�����ł����Ԃɂ���
        _isFalling = true;
    }
}
