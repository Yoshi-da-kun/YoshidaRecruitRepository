
using UnityEngine;

/// --------------------------------------------------
/// #PlayerInput.cs
/// �쐬��:�g�c�Y��
/// 
/// �v���C���[�̓��͏����̊֐����܂Ƃ߂��X�N���v�g
/// --------------------------------------------------

namespace ControllerInput
{
    public static class PlayerInput
    {
        /// <summary>
        /// ���x�������s�����͒l��Ԃ�(���Ȃ猸���A���Ȃ����)
        /// </summary>
        public static float SpeedControlInput()
        {
            return Input.GetAxis("SpeedControl");
        }


        /// <summary>
        /// ������s�����߂̃X�e�B�b�N�ƃL�[�̓��͒l��Ԃ�
        /// </summary>
        public static Vector2 TurningInput()
        {
            // �X�e�B�b�N���L�[�̓��͒l���i�[
            float horizontalInputVolume = Input.GetAxisRaw("Horizontal");
            float verticalInputVolume = Input.GetAxisRaw("Vertical");

            // ����ʂ��i�[����
            return new Vector2(horizontalInputVolume, verticalInputVolume);
        }


        /// <summary>
        /// �e�𔭎˂���{�^���̓��͂�Ԃ�
        /// </summary>
        public static bool BulletShotInput()
        {
            return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0");
        }


        /// <summary>
        /// ���[�v�X�L���̓���
        /// </summary>
        public static bool TimeWarpBombInput()
        {
            //// ���u��
            return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown("joystick button 3");
        }


        /// <summary>
        /// �^�C�g���ŃX�^�[�g���邽�߂̓���
        /// </summary>
        public static bool TitlePressInput()
        {
            return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 0");
        }

        
        /// <summary>
        /// �Q�[���I���⃁�j���[���J������
        /// </summary>
        public static bool EscapeInput()
        {
            return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7");
        }
    }
}
