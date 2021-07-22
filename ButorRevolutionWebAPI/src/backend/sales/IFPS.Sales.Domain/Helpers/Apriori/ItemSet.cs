using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARM
{
    public class ItemSet<T> : HashSet<T>
    {
        private double mSupport = 0;
        private int mSupportLevel = 0;

        public double Support
        {
            get { return mSupport; }
            set { mSupport = value; }
        }

        public int SupportLevel
        {
            get { return mSupportLevel; }
            set { mSupportLevel = value; }
        }

        public string Signature
        {
            get
            {
                List<T> item_list = this.ToList();
                item_list.Sort();
                string itemset_id = string.Join(",", item_list);

                return itemset_id;
            }
        }

        public static string CreateSignature(params T[] items)
        {
            List<T> item_list = items.ToList();
            item_list.Sort();
            string itemset_id = string.Join(",", item_list);

            return itemset_id;
        }

        public override string ToString()
        {
            return string.Format("({0})", Signature);
        }

        public ItemSet<T> Clone()
        {
            ItemSet<T> clone = new ItemSet<T>();
            clone.Support = mSupport;
            foreach (T item in this)
            {
                clone.Add(item);
            }
            return clone;
        }
    }
}