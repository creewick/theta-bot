using System;
using System.Collections.Generic;

namespace theta_bot
{
    public class Cache<Key, Value>
    {
        private readonly Dictionary<Key, Value> cache;
        private readonly Func<Key, Value> getValue;
        private readonly bool deleteAfterRead;

        public Cache(Func<Key, Value> getValue, bool deleteAfterRead)
        {
            cache = new Dictionary<Key, Value>();
            this.getValue = getValue;
            this.deleteAfterRead = deleteAfterRead;
        }
        
        public Value Get(Key key)
        {
            if (!cache.ContainsKey(key)) 
                return getValue(key);
            
            var value = cache[key];
            if (deleteAfterRead) cache.Remove(key);
            return value;
        }

        public void Set(Key key, Value value) 
            => cache[key] = value;
    }
}