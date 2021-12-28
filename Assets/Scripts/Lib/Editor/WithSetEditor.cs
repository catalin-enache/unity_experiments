

namespace Experiments.Lib.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(WithStringSet))]
    public class WithSetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (((WithStringSet) target).Set.isEditMode)
            {
                if (GUILayout.Button("Save Changes"))
                {
                    ((WithStringSet) target).Set.isEditMode = false;
                }
            }
            if (GUILayout.Button("Debug Print"))
            {
                ((WithStringSet) target).Set.DebugPrint();
            }
        }
    }
}
