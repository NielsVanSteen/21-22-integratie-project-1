using Domain.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UI.MVC.Models.Shared;

public class ExtraUserInfoParser
{
        // Properties.
        private ICollection<UserPropertyValue> _parsedValues;
        
        // Constructor.
        public ExtraUserInfoParser() {}

        // Methods.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Parses <see cref="extraStringValues"/> to their correct and structured classes. Used to store in the database.
        /// </summary>
        /// <param name="extraStringValues"></param>
        /// <param name="userPropertyNames">List of all optional information labels. All these values should have a corresponding value in <see cref="extraStringValues"/></param>
        /// <param name="modelState">ModelState, used to add modelErrors if necessary.</param>
        public IEnumerable<UserPropertyValue> ParseUserPropertyValues(IList<string> extraStringValues, IList<UserPropertyName> userPropertyNames, ModelStateDictionary modelState)
        {
            _parsedValues = new List<UserPropertyValue>();
            
            // Number of values != number of inputs -> parsing not succeeded.
            if (extraStringValues != null && extraStringValues.Count() != userPropertyNames.Count())
                return null;
    
            // No optional registration information -> parsing succeeded.
            if (extraStringValues == null)
                return null;
        
            // Iterate all the values and try to parse them.
            for (int i = 0; i < extraStringValues.Count(); i++)
            {
                string stringValue = extraStringValues[i];
                var propertyName = userPropertyNames[i];
                
                // Check if the field was optional AND user didn't put something in.
                if (stringValue == null && !propertyName.IsRequired)
                {
                    _parsedValues.Add(new UserPropertyNumericValue() { UserPropertyName = propertyName, Value = null });
                    continue;
                }
                
                // Field is not optional -> parse.
                switch (propertyName.UserPropertyType)
                {
                    case UserPropertyType.String:
                        ParseStringValue(stringValue, propertyName, modelState);
                        break;
                    case UserPropertyType.Integer:
                        ParseIntValue(stringValue, propertyName, modelState);
                        break;
                    case UserPropertyType.Double:
                        ParseDoubleValue(stringValue, propertyName, modelState);
                        break;
                    case UserPropertyType.Date:
                        ParseDateValue(stringValue, propertyName, modelState);
                        break;
                } // Switch.
            } // For.

            return _parsedValues;
        } // ParseUserPropertyValues.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method adds the string to <see cref="_parsedValues"/>.
        /// </summary>
        /// <param name="stringValue">The value to add.</param>
        /// <param name="userPropertyName">The property the value belongs to.</param>
        /// <param name="user">The user who entered value.</param>
        /// <param name="modelState">ModelState, used to add modelErrors if necessary.</param>
        /// <returns></returns>
        private bool ParseStringValue(string stringValue, UserPropertyName userPropertyName, ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(stringValue) && userPropertyName.IsRequired)
            {
                string name = userPropertyName.UserPropertyLabel;
                modelState.AddModelError(name, name + " Mag niet leeg zijn!");
                return false;
            } // If.
            
            // Add the value.
            _parsedValues.Add(new UserPropertyStringValue() { UserPropertyName = userPropertyName, Value = stringValue});
            return true;
        } // ParseStringValue.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method tries to parse a string value to an int. and adds it to <see cref="_parsedValues"/> if the parsing succeeded.
        /// </summary>
        /// <param name="stringValue">The value that should be parsed to an int.</param>
        /// <param name="userPropertyName">The property the value belongs to.</param>
        /// <param name="user">The user who entered value.</param
        /// <param name="modelState">ModelState, used to add modelErrors if necessary.</param>
        /// <returns></returns>
        private bool ParseIntValue(string stringValue, UserPropertyName userPropertyName, ModelStateDictionary modelState)
        {
            // Try to parse.
            if (!int.TryParse(stringValue, out int intValue))
            {
                string name = userPropertyName.UserPropertyLabel;
                modelState.AddModelError(name, name + " moet een heel nummer zijn, zonder decimalen!");
                return false;   
            } // if.
            
            // Add the parsed value.
            _parsedValues.Add(new UserPropertyNumericValue() { UserPropertyName = userPropertyName, Value = intValue });
            return true;
        } // ParseStringValue.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method tries to parse a string value to a double. and adds it to <see cref="_parsedValues"/> if the parsing succeeded.
        /// </summary>
        /// <param name="stringValue">The value that should be parsed to a double.</param>
        /// <param name="userPropertyName">The property the value belongs to.</param>
        /// <param name="user">The user who entered value.</param>
        /// <param name="modelState">ModelState, used to add modelErrors if necessary.</param>
        /// <returns></returns>
        private bool ParseDoubleValue(string stringValue, UserPropertyName userPropertyName, ModelStateDictionary modelState)
        {
            // Try to parse.
            if (!double.TryParse(stringValue, out double doubleValue))
            {
                string name = userPropertyName.UserPropertyLabel;
                modelState.AddModelError(name, name + " Moet een nummer zijn!");
                return false;
            } // If.

            // Add the parsed value.
            _parsedValues.Add(new UserPropertyDecimalValue() { UserPropertyName = userPropertyName, Value = doubleValue});
            return true;
        } // ParseStringValue.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This method tries to parse a string value to a dateOnly. and adds it to <see cref="_parsedValues"/> if the parsing succeeded.
        /// </summary>
        /// <param name="stringValue">The value that should be parsed to a dateOnly.</param>
        /// <param name="userPropertyName">The property the value belongs to.</param>
        /// <param name="user">The user who entered value.</param>
        /// <param name="modelState">ModelState, used to add modelErrors if necessary.</param>
        /// <returns></returns>
        private bool ParseDateValue(string stringValue, UserPropertyName userPropertyName, ModelStateDictionary modelState)
        {
            // Try to parse.
            if (!DateOnly.TryParse(stringValue, out DateOnly dateValue))
            {
                string name = userPropertyName.UserPropertyLabel;
                modelState.AddModelError(name, name + " Moet een datum zijn!");
                return false;
            } // If.

            // Add the parsed value.
            _parsedValues.Add(new UserPropertyDateValue() { UserPropertyName = userPropertyName, Value = dateValue});
            return true;
        } // ParseStringValue.
}