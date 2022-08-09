using System;
using System.Collections.Generic;
using Eto.Forms;

namespace StrucEngLib
{
    /// <summary>
    /// A Combobox Implementation which shares input across all instances and auto suggests already entered text.
    /// </summary>
    public class ComboBoxWithMemory : ComboBox
    {
        private readonly string _memoryBucket;

        public class ComboBoxMemory
        {
            public event EventHandler MemoryChanged;

            private readonly Dictionary<string, HashSet<string>> _bucketMemory =
                new Dictionary<string, HashSet<string>>();

            public Dictionary<string, HashSet<string>> BucketMemory => _bucketMemory;

            public void OnMemoryChanged(string text, string bucket)
            {
                if (!BucketMemory.ContainsKey(bucket))
                {
                    BucketMemory[bucket] = new HashSet<string>();
                }
                BucketMemory[bucket].Add(text);
                MemoryChanged?.Invoke(this, new ComboboxMemoryArgs() {Memory = text, Bucket = bucket});
            }
        }

        public class ComboboxMemoryArgs : EventArgs
        {
            public String Memory { get; set; }
            public String Bucket { get; set; }
        }

        protected static readonly ComboBoxMemory Memory = new ComboBoxMemory();

        protected HashSet<string> MemorySet;

        /// <summary>
        /// The bucket indicates the notification channel for updates
        /// </summary>
        public ComboBoxWithMemory(string memoryBucket = null)
        {
            if (memoryBucket == null)
            {
                return;
            }
            
            _memoryBucket = memoryBucket;
            MemorySet = new HashSet<string>();
            AutoComplete = true;

            if (Memory.BucketMemory.ContainsKey(memoryBucket))
            {
                foreach (var m in Memory.BucketMemory[memoryBucket])
                {
                    Items.Add(m);
                }
            }

            Memory.MemoryChanged += (sender, args) =>
            {
                var a = (ComboboxMemoryArgs) args;
                if (a.Bucket == _memoryBucket && !MemorySet.Contains(a.Memory))
                {
                    Items.Add(a.Memory);
                }
            };

            Items.CollectionChanged += (sender, args) =>
            {
                foreach (var i in args.NewItems)
                {
                    MemorySet.Add(i.ToString());
                }
            };

            LostFocus += (sender, args) => { Memory.OnMemoryChanged(Text, _memoryBucket); };
        }
    }
}