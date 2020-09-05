using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThpsSaveManager
{
    public class SaveEventArgs
    {
        public SaveViewModel Save { get; }

        public SaveEventArgs(SaveViewModel save)
        {
            Save = save;
        }
    }
}
