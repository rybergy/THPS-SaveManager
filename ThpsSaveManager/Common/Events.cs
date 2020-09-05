using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThpsSaveManager
{
    public delegate void TakesString(string text);

    public static class Events
    {
        public static event TakesString SetStatusText;

        public static void StatusText(string text)
        {
            SetStatusText?.Invoke(text);
        }
    }
}
