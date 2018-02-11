﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObject : ScriptableObject {
	protected Sprite sprite;

	public Sprite getSprite() {
		return sprite;
	}

	public void setSprite(Sprite sprite) {
		this.sprite = sprite;
	}
}