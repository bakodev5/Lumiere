﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : BaseObject 
{
	public EntityAction[] actions; 	//will hold all actions that this entity can perform
    public Vector2 colliderSize; // the size of the entity's collider

    protected float maxHealth;
    protected float currHealth;

    /// <summary>
    /// Spawn the entity at the specified location.
    /// </summary>
    /// <param name="location">Location to spawn this entity.</param>
    /// <returns>Returns the GameObject representing this entity.</returns>
    virtual public GameObject Spawn (Vector2 location, float maxHealth = 1.0f)
    {
        this.maxHealth = this.currHealth = maxHealth;

        GameObject entity = new GameObject (this.name);
        entity.transform.position = location;

        SpriteRenderer renderer = entity.AddComponent<SpriteRenderer> ();
        renderer.sortingOrder = 1;

        EntitySpriteManager entitySpriteManager = entity.AddComponent<EntitySpriteManager> ();
        entitySpriteManager.entity = this;

        BoxCollider2D collider = entity.AddComponent<BoxCollider2D> ();
        collider.size = colliderSize;

        Rigidbody2D rigidbody = entity.AddComponent<Rigidbody2D> ();
        rigidbody.gravityScale = 0f;
        rigidbody.angularDrag = 0f;
        rigidbody.freezeRotation = true;

        EntityActionManager entityActionManager = entity.AddComponent<EntityActionManager> ();
        entityActionManager.entity = this;

        return entity;
    }

    virtual public void InflictDamage(float damageAmount)
    {
        this.currHealth -= damageAmount;
        if (currHealth <= 0)
            this.Die();
    }

    virtual protected void Die()
    {

    }
}
