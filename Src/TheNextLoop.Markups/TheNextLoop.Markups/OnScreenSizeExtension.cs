
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TheNextLoop.Markups.Enums;
using TheNextLoop.Markups.Extensions;
using TheNextLoop.Markups.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheNextLoop.Markups
{
    /// <summary>
    /// Markup Xaml para definir valores  dependendo do tamanho da tela do celular do usuario.
    /// </summary>
    public class OnScreenSize : IMarkupExtension
    {

        private static List<ScreenInfo> _screenSizes = new List<ScreenInfo>
        {
            { new ScreenInfo(480,800, eScreenSizes.ExtraSmall)}, //Samsung Galaxy S,
            { new ScreenInfo(720,1280, eScreenSizes.Small)}, //Nesus S
            { new ScreenInfo(828,1792, eScreenSizes.Medium)}, //iphone 11
            { new ScreenInfo(1284,2778, eScreenSizes.Large)}, //Apple iPhone 12 Pro Max
            { new ScreenInfo(1440,3200, eScreenSizes.ExtraLarge)}, //Samsung Galaxy S20+	
            { new ScreenInfo(2732,2048, eScreenSizes.ExtraLarge)}, //Apple iPad Pro 12.9
        };



        private Dictionary<eScreenSizes, object> _values = new Dictionary<eScreenSizes, object>() {
            { eScreenSizes.ExtraSmall, null},
            { eScreenSizes.Small, null},
            { eScreenSizes.Medium,  null},
            { eScreenSizes.Large,  null},
            { eScreenSizes.ExtraLarge,  null},
        };



        public OnScreenSize()
        {
        }


        /// <summary>
        /// Screen-size do device.
        /// </summary>
        private static eScreenSizes? deviceScreenSize;

        /// <summary>
        /// Tamanho-padrao na tela que deve ser assumido quando não for possivel determinar o tamanho dela com base na lista <see cref="_screenSizes"/>
        /// </summary>
        public object DefaultSize { get; set; } 


        public object ExtraSmall
        {
            get
            {

                return _values[eScreenSizes.ExtraSmall];
            }
            set
            {
                _values[eScreenSizes.ExtraSmall] = value;
            }
        }

        public object Small
        {
            get
            {

                return _values[eScreenSizes.Small];
            }
            set
            {
                _values[eScreenSizes.Small] = value;
            }
        }
        public object Medium
        {
            get
            {

                return _values[eScreenSizes.Medium];
            }
            set
            {
                _values[eScreenSizes.Medium] = value;
            }
        }

        public object Large
        {
            get
            {

                return _values[eScreenSizes.Large];
            }
            set
            {
                _values[eScreenSizes.Large] = value;
            }
        }

        public object ExtraLarge
        {
            get
            {

                return _values[eScreenSizes.ExtraLarge];
            }
            set
            {
                _values[eScreenSizes.ExtraLarge] = value;
            }
        }



        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueProvider = serviceProvider?.GetService<IProvideValueTarget>() ?? throw new ArgumentException();

            BindableProperty bp;
            PropertyInfo pi = null;
            Type propertyType = null;

            if (valueProvider.TargetObject is Setter setter)
            {
                bp = setter.Property;
            }
            else
            {
                bp = valueProvider.TargetProperty as BindableProperty;
                pi = valueProvider.TargetProperty as PropertyInfo;
            }

            propertyType = bp?.ReturnType ?? pi?.PropertyType ?? throw new InvalidOperationException("Não foi posivel determinar a propriedade para fornecer o valor.");

            var value = GetValue(serviceProvider);

            return value.ConvertTo(propertyType, bp);
        }


        private object GetValue(IServiceProvider serviceProvider)
        {
            var screenSize = GetScreenSize();
            if (screenSize != eScreenSizes.NotSet)
            {
                if (_values[screenSize] != null)
                {
                    return _values[screenSize];
                }
            }

            if (DefaultSize == null)
            {
                throw new XamlParseException("OnScreenExtension requires a DefaultSize set.");
            }
            else
            {
                return DefaultSize;
            }
        }


        private eScreenSizes GetScreenSize()
        {
            if (TryGetScreenSize(out var screenSize))
            {
                return screenSize;
            }

            return  eScreenSizes.NotSet;
        }


        private static bool TryGetScreenSize(out eScreenSizes screenSize)
        {
            if (deviceScreenSize != null)
            {
                if (deviceScreenSize.Value == eScreenSizes.NotSet)
                {
                    screenSize = deviceScreenSize.Value;
                    return false;
                }
                else
                {
                    screenSize = deviceScreenSize.Value;
                    return true;
                }
            }


            var device = DeviceDisplay.MainDisplayInfo;

            var deviceWidth = device.Width;
            var deviceHeight = device.Height;


            if (Xamarin.Essentials.DeviceInfo.Idiom == Xamarin.Essentials.DeviceIdiom.Tablet)
            {
                deviceWidth = Math.Max(device.Width, device.Height);
                deviceHeight = Math.Min(device.Width, device.Height);
            }


            foreach (var sizeInfo in _screenSizes)
            {
                if (deviceWidth <= sizeInfo.Width &&
                    deviceHeight <= sizeInfo.Height)
                {
                    deviceScreenSize = sizeInfo.ScreenSize;
                    screenSize = deviceScreenSize.Value;
                    return true;
                }
            }

            deviceScreenSize = eScreenSizes.NotSet;
            screenSize = deviceScreenSize.Value;
            return false;
        }

    }



    

  
}
