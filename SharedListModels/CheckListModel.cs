using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedListModels
{
     public class CheckListModel
    {
        public string Id => Guid.NewGuid().ToString();
        public string Name { get; set; }

        public List<CheckListItemModel> Items { get; set;}

        public string ShortRef => Id.Substring(0,8);


    }
}
