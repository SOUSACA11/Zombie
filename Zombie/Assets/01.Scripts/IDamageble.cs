using UnityEngine;

public interface IDamageble
    //�ǰ� ���� �� �ִ� ������Ʈ
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
