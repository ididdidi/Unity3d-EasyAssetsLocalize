﻿using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
    public abstract class ReceiversLocalizationTagEditor<T> : LocalizationTagEditor where T : Object
    {
        private ReceiversLocalizationTag<T> tag;
        private bool receiversfoldout;

        public override void OnEnable()
        {
            base.OnEnable();
            tag = target as ReceiversLocalizationTag<T>;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            tag.receivers = ExtendedEditorGUI.ArrayFields(tag.receivers, "Recivers", ref receiversfoldout, true);
        }
    }
}