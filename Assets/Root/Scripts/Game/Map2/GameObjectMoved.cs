using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map2
{
    public class GameObjectMoved
    {
        public GameObject current;
        public GameObject target;
        public float speed;
        public Action action;

        public GameObjectMoved(GameObject current, GameObject target, float speed, Action action)
        {
            this.current = current;
            this.target = target;
            this.speed = speed;
            this.action = action;
        }
    }
}
