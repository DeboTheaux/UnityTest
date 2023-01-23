using System;
using UnityEngine;

public enum ConditionOperator
{
    /// <summary>
    /// A field is visible/enabled if at least ONE condition is true.
    /// </summary>
    And,
    /// <summary>
    /// A field is visible/enabled if at least ONE condition is true.
    /// </summary>
    Or,
}

public enum ActionOnConditionFail
{
    /// <summary>
    /// If condition(s) are false, don't draw the field at all.
    /// </summary>
    DontDraw,
    /// <summary>
    /// If condition(s) are false, just set the field as disabled.
    /// </summary>
    JustDisable,
}


[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ShowIfAttribute : PropertyAttribute
{
    public ActionOnConditionFail Action { get; set; } = ActionOnConditionFail.DontDraw;
    public ConditionOperator Operator { get; set; } = ConditionOperator.And;
    public string[] Conditions { get; }


    public virtual bool InvertResult { get; } = false;


    public ShowIfAttribute(params string[] conditions)
    {
        Conditions = conditions;
    }

    public ShowIfAttribute(ActionOnConditionFail action, params string[] conditions) : this(conditions)
    {
        Action = action;
    }


    public ShowIfAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator, params string[] conditions)
        : this(action, conditions)
    {
        Operator = conditionOperator;
    }
}

public class ShowIfNotAttribute : ShowIfAttribute
{
    public override bool InvertResult => true;


    public ShowIfNotAttribute(params string[] conditions) : base(conditions)
    {
    }

    public ShowIfNotAttribute(ActionOnConditionFail action, params string[] conditions) : base(action, conditions)
    {
    }

    public ShowIfNotAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator, params string[] conditions)
        : base(action, conditionOperator, conditions)
    {
    }
}
