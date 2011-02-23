using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyData.Query
{
    public interface IFrom
    {
        IQuery From(Type type);
    }
}
