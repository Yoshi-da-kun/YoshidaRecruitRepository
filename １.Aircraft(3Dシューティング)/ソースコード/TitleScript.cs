
using UnityEngine;
using ControllerInput;

/// --------------------------------------------------
/// #TitleScript.cs
/// �쐬��:�g�c�Y��
/// 
/// �^�C�g���ł̏������s���X�N���v�g
/// --------------------------------------------------

public class TitleScript : MonoBehaviour
{
    [SerializeField, Label("FadeChangeScene�̃X�N���v�g")]
    private FadeChangeScene _fadeChangeScene;


    private void Update()
    {
        // ����Scene�ɂ������߂̓��͂������Ƃ�
        if (PlayerInput.TitlePressInput())
        {
            // �t�F�[�h�C�����J�n����Scene���ړ�����
            _fadeChangeScene.FadeInStart();
        }

        // �Q�[���I���̓��͂������Ƃ�
        if (PlayerInput.EscapeInput())
        {
            // �A�v���P�[�V�������I������
            Application.Quit();
        }
    }
}
