
using UnityEngine;

/// --------------------------------------------------
/// #WaveStateManagement.cs
/// �쐬��:�g�c�Y��
/// 
/// �E�F�[�u�̏�Ԃ�Scene�̏�ԍ��킹�ĊǗ����邽�߂̃X�N���v�g
/// --------------------------------------------------

public class WaveStateManagement : MonoBehaviour
{
    private void Start()
    {
        // �V���ɃE�F�[�u���J�n����Ƃ��̏���
        WaveState.TaragetCountUpdate();
    }
}
