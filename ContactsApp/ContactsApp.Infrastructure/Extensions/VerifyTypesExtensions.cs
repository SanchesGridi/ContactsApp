using System;

namespace ContactsApp.Infrastructure.Extensions
{
    public static class VerifyTypesExtensions
    {
        private const string ThisEmergencyClass = nameof(VerifyTypesExtensions);
        private const string ExFormat = "Exception thrown in class: ";
        private const string ExArgument = "Argument: ";
        private const string ExMessageForS = "Message: string is null or white space";
        private const string ExMessageForR = "Message: reference equal null";
        
        public static void VerifyTrueReference(this object @class, string message = null)
        {
            var addition = message == null ? string.Empty : $"; {message}";

            if (@class.GetType().IsValueType)
            {
                throw new InvalidOperationException($"{ExFormat}[{ThisEmergencyClass}]; type must be reference{addition}");
            }
        }
        public static void VerifyReference<TClass>(this TClass @this, string thisName = null) where TClass : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(thisName ?? nameof(@this), ExMessageForR);
            }

            @this.VerifyTrueReference();
        }
        public static void VerifyReferenceInClass<TClass>(this TClass @this, string thisName = null, string emergencyClass = null, string message = null) where TClass : class
        {
            var additionMessage = "; " + message == null ? ExMessageForR : message;

            if (@this == null)
            {
                throw new ArgumentNullException(thisName ?? nameof(@this), $"{ExFormat}[{emergencyClass ?? ThisEmergencyClass}]{additionMessage}");
            }

            @this.VerifyTrueReference();
        }
        public static TClass VerifyReferenceAndSet<TClass>(this TClass @this, string thisName = null) where TClass : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(thisName ?? nameof(@this), $"{ExFormat}[{ThisEmergencyClass}]; {ExMessageForR}");
            }

            @this.VerifyTrueReference();

            return @this;
        }

        public static void VerifyInterfaceRealization(this Type @this, Type @interface, string message = "Incorrect realization type")
        {
            if (@interface.IsInterface)
            {
                var interfaceName = @interface.FullName;
                var supposedInterfaceType = @this.GetInterface(interfaceName);

                if (supposedInterfaceType == null)
                {
                    throw new ArgumentException(message);
                }
            }
            else
            {
                throw new ArgumentException("Type is not an interface");
            }
        }

        public static void VerifyString(this string @this, string thisName, string emergencyClass)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                throw new ArgumentException($"{ExArgument}[{thisName}]; {ExFormat}[{emergencyClass}]; {ExMessageForS}.");
            }
        }
        public static string VerifyStringAndSet(this string @this, string emergencyClass = null)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                throw new ArgumentException($"{ExFormat}[{emergencyClass ?? ThisEmergencyClass}]; {ExMessageForS}");
            }

            return @this;
        }
    }
}