﻿namespace Domain.User;

///<author>Niels Van Steen</author>
/// <summary>
/// A specific implementation for <see cref="UserPropertyValue"/>
/// Holds no other information other than to specify the generic type.
/// </summary>
public class UserPropertyDecimalValue : UserPropertyValue
{
    // Fields.
    
    ///<author>Niels Van Steen</author>
    /// <summary>
    /// This property holds the value of a <see cref="UserPropertyName"/> for a single <see cref="User"/> E.g., '20' for property 'Age'.
    /// Where T instance of <see cref="UserPropertyType"/>
    /// </summary>
    public double? Value { get; set; }

    // Constructor.
    public UserPropertyDecimalValue() {}

    public UserPropertyDecimalValue(UserPropertyName userPropertyName, User user, double? value) : base(userPropertyName, user)
    {
        Value = value;
    } // UserPropertyDecimalValue.
}