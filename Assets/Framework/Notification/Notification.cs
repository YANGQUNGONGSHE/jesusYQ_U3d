using System;
using System.Collections.Generic;
using System.IO;

namespace WongJJ.Game.Core
{
    public class Notification : IEnumerable<KeyValuePair<string, object>>
    {
        private Dictionary<string, object> dicDatas = null;

        public string Name { get; set;}

        public object Sender { get; set;}

        public object Content { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="KFramework.Notification"/> with the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public object this[string key]
        {
            get
            {
                if (dicDatas == null && ! dicDatas.ContainsKey(key))
                    return null;
                return dicDatas[key];
            }
            set
            {
                if (dicDatas == null)
                {
                    dicDatas = new Dictionary<string, object>();
                }
                if (dicDatas.ContainsKey(key))
                {
                    dicDatas[key] = value;
                }
                else
                {
                    dicDatas.Add(key, value);
                }
            }
        }

        #region IEnumerable implementation
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            if (null == dicDatas)
                yield break;
            foreach (KeyValuePair<string, object> kvp in dicDatas)
            {
                yield return kvp;
            }
        }
        #endregion

        #region IEnumerable implementation
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dicDatas.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="KFramework.Notification"/> class.
        /// </summary>
        /// <param name="_notification">Notification.</param>
        public Notification(Notification _notification)
        {
            Name = _notification.Name;
            Sender = _notification.Sender;
            Content = _notification.Content;
            foreach (KeyValuePair<string, object> kvp in _notification.dicDatas)
            {
                this[kvp.Key] = kvp.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KFramework.Notification"/> class.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_sender">Sender.</param>
        public Notification(string _name, object _sender)
        {
            Name = _name;
            Sender = _sender;
            Content = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KFramework.Notification"/> class.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_sender">Sender.</param>
        /// <param name="_content">Content.</param>
        public Notification(string _name, object _sender, object _content)
        {
            Name = _name;
            Sender = _sender;
            Content = _content;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KFramework.Notification"/> class.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_sender">Sender.</param>
        /// <param name="_content">Content.</param>
        /// <param name="_params">Parameters.</param>
        public Notification(string _name, object _sender, object _content, params object[] _params)
        {
            Name = _name;
            Sender = _sender;
            Content = _content;
            if (_params.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (object _dicParam in _params)
                {
                    foreach (KeyValuePair<string, object> kvp in _dicParam as Dictionary<string, object>)
                    {
                        this[kvp.Key] = kvp.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Add the specified key and value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(string key, object value)
        {
            this[key] = value;
        }

        /// <summary>
        /// Remove the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public void Remove(string key)
        {
            if (dicDatas != null && dicDatas.ContainsKey(key))
            {
                dicDatas.Remove(key);
            }
        }

        /// <summary>
        /// Send this instance.
        /// </summary>
        public void PostNotification()
        {
            NotificationCenter.DefaultCenter().PostNotification(this);
        }
    }
}

