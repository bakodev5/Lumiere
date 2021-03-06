﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lumiere/Entity/NPC/Monster")]
public class Monster : NPC
{
    public GameObject damageParticles;
    public GameObject healthBarPrefab;

    public RuntimeAnimatorController animatorController;

    override public GameObject Spawn(Map map, Vector2 location)
    {
        GameObject monster = base.Spawn(map, location);
        monster.AddComponent<Timer>();
        GameObject particles = Instantiate(damageParticles, monster.transform);
        GameObject healthBar = Instantiate(healthBarPrefab, monster.transform, false);
        healthBar.AddComponent<EnemyHealthBarManager>();

        Animator anim = monster.AddComponent<Animator>();
        anim.runtimeAnimatorController = animatorController;
        MovementAnimation moveAnim = monster.AddComponent<MovementAnimation>();

        particles.transform.localPosition = Vector3.back;
        monster.layer = LayerMask.NameToLayer("Enemy");

        MonsterObject obj = new MonsterObject(monster, CalcMaxHealth(map));

        this.entityObject = obj;
        obj.entityDropGen = entityDropGen;

        monster.GetComponent<EntityHealthManager>().entityObj = obj;

        return monster;
    }

    public float CalcMaxHealth(Map map)
    {
        return (float)(maxHealth + maxHealth * (float)(map.levelNumber) * map.difficulty * .1f);
    }

}
