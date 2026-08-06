using UnityEngine;

[System.Serializable]
public abstract class Vow
{
    public abstract string GetName();
    public abstract string GetDescriptionGood();
    public abstract string GetDescriptionBad();
    
    public virtual void Update() {}

    public virtual void OnTakeEnemyDamage(GameObject enemy, PlayerManager playerManager) {}
}

// public class HurtReloadVow : Vow
// {
//     public override string GetName() {return "Blood Ammo";}
//     public override string GetDescriptionGood() {return "Upon taking damage, instantly reload half your gun's ammo";}
//     public override string GetDescriptionBad() {return "Manual reload time +50%";} // NOT IMPLEMENTED

//     public override void OnTakeEnemyDamage(GameObject enemy, PlayerManager playerManager)
//     {
//         playerManager.Shoot.InstantReload(0.5f);
//     }
// }

public class TestVow : Vow
{
    public override string GetName() {return "Test Vow";}
    public override string GetDescriptionGood() {return "Test Vow Description Good";}
    public override string GetDescriptionBad() {return "Test Vow Description Bad";}

    public override void Update()
    {
        Debug.Log("update TestVow");
    }
}