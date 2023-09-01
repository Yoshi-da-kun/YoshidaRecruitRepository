
using UnityEngine;

/// --------------------------------------------------------
/// #FieldParameter.cs
/// 
/// �}�b�v�Ɋւ���p�����[�^���܂Ƃ߂��X�N���v�g
/// --------------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/FieldParameter", fileName = "NewFieldParameter")]
public class FieldParameter : ScriptableObject
{
    // �P�u���b�N�̑傫��
    public Vector2 _oneBlockSize { get { return new Vector2(1, 1); } }

    // �t�B�[���h�̐������n�߂�ʒu
    public Vector2 _fieldGenerateStartPosition { get { return new Vector2(1, 1); } }

    [field: SerializeField, Label("�t�B�[���h�̏c(�i)�̑傫��"), Range(2, 50)]
    public int _fieldColumnSize { get; private set; } = 18;

    [field: SerializeField, Label("�S�[���܂ł̉�(��)�̑傫��"), Range(10, 100)]
    public int _rowSizeToGoal { get; private set; } = 30;

    [field: SerializeField, Label("Scene�ɕ\������t�B�[���h�̐�(�Q����)"), Range(2, 30)]
    public int _numberOfActiveField { get; private set; } = 2;


    [field: Header("�u���b�N�̂ЂƂ܂Ƃ܂�(�`�����N)�֘A")]

    [field: SerializeField, Label("�u���b�N�ЂƂ܂Ƃ܂�̍ő�̑傫��"), Range(2, 40)]
    public int _blockChunkMaxSize { get; private set; } = 5;

    [field: SerializeField, Label("�u���b�N�ЂƂ܂Ƃ܂�̑傫���Ȃ�₷��"), Range(2, 10)]
    public int _easeOfChunksLager { get; private set; } = 5;

    [field: SerializeField, Label("�u���b�N��i������̑傫���Ȃ�ő�l(�Q����)"), Range(1, 10)]
    public int _increasedMaxBlockSizePerColumn { get; private set; } = 2;

    [field: SerializeField, Label("�u���b�N��i������̏������Ȃ�ő�l"), Range(1, 10)]
    public int _decreasedMaxBlockSizePerColumn { get; private set; } = 4;

    [field: SerializeField, Label("�񐔂̍ŏ��l(�Q����)"), Range(1, 4)]
    public int _generateMinRowSize { get; private set; } = 2;

    [field: SerializeField, Label("�i���̍ŏ��l(�Q����)"), Range(1, 4)]
    public int _generateMinColumnSize { get; private set; } = 2;


    [field: Header("�e�u���b�N�`�����N�̐�")]

    [field: SerializeField, Label("�󂹂Ȃ��u���b�N�`�����N�̍ŏ���"), Range(1, 30)]
    public int _minUnbreakableChunkCount { get; private set; } = 3;

    [field: SerializeField, Label("�󂹂Ȃ��u���b�N�`�����N�̍ő吔"), Range(0, 30)]
    public int _maxUnbreakableChunkCount { get; private set; } = 7;

    [field: SerializeField, Label("�؃u���b�N�`�����N�̍ŏ���"), Range(1, 30)]
    public int _minWoodChunkCount { get; private set; } = 3;

    [field: SerializeField, Label("�؃u���b�N�`�����N�̍ő吔"), Range(0, 30)]
    public int _maxWoodChunkCount { get; private set; } = 7;

    [field: SerializeField, Label("�S�u���b�N�`�����N�̍ŏ���"), Range(1, 30)]
    public int _minIronChunkCount { get; private set; } = 3;

    [field: SerializeField, Label("�S�u���b�N�`�����N�̍ő吔"), Range(0, 30)]
    public int _maxIronChunkCount { get; private set; } = 7;


    [field: Header("�e�`�����N�̔z�u�֘A")]

    [field: SerializeField, Label("�}�b�v�̋�搔"), Range(4, 30)]
    public int _numberOfFieldSelection { get; private set; } = 6;

    [field: SerializeField, Label("�e���̃`�����N�̍ő吔"), Range(1, 30)]
    public int _maxChunkPerFieldSelection { get; private set; } = 3;
}