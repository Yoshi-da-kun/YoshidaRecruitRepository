
using UnityEngine;

/// --------------------------------------------------
/// #FieldParameter.cs
/// �쐬��:�g�c�Y��
/// 
/// �t�B�[���h�̑傫�����A�t�B�[���h��Ղ�̏��B�Ղ�̏�Ԃ��܂Ƃ߂��X�N���v�g�ł�
/// --------------------------------------------------

[CreateAssetMenu (menuName = "Parameters/FieldParameters", fileName = "NewFieldParameter")]
public class FieldParameter : ScriptableObject
{
    [field: Header("�t�B�[���h�̑傫��")]
    [field: SerializeField, Label("(�c)�i��")]
    public sbyte _fieldColumnSize { get; private set; }

    [field: SerializeField, Label("(��)�s��")]
    public sbyte _fieldRowSize { get; private set; }

    [field: SerializeField, Label("�\������t�B�[���h��Prefab")]
    private GameObject _fieldPrefab;

    // �t�B�[���h�̃I�u�W�F�N�g�̃��[���h���W��ł̑傫��(Scale)
    public Vector2 _scaleOfOneMass { get; private set; }

    // �Ղ�̃f�t�H���g�̑傫��
    public Vector2[] _puyoLocalScales { get; private set; }


    [field: SerializeField, Header("�Ղ�̌�����")]
    public GameObject[] _puyoSprits{ get; private set; }

    [field: Header("�Ղ�Ɋւ���l")]
    [field: SerializeField, Label("�Ղ悪�����鎞��")]
    public sbyte _puyoEraceTime { get; private set; }

    [field: SerializeField, Label("�Ղ悪�����鐔")]
    public sbyte _puyoEraceLinkCount { get; private set; }


    [field: Header("�Q�[���I�[�o�[�ɂȂ�}�X(�����[�̃}�X���O�Ƃ���)")]

    [field: SerializeField, Label("�i")]
    public sbyte _gameOverColumn { get; private set; }
    [field: SerializeField, Label("�s")]
    public sbyte _gameOverRow { get; private set; }


    // ��]���̐e�Ղ�ɑ΂���q�Ղ�̈ʒu(��]��Ԃ��ꎟ���v�f�B�e�Ղ�̈ʒu���񎟌��v�f�ŁA�e�Ղ��v�f�O�Ƃ���)
    public Vector2Int[][] _puyosEachRotationDirections 
    { 
        get 
        { 
            return new[] 
            {
                new[]{ new Vector2Int(0, 0), new Vector2Int( 0, 1), new Vector2Int( 1, 0), new Vector2Int( 1, 1) },
                new[]{ new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 0,-1), new Vector2Int( 1,-1) },
                new[]{ new Vector2Int(0, 0), new Vector2Int( 0,-1), new Vector2Int(-1, 0), new Vector2Int(-1,-1) },
                new[]{ new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 0, 1), new Vector2Int(-1, 1) }
            }; 
        } 
    }

    // ��]�ԍ��̏����l
    public readonly sbyte _initialRotateNumber = 0;


    /// <summary>
    /// �t�B�[���h�̃��[���h��ł�Scale�����߂�
    /// </summary>
    private void OnEnable()
    {
        // �t�B�[���h�̃��[���h���W��̑傫�����i�[����
        Vector2 fieldWorldScale = _fieldPrefab.transform.lossyScale;

        // �P�}�X������̑傫�������߂�(�s�̓t�B�[���h�̌����ڂ���1�}�X��ɂ͂ݏo�邽��1����)
        _scaleOfOneMass = new Vector2(fieldWorldScale.x / _fieldRowSize, fieldWorldScale.y / (_fieldColumnSize - 1));

        // �z��̑傫�����`
        _puyoLocalScales = new Vector2[_puyoSprits.Length];

        // �Ղ��Prefab�̑傫�����P�}�X�̑傫���ɕύX����
        for (int i = 0; i < _puyoSprits.Length; i++)
        {
            _puyoLocalScales[i] = _puyoSprits[i].transform.localScale * _scaleOfOneMass;
        }

        // �Q�[���I�[�o�[�}�X���t�B�[���h�̊O�̎��̏���
        if (_gameOverColumn >= _fieldColumnSize || _gameOverRow >= _fieldRowSize)
        {
            Debug.LogError("�Q�[���I�[�o�[�}�X���t�B�[���h�̊O�ł�");
        }
    }
}
