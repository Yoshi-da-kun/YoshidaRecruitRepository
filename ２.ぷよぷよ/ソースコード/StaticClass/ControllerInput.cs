
using UnityEngine;

/// ---------------------------------
/// #ControllerInput.cs
/// 作成者:吉田雄伍
/// 
/// コントローラやキーボードの入力関数をまとめたスクリプト
/// --------------------------------------------------

namespace ControllerInputFunction
{
    public static class ControllerInput
    {
        /// <summary>
        /// ゲームを再スタートする入力がされているかを返す
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
        /// 右回転するときの入力がされているかを返す
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
        /// 左回転するときの入力がされているかを返す
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
        /// スティックの横軸入力がされているかを返す
        /// </summary>
        public static float HorizontalInput()
        {
            return Input.GetAxis("Horizontal");
        }


        /// <summary>
        /// スティックの縦軸入力がされているかを返す
        /// </summary>
        public static float VerticalInput()
        {
            return Input.GetAxis("Vertical");
        }

    }
}
