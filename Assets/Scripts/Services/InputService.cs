using System;
using UnityEngine;
using Zenject;

namespace Services
{
    public class InputService: ITickable
    {
        public event Action Tap;
        public event Action Escape;
        
        public void Tick()
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 1))
            {
                Tap?.Invoke();
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Escape?.Invoke();
            }
        }
    }
}