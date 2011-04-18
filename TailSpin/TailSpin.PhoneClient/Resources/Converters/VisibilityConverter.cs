//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Resources.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>  
    /// A type converter for visibility and boolean values.  
    /// </summary>  
    public class VisibilityConverter : IValueConverter
    {
        public bool Negative { get; set; }

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var visibility = (bool)value;
            return ((this.Negative && !visibility) || (!this.Negative && visibility)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var visibility = (Visibility)value;
            return this.Negative ? visibility != Visibility.Visible : visibility == Visibility.Visible;
        }
    }
}