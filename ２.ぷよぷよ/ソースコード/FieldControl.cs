
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #FieldControl.cs
/// �쐬��:�g�c�Y��
/// 
/// �t�B�[���h�̃I�u�W�F�N�g�ɃA�^�b�`���Ă�������
/// �t�B�[���h�̃f�[�^�Ɛݒu�ς݂̂Ղ���Ǘ�����X�N���v�g�ł�
/// --------------------------------------------------

public class FieldControl : MonoBehaviour
{
    [SerializeField]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    // �t�B�[���h�̃p�����[�^
    private FieldParameter _fieldParameter;

    // �Q�[�����̃T�E���h���܂Ƃ߂�������Ղ�
    private InGameSounds _inGameSounds;

    // �Q�[���i�s�������t���O���܂Ƃ߂��X�N���v�g
    private PlayerFlags _playerFlags;

    // �Ղ�̐ݒu�A�������o���s���X�N���v�g
    private PuyoPresentation _puyoPresentation;

    // �t�B�[���h��̂Ղ�f�[�^(�Ղ�̐F�ԍ��Ŋi�[���Ă���)
    private sbyte[,] _puyoColorsData = default;
    private Transform[,] _puyoTransforms = default;

    // �Ղ悪�����ς݂��������z��
    private bool[,] _isSearchedPuyos = default;

    // �t�B�[���h�̂Ղ�̍���
    private sbyte[] _fieldHeightData = default;

    // �����Ղ�̈ʒu
    private List<int> _eracePuyoColumns = new List<int>(), _eracePuyoRows = new List<int>();
    // �A�����Ă���Ղ�̈ʒu
    private List<int> _linkingPuyoRows = new List<int>(), _linkingPuyoColumns = new List<int>();

    private bool _isInstallingPresentation = default;
    private bool _isEracingPresentation = default;

    // �Ղ�������Ƃ��A�e�s�̏����������i�[����
    private sbyte[] _eachRowEraceCount = default;

    // �e�s�Ɨ�̍��W
    private float[] _eachRowFieldPositions = default;
    private float[] _eachColumnFieldPositions = default;

    // �Ղ�̌��ʉ���炷���߂̃I�[�f�B�I�\�[�X
    private AudioSource _puyoSeAudioSource;

    [SerializeField, Header("�Q�[���I�[�o�[���̃e�L�X�g")]
    private GameObject _gameOverText;

    #region �Ղ�̒l��A�����Ɉ����萔

    // �t�B�[���h�f�[�^�ɓ�����}�X�̒l
    private const sbyte EMPTY_PUYO_NUMBER = 0;

    // �Ղ悪�Ȃ����Ă��邩��������������Ƃ��̐�
    private const sbyte SEARCH_DIRECTION_NUMBER = 4;
    private readonly sbyte[] SEARCH_DESTINATION_ROW = new sbyte[SEARCH_DIRECTION_NUMBER] { 0, 0, 1, -1 };
    private readonly sbyte[] SEARCH_DESTINATION_COLUMN = new sbyte[SEARCH_DIRECTION_NUMBER] { 1, -1, 0, 0 };

    #endregion


    /// <summary>
    /// �t�B�[���h�̍��W�̌v�Z��A�Ղ�̑傫���Ȃǂ̐ݒ���s��
    /// </summary>
    private void Awake()
    {
        // �t�B�[���h�Ɋւ���p�����[�^���擾
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;
        // �T�E���h���܂Ƃ߂��X�N���v�g���擾
        _inGameSounds = _summarizeScriptableObjects._inGameSounds;

        // �Q�[���̐i�s�ɂ������t���O���Ǘ�����X�N���v�g���擾
        _playerFlags = this.GetComponent<PlayerFlags>();
        // �Ղ�̉��o���s���X�N���v�g���擾
        _puyoPresentation = this.GetComponent<PuyoPresentation>();

        // �t�B�[���h�������z��^�̕ϐ��̑傫�����`����(��ɂ͈�}�X�]���ɍ��A�͂ݏo�����Ղ����������)
        _puyoColorsData = new sbyte[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];
        _puyoTransforms = new Transform[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];

        // �t�B�[���h��񐔂���Ȃ�z��̑傫�����`����
        _isSearchedPuyos = new bool[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];
        _eachRowEraceCount = new sbyte[_fieldParameter._fieldRowSize];


        // �e�s�Ɨ�̍��W
        _eachRowFieldPositions = new float[_fieldParameter._fieldRowSize];
        _eachColumnFieldPositions = new float[_fieldParameter._fieldColumnSize];

        // �t�B�[���h�̍����[�̍��W�����߂�
        Vector2 bottomLeftEdgeMassPos = this.transform.position - this.transform.lossyScale / 2;
        //�@�t�B�[���h�̍����[�̃}�X�̒��S�����߂�
        bottomLeftEdgeMassPos += _fieldParameter._scaleOfOneMass / 2;

        // �s�����J��Ԃ�
        for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
        {
            // �e�s�̍��W�����߂�
            _eachRowFieldPositions[i] = bottomLeftEdgeMassPos.x + _fieldParameter._scaleOfOneMass.x * i;
        }
        // �񐔕��J��Ԃ�
        for (int i = 0; i < _fieldParameter._fieldColumnSize; i++)
        {
            // �s�̍��W�����߂�
            _eachColumnFieldPositions[i] = bottomLeftEdgeMassPos.y + _fieldParameter._scaleOfOneMass.y * i;
        }

        // �t�B�[���h�̍����f�[�^�̑傫�����`
        _fieldHeightData = new sbyte[_fieldParameter._fieldRowSize];

        // ���ʉ���炷���߃I�[�f�B�I�\�[�X���擾
        _puyoSeAudioSource = this.GetComponent<AudioSource>();
    }


    /// <summary>
    /// �Ղ悪�����Ă���ԂȂǁA�t���O�ɉ����ē���̏������s��
    /// </summary>
    private void FixedUpdate()
    {
        // �Ղ�̐ݒu���o���̏���
        if (_isInstallingPresentation)
        {
            // �Ղ�̐ݒu���o���I�������Ƃ��A�ݒu�������s���B
            if (!_puyoPresentation._duringInstallPresentation)
            {

            }
        }

        // �Ղ�̏������o���̏���
        if (_isEracingPresentation)
        {
            // �Ղ�̏������o���I�������Ƃ��A�����������s���B
            if (!_puyoPresentation._duringEracePresentation)
            {
                // �������o���Ă��Ȃ���Ԃ̃t���O���Z�b�g
                _isEracingPresentation = false;

                // �Ղ�����������t���O���Z�b�g
                _playerFlags._isPuyoEraced = true;

                // �Ղ�������������̌��ʉ����Ȃ炷
                _puyoSeAudioSource.PlayOneShot(_inGameSounds._puyoEraceSE);

                // �Ղ����������
                PuyoEraceProcess();
            }
        }
    }


    /// <summary>
    /// ������Ղ悪���邩�Ֆʏ�̂��ׂĂ�T������
    /// </summary>
    private void SearchForEracePuyosAllMass()
    {
        // �Ղ���������邩�������t���O
        bool isPuyoErace = false;

        // ���ׂĂ̍s����������܂ŌJ��Ԃ�
        for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
        {
            // ����������Ƃ��ɃX�L�b�v����}�X��
            sbyte searchSkipMass = 2;
            // �������J�n�����(�i)
            int serchStartColumn = 0;

            // ��������s�������s�Ȃ�A�������J�n��������i�グ��
            if (i % 2 == 0)
            {
                serchStartColumn = 1;
            }

            // �Ղ悪�A�����Ă��邩���}�X���Ƃ�(�P�i��΂��ŉ�������)��������
            for (int j = serchStartColumn; j < _fieldParameter._fieldColumnSize; j += searchSkipMass)
            {
                // ��������}�X�̂Ղ�̐F���i�[
                sbyte searchColor = _puyoColorsData[i, j];

                // ��������}�X����Ȃ玟�̍s�ɂ���
                if (searchColor == 0)
                {
                    break;
                }

                // ��������}�X�������ς݂Ȃ�A���̒i�ɂ���
                if (_isSearchedPuyos[i, j])
                {
                    continue;
                }

                // �����}�X�̂Ղ�Ɠ����F�̂Ղ悪����ɂ��邩�T������
                SearchSurroundingSameColor(searchColor, i, j);

                // �Ղ�̘A�������Ղ悪�����鐔�𒴂����Ƃ��̏���
                if (_linkingPuyoRows.Count >= _fieldParameter._puyoEraceLinkCount)
                {
                    // �Ղ�����t���O���Z�b�g
                    isPuyoErace = true;

                    // ��������Ղ�̔z��ɘA�����Ă���Ղ���i�[����
                    for (int k = 0; k < _linkingPuyoRows.Count; k++)
                    {
                        _eracePuyoRows.Add(_linkingPuyoRows[k]);
                        _eracePuyoColumns.Add(_linkingPuyoColumns[k]);
                    }
                }

                // �A�����Ă���Ղ�ʒu�̔z���������
                _linkingPuyoRows = new List<int>();
                _linkingPuyoColumns = new List<int>();
            }
        }
        // �Ղ悪�����ς݂��������z�������������
        _isSearchedPuyos = new bool[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];

        // �Ղ悪�����Ă��Ȃ��Ȃ�A�����f�[�^���X�V���ď������I����
        if (!isPuyoErace)
        {
            // �Q�[���I�[�o�[�}�X�ɐݒu���Ă��邩�𔻒肷��
            if (_puyoColorsData[_fieldParameter._gameOverRow, _fieldParameter._gameOverColumn] != 0)
            {
                // �Q�[���I�[�o�[�̏������s��
                GameOverProcess();

                return;
            }

            // �t�B�[���h�̍����f�[�^���X�V
            for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
            {
                FieldHeightDataUpdate(i);
            }

            // �Ղ悪���������Ƃɗ����鏈��
            PuyoFallProcessAfterErace();

            // �S�Ă̂Ղ悪�����I������Ƃ��̃t���O���Z�b�g
            _playerFlags._isAllPuyoEraced = true;

            return;
        }

        // �Ղ�������̃t���O���Z�b�g
        _isEracingPresentation = true;

        // ��������Ղ��Transform
        List<Transform> eracePuyoTransforms = new List<Transform>();

        // ��������Ղ��Transform���i�[����
        for (int i = 0; i < _eracePuyoRows.Count; i++)
        {
            eracePuyoTransforms.Add(_puyoTransforms[_eracePuyoRows[i], _eracePuyoColumns[i]]);
        }

        // �Ղ�̏������̉��o���s��
        _puyoPresentation.PuyoEracePresentationStart(eracePuyoTransforms);
    }


    /// <summary>
    /// �Ղ悪���������̏���
    /// </summary>
    private void PuyoEraceProcess()
    {
        // ������Ղ�̐������J��Ԃ��āA�����鏈�����s��
        for (int i = 0; i < _eracePuyoColumns.Count; i++)
        {
            // �Ղ��Scene�ォ�����
            _puyoTransforms[_eracePuyoRows[i], _eracePuyoColumns[i]].gameObject.SetActive(false);

            // �t�B�[���h�f�[�^�̏������Ղ�̈ʒu����ɂ���
            _puyoColorsData[_eracePuyoRows[i], _eracePuyoColumns[i]] = EMPTY_PUYO_NUMBER;

            // �Ղ�̏��������𑫂�
            _eachRowEraceCount[_eracePuyoRows[i]] += 1;
        }

        // �Ղ悪���������Ƃɗ����鏈��
        PuyoFallProcessAfterErace();

        // �i�[���ꂽ�����Ղ��������
        _eracePuyoRows = new List<int>();
        _eracePuyoColumns = new List<int>();

        // �Ղ悪�����ς݂��������z���������
        _isSearchedPuyos = new bool[_fieldParameter._fieldRowSize, _fieldParameter._fieldColumnSize];

        // �e�s�̂Ղ�̏���������������
        _eachRowEraceCount = new sbyte[_fieldParameter._fieldRowSize];

        // �Ղ悪�����邩��T�����鏈��
        SearchForEracePuyosAllMass();
    }


    /// <summary>
    /// �Ղ�̘A�����Ă��邩��T�����A���̐���Ԃ����\�b�h
    /// </summary>
    /// <param name="searchColor"> �T������Ղ�̐F�ԍ� </param>
    /// <param name="searchOriginRow" name="searchOriginColumn"> �T�������錳(���S)�ɂȂ�s�ƒi�̈ʒu </param>
    private void SearchSurroundingSameColor(sbyte searchColor, int searchOriginRow, int searchOriginColumn)
    {
        // �����J�n����ʒu�̂Ղ��A���Ղ�̔z��ɒǉ�����
        _linkingPuyoRows.Add(searchOriginRow);
        _linkingPuyoColumns.Add(searchOriginColumn);

        // �����J�n�ʒu�������ς݂ɂ���
        _isSearchedPuyos[searchOriginRow, searchOriginColumn] = true;

        // �㉺���E�̂Ղ�̐F�̊m�F���s��
        for (sbyte i = 0; i < SEARCH_DIRECTION_NUMBER; i++)
        {
            // ��������s�����߂�
            int searchDestinationRow = searchOriginRow + SEARCH_DESTINATION_ROW[i];

            // ���݂��Ȃ��s�Ȃ玟�̗v�f��
            if (searchDestinationRow < 0 || searchDestinationRow >= _fieldParameter._fieldRowSize)
            {
                continue;
            }

            // ��������i�����߂�
            int searchDestinationColumn = searchOriginColumn + SEARCH_DESTINATION_COLUMN[i];

            // ���݂��Ȃ��i�Ȃ玟�̗v�f��
            if (searchDestinationColumn < 0 || searchDestinationColumn >= _fieldParameter._fieldColumnSize)
            {
                continue;
            }

            // ���������Ղ悪�����ς݂܂��́A�Ⴄ�F�Ȃ玟�̗v�f��
            if (_isSearchedPuyos[searchDestinationRow, searchDestinationColumn] || 
                _puyoColorsData[searchDestinationRow, searchDestinationColumn] != searchColor)
            {
                continue;
            }

            // ���������Ղ悩�炳��Ɍ�������(�ċA�֐�)
            SearchSurroundingSameColor(searchColor, searchDestinationRow, searchDestinationColumn);
        }
    }


    /// <summary>
    /// �Ղ悪���������Ƃɗ����鏈��
    /// </summary>
    private void PuyoFallProcessAfterErace()
    {
        // �s�����J��Ԃ�
        for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
        {
            // ���̍s�́A�Ղ�̏����������O�Ȃ玟�̍s��
            if (_eachRowEraceCount[i] == 0)
            {
                continue;
            }

            // �Ղ�̗����n�_�̍���
            sbyte puyoDropRow = 0;

            // �i�����J��Ԃ�(j�͌�������i)
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                // ���̍s�̂Ղ�̏����������[���ɂȂ�A���̌��}�X���������玟�̍s��
                if (_eachRowEraceCount[i] < 0)
                {
                    break;
                }

                // �����}�X����}�X�̂Ƃ�
                if (_puyoColorsData[i, j] == EMPTY_PUYO_NUMBER)
                {
                    // �e�s�̏������Ղ�̐������炷
                    _eachRowEraceCount[i] -= 1;

                    continue;
                }


                ///-- �����Ă���Ղ�𗎉������鏈�� --///

                // �Ղ悪�����Ă��Ȃ��Ƃ��̏���
                if(puyoDropRow == j)
                {
                    // �Ղ�̗����n�_����i�グ��
                    puyoDropRow++;

                    continue;
                }

                // �Ղ�𗎉��n�_�܂ňړ�����
                _puyoTransforms[i, j].position = new Vector3(_eachRowFieldPositions[i], _eachColumnFieldPositions[puyoDropRow], 0);

                // �Ղ�f�[�^�������Ƃ����̒i�܂ňړ�����
                _puyoColorsData[i, puyoDropRow] = _puyoColorsData[i, j];
                _puyoTransforms[i, puyoDropRow] = _puyoTransforms[i, j];

                // ��������Ղ�̏ꏊ����ɂ���
                _puyoColorsData[i, j] = EMPTY_PUYO_NUMBER;
                _puyoTransforms[i, j] = null;

                // �Ղ�̗����n�_����i�グ��
                puyoDropRow++;
            }
        }

        // �t�B�[���h�̍����f�[�^���X�V
        for (int i = 0; i < _fieldParameter._fieldRowSize; i++)
        {
            FieldHeightDataUpdate(i);
        }
    }


    /// <summary>
    /// �t�B�[���h�̍����f�[�^���X�V����
    /// </summary>
    /// <param name="updateRow"> �X�V����� </param>
    private void FieldHeightDataUpdate(int updateRow)
    {
        // ��̍���(��)
        sbyte verticalNumber = 0;

        // �t�B�[���h�f�[�^����Ղ�̍����𐔂��āA�i�[����
        for (sbyte i = 0; _puyoColorsData[updateRow, i] != EMPTY_PUYO_NUMBER; i++)
        {
            verticalNumber++;

            // �t�B�[���h�̍ŏ㕔�ɒB���Ă���ꍇ�A�����̌v�����I������
            if (i == _fieldParameter._fieldColumnSize - 1)
            {
                break;
            }
        }

        // �X�V�����̍���(��)���X�V����
        _fieldHeightData[updateRow] = verticalNumber;
    }


    /// <summary>
    /// �V���ɂՂ��ݒu�����Ƃ��̏���
    /// </summary>
    /// <param name="installlPuyoDatas"> �Ղ�� �F, Transform, �s, �� </param>
    public void PuyoInstallProcess(List<PuyoStructs.ControlPuyoData> installlPuyoDatas)
    {
        // �Ղ��ݒu�����Ƃ��̌��ʉ����Ȃ炷
        _puyoSeAudioSource.PlayOneShot(_inGameSounds._puyoInstallSE);

        // �t�B�[���h�f�[�^�̍X�V���s��
        for (int i = 0; i < installlPuyoDatas.Count; i++)
        {
            // �Ղ�̐ݒu�ʒu���t�B�[���h�̍����ȏ�Ȃ�A���̂Ղ�������Ď��̗v�f��
            if (installlPuyoDatas[i].Column >= _fieldParameter._fieldColumnSize)
            {
                installlPuyoDatas[i].Transform.gameObject.SetActive(false);

                continue;
            }

            // �t�B�[���h�f�[�^���X�V����
            _puyoColorsData[installlPuyoDatas[i].Row, installlPuyoDatas[i].Column] = installlPuyoDatas[i].Color;
            _puyoTransforms[installlPuyoDatas[i].Row, installlPuyoDatas[i].Column] = installlPuyoDatas[i].Transform;

            // �t�B�[���h�̍����f�[�^���X�V
            FieldHeightDataUpdate(installlPuyoDatas[i].Row);
        }


        // �󒆂ɕ����Ă���Ղ悪�������Ƃ��ɂՂ�𗎉������鏈��
        for (int i = 0; i < installlPuyoDatas.Count; i++)
        {
            // �ݒu���ꂽ�Ղ�̂ЂƂ��̒i
            int underInstallPuyoColumn = installlPuyoDatas[i].Column - 1;

            // �ݒu�Ղ�̈�i�����z��O�܂��́A�Ȃɂ��̗v�f�������Ă�Ύ��̗v�f��
            if (underInstallPuyoColumn < 0 || _puyoColorsData[installlPuyoDatas[i].Row, installlPuyoDatas[i].Column - 1] != EMPTY_PUYO_NUMBER)
            {
                continue;
            }

            ///-- �󒆂ɂՂ悪�������ꍇ�̏��� --///

            // �󒆂ɕ����Ă���Ղ�̏�ɂՂ悪����ԌJ��Ԃ��A����������
            for (int j = 0; installlPuyoDatas[i].Column + j < _fieldParameter._fieldColumnSize; j++)
            {
                // ���Ƃ��Ղ�̒i�ƁA�Ղ悪���n����i
                int toBeDropedColumn = installlPuyoDatas[i].Column + j;
                int toBeInstallColumn = _fieldHeightData[installlPuyoDatas[i].Row] + j;

                // �󒆂ɕ����Ă���Ղ悪�Ȃ��Ȃ����珈������߂�
                if (_puyoColorsData[installlPuyoDatas[i].Row, toBeDropedColumn] == EMPTY_PUYO_NUMBER)
                {
                    break;
                }

                // �Ղ�𗎉�������
                _puyoTransforms[installlPuyoDatas[i].Row, toBeDropedColumn].position = new Vector3(_eachRowFieldPositions[installlPuyoDatas[i].Row], _eachColumnFieldPositions[toBeInstallColumn], 0);

                // �Ղ�̃f�[�^�𗎉��ʒu�Ɉړ�����
                _puyoColorsData[installlPuyoDatas[i].Row, toBeInstallColumn] = _puyoColorsData[installlPuyoDatas[i].Row, toBeDropedColumn];
                _puyoTransforms[installlPuyoDatas[i].Row, toBeInstallColumn] = _puyoTransforms[installlPuyoDatas[i].Row, toBeDropedColumn];

                // ���Ƃ��ƂՂ悪�������ꏊ����ɂ���
                _puyoColorsData[installlPuyoDatas[i].Row, toBeDropedColumn] = EMPTY_PUYO_NUMBER;
                _puyoTransforms[installlPuyoDatas[i].Row, toBeDropedColumn] = null;
            }

            // �t�B�[���h�̍����f�[�^���X�V
            FieldHeightDataUpdate(installlPuyoDatas[i].Row);
        }

        // ������Ղ悪���邩��T������
        SearchForEracePuyosAllMass();
    }


    /// <summary>
    /// �Q�[���I�[�o�[���̏������s��
    /// </summary>
    private void GameOverProcess() 
    {
        // �Q�[���I�[�o�[�e�L�X�g��\������
        _gameOverText.SetActive(true);
    }


    /// <summary>
    /// �t�B�[���h��̂Ղ�̍�����Ԃ����\�b�h
    /// </summary>
    public sbyte[] GetFieldHeightData()
    {
        return _fieldHeightData;
    }


    /// <summary>
    /// �t�B�[���h�̗�ƍs�̒��S���W��Ԃ�
    /// </summary>
    public (float[], float[]) GetFieldMassPos()
    {
        return (_eachRowFieldPositions, _eachColumnFieldPositions);
    }
}