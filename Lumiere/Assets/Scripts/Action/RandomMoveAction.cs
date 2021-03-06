﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lumiere/Actions/EntityActions/RandomMoveAction")]
public class RandomMoveAction : MonsterMoveAction
{
    public float directionChangeTimer = 5f;
    private float timer = 0f;
    private bool initialized = false;

    public override bool Validate(GameObject obj)
    {
        if (!base.Validate(obj))
        {
            return false;
        }

        //If monster is requesting to move for the first time, then allow the move.
        if (!initialized)
        {
            initialized = true;
            //We have validated once, set the timer to what it should be after one validation
            timer = Time.deltaTime;
            return true;
        }

        //Otherwise, only change movement every directionChangeTimer seconds.
        if (timer >= directionChangeTimer)
        {
            //Reset the timer to what it would've been after one validation
            timer = Time.deltaTime;
            return true;
        }

        //Increment the timer every time Action is validated
        timer += Time.deltaTime;
        return false;
    }

    public override bool Execute(GameObject obj)
    {
        Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();

        //Safety check for rigidbody
        if (rigidbody == null)
        {
            return false;
        }

        //Choose a random axis to move along (either the horizontal or the vertical)
        bool isHorizontal = Random.Range(-1.0f, 1.0f) >= 0.0f;
        //Choose whether to move in the positive or negative sense
        float directionModifier = Mathf.Sign(Random.Range(-1.0f, 1.0f));
        if (isHorizontal)
        {
            rigidbody.velocity = new Vector2(directionModifier * speed, 0f);
        }
        else
        {
            rigidbody.velocity = new Vector2(0f, directionModifier * speed);
        }

        return true;
    }
}
