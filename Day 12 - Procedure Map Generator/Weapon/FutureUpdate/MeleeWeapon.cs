using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : BaseWeapon
{
    public float readyToStrikeSec;
    [Range(0, 100)]
    public float Mobility;
    [Range(0, 100)]
    public float Ergonomic;
}
