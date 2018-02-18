﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObject : ScriptableObject 
{
	public Sprite sprite;

	public Sprite GetSprite() 
	{
		return sprite;
	}

	public void SetSprite(Sprite sprite) 
	{
		this.sprite = sprite;
	}
}
