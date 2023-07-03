using UnityEngine;

public interface IDamageble
    //피격 당할 수 있는 오브젝트
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
