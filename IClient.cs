using System;
using System.Collections.Generic;
using System.Text;

namespace Identity
{
    public interface IClient
    {
        public bool Add(Client p);
        public bool Remove(Client p);
        public bool Contains(Client p);
        public bool Replace(Client pOld, Client pNew);
        public Client[] ToSortedArray();
        public Client Get(Client p);
    }
}