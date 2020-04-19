using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEntity : MonoBehaviour
{
    public delegate void OnDamage(int amount);
    public OnDamage onDamage;
    public void Damage(int amount)
    {
        onDamage?.Invoke(amount);
    }
}
