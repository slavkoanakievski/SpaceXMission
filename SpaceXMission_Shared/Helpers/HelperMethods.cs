using Microsoft.AspNetCore.Identity;

namespace SpaceXMission_Shared
{
    public static class HelperMethods
    {
        public static void TrimStringProperties(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(obj);
                    property.SetValue(obj, value?.Trim());
                }
            }
        }

        public static string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }


    }
}
