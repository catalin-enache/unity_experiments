using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Experiments.Lib
{
    public class WithTags : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<string> _tags = new List<string>();
        // Unity doesn't know how to serialize a HashSet
        public HashSet<string> Tags = new HashSet<string>();

        // https://docs.unity3d.com/2019.1/Documentation/ScriptReference/ISerializationCallbackReceiver.html
        public void OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            if(!EditorApplication.isPlaying
               && !EditorApplication.isUpdating
               && !EditorApplication.isCompiling) return;
            #endif

            _tags.Clear();
            foreach(var aTag in Tags)
            {
                _tags.Add(aTag);
            }
            // Debug.Log("OnBeforeSerialize: " + _tags.Count);
        }
        
        public void OnAfterDeserialize()
        {
            Tags.Clear();
            foreach(var aTag in _tags)
            {
                Tags.Add(aTag);
            }
            // Debug.Log("OnAfterDeserialize: " + Tags.Count);
        }
        
        void OnGUI()
        {
            // This is for debugging.
            // foreach (var _tag in Tags)
            // {
            //     GUILayout.Label("Tag: " + _tag);
            // }
        }
    }
}
