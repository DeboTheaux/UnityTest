using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//https://stackoverflow.com/questions/58441744/how-to-enable-disable-a-list-in-unity-inspector-using-a-bool
[CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
public class ShowIfAttributeDrawer : PropertyDrawer
{
    public const MemberTypes AcceptedMembers = MemberTypes.Field | MemberTypes.Property | MemberTypes.Method;
    public const BindingFlags MemberBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // The only way to know the first element of an array
        if (IsListOrArray(fieldInfo) && label.text == "Element 0")
            return base.GetPropertyHeight(property, label) + 10f;

        // Calculate the property height, if we don't meet the condition and the draw
        // mode is DontDraw, then height will be 0.
        var showIfAttribute = attribute as ShowIfAttribute;
        if (showIfAttribute.Action == ActionOnConditionFail.DontDraw && !ShouldDraw(property))
        {
            // Debug.Log("Not drawing");
            return 0;
        }

        return base.GetPropertyHeight(property, label) + 10f;
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var shouldDraw = ShouldDraw(property);
        //Debug.Log($"Property {property.name} will be drawn: {shouldDraw}");
        // Early out, if conditions met, draw and go.
        if (shouldDraw)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        else if (!shouldDraw && IsListOrArray(fieldInfo) && label.text == "Element 0")
        {
            label.text = "(This list is hidden, PropertyDrawer won't hide it completely)";
            label.tooltip = "Yeah, it kinda sucks.";
            var style = new GUIStyle();
            style.normal.textColor = Color.yellow;
            EditorGUI.LabelField(position, label, style);
        }
        else if ((attribute as ShowIfAttribute).Action == ActionOnConditionFail.JustDisable)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();
        }
    }



    public bool ShouldDraw(SerializedProperty property) =>
        ShouldDraw(property.serializedObject.targetObject, (ShowIfAttribute)attribute);

    public static bool ShouldDraw(object target, ShowIfAttribute attribute)
    {
        var result = MeetsConditions(target, attribute.Conditions, attribute.Operator);
        // Use XOR to invert the result when the attribute is set to
        return result ^ attribute.InvertResult;
    }

    // Made method static for unit testing
    public static bool MeetsConditions(object target, IEnumerable<string> conditionMemberNames,
        ConditionOperator conditionOperator = ConditionOperator.And)
    {
        Type targetType = target.GetType();

        // Get all the members, including fields, methods and getter properties
        //Debug.Log($"{targetType} members:\n{string.Join("\n", (IEnumerable<MemberInfo>)targetType.GetMembers())}");
        var members = new List<MemberInfo>(conditionMemberNames
            .Select((memberName) => targetType.GetMember(memberName, AcceptedMembers, MemberBindingFlags).FirstOrDefault())
            .Where((member) => member != null)
        );
        //Debug.Log($"Will check members with names: {string.Join(", ", conditionMemberNames)}\nObtained {members.Count}: {string.Join(", ", members)}");

        // Check for member validity
        var exceptions = new List<Exception>();
        foreach (MemberInfo member in members)
        {
            if (member is MethodInfo method && method.GetParameters().Length > 0)
                exceptions.Add(new ShowIfException("Method can't have parameters.", member));

            if (GetReturnType(member) != typeof(bool))
                exceptions.Add(new ShowIfException("Member has to return bool or be of type bool.", member));
        }
        if (exceptions.Count > 1)
            throw new AggregateException("Multiple errors with the members.", exceptions);
        else if (exceptions.Count == 1)
            throw exceptions[0];

        // Check conditions
        // Optimized, since with And the first false will return false and with Or the first true will return true
        switch (conditionOperator)
        {
            case ConditionOperator.And:
                return members.All(MemberIsTrue);
            case ConditionOperator.Or:
                return members.Any(MemberIsTrue);
            default:
                throw new NotImplementedException(conditionOperator.ToString());
        }

        // Local function, check if the member returns true depending if it's a field, property or member
        bool MemberIsTrue(MemberInfo member)
        {
            //Debug.Log($"Checking member \"{member}\"", target as UnityEngine.Object);
            switch (member)
            {
                case FieldInfo field:
                    return (bool)field.GetValue(target);
                case PropertyInfo property:
                    return (bool)property.GetValue(target);
                case MethodInfo method:
                    return (bool)method.Invoke(target, null);
                default:
                    return false;
            }
        }

        // Tries to get the method return type to ensure is a typeof(bool)
        Type GetReturnType(MemberInfo member)
        {
            switch (member)
            {
                case FieldInfo field:
                    return field.FieldType;
                case PropertyInfo property:
                    return property.PropertyType;
                case MethodInfo method:
                    return method.ReturnType;
                default:
                    return null;
            }
        }
    }


    public static bool IsListOrArray(FieldInfo fieldInfo)
    {
        Type type = fieldInfo.FieldType;
        return type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
    }
}


[Serializable]
public class ShowIfException : Exception
{
    public override string Message => (Member != null) ? $"{base.Message} (Member: {Member.Name})" : base.Message;

    public MemberInfo Member { get; }


    public ShowIfException() { }
    public ShowIfException(string message) : base(message) { }

    public ShowIfException(string message, MemberInfo member) : this(message)
    {
        Member = member;
    }

    public ShowIfException(string message, Exception inner) : base(message, inner) { }
    protected ShowIfException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}