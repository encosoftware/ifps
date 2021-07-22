using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARM
{
    public class Apriori<T>
    {
        private bool mDoSortingOnItemSetsOfSameSize = true;
        bool DoSortingOnItemSetsOfSameSize
        {
            get { return mDoSortingOnItemSetsOfSameSize; }
            set { mDoSortingOnItemSetsOfSameSize = value; }
        }

        public delegate void ItemSetSizeUpdateHandle(int current_itemset_size, int max_itemset_size);
        public delegate bool DoValidateItemSetHandle(ItemSet<T> itemset);
        public event DoValidateItemSetHandle DoValidateItemSet;

        protected bool IsValidItemSet(ItemSet<T> itemset)
        {
            if (DoValidateItemSet != null)
            {
                return DoValidateItemSet(itemset);
            }
            return true;
        }

        public void CalculateConfidences(List<HashSet<T>> transactions)
        {

        }

        /// <summary>
        /// Method that returns the frequent item sets grouped by support level
        /// </summary>
        /// <param name="database">The item database in terms of list of sets of items, each set represents a particular record in the item database</param>
        /// <param name="data_items">The item terminal set for the item database</param>
        /// <param name="min_support_level">The min support level (i.e. support count) below which the item set won't be considered frequent (therefore won't be included in the result returned</param>
        /// <param name="handle">A handle for user to display and write progress information</param>
        /// <returns>The Dictionary object in which the key represents the size of the frequent item set (e.g. if the frequent item set has 3 items, then the key will be 3), and the list of frequent item set having that item set size as the Dictionary value</returns>
        public Dictionary<int, List<ItemSet<T>>> BuildFreqItemSet(List<HashSet<T>> database, List<T> data_items, int min_support_level, int defined_max_item_set_size = -1, ItemSetSizeUpdateHandle handle = null)
        {
            Dictionary<int, List<ItemSet<T>>> itemsets_by_size = new Dictionary<int, List<ItemSet<T>>>();

            List<ItemSet<T>> itemsets = new List<ItemSet<T>>();

            int maxItemSetSize = 0;
            if (defined_max_item_set_size == -1)
            {
                foreach (HashSet<T> record in database)
                {
                    int rec_size = record.Count;
                    if (rec_size > maxItemSetSize)
                    {
                        maxItemSetSize = rec_size;
                    }
                }
            }
            else
            {
                maxItemSetSize = defined_max_item_set_size;
            }

            foreach (T data_item in data_items)
            {
                int support_level = 0;
                foreach (HashSet<T> record in database)
                {
                    if (record.Contains(data_item))
                    {
                        support_level += 1;
                    }
                }

                ItemSet<T> itemset = new ItemSet<T>();
                itemset.Support = (double)support_level / database.Count;
                itemset.SupportLevel = support_level;
                if (support_level >= min_support_level)
                {
                    itemset.Add(data_item);
                    itemsets.Add(itemset);
                }
            }

            itemsets_by_size[1] = itemsets;

            if (mDoSortingOnItemSetsOfSameSize)
            {
                itemsets.Sort((is1, is2) =>
                {
                    return is2.Support.CompareTo(is1.Support);
                });
            }

            BuildFreqItemSetAtLevel(database, min_support_level, itemsets_by_size, 2, maxItemSetSize, handle);

            return itemsets_by_size;
        }

        private void BuildFreqItemSetAtLevel(List<HashSet<T>> database,
            int min_support_level,
            Dictionary<int, List<ItemSet<T>>> itemsets_by_size, int current_itemset_size, int maxItemSetSize, ItemSetSizeUpdateHandle handle = null)
        {
            int prev_level = current_itemset_size - 1;
            List<ItemSet<T>> prev_itemsets = itemsets_by_size[prev_level];
            List<ItemSet<T>> new_itemsets = new List<ItemSet<T>>();
            HashSet<string> temp_itemset_names = new HashSet<string>();

            for (int i = 0; i < prev_itemsets.Count - 1; ++i)
            {
                ItemSet<T> itemset1 = prev_itemsets[i];
                for (int j = i + 1; j < prev_itemsets.Count; ++j)
                {
                    ItemSet<T> itemset2 = prev_itemsets[j];
                    ItemSet<T> temp_itemset = new ItemSet<T>();

                    foreach (T item in itemset1)
                    {
                        temp_itemset.Add(item);
                    }
                    foreach (T item in itemset2)
                    {
                        temp_itemset.Add(item);
                    }

                    if (temp_itemset.Count == current_itemset_size && IsValidItemSet(temp_itemset))
                    {
                        string itemset_id = temp_itemset.Signature;
                        if (!temp_itemset_names.Contains(itemset_id))
                        {
                            temp_itemset_names.Add(itemset_id);


                            int support_level = 0;
                            foreach (HashSet<T> record in database)
                            {
                                bool found = true;
                                foreach (T data_item in temp_itemset)
                                {
                                    if (!record.Contains(data_item))
                                    {
                                        found = false;
                                    }
                                }
                                if (found)
                                {
                                    support_level += 1;
                                }
                            }


                            if (support_level > min_support_level)
                            {
                                temp_itemset.Support = (double)support_level / database.Count;
                                temp_itemset.SupportLevel = support_level;
                                new_itemsets.Add(temp_itemset);
                            }
                        }
                    }
                }
            }

            itemsets_by_size[current_itemset_size] = new_itemsets;

            if (mDoSortingOnItemSetsOfSameSize)
            {
                new_itemsets.Sort((is1, is2) =>
                {
                    return is2.Support.CompareTo(is1.Support);
                });
            }

            if (handle != null)
            {
                handle(current_itemset_size, maxItemSetSize);
            }

            if (new_itemsets.Count > 1 && current_itemset_size + 1 <= maxItemSetSize)
            {
                BuildFreqItemSetAtLevel(database, min_support_level, itemsets_by_size, current_itemset_size + 1, maxItemSetSize, handle);
            }
        }

    }
}