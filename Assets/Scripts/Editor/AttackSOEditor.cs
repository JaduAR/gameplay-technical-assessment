#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(Attack))]
public class AttackSOEditor : Editor
{
    private Attack targetObject;

    public override void OnInspectorGUI()
    {
        targetObject = (Attack)target;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical();

        targetObject.AnimationStateName = EditorGUILayout.TextField("Animation State Name", targetObject.AnimationStateName);
        targetObject.IdleAnimationStateName = EditorGUILayout.TextField("Idle Animation State Name", targetObject.IdleAnimationStateName);
        targetObject.NextAttackTransition = (Attack)EditorGUILayout.ObjectField("Next Attack Transition", targetObject.NextAttackTransition, typeof(Attack), false);
        targetObject.CanDoDamage = EditorGUILayout.Toggle("Can Do Damage", targetObject.CanDoDamage);

        targetObject.HandIndex = (EHand)EditorGUILayout.EnumPopup("Hand Index:", targetObject.HandIndex);

        if (targetObject.CanDoDamage)
        {
            targetObject.Damage = EditorGUILayout.FloatField("Damage", targetObject.Damage);
            targetObject.EnableCollisionAtTime = EditorGUILayout.FloatField("Enable Collision At Time", targetObject.EnableCollisionAtTime);
        }

        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }
}

#endif