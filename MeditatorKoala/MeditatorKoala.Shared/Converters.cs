using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Globalization;

namespace MeditatorKoala
{
    public class WidthConverter : IValueConverter
    {        
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Retrieve the format string and use it to format the value.
            string str_percentage = parameter as string;
            if (!string.IsNullOrEmpty(str_percentage))
            {
                var percentage = Double.Parse(str_percentage, CultureInfo.InvariantCulture);
                //System.Diagnostics.Debug.WriteLine("Parameter:" + str_percentage + "Value:" + (double)value + " | %:" + percentage + " >>>>:" + String.Format("{0}", percentage * (double)value));
                return String.Format("{0}", percentage * (double)value);
            }
            // If the format string is null or empty, simply call ToString()
            // on the value.
            return value.ToString();
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class LeftThicknessConverter : IValueConverter
    {       
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Retrieve the format string and use it to format the value.
            string str_percentage = parameter as string;
            if (!string.IsNullOrEmpty(str_percentage))
            {
                var percentage = Double.Parse(str_percentage, CultureInfo.InvariantCulture);
                return new Thickness(percentage * (double)value, 0, 0, 0);
            }
            // If the format string is null or empty, simply call ToString()
            // on the value.
            return value.ToString();
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class LeftRightThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Retrieve the format string and use it to format the value.
            string str_percentage = parameter as string;
            if (!string.IsNullOrEmpty(str_percentage))
            {
                var percentage = Double.Parse(str_percentage, CultureInfo.InvariantCulture);
                var tickness = percentage * (double)value;
                return new Thickness(tickness, 0, tickness, 0);
            }
            // If the format string is null or empty, simply call ToString()
            // on the value.
            return value.ToString();
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class FontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Retrieve the format string and use it to format the value.
            string str_percentage = parameter as string;
            if (!string.IsNullOrEmpty(str_percentage) && ((double)value > 0.0))
            {
                var percentage = Double.Parse(str_percentage, CultureInfo.InvariantCulture);
                var fontSize = percentage * (double)value;                
                if (fontSize > 72.0) fontSize = 72;                
                return  String.Format("{0}", fontSize);
            }            
            return "11";
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
