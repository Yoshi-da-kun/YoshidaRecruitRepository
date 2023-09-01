
using UnityEngine;

/// ---------------------------------
/// #ControllerInput.cs
/// �쐬��:�g�c�Y��
/// 
/// �R���g���[����L�[�{�[�h�̓��͊֐����܂Ƃ߂��X�N���v�g
/// --------------------------------------------------

namespace ControllerInputFunction
{
    public static class ControllerInput
    {
        /// <summary>
        /// �Q�[�����ăX�^�[�g������͂�����Ă��邩��Ԃ�
        /// </summary>
        public static bool RestartInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButton("OptionInput"))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// �E��]����Ƃ��̓��͂�����Ă��邩��Ԃ�
        /// </summary>
        public static bool RightRotationInput()
        {
            if (Input.GetButton("RightRotation") || Input.GetKey(KeyCode.E))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// ����]����Ƃ��̓��͂�����Ă��邩��Ԃ�
        /// </summary>
        public static bool LeftRotationInput()
        {
            if (Input.GetButton("LeftRotation") || Input.GetKey(KeyCode.Q))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// �X�e�B�b�N�̉������͂�����Ă��邩��Ԃ�
        /// </summary>
        public static float HorizontalInput()
        {
            return Input.GetAxis("Horizontal");
        }


        /// <summary>
        /// �X�e�B�b�N�̏c�����͂�����Ă��邩��Ԃ�
        /// </summary>
        public static float VerticalInput()
        {
            return Input.GetAxis("Vertical");
        }

    }
}
