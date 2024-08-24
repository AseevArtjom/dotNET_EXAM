using dotNET_EXAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace dotNET_EXAM.Services
{
    public class RolesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var userRoles = value as ICollection<UserRole>;
            if (userRoles != null)
            {
                return string.Join(", ", userRoles.Select(ur => ur.Role.Name));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
