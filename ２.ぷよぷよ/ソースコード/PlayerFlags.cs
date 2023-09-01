
using System.Collections;
using UnityEngine;

/// --------------------------------------------------
/// #InGameFlags.cs
/// �쐬��:�g�c�Y��
/// 
/// �t�B�[���h�̃I�u�W�F�N�g�ɃA�^�b�`���Ă�������
/// �Q�[�����̃X�N���v�g�Ԃŋ��L����t���O���܂Ƃ߂��X�N���v�g�ł�
/// --------------------------------------------------

public class PlayerFlags : MonoBehaviour
{
    // �l�N�X�g�Ղ�̍X�V(���I)���J�n���邽�߂̃t���O
    [HideInInspector]
    public bool _isNextPuyoUpdate = default;

    // �Ղ���������ꂽ���������t���O(�Ղ悪���������x��true�ɂȂ�)
    [HideInInspector]
    public bool _isPuyoEraced = default;

    // �Ղ�ݒu��A���ׂĂ̂Ղ�������I��������������t���O
    [HideInInspector]
    public bool _isAllPuyoEraced = default;
}
