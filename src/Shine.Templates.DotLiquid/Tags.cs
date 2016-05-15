using System.Collections.Generic;
using System.IO;
using DotLiquid;

namespace Shine.Templates.DotLiquid
{
    internal class Tags
    {
        public class StaticTag : Tag
        {
            private string _path;

            public override void Initialize(string tagName, string markup, List<string> tokens)
            {
                base.Initialize(tagName, markup, tokens);
                _path = markup.Trim().Trim('"', '\'');
            }

            public override void Render(global::DotLiquid.Context context, TextWriter result)
            {
                result.Write("/static/" + _path);
            }
        }
    }
}