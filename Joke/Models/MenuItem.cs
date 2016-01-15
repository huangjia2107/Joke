using Joke.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Joke.Models
{
    public class MenuItem
    {                        
        public string Icon { get; set; }
        public double IconFontSize { get; set; }    
        public string Title { get; set; }
        public JokeAPI JokeAPI { get; set; }
    }
}
