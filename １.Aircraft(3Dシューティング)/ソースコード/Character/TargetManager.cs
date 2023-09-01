
/// --------------------------------------------------
/// #TargetManager.cs
/// �쐬��:�g�c�Y��
/// 
/// �^�[�Q�b�g�I�u�W�F�N�g(�G��I)�ɋ��ʂ��鏈�����܂Ƃ߂��X�N���v�g
/// --------------------------------------------------

public class TargetManager : CharacterBase
{
    /// <summary>
    /// �I�u�W�F�N�g���L�������ꂽ�Ƃ��A�^�[�Q�b�g��ǉ�����
    /// </summary>
    new protected void OnEnable()
    {
        base.OnEnable();

        // �^�[�Q�b�g��ǉ�����
        WaveState.NewTargetAdd(this.gameObject);
    }

    // �I�u�W�F�N�g�����������ꂽ�Ƃ��A�^�[�Q�b�g�����X�V����
    protected void OnDisable()
    {

        // �^�[�Q�b�g�����X�V����
        WaveState.TaragetCountUpdate();
    }
}
