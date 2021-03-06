using UnityEngine;

[CreateAssetMenu()]
public class PlayerControllerConfig : ScriptableObject
{
    public int speed = 1000;
    public int maxSpeed = 7;
    public int jumpPower = 18;
    public int interactRange = 1;

    public int meleeWeaponCoefficient = 5;
    public int rangedWeaponCoefficient = 5;
    public int healthCoefficient = 20;

    public bool isArrowDirect = true;
    public int arrowSpeed = 10;
}
