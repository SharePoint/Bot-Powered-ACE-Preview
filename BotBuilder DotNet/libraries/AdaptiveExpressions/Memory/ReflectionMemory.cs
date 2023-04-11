﻿using System;
using System.Collections.Concurrent;
using System.Reflection;
using AdaptiveExpressions.Properties;

namespace AdaptiveExpressions.Memory
{
    /// <summary>
    /// Internal class to duck type IMemory interface via reflection.
    /// </summary>
    internal class ReflectionMemory : IMemory
    {
        // cache of type => either Methods or null 
        private static ConcurrentDictionary<Type, Methods> methodsCache = new ConcurrentDictionary<Type, Methods>();

        private object _obj;
        private Methods _methods;

        private ReflectionMemory(object obj, Methods methods)
        {
            _obj = obj;
            _methods = methods;
        }

        public void SetValue(string path, object value)
        {
            _methods.SetValue.Invoke(_obj, new object[] { value });
        }

        public bool TryGetValue(string path, out object value)
        {
            value = null;
            var args = new object[] { path, null };
            var result = (bool)_methods.TryGetValue.Invoke(_obj, args);
            if (result)
            {
                value = args[1];

                if (value is IExpressionProperty ep)
                {
                    value = ep.GetObject(_obj);
                }
            }

            return result;
        }

        public string Version()
        {
            return (string)_methods?.Version?.Invoke(_obj, Array.Empty<object>());
        }

        internal static ReflectionMemory Create(object obj)
        {
            if (methodsCache.TryGetValue(obj.GetType(), out Methods methods))
            {
                if (methods != null)
                {
                    return new ReflectionMemory(obj, methods);
                }

                // cached negative result
                return null;
            }

            // if we can Duck type to IMemory contract
            var version = obj.GetType().GetMethod("Version", BindingFlags.Public | BindingFlags.Instance);
            if (version != null)
            {
                var setValue = obj.GetType().GetMethod("SetValue", BindingFlags.Public | BindingFlags.Instance);
                if (setValue != null)
                {
                    var tryGetValue = obj.GetType().GetMethod("TryGetValue", new Type[] { typeof(string), typeof(object).MakeByRefType() });
                    if (tryGetValue != null)
                    {
                        methods = new Methods()
                        {
                            Version = version,
                            TryGetValue = tryGetValue,
                            SetValue = setValue
                        };
                        methodsCache.TryAdd(obj.GetType(), methods);
                        return new ReflectionMemory(obj, methods);
                    }
                }
            }

            // remember this isn't IMemory object
            methodsCache.TryAdd(obj.GetType(), null);
            return null;
        }

        private class Methods
        {
            public MethodInfo TryGetValue { get; set; }

            public MethodInfo SetValue { get; set; }

            public MethodInfo Version { get; set; }
        }
    }
}
