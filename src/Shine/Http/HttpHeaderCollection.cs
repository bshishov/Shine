using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shine.Http
{
    public class HttpHeaderCollection : IEnumerable<HttpHeaderField>
    {
        private readonly List<HttpHeaderField> _fields = new List<HttpHeaderField>();

        public HttpHeaderCollection()
        {
        }

        public void Add(HttpHeaderField field, bool folding = true)
        {
            if (folding)
            {
                var existing = _fields.FirstOrDefault(f => f.Name.Equals(field.Name, StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                    _fields.Add(field);
                else
                    existing.Fold(field);
            }
            else
            {
                _fields.Add(field);
            }
        }

        public void Add(string name, string value, bool folding = true)
        {
            this.Add(new HttpHeaderField(name, value));
        }

        public IEnumerator<HttpHeaderField> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        public string this[string name]
        {
            get { return _fields.FirstOrDefault(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Value; }
        }
    }
}
