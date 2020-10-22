using System;
using System.Security;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ContactsApp.Infrastructure.Extensions
{
    public static class CsTypesExtensions
    {
        public static TRef AsRef<TRef>(this object @this) where TRef : class
        {
            if (typeof(TRef).IsClass)
            {
                return @this.VerifyReferenceAndSet() as TRef;
            }
            else
            {
                throw new InvalidCastException("Type is not reference");
            }
        }

        public static bool IsNotEmpty(this string @this)
        {
            return !string.IsNullOrWhiteSpace(@this);
        }
        public static bool IsEmpty(this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        public static bool CheckStringKey<TValue>(this Dictionary<string, TValue> @this, string key)
        {
            return key.IsNotEmpty() ? @this.ContainsKey(key) : false;
        }
        public static Dictionary<TKey, TValue> InitializeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            return pairs != null ? new Dictionary<TKey, TValue>(pairs) : new Dictionary<TKey, TValue>();
        }

        public static SecureString StringToSecureString(this string @this)
        {
            var secure = new SecureString();

            foreach (var chr in @this.ToCharArray())
            {
                secure.AppendChar(chr);
            }
            secure.MakeReadOnly();

            return secure;
        }
        public static string SecureStringToString(this SecureString secure)
        {
            var pointer = IntPtr.Zero;

            return pointer.ValueInvokeTryCatchFinally(@try: ptr =>
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secure);

                return Marshal.PtrToStringUni(ptr);
            }, @finally: ptr => Marshal.ZeroFreeGlobalAllocUnicode(ptr), @throw: true);
        }

        public static TAny ReferenceInvokeTryCatchFinally<TClass, TAny>(
            this TClass @this,
            Func<TClass, TAny> @try,
            string thisName = null,
            Func<Exception, TClass, TAny> @catch = null,
            string catchName = null,
            Action<TClass> @finally = null,
            bool @throw = false) where TClass : class
        {
            @this.VerifyReferenceInClass(thisName ?? nameof(@this), nameof(CsTypesExtensions));

            return @this.GetBoyTryCatchFinally(@try, @catch, catchName, @finally, @throw);
        }
        public static TAny ValueInvokeTryCatchFinally<TStruct, TAny>(
            this TStruct @this,
            Func<TStruct, TAny> @try,
            Func<Exception, TStruct, TAny> @catch = null,
            string catchName = null,
            Action<TStruct> @finally = null,
            bool @throw = false) where TStruct : struct
        {
            return @this.GetBoyTryCatchFinally(@try, @catch, catchName, @finally, @throw);
        }
        private static TAny GetBoyTryCatchFinally<TValue, TAny>(this TValue @this,
           Func<TValue, TAny> @try,
           Func<Exception, TValue, TAny> @catch = null,
           string catchName = null,
           Action<TValue> @finally = null,
           bool @throw = false)
        {
            if (!@throw)
            {
                @catch.VerifyReferenceInClass(catchName ?? nameof(@catch), nameof(CsTypesExtensions));
            }

            try
            {
                return @try.Invoke(@this);
            }
            catch (Exception ex)
            {
                if (@throw)
                {
                    throw;
                }
                else
                {
                    return @catch.Invoke(ex, @this);
                }
            }
            finally
            {
                @finally?.Invoke(@this);
            }
        }
    }
}