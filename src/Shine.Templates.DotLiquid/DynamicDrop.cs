using System;
using System.Collections.Generic;
using DotLiquid;

namespace Shine.Templates.DotLiquid
{
    public class DynamicDrop : Drop
    {
        private readonly object _context;
       
        public DynamicDrop(object context)
        {
            _context = context;
        }

        public override object BeforeMethod(string propertyName)
        {
            if (_context == null)
                return null;

            if (string.IsNullOrEmpty(propertyName))
                return null;
            
            
            var dict = _context as Dictionary<string, object>;
            if (dict != null)
            {
                return dict.ContainsKey(propertyName) ? Wrap(dict[propertyName]) : null;
            }
            
            var prop = _context.GetType().GetProperty(propertyName);
            return Wrap(prop?.GetValue(_context, null));
        }

        public static object Wrap(object raw)
        {
            if (raw == null)
                return null;

            if (Convert.GetTypeCode(raw) == TypeCode.Object)
                return new DynamicDrop(raw);

            return raw;
        }
    }
}
