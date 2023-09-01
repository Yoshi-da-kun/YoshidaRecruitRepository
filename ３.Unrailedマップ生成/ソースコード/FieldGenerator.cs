
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------------
/// #FieldGenerator.cs
/// 
/// �t�B�[���h�𐶐�����X�N���v�g
/// --------------------------------------------------------

public class FieldGenerator : MonoBehaviour
{
    [SerializeField, Label("�t�B�[���h�̃p�����[�^")]
    private FieldParameter _fieldParameter;

    #region �e�u���b�N���Ƃ�Prefab��ϐ�
    
    [SerializeField, Header("�e�u���b�N��Prefab"),StringIndex(new string[] { "�󔒂̃u���b�N", "����ȏ��u���b�N", "�󂹂Ȃ��u���b�N", "�S�̃u���b�N", "�؍ނ̃u���b�N" })]
    private GameObject[] _blockPrefabs;
    // �e�u���b�N��ObjectPool�̃t�H���_�̖��O(���e�u���b�N�̔ԍ��Ə��Ԃ����킹�邱��)
    private readonly string[] _blockPoolNames = {"NothingBlocks", "FlatBlocks", "UnbreakableBlocks", "IronBlocks", "WoodBlocks"};

    // �e�u���b�N��ObjectPool�̃t�H���_
    private Transform[] _blockPoolFolderTransforms = default;

    // �e�u���b�N�̔ԍ�
    private const sbyte NOTHING_BLOCK_NUMBER = 0;
    private const sbyte FLAT_BLOCK_NUMBER = 1;
    private const sbyte UNBREAKABLE_BLOCK_NUMBER = 2;
    private const sbyte IRON_BLOCK_NUMBER = 3;
    private const sbyte WOOD_BLOCK_NUMBER = 4;

    #endregion �e�u���b�N���Ƃ�Prefab��ϐ�

    // �e�}�X��ԍ��Ŋi�[����t�B�[���h�f�[�^
    private sbyte[,] _fieldDatas = default;

    // �t�B�[���h��̃u���b�N��Transform(�t�B�[���h�����E�����łQ�������Ă��邽��List�����)
    private List<Transform>[] _blockTransforms = new List<Transform>[2];

    // ���ɐ�������u���b�N�ƁA�]�������u���b�N��Transform�������|�C���^
    private sbyte _newGenerateBlockTransformsPointer = default;
    private sbyte _extraGenerateBlockTransformsPointer = default;


    // ���̃E�F�[�u(����)�Ɏc�������ς݃t�B�[���h�̗�̑傫��
    private int _rowSizeLeftForNextWave = default;

    // �t�B�[���h�̗񐔂̍ő�l
    private int _fieldMaxRowSize = default;

    // �t�B�[���h�̗]���Ȑ������J�n�����
    private int _extraFieldStartRow = default;

    // ��悠����̗�
    private int _rowSizePerSelection = default;


    private void Start()
    {

        SwitchingNewClimateProcess();
    }

    /// <summary>
    /// ���S�V�K�̃t�B�[���h(Scene)�ɂȂ�Ƃ��̏���
    /// </summary>
    private void SwitchingNewClimateProcess()
    {
        // ��悠����̗񐔂����߂�
        _rowSizePerSelection = _fieldParameter._rowSizeToGoal / _fieldParameter._numberOfFieldSelection;

        // �t�B�[���h�f�[�^�p�̔z��̑傫�����`(��̑傫�� + �]���ɐ��������(�v���X1���), �i�̑傫��)
        _fieldDatas = new sbyte
            [_fieldParameter._rowSizeToGoal * _fieldParameter._numberOfActiveField + _rowSizePerSelection, _fieldParameter._fieldColumnSize];

        // �u���b�N��Object�̐e�I�u�W�F�N�g�̔z��̑傫�����`
        _blockPoolFolderTransforms = new Transform[_blockPoolNames.Length];

        // �e�u���b�N��Pool�p�̃I�u�W�F�N�g�𐶐�����
        for (int i = 0; i < _blockPoolNames.Length; i++)
        {
            _blockPoolFolderTransforms[i] = new GameObject(_blockPoolNames[i]).transform;
        }

        // �t�B�[���h��̃u���b�N��Transform�z��̑傫�����`(�\������Ă���t�B�[���h�p + �]�����������t�B�[���h�p)
        _blockTransforms = new List<Transform>[_fieldParameter._numberOfActiveField + 1];
        for (int i = 0; i < _blockTransforms.Length; i++)
        {
            _blockTransforms[i] = new List<Transform>();
        }
        // �]���ɐ��������u���b�N��Transform�������|�C���^�����߂�
        _extraGenerateBlockTransformsPointer = _newGenerateBlockTransformsPointer;
        _extraGenerateBlockTransformsPointer++;

        // �t�B�[���h�̗񐔂̍ő�l�����߂�
        _fieldMaxRowSize = _fieldDatas.GetLength(0);
        
        // ���̃E�F�[�u(����)�Ɏc�������ς݃t�B�[���h�̗�̑傫�������߂�
        _rowSizeLeftForNextWave = _fieldMaxRowSize - _fieldParameter._rowSizeToGoal;
        
        // �t�B�[���h�̗]���������J�n���������߂�
        _extraFieldStartRow = _fieldMaxRowSize - _rowSizePerSelection;
    }

    private void Update()
    {
        // �X�y�[�X�L�[���������Ƃ��A�t�B�[���h�𐶐�����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateNewWaveField();
        }
    }


    /// <summary>
    /// �E�F�[�u�i�s���̃t�B�[���h�̐������s��
    /// </summary>
    private void GenerateNewWaveField()
    {
        // �����ς݂̃t�B�[���h�̍폜�ƈړ����s��
        RemoveAndMoveOldWaveField();

        // �V�����t�B�[���h�̃f�[�^���Z�b�g���鏈��
        FieldDataSetter();

        // ���������u���b�N���i�[����z��̃|�C���^
        int instantBlockTransformsPointer = _newGenerateBlockTransformsPointer;

        // �V������������t�B�[���h�̗񐔕��J��Ԃ�
        for (int i = _rowSizeLeftForNextWave; i < _fieldMaxRowSize; i++)
        {
            // ��������񂪗]�������̗�ɂȂ�����Transform�̃|�C���^��؂�ւ���
            if (i == _extraFieldStartRow)
            {
                instantBlockTransformsPointer = _extraGenerateBlockTransformsPointer;
            }
            // ���݂̗�̂��ׂĒi�̃u���b�N�𐶐�����
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                // �u���b�N�̐����ʒu�����߂�
                Vector3 blockGeneratePosition = 
                    _fieldParameter._fieldGenerateStartPosition + _fieldParameter._oneBlockSize * new Vector2(i, j);
                
                // �u���b�N��Scene��ɐ������āA�u���b�N�p�̔z��Ɋi�[
                _blockTransforms[instantBlockTransformsPointer].Add(GenerateBlock(_fieldDatas[i, j], blockGeneratePosition));
            }
        }
    }


    /// <summary>
    /// ���̃E�F�[�u�i�s���ɁA�����ς݂̃t�B�[���h�̈ړ��Ə������s��
    /// </summary>
    private void RemoveAndMoveOldWaveField()
    {
        // �����ς݂̗]��������̋󔒃u���b�N�𖄂߂鏈��
        for (int i = _extraFieldStartRow; i < _fieldMaxRowSize; i++)
        {
            // ���݂̗�̂��ׂĂ̒i�̋󔒃u���b�N�𕽂�ȃu���b�N�ɍX�V����
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                // �]�������̈ʒu�ɋ󔒃u���b�N�ȊO���i�[����Ă����玟�̗v�f��
                if (_fieldDatas[i, j] != NOTHING_BLOCK_NUMBER)
                {
                    continue;
                }
                // ����ȃu���b�N���i�[����
                _fieldDatas[i, j] = FLAT_BLOCK_NUMBER;
            }
        }
        // ���̃E�F�[�u�Ɏc���t�B�[���h�f�[�^���ړ�����
        for (int i = 0; i < _rowSizeLeftForNextWave; i++)
        {
            // ���݂̗�̂��ׂĒi�̃u���b�N�ԍ�(�t�B�[���h�f�[�^)���ړ�����
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                _fieldDatas[i, j] = _fieldDatas[_fieldParameter._rowSizeToGoal + i, j];
            }
        }
        // �V�K��������t�B�[���h�̏������Ɏg���u���b�N�ԍ�(����ȃu���b�N)
        sbyte _initializeBlockNumber = FLAT_BLOCK_NUMBER;

        // �V�K��������t�B�[���h�f�[�^������������
        for (int i = _rowSizeLeftForNextWave; i < _fieldMaxRowSize; i++)
        {
            // �]�������p�̗�ɂȂ����珉�����p�̃u���b�N�ԍ����󔒃u���b�N�ɂ���
            if (i == _extraFieldStartRow)
            {
                _initializeBlockNumber = NOTHING_BLOCK_NUMBER;
            }
            // ���݂̗�̂��ׂĒi�̃t�B�[���h�f�[�^������������
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                _fieldDatas[i, j] = _initializeBlockNumber;
            }
        }
        // �������ꂽ�u���b�NTransform�������|�C���^���X�V
        _newGenerateBlockTransformsPointer++;
        _extraGenerateBlockTransformsPointer++;

        // �V�K�����p�̃|�C���^��Transform�̔z�񐔂𒴂�����A�z��̂͂��߂ɖ߂�
        if (_newGenerateBlockTransformsPointer >= _blockTransforms.Length)
        {
            _newGenerateBlockTransformsPointer = 0;
        }
        // �]�������p�̃|�C���^��Transform�̔z�񐔂𒴂�����A�z��̂͂��߂ɖ߂�
        if (_extraGenerateBlockTransformsPointer >= _blockTransforms.Length)
        {
            _extraGenerateBlockTransformsPointer = 0;
        }

        // ���̃E�F�[�u�Ɏc���Ȃ��u���b�N���폜���鏈��
        for (int i = 0; i < _blockTransforms[_extraGenerateBlockTransformsPointer].Count; i++)
        {
            _blockTransforms[_extraGenerateBlockTransformsPointer][i].gameObject.SetActive(false);
        }
        // �u���b�N���폜����List��������
        _blockTransforms[_extraGenerateBlockTransformsPointer] = new List<Transform>();

        // ���E�F�[�u�Ɏc���u���b�N�̍��W�̈ړ��ʂ����߂�
        float movemetForBlockToBeLeft = _fieldParameter._rowSizeToGoal * _fieldParameter._oneBlockSize.x;

        // ���E�F�[�u�Ɏc���u���b�N�̈ړ����s������
        for (int i = 0; i < _blockTransforms.Length; i++)
        {
            // �ړ�����Transform���i�[����Ă��Ȃ���΁A�������I������
            if (_blockTransforms[i] == null)
            {
                continue;
            }
            // ���̃E�F�[�u�Ɏc���u���b�N�̍��W���ړ�����
            for (int j = 0; j < _blockTransforms[i].Count; j++)
            {
                _blockTransforms[i][j].position -= new Vector3(movemetForBlockToBeLeft, 0, 0);
            }
        }
    }


    /// <summary>
    /// �u���b�N�̐������s��
    /// </summary>
    /// <param name="generateBlockNumber"> ��������u���b�N�ԍ� </param>
    /// <param name="generatePosition"> ����������W </param>
    private Transform GenerateBlock(int generateBlockNumber, Vector2 generatePosition)
    {
        // �Ďg�p�ł���I�u�W�F�N�g�����邩����������(�I�u�W�F�N�g�v�[��)
        for (int i = 0; i < _blockPoolFolderTransforms[generateBlockNumber].childCount; i++)
        {
            // ��������I�u�W�F�N�g
            Transform searchObject = _blockPoolFolderTransforms[generateBlockNumber].GetChild(i);

            // �L��������Ă����玟�̗v�f��
            if (searchObject.gameObject.activeSelf == true)
            {
                continue;
            }
            // �Ďg�p����I�u�W�F�N�g��L����
            searchObject.gameObject.SetActive(true);

            // �Ďg�p����I�u�W�F�N�g�𐶐��ʒu�Ɉړ�����
            searchObject.position = generatePosition;

            // �Ďg�p����I�u�W�F�N�g��Ԃ�
            return searchObject;
        }

        // �I�u�W�F�N�g��V������������
        Transform instantiateObject = Instantiate
            (_blockPrefabs[generateBlockNumber], generatePosition, Quaternion.identity, _blockPoolFolderTransforms[generateBlockNumber]).transform;

        // �V�������������I�u�W�F�N�g��Ԃ�
        return instantiateObject;
    }


    /// <summary>
    /// �t�B�[���h�f�[�^���i�[���Ă�������
    /// </summary>
    private void FieldDataSetter()
    {
        // �]�������������A�V������������t�B�[���h�̗񐔕����������s��
        for (int i = 0; i < _fieldParameter._rowSizeToGoal; i++)
        {
            // �i�����J��Ԃ��A����ȃu���b�N�̔ԍ����i�[����
            for (int j = 0; j < _fieldParameter._fieldColumnSize; j++)
            {
                _fieldDatas[_rowSizeLeftForNextWave + i, j] = FLAT_BLOCK_NUMBER;
            }
        }
        #region �e�u���b�N�`�����N�𐶐����鏈��

        // ���Ȃ��u���b�N�𐶐����鏈��
        if (_fieldParameter._maxUnbreakableChunkCount >= _fieldParameter._minUnbreakableChunkCount)
        {
            // ���Ȃ��u���b�N�̃`�����N���𒊑I����
            int numberOfUnbreakableChunk = Random.Range(_fieldParameter._minUnbreakableChunkCount, _fieldParameter._maxUnbreakableChunkCount + 1);
            // ���Ȃ��u���b�N�̐������s��
            ChunkDeployment(UNBREAKABLE_BLOCK_NUMBER, numberOfUnbreakableChunk);
        }
        // �S�̃u���b�N�𐶐�����
        if (_fieldParameter._maxIronChunkCount >= _fieldParameter._minIronChunkCount)
        {
            // �S�u���b�N�̃`�����N���𒊑I����
            int numberOfIronChunk = Random.Range(_fieldParameter._minIronChunkCount, _fieldParameter._maxIronChunkCount + 1);
            // �S�u���b�N�̐������s��
            ChunkDeployment(IRON_BLOCK_NUMBER, numberOfIronChunk);
        }
        // �؂̃u���b�N�𐶐����鏈��
        if (_fieldParameter._maxWoodChunkCount >= _fieldParameter._minWoodChunkCount)
        {
            // �؂̃u���b�N�̃`�����N���𒊑I����
            int numberOfWoodChunk = Random.Range(_fieldParameter._minWoodChunkCount, _fieldParameter._maxWoodChunkCount + 1);
            // �؂̃u���b�N�̐������s��
            ChunkDeployment(WOOD_BLOCK_NUMBER, numberOfWoodChunk);
        }

        #endregion �e�u���b�N�`�����N�𐶐����鏈��
    }


    /// <summary>
    /// �`�����N��z�u����
    /// </summary>
    private void ChunkDeployment(sbyte blockNumber, int numberOfGenerateChunk)
    {
        // �t�B�[���h����斈�ɕ�����
        int[] chunkCountPerSelections = new int[_fieldParameter._numberOfFieldSelection];

        // �܂��`�����N�����ő�ɒB���Ă��Ȃ����ԍ�
        List<int> notMaxSelectionNumbers = new List<int>();

        // ���ԍ����i�[����
        for (int i = 0; i < _fieldParameter._numberOfFieldSelection; i++)
        {
            notMaxSelectionNumbers.Add(i);
        }

        // �����ɉ����ă`�����N�������_���ɔz�u����
        for (int i = 0; i < numberOfGenerateChunk; i++)
        {
            // �ǂ̋��ɐ������邩�𒊑I����
            int generateSelectionPointer = Random.Range(0, notMaxSelectionNumbers.Count);

            // ����������̍��[�̗�����߂�
            int generateSelectionRow = _rowSizeLeftForNextWave + notMaxSelectionNumbers[generateSelectionPointer] * _rowSizePerSelection;

            // �`�����N�̐����ʒu�����߂�(����������̗� + �����ł̃����_���ȗ�, �i���̍ő�l�ȓ��̃����_���Ȓi��)
            Vector2Int generateMatrix = new Vector2Int
                (generateSelectionRow + Random.Range(0, _rowSizePerSelection), Random.Range(0, _fieldParameter._fieldColumnSize));

            // �u���b�N�`�����N�𐶐�����
            GenerateBlockChunk(blockNumber, generateMatrix);

            // ���I�������̃`�����N�������Z����
            chunkCountPerSelections[notMaxSelectionNumbers[generateSelectionPointer]]++;

            // ����������̃`�����N�����ő�̂Ƃ�
            if (chunkCountPerSelections[notMaxSelectionNumbers[generateSelectionPointer]] == _fieldParameter._maxChunkPerFieldSelection)
            {
                // �ő吔�ɒB�������ԍ�(�폜����ԍ�)
                int deleteSelectionNumberPointer = generateSelectionPointer;

                // �����̗v�f�ƍ폜������ԍ������ւ��A�폜����
                notMaxSelectionNumbers[generateSelectionPointer] = notMaxSelectionNumbers[notMaxSelectionNumbers.Count - 1];
                notMaxSelectionNumbers[notMaxSelectionNumbers.Count - 1] = deleteSelectionNumberPointer;

                notMaxSelectionNumbers.RemoveAt(notMaxSelectionNumbers.Count - 1);
            }
            // ���ׂĂ̋�悪�ő吔�ɒB�������������I������
            if (notMaxSelectionNumbers.Count == 0)
            {
                return;
            }
        }
    }


    /// <summary>
    /// �u���b�N�̂ЂƂ܂Ƃ܂�(�`�����N)�𐶐�����
    /// </summary>
    private void GenerateBlockChunk(sbyte generateBlockNumber, Vector2Int generateStartMatrix)
    {
        // �Ō�ɐ���������̑傫��
        int lastGeneratedRowSize = _fieldParameter._generateMinRowSize;

        // ��������񐔂����炷��Ԃ̂Ƃ��t���O
        bool isGeneratedSizeSmoller = default;

        // ���������̐����J�n��
        int generateStartRow = generateStartMatrix.x;

        // �u���b�N�ЂƂ܂Ƃ܂萶������܂ŌJ��Ԃ�
        for (int i = 0; i < _fieldParameter._blockChunkMaxSize; i++)
        {
            // �Ō�ɐ��������񐔂ɑ΂��鎟�̒i�̗�
            int plannedGenerateBlockSize = default;

            // �񐔂𑝂₹���Ԃ̂Ƃ�
            if (!isGeneratedSizeSmoller)
            {
                // �񐔂𑝌����钊�I�̒l�̍ŏ��l
                sbyte decreaseLotteryMinValue = 0;

                // �ŏ��̒i���ɒB���Ă��Ȃ��Ƃ��A�񐔂����炷��ԂɂȂ�Ȃ��悤�ɂ���
                if (i < _fieldParameter._generateMinColumnSize)
                {
                    decreaseLotteryMinValue = 1;
                }
                // �񐔂��Ō�ɐ��������񐔈ȏ�ɂ��邩�A���炷���𒊑I����(0:���炷 �ȊO:���₷)
                int increaseOrDecreaseValue = Random.Range(decreaseLotteryMinValue, _fieldParameter._easeOfChunksLager);

                // �񐔂𑝂₷�Ƃ��̏���
                if (increaseOrDecreaseValue > 0)
                {
                    // �傫���Ȃ�\�������悤�ɗ񐔂𒊑I����
                    plannedGenerateBlockSize = Random.Range(0, _fieldParameter._increasedMaxBlockSizePerColumn + 1);
                }
                // �񐔂����炵�n�߂�Ƃ��̏���
                else
                {
                    // �񐔂��傫���Ȃ�Ȃ��悤�ɗ񐔂𒊑I����
                    plannedGenerateBlockSize = Random.Range(-_fieldParameter._decreasedMaxBlockSizePerColumn, 1); ;

                    // �񐔂����炷�Ƃ��̃t���O���Z�b�g
                    isGeneratedSizeSmoller = true;
                }
            }
            // �񐔂����炷��Ԃ̂Ƃ�
            else
            {
                // �񐔂��傫���Ȃ�Ȃ��悤�ɗ񐔂𒊑I����
                plannedGenerateBlockSize = Random.Range(-_fieldParameter._decreasedMaxBlockSizePerColumn, 1);
            }

            // �����񐔂��ŏ����ȏ�̂Ƃ��ɁA�Ō�ɐ���������͈͓̔��ŊJ�n�ʒu�𒊑I���鏈��
            // ��������񐔂��������Ȃ����Ƃ��A���͈̔͂Œ��I����
            if (plannedGenerateBlockSize < 0)
            {
                generateStartRow += Random.Range(0, -plannedGenerateBlockSize + 1);
            }
            // ��������񐔂��傫���Ȃ����Ƃ��A���̓�{�͈̔͂Œ��I����
            else if (plannedGenerateBlockSize > 0)
            {
                generateStartRow += Random.Range(-plannedGenerateBlockSize, 1);
            }
            // ���݂̐�������񐔂����߂�
            int currentGenerateBlockSize = lastGeneratedRowSize + plannedGenerateBlockSize;

            // ���I�����񐔕��̃u���b�N�̃f�[�^���X�V����
            for (int j = 0; j < currentGenerateBlockSize; j++)
            {
                // �f�[�^���X�V�����ƒi
                int updateRow = generateStartRow + j;
                int updateColumn = generateStartMatrix.y + i;

                // �X�V����񂩒i���t�B�[���h�f�[�^�̊O�̂Ƃ��A�f�[�^�X�V���I������
                if (updateRow >= _fieldMaxRowSize || updateColumn >= _fieldParameter._fieldColumnSize)
                {
                    break;
                }
                // �u���b�N�f�[�^���X�V����
                _fieldDatas[updateRow, updateColumn] = generateBlockNumber;
            }
            // �񐔂̍ŏ��l�ȉ��ɂȂ����琶�����I������
            if (currentGenerateBlockSize < _fieldParameter._generateMinRowSize)
            {
                return;
            }
            // �Ō�ɐ���������̑傫�����X�V����
            lastGeneratedRowSize = currentGenerateBlockSize;
        }
    }
}
