using UnityEngine;

public interface IDamageble
{
    void OnDamage(float damage, Vector3 hitpoint, Vector3 hitNormal);
}
