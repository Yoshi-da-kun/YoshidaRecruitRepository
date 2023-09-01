
using UnityEngine;

/// --------------------------------------------------
/// #AircraftBullet.cs
/// ì¬Ò:‹g“c—YŒŞ
/// 
/// ’e‚Ìƒpƒ‰ƒ[ƒ^‚ğ‚Ü‚Æ‚ß‚½ƒXƒNƒŠƒvƒg
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/BulletParameter" ,fileName = "NewBulletParameter")]
public class BulletParameter : ScriptableObject
{
    [field: SerializeField, Label("’e‚ÌPrefab")]
    public GameObject _bulletPrefab { get; private set; }

    [field: SerializeField, Label("’e‚Ì‘¬“x")]
    public float _bulletSpeed { get; private set; }

    [field: SerializeField, Label("’e‚ÌUŒ‚—Í")]
    public int _bulletPower { get; private set; }

    [field: SerializeField, Label("’e‚ª”­Ë‚³‚ê‚Ä‚©‚çÁ‚¦‚é‚Ü‚Å‚ÌŠÔ")]
    public int _bulletBreakTime { get; private set; }

    [field: SerializeField, Label("’e‚ª“–‚½‚Á‚½‚ÌƒGƒtƒFƒNƒg")]
    public GameObject _hitEffect { get; private set; }

    [field: SerializeField, Label("’e‚ª“–‚½‚Á‚½‚ÌSE")]
    public AudioClip _hitSound { get; private set; }

    [field: SerializeField, Label("’e‚ğ”­Ë‚µ‚½‚ÌSE")]
    public AudioClip _shotSound { get; private set; }
}
