
using UnityEngine;

/// --------------------------------------------------------
/// #BreakableObjects.cs
/// 
/// �j��\�ȃI�u�W�F�N�g�̌p�����ɂȂ�X�N���v�g
/// --------------------------------------------------------

public class BreakableObjects : MonoBehaviour
{
    // �I�u�W�F�N�g�������Ă���c��HP
    private int _currentHp = default;

    private int _initializeHp = 3;

    // �_���[�W���󂯂����̃t���O
    protected bool _isDamaged = default;

    // ��ꂽ�Ƃ��̃t���O
    protected bool _isBreaked = default;


    private void OnEnable()
    {
        // Hp��������
        _currentHp = _initializeHp;
    }



    /// <summary>
    /// �U�����󂯂��Ƃ��̏���
    /// </summary>
    public void DamagedProcess(int damageVolume)
    {
        // ����HP���_���[�W�����炷
        _currentHp -= damageVolume;

        // �̗͂��Ȃ��Ȃ����Ƃ��A��ꂽ�Ƃ��̏������s��
        if (_currentHp < 0)
        {
            // ��ꂽ�t���O���Z�b�g
            _isBreaked = true;

            // �I�u�W�F�N�g�𖳌�������
            this.gameObject.SetActive(false);
        }

        // �_���[�W���������t���O���Z�b�g����
        _isDamaged = true;
    }
}
