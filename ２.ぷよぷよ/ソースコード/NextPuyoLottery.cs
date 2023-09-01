
using UnityEngine;

/// --------------------------------------------------
/// #NextPuyoLotteryProcess.cs
/// �쐬��:�g�c�Y��
/// 
/// �l�N�X�g�̂Ղ�̔ԍ��𒊑I����X�N���v�g
/// NextPuyoControl.cs�Ɍp�����Ă��܂�
/// --------------------------------------------------

public class NextPuyoLottery : MonoBehaviour
{
    [SerializeField, Header("�p�����[�^��ScriptableObject���܂Ƃ߂��X�N���v�g")]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    private FieldParameter _fieldParameter;

    private void Awake()
    {
        // �t�B�[���h�Ɋւ���p�����[�^���擾
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;
    }


    /// <summary>
    /// �����_���ɂՂ�𒊑I����X�N���v�g�ł�
    /// </summary>
    public sbyte NextPuyoNumberLottery()
    {
        return (sbyte)Random.Range(1, _fieldParameter._puyoSprits.Length);
    }
}