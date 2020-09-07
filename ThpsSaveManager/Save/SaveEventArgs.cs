using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThpsSaveManager
{
    public class SaveEventArgs
    {
        public SaveListElementViewModel Save { get; }

        public SaveEventArgs(SaveListElementViewModel save)
        {
            Save = save;
        }
    }
}
