
using UnityEngine;

/// --------------------------------------------------
/// #CharacterBase.cs
/// �쐬��:�g�c�Y��
/// 
/// �L�����N�^�[�ɋ��ʂ��鏈�����܂Ƃ߂��X�N���v�g
/// �L�����N�^�[�Ƃ́AHP��������
/// --------------------------------------------------

public class CharacterBase : MonoBehaviour
{
    [SerializeField, Label("���̃L�����̃p�����[�^")]
    protected CharacterBaseParameter _characterParameter;

    // ���݂̃L�����N�^�[��HP
    protected int _currentCharacterHp = default;

    // �_���[�W���󂯂����Ƃ������t���O
    protected bool _isDamage = false;

    // �L���������S���Ă��邩�������t���O
    protected bool _isDead = false;



    protected void OnEnable()
    {
        // �L�����N�^�[���X�|�[���������̏���
        SpawnProcess();
    }


    /// <summary>
    /// �L�������X�|�[�������Ƃ��̏���
    /// </summary>
    private void SpawnProcess()
    {
        // �L������HP�̏�����
        _currentCharacterHp = _characterParameter._maxHp;
        
        // ���S�t���O�̏�����
        _isDead = false;
    }


    /// <summary>
    /// �L�����N�^�[���_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="DamagePoint"> �^����U����(�_���[�W) </param>
    public void TakesDamage(int DamagePoint)
    {
        // ����HP����U�����ꂽ�l������
        _currentCharacterHp -= DamagePoint;
        
        // �_���[�W�t���O���Z�b�g����
        _isDamage = true;

        // �̗͂��O�ȉ��̂Ƃ��A���S�t���O���Z�b�g����
        if (_currentCharacterHp <= 0)
        {
            _isDead = true;
        }
    }
}
