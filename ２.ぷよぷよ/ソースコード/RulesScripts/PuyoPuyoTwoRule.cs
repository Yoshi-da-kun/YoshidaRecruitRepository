
using UnityEngine;


/// --------------------------------------------------
/// #PuyoTwoScripts.cs
/// �쐬��:�g�c�Y��
/// 
/// �Ղ�Ղ�ʃ��[���p�̃X�N���v�g�ł�
/// --------------------------------------------------

public class PuyoPuyoTwoRule : MonoBehaviour
{
    // �Q�[�����̐i�s���Ǘ�����t���O���܂Ƃ߂��X�N���v�g�ł�
    private PlayerFlags _playerFlags;


    private void Start()
    {
        // �Q�[�����̐i�s���Ǘ�����t���O���܂Ƃ߂��X�N���v�g���擾
        _playerFlags = this.GetComponent<PlayerFlags>();
        
    }


    /// <summary>
    /// �S�Ă̂Ղ悪�����������̂Ղ�𒊑I���郁�\�b�h
    /// </summary>
    private void Update()
    {
        // �S�Ă̂Ղ悪�������ꂽ��
        if (_playerFlags._isAllPuyoEraced)
        {
            // ���̂Ղ�̒��I���J�n����
            _playerFlags._isNextPuyoUpdate = true;

            // �S�Ă̂Ղ悪�������Ƃ��̃t���O�����Ƃɖ߂�
            _playerFlags._isAllPuyoEraced = false;
        }
    }
}
