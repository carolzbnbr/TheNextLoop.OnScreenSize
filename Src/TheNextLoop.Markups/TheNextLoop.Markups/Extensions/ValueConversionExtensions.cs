using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace TheNextLoop.Markups.Extensions
{

	public static class ValueConversionExtensions
	{
        private static Dictionary<Type, TypeConverter> _converter = new Dictionary<Type, TypeConverter>();

        /// <summary>
        /// Tenta converter o valor de <paramref name="value"/> para <paramref name="toType"/>, usando se necessário as informações de <paramref name="bindableProperty"/>
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="toType">destination type</param>
        /// <param name="bindableProperty"></param>
        /// <returns></returns>
        public static object ConvertTo(this object value, Type toType, BindableProperty bindableProperty)
		{
            object returnValue;
            if (_converter.TryGetValue(toType, out var  converter))
            {
                returnValue = converter.ConvertFromInvariantString((string)value);
                return returnValue;
            }

            if (toType.IsEnum)
            {
                returnValue = Enum.Parse(toType, (string)value);
                return returnValue;
            }


            if (toType.Namespace.StartsWith("Xamarin.Forms"))
            {
                var typeConverter = toType.GetCustomAttribute<TypeConverterAttribute>(true);

                if (typeConverter != null && typeConverter.ConverterTypeName != null)
                {
                    var convertertype = Type.GetType(typeConverter.ConverterTypeName);

                    converter = (TypeConverter)Activator.CreateInstance(convertertype);

                    _converter.Add(toType, converter);
                    return converter.ConvertFromInvariantString((string)value);
                }
            }

            
            if (bindableProperty != null && toType == typeof(System.Double) && bindableProperty.PropertyName.Equals("FontSize", StringComparison.InvariantCultureIgnoreCase))
            {
                returnValue = new FontSizeConverter().ConvertFromInvariantString((string)value);
                return returnValue;
            }


            returnValue = Convert.ChangeType(value, toType, CultureInfo.InvariantCulture);
            return returnValue;
        }

		
	}
}
