using UnityEngine;

public interface IDamageble
    //�ǰ� ���� �� �ִ� ������Ʈ
{
    void OnDamage(float damage, Vector3 hitpoint, Vector3 hitNormal);
}
