using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Frame.UI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class EventGroup
    {
        private Dictionary<List<KeyCode>, Action<List<KeyCode>,bool>> KeyEventTable;
        private Dictionary<List<string>, Action<List<string>,bool>> AxisEventTable;

        public bool IsActive { get; set; } = true;
        public bool IsUpdating { get; set; } = true;

        public EventGroup()
        {
            KeyEventTable = new Dictionary<List<KeyCode>, Action<List<KeyCode>, bool>>(new KeyCodeListEqualityComparer());
            AxisEventTable = new Dictionary<List<string>, Action<List<string>, bool>>(new StringListEqualityComparer());
        }

        public void AddEvent(Action<List<KeyCode>,bool> action, params KeyCode[] keys)
        {
            var keyList = new List<KeyCode>(keys);
            if (!KeyEventTable.ContainsKey(keyList))
                KeyEventTable.Add(keyList, action);
            else
                KeyEventTable[keyList] += action;
        }

        public void AddEvent(Action<List<string>,bool> action, params string[] axis)
        {
            var axisList = new List<string>(axis);
            if (!AxisEventTable.ContainsKey(axisList))
                AxisEventTable.Add(axisList, action);
            else
                AxisEventTable[axisList] += action;
        }

        public void RemoveEvent(params KeyCode[] keys)
        {
            var keyList = new List<KeyCode>(keys);
            if (KeyEventTable.ContainsKey(keyList))
                KeyEventTable.Remove(keyList);
            else
                Debug.LogWarning($"Keys {string.Join(", ", keys)} do not exist in the event table.");
        }

        public void RemoveEvent(params string[] axis)
        {
            var axisList = new List<string>(axis);
            if (AxisEventTable.ContainsKey(axisList))
                AxisEventTable.Remove(axisList);
            else
                Debug.LogWarning($"Axis {string.Join(", ", axis)} do not exist in the event table.");
        }

        public void Update()
        {
            if (IsActive)
            {
                foreach (var pair in KeyEventTable)
                {
                    pair.Value?.Invoke(pair.Key,IsUpdating);
                }

                foreach (var pair in AxisEventTable)
                {
                    pair.Value?.Invoke(pair.Key, IsUpdating);
                }
            }
        }

        private class KeyCodeListEqualityComparer : IEqualityComparer<List<KeyCode>>
        {
            public bool Equals(List<KeyCode> x, List<KeyCode> y)
            {
                if (x.Count != y.Count)
                    return false;
                for (int i = 0; i < x.Count; i++)
                {
                    if (!y.Contains(x[i]))
                        return false;
                }
                return true;
            }

            public int GetHashCode(List<KeyCode> obj)
            {
                int hash = 17;
                foreach (var key in obj)
                {
                    hash = hash * 31 + key.GetHashCode();
                }
                return hash;
            }
        }

        private class StringListEqualityComparer : IEqualityComparer<List<string>>
        {
            public bool Equals(List<string> x, List<string> y)
            {
                if (x.Count != y.Count)
                    return false;
                for (int i = 0; i < x.Count; i++)
                {
                    if (y.Contains(x[i]))
                        return false;
                }
                return true;
            }

            public int GetHashCode(List<string> obj)
            {
                int hash = 17;
                foreach (var str in obj)
                {
                    hash = hash * 31 + str.GetHashCode();
                }
                return hash;
            }
        }
    }

}
