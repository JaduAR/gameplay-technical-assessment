#if UNITY_EDITOR

using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackTransition))]
public class AttackTransitionEditor : Editor
{
    private AttackTransition targetObject;

    public override void OnInspectorGUI()
    {
        targetObject = (AttackTransition)target;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical();

        GUIContent content = new GUIContent("Animation State Name",
             GetTooltip(targetObject.GetType().GetField("AnimationStateName"), true));
        targetObject.AnimationStateName = EditorGUILayout.TextField(content, targetObject.AnimationStateName);

        content = new GUIContent("Idle Animation State Name",
             GetTooltip(targetObject.GetType().GetField("IdleAnimationStateName"), true));
        targetObject.IdleAnimationStateName = EditorGUILayout.TextField(content, targetObject.IdleAnimationStateName);

        content = new GUIContent("Next Attack Transition",
             GetTooltip(targetObject.GetType().GetField("NextAttackTransition"), true));
        targetObject.NextAttackTransition = (AttackTransition)EditorGUILayout.ObjectField(content, targetObject.NextAttackTransition, typeof(AttackTransition), false);

        content = new GUIContent("Can Do Damage",
             GetTooltip(targetObject.GetType().GetField("CanDoDamage"), true));
        targetObject.CanDoDamage = EditorGUILayout.Toggle(content, targetObject.CanDoDamage);

        content = new GUIContent("Hand Index",
             GetTooltip(targetObject.GetType().GetField("HandIndex"), true));
        targetObject.HandIndex = (EHand)EditorGUILayout.EnumPopup(content, targetObject.HandIndex);

        if (targetObject.CanDoDamage)
        {
            content = new GUIContent("Damage",
             GetTooltip(targetObject.GetType().GetField("Damage"), true));
            targetObject.Damage = EditorGUILayout.FloatField(content, targetObject.Damage);

            content = new GUIContent("Enable Collision At Time",
             GetTooltip(targetObject.GetType().GetField("EnableCollisionAtTime"), true));
            targetObject.EnableCollisionAtTime = EditorGUILayout.FloatField(content, targetObject.EnableCollisionAtTime);
        }

        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    public static string GetTooltip(FieldInfo field, bool inherit)
    {
        TooltipAttribute[] attributes
             = field.GetCustomAttributes(typeof(TooltipAttribute), inherit)
             as TooltipAttribute[];

        string ret = "";
        if (attributes.Length > 0)
            ret = attributes[0].tooltip;

        return ret;
    }
}

#endif