
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #NextPuyoControl.cs
/// �쐬��:�g�c�Y��
/// 
/// �t�B�[���h�̃I�u�W�F�N�g�ɃA�^�b�`���Ă�������
/// �l�N�X�g�̂Ղ���R���g���[���A��������X�N���v�g
/// --------------------------------------------------

public class NextPuyoControl : MonoBehaviour
{
    [SerializeField, Header("�p�����[�^��ScriptableObject���܂Ƃ߂��X�N���v�g")]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    // �t�B�[���h�̃p�����[�^
    private FieldParameter _fieldParameter;

    // �v���C���[�Ɋւ���p�����[�^(ScriptableObject)
    private PlayerParameter _playerParameter;

    // �v���C���[�̓��͂Ɠ�����s���X�N���v�g
    private PlayerOperation _playerOperation;

    // �l�N�X�g�Ղ�̒��I������X�N���v�g
    private NextPuyoLottery _nextPuyoLottery;

    // �Q�[���i�s�������t���O���܂Ƃ߂��X�N���v�g
    private PlayerFlags _playerFlags;


    // �Ղ�̐F�ԍ���Transform���i�[����W���O�z��ƁA���ɑ��삷��Ղ�̃|�C���^
    private sbyte[][] _puyosColors = default;
    private Transform[][] _puyosTransforms = default;
    private int _controlPuyoPointer = default;

    [SerializeField, Header("�l�N�X�g�̂Ղ�̈ʒu(�ԍ��̒Ⴂ���Ɏ��̂Ղ���W������)")]
    private Transform[] _nextPuyoPositions = default;

    // �l�N�X�g�̂Ղ�̈ړ�����(��b������)
    private Vector3[] _nextPuyoMoveSpeed = default;

    // �Ղ�̏o���ʒu
    private Vector3 _controlPuyoGeneratePosition;


    // �l�N�X�g�Ղ悪�����Ă���Ԃ̌v������
    private float _nextMoveElapsedTime = default;

    // �l�N�X�g�̒����Ղ悪�ړ����Ă��邩
    private bool _nextPuyosInstantiated = default;


    private void Start()
    {
        // �t�B�[���h�Ɋւ���p�����[�^���擾
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;
        // �v���C���[�Ɋւ���p�����[�^���擾
        _playerParameter = _summarizeScriptableObjects._playerParameter;

        // �Ղ�̒��I���s���X�N���v�g���擾
        _nextPuyoLottery = this.GetComponent<NextPuyoLottery>();
        // �Q�[���̐i�s�ɂ������t���O���Ǘ�����X�N���v�g���擾
        _playerFlags = this.GetComponent<PlayerFlags>();
        // �v���C���[�̓��͂Ɠ�����s���X�N���v�g���擾
        _playerOperation = this.GetComponent<PlayerOperation>();

        // �W���O�z��̈ꎟ���ڂ̗v�f�����`
        _puyosColors = new sbyte[_nextPuyoPositions.Length - 1][];
        _puyosTransforms = new Transform[_nextPuyoPositions.Length - 1][];

        // �W���O�z��̓񎟌��ڂ̗v�f�����`
        for (int i = 0; i < _puyosColors.Length; i++)
        {
            // �Ղ悪��ȊO�̏ꍇ�̓��������Ă��Ȃ����߁A�Q(�Ղ�̐�)�Ƃ��Ă��܂�
            _puyosColors[i] = new sbyte[2];
            _puyosTransforms[i] = new Transform[2];
        }

        // �l�N�X�g���̈ړ������������z��̑傫�����`����
        _nextPuyoMoveSpeed = new Vector3[_nextPuyoPositions.Length - 1];

        // �l�N�X�g���̈ړ����x�����߂�
        for (int i = 0; i < _nextPuyoMoveSpeed.Length; i++)
        {
            // �l�N�X�g���̈�b������̈ړ��������v�Z����
            _nextPuyoMoveSpeed[i] = (_nextPuyoPositions[i].position - _nextPuyoPositions[i + 1].position) / _playerParameter._nextPuyoMoveTime;
        }

        // �t�B�[���h�̍����[�̍��W�����߂�
        Vector2 bottomLeftEdgeMassPos = this.transform.position - this.transform.lossyScale / 2;

        //�@�t�B�[���h�̍����[�̃}�X�̒��S���߂�
        bottomLeftEdgeMassPos += _fieldParameter._scaleOfOneMass / 2;

        // ���삷��Ղ�̐������W�����߂�
        _controlPuyoGeneratePosition = new Vector3(bottomLeftEdgeMassPos.x + _fieldParameter._scaleOfOneMass.x * _playerParameter._puyoInstantRow, 
            bottomLeftEdgeMassPos.y + _fieldParameter._scaleOfOneMass.y * _fieldParameter._fieldColumnSize - 1, 0);

        // �Q�[�����J�n����
        GamePlayStart();
    }


    private void FixedUpdate()
    {
        // �l�N�X�g�̂Ղ�X�V������Ȃ���Ώ������Ȃ�
        if (!_playerFlags._isNextPuyoUpdate)
        {
            return;
        }

        // �Ղ�𐶐����Ă��Ȃ��Ƃ��̏���
        if (!_nextPuyosInstantiated)
        {
            // �Ղ�𐶐����鏈��
            NextPuyoInstantiate();

            return;
        }

        // �l�N�X�g�̘g�����ړ����鏈��
        NextPuyoPositionUpdate();
    }


    /// <summary>
    /// �l�N�X�g�Ղ�̈ړ����s������
    /// </summary>
    private void NextPuyoPositionUpdate()
    {
        // �l�N�X�g�Ղ�̈ړ����Ԃ��v��
        _nextMoveElapsedTime += Time.fixedDeltaTime;

        // �l�N�X�g�Ղ悷�ׂĂ̈ړ����s���܂ŌJ��Ԃ�
        for (int i = 0; i < _puyosColors.Length; i++)
        {
            // �������l�N�X�g�Ղ�̃|�C���^
            int moveNextPuyoPointer = _controlPuyoPointer + i;

            // �|�C���^���v�f���𒴂������ɐ������ʒu�ɂ���
            if (moveNextPuyoPointer >= _puyosColors.Length)
            {
                moveNextPuyoPointer -= _puyosColors.Length;
            }

            // �l�N�X�g�Ղ���ړ�������
            for (int j = 0; j < _puyosColors[i].Length; j++)
            {
                // �Ղ���ړ�����
                _puyosTransforms[moveNextPuyoPointer][j].position += _nextPuyoMoveSpeed[i] * Time.fixedDeltaTime;
            }
        }
        // �l�N�X�g�Ղ�̈ړ����I��������
        if (_nextMoveElapsedTime < _playerParameter._nextPuyoMoveTime)
        {
            return;
        }


        ///-- �l�N�X�g�Ղ�̈ړ����I�������Ƃ��̏��� --///

        // �l�N�X�g�Ղ�̐����A�X�V�t���O�ƈړ����Ԃ�������
        _playerFlags._isNextPuyoUpdate = false;
        _nextPuyosInstantiated = false;
        _nextMoveElapsedTime = 0;

        // �l�N�X�g�̂Ղ悪�������ʒu(�l�N�X�g�̘g)�ɂ��邩���m�F���鏈��
        for (int i = 0; i < _puyosColors.Length; i++)
        {
            // �������ʒu�ɂ��邩���m�F����Ղ�̃|�C���^
            int checkPuyoPointer = _controlPuyoPointer + i;

            // �|�C���^���Ղ���i�[���Ă���z��̐��𒴂�����A�|�C���^�̈ʒu�𐳂����Ȃ���
            if (checkPuyoPointer >= _puyosColors.Length)
            {
                checkPuyoPointer -= _puyosColors.Length;
            }

            // �l�N�X�g�Ղ悪���������W�ɂ���Ȃ玟�̗v�f��
            if (_puyosTransforms[checkPuyoPointer][0].position == _nextPuyoPositions[i].position)
            {
                continue;
            }

            // �l�N�X�g�Ղ悪���������W�ɂȂ��Ȃ琳�������W�ɂ���
            for (int j = 0; j < _puyosColors[i].Length; j++)
            {
                // �Ղ�̐e�Ղ�ɑ΂���Ղ�̑��΍��W�����߂�
                Vector2 puyoRelativePosition =
                    _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][j] * _fieldParameter._scaleOfOneMass;

                // �Ղ�̃��[���h���W�����߂�
                Vector3 puyoPosition = _nextPuyoPositions[i].position + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

                // �Ղ���ړ�����
                _puyosTransforms[checkPuyoPointer][j].position = puyoPosition;
            }
        }

        // ���삷��Ղ�� �F, Transform, �s, �i
        List<PuyoStructs.ControlPuyoData> controlPuyoDatas = new List<PuyoStructs.ControlPuyoData>();

        // �l�N�X�g�Ղ悩��t�B�[���h��ɏo������
        for (int i = 0; i < _puyosTransforms[_controlPuyoPointer].Length; i++)
        {
            // �Ղ�̐e�Ղ�ɑ΂��鑊�΍��W�����߂�
            Vector2 puyoRelativePosition =
                _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][i] * _fieldParameter._scaleOfOneMass;

            // �t�B�[���h�ɏo���Ղ�̍��W�����߂�
            Vector3 puyoPosition =
                _controlPuyoGeneratePosition + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

            // �Ղ���ړ�����
            _puyosTransforms[_controlPuyoPointer][i].position = puyoPosition;

            // ���삷��Ղ�̃f�[�^
            PuyoStructs.ControlPuyoData controlPuyoData = default;

            // ���삷��Ղ�̐F��Transform���i�[����
            controlPuyoData.Color = _puyosColors[_controlPuyoPointer][i];
            controlPuyoData.Transform = _puyosTransforms[_controlPuyoPointer][i];
            // ���삷��Ղ�̗�ƍs���i�[����
            controlPuyoData.Row = _playerParameter._puyoInstantRow + _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][i].x;
            controlPuyoData.Column = _fieldParameter._fieldColumnSize - 1 + _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][i].y;

            // ���삷��Ղ�̃f�[�^���i�[����
            controlPuyoDatas.Add(controlPuyoData);
        }

        // ���삷��Ղ�̃f�[�^��n���A�v���C���[�̑�����J�n����
        _playerOperation.PlayerControlStart(controlPuyoDatas);
    }


    /// <summary>
    /// �Ղ��V�����������鏈��
    /// </summary>
    private void NextPuyoInstantiate()
    {
        // ���삷��Ղ�̃|�C���^���C���N�������g
        _controlPuyoPointer++;

        // �|�C���^���Ղ���i�[���Ă���z��̐��𒴂�����0�ɖ߂�
        if (_controlPuyoPointer >= _puyosColors.Length)
        {
            _controlPuyoPointer = 0;
        }

        // ��ԉ����l�N�X�g�Ղ�f�[�^�̃|�C���^
        int lastNextPuyoPointer = _controlPuyoPointer - 1;

        // ��ԉ����l�N�X�g�Ղ�̃|�C���^��0�����ɂȂ��Ă��܂�����v�f�̖����ɂ���
        if (lastNextPuyoPointer < 0)
        {
            lastNextPuyoPointer = _puyosColors.Length - 1;
        }

        // �Ղ悪�A���Ő�������Ă���Ƃ�
        if (_nextPuyosInstantiated)
        {
            // �l�N�X�g�̂Ղ悪�������ʒu(�l�N�X�g�̘g)�ɂ��邩���m�F���鏈��
            for (int i = 0; i < _puyosColors.Length; i++)
            {
                // �������ʒu�ɂ��邩���m�F����Ղ�̃|�C���^
                int puyoPointer = _controlPuyoPointer + i;

                // �|�C���^���Ղ���i�[���Ă���z��̐��ȏ�Ȃ�A�|�C���^�̈ʒu�𐳂����Ȃ���
                if (puyoPointer >= _puyosColors.Length)
                {
                    puyoPointer -= _puyosColors.Length;
                }

                // �Ղ悪�܂���������Ă��Ȃ��Ȃ�`�F�b�N���I������
                if (_puyosTransforms[puyoPointer][0] == null)
                {
                    continue;
                }

                // �l�N�X�g�̈ړ���̍��W���i�[���Ă���z��̃|�C���^
                int nextPositionsPointer = i + 1;

                // �l�N�X�g�Ղ悪���������W�ɂ���Ȃ玟�̗v�f��
                if (_puyosTransforms[puyoPointer][0].position == _nextPuyoPositions[nextPositionsPointer].position)
                {
                    continue;
                }

                // �l�N�X�g�Ղ�̐��������W�ɂȂ��Ƃ��A���������W�ɂ���
                for (int j = 0; j < _puyosColors[puyoPointer].Length; j++)
                {
                    // �Ղ�̐e�Ղ�ɑ΂��鑊�΍��W�����߂�
                    Vector2 puyoRelativePosition =
                        _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][j] * _fieldParameter._scaleOfOneMass;

                    // ���̂Ղ�̍��W�����߂�
                    Vector3 puyoPosition = _nextPuyoPositions[nextPositionsPointer].position + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

                    // �Ղ���ړ�����
                    _puyosTransforms[puyoPointer][j].position = puyoPosition;
                }
            }
        }

        // �l�N�X�g�̂Ղ�𒊑I����
        sbyte[] lotteryNextPuyosColors = new sbyte[] { _nextPuyoLottery.NextPuyoNumberLottery(), _nextPuyoLottery.NextPuyoNumberLottery() };

        // ��ԉ����l�N�X�g�ɂՂ��ǉ����AScene��ɐ������鏈��
        for (int i = 0; i < lotteryNextPuyosColors.Length; i++)
        {
            // ���������Ղ��F�z��ɒǉ�
            _puyosColors[lastNextPuyoPointer][i] = lotteryNextPuyosColors[i];

            // �Ղ�̐e�Ղ�ɑ΂��鑊�΍��W�����߂�
            Vector2 puyoRelativePosition =
                _fieldParameter._puyosEachRotationDirections[_fieldParameter._initialRotateNumber][i] * _fieldParameter._scaleOfOneMass;

            // ���̂Ղ�̃��[���h���W�����߂�
            Vector3 puyoPosition = _nextPuyoPositions[_nextPuyoPositions.Length - 1].position
                + new Vector3(puyoRelativePosition.x, puyoRelativePosition.y, 0);

            // �Ղ�𐶐����ATransform�z��Ɋi�[����(Scene��ɐ�������Ղ�͐F�ԍ� - 1(�F�ԍ��̋�}�X���O�ł��邽��)
            _puyosTransforms[lastNextPuyoPointer][i] =
                Instantiate(_fieldParameter._puyoSprits[_puyosColors[lastNextPuyoPointer][i] - 1], puyoPosition, Quaternion.identity).transform;

            // ���������Ղ�̑傫����ύX����
            _puyosTransforms[lastNextPuyoPointer][i].localScale = _fieldParameter._puyoLocalScales[lotteryNextPuyosColors[i]];
        }

        // �l�N�X�g�Ղ�𐶐��ς݃t���O���Z�b�g
        _nextPuyosInstantiated = true;
    }


    /// <summary>
    /// �Q�[���v���C���J�n����Ƃ��̏���
    /// </summary>
    private void GamePlayStart()
    {
        // �l�N�X�g�̐��ɉ����āA�����Ղ�𐶐�����
        for (int i = 0; i < _nextPuyoPositions.Length - 1; i++)
        {
            NextPuyoInstantiate();
        }

        // �Ղ�̑�����J�n����
        _playerFlags._isNextPuyoUpdate = true;
    }
}
