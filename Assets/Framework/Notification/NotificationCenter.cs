using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace WongJJ.Game.Core
{
    public class NotificationCenter : Singletion<NotificationCenter>
    {
        public delegate void NotificationEvent(Notification notification);
        
        public static NotificationCenter DefaultCenter()
        {
            return Instance;
        }

        private Dictionary<string, List<NotificationEvent>> dicNotificationEvents = null;

        /// <summary>
        /// Init this instance.
        /// </summary>
        public override void Init()
        {
            dicNotificationEvents = new Dictionary<string, List<NotificationEvent>>();
        }

        /// <summary>
        /// Adds the observer.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_event">Event.</param>
        public void AddObserver(string _name, NotificationEvent _event)
        {
            List<NotificationEvent> list = null;
            if (dicNotificationEvents.ContainsKey(_name))
            {
                list = dicNotificationEvents[_name];
            }
            else
            {
                list = new List<NotificationEvent>();
                dicNotificationEvents.Add(_name, list);
            }

            // no same messageEvent then add
            if (!list.Contains(_event))
            {
                list.Add(_event);
            }
        }

        /// <summary>
        /// Removes the observer.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_event">Event.</param>
        public void RemoveObserver(string _name, NotificationEvent _event)
        {
            if (dicNotificationEvents.ContainsKey(_name))
            {
                List<NotificationEvent> list = dicNotificationEvents[_name];
                if (list.Contains(_event))
                {
                    list.Remove(_event);
                }
                if (list.Count <= 0)
                {
                    dicNotificationEvents.Remove(_name);
                }
            }
        }

        /// <summary>
        /// Removes all listener.
        /// </summary>
        public void RemoveAllListener()
        {
            dicNotificationEvents.Clear();
        }
         
        /// <summary>
        /// Posts the notification.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_sender">Sender.</param>
        public void PostNotification(string _name, object _sender)
        {
            PostNotification(new Notification(_name, _sender)); 
        }

        /// <summary>
        /// Posts the notification.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_sender">Sender.</param>
        /// <param name="_content">Content.</param>
        public void PostNotification(string _name, object _sender, object _content)
        {
            PostNotification(new Notification(_name, _sender, _content)); 
        }

        /// <summary>
        /// Posts the notification.
        /// </summary>
        /// <param name="_name">Name.</param>
        /// <param name="_sender">Sender.</param>
        /// <param name="_content">Content.</param>
        /// <param name="_params">Parameters.</param>
        public void PostNotification(string _name, object _sender, object _content, params object[] _params)
        {
            PostNotification(new Notification(_name, _sender, _content, _params)); 
        }

        /// <summary>
        /// Posts the notification.
        /// </summary>
        /// <param name="_notification">Notification.</param>
        public void PostNotification(Notification _notification)
        {
            PostNotificationDispatcher(_notification);
        }

        /// <summary>
        /// Posts the notification dispatcher.
        /// </summary>
        /// <param name="_notification">Notification.</param>
        private void PostNotificationDispatcher(Notification _notification)
        {
            if (dicNotificationEvents == null || !dicNotificationEvents.ContainsKey(_notification.Name))
            {
//                /Debug.LogError("NotificationCenter PostNotification(); can not find NotificationEvent with name " + _notification.Name);
                return;
            }
            List<NotificationEvent> list = dicNotificationEvents[_notification.Name];
            for (int i=0; i<list.Count; i++)
            {
                NotificationEvent notificationEvent = list[i];
                if (null != notificationEvent)
                {
                    notificationEvent(_notification);
                }
            }
        }
    }
}

