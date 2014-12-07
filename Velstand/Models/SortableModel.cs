using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Velstand.Models
{
    public class SortableModel
    {
        public int PrimarySortOrder { get; set; }
        public IPublishedContent Content { get; set; }

        public SortableModel(IPublishedContent content)
        {
            this.Content = content;
        }
    }
}
