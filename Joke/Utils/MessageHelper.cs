using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joke.Utils
{
    public class PopupToastArgs
    {
        public bool IsCancel { get; set; }
        public string Msg { get; set; }
    }

    public class MessageHelper
    {
        public static readonly string PopupToastToken = "PopupTost";
    }
}
