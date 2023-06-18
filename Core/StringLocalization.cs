using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class StringLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<string> { }
        [SerializeField] private Handler handler;

        public override Type Type => typeof(string);

        public override void SetLocalization(object data)
        {
            handler?.Invoke((string)data);
        }
    }
}
