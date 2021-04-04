using System.Collections.Specialized;
using System.Collections.Generic;

namespace Toolbox
{
    public class Utilities
    {
        public static IEnumerable<string> StringCollectionToEnumerable(StringCollection c)
        {
            foreach (var s in c)
            {
                yield return s;
            }
        }

        public static List<string> StringCollectionToList(StringCollection c)
        {
            var lraw = StringCollectionToEnumerable(c);
            var l = new List<string>(lraw);
            return l;
        }
    }
}
