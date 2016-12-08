using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
using System.Data.Objects;

namespace SumTotal.Support.Extensions
{
    public static class EntityExtensions
    {
        public static EntityCollection<T> SafeLoad<T>(this EntityCollection<T> collection) where T : EntityObject
        {
            if (collection.IsLoaded)
                return collection;
            else
            {
                collection.Load();
                return collection;
            }
        }
    }
}
