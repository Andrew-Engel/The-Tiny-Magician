using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name;
    public int experiencePoints = 0;

    public Character ()
    {
        name = "Not assigned";
    }
    public Character (string name)
    {
        this.name = name;
    }
    public virtual void PrintStatsInfo()
    { 
        Debug.LogFormat("Hero: {0}, XP: {1}",name,experiencePoints); 
    }
    private void Reset ()
    {
        this.name = "Not Assigned";
        this.experiencePoints = 0;
    }
}

public class Paladin : Character
{
    public Weapon weapon;


     public Paladin (string name, Weapon weapon): base (name)
    {
        this.weapon = weapon;
    }

    public override void PrintStatsInfo()
    {
        Debug.LogFormat("Hello {0}, good to see you! I see you have {1} as a weapon, that's a good choice.", name, weapon.name);
    }
}

public struct Weapon
{
    public string name;
    public int damage;

    public Weapon (string name, int damage)
    {
        this.name = name;
        this.damage = damage;
    }
public void PrintWeaponStats ()
    {
        Debug.LogFormat("Weapon: {0}, Damage: {1}", name, damage);
    }
}