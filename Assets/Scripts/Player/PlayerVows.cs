using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVows : MonoBehaviour
{
    private PlayerShoot _playerShoot;

    public List<VowsList> Vows = new List<VowsList>();

    private void Awake()
    {
        _playerShoot = GetComponent<PlayerShoot>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeEnemyDamage += CallOnTakeEnemyDamage;

        HurtReloadVow vow = new HurtReloadVow();
        Vows.Add(new VowsList(vow, vow.GetName(), vow.GetDescriptionGood(), vow.GetDescriptionBad()));

        StartCoroutine(CallUpdate());
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeEnemyDamage -= CallOnTakeEnemyDamage;
    }

    private IEnumerator CallUpdate()
    {
        foreach (VowsList v in Vows)
        {
            v.Vow.Update();
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallUpdate());
    }

    private void CallOnTakeEnemyDamage(GameObject enemy)
    {
        foreach (VowsList v in Vows)
        {
            v.Vow.OnTakeEnemyDamage(enemy, PlayerManager.Instance);
        }
    }
}
