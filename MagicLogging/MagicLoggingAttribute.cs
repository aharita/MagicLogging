using System;
using System.Collections.Generic;

namespace MagicLogging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MagicLoggingAttribute : Attribute
    {
        public List<string> PropertiesToLog = new List<string>();

        public MagicLoggingAttribute()
        {

        }

        public MagicLoggingAttribute(params string[] properties)
        {
            PropertiesToLog.AddRange(properties);
        }

        public static string MagicLog(object obj)
        {
            var result = MagicLogger(obj);
            return string.Join("|", result);
        }

        private static IEnumerable<string> MagicLogger(object obj, string parentType = "")
        {
            var messageList = new List<string>();

            if (obj == null) return messageList;

            var type = obj.GetType();
            var attributes = GetCustomAttributes(type);

            if (String.IsNullOrEmpty(parentType))
            {
                parentType = type.Name;
            }
            else
            {
                parentType = String.Format("{0}.{1}", parentType, type.Name);
            }

            foreach (var attr in attributes)
            {
                if (attr is MagicLoggingAttribute)
                {
                    var logAttrInstance = ((MagicLoggingAttribute)attr);
                    var properties = type.GetProperties();

                    foreach (var property in properties)
                    {
                        if (logAttrInstance.PropertiesToLog.Count != 0 && !logAttrInstance.PropertiesToLog.Contains(property.Name))
                        {
                            continue;
                        }

                        bool suppress = false;
                        var val = property.GetValue(obj, null);
                        if (val != null)
                        {
                            messageList.AddRange((MagicLogger(val, parentType)));

                            var t = val.GetType();
                            var a = GetCustomAttributes(t);

                            foreach (var littleAttr in a)
                            {
                                if (littleAttr is MagicLoggingAttribute)
                                {
                                    suppress = true;
                                }
                            }
                        }

                        string logMessage = String.Format("{0}.{1}={2}", parentType, property.Name, val);
                        if (!suppress)
                        {
                            messageList.Add(logMessage);
                        }
                    }
                }
            }

            return messageList;
        }
    }
}
