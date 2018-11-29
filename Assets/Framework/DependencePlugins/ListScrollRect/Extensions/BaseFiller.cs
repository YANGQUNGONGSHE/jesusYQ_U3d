using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WongJJ.Game.Core.ListRectExtensions
{
    public abstract class BaseFiller<T> : MonoBehaviour, IContentFiller
        where T : class
    {
        /// <summary>
        /// 预设
        /// </summary>
        public GameObject CellPrefab;

        /// <summary>
        /// 点击事件
        /// </summary>
        [HideInInspector]
        public Action<int,T> OnCellClick;

        /// <summary>
        /// 长按事件
        /// </summary>
        [HideInInspector]
        public Action<int,T> OnCellLongPress;

        /// <summary>
        /// 数据源
        /// </summary>
        [HideInInspector]
        public List<T> DataSource;

        /// <summary>
        /// 滑动组件
        /// </summary>
        protected ListScrollRect MScrollView;
        public ListScrollRect ScrollView { get { return MScrollView; } }

        private void Awake()
        {
            DataSource = new List<T>();
            MScrollView = GetListScrollRect();
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        int IContentFiller.GetItemCount()
        {
            return OnGetItemCount();
        }

        int IContentFiller.GetItemType(int index)
        {
            return OnGetItemType(index);
        }

        GameObject IContentFiller.GetListItem(int index, int itemType, GameObject obj)
        {
            if (obj == null)
            {
                obj = Instantiate(CellPrefab);
            }
            OnGetListItem(index, itemType, obj);
            return obj;
        }

        protected virtual void OnAwake() { }

        protected virtual void OnStart() { }

        protected virtual void OnUpdate() { }

        protected abstract ListScrollRect GetListScrollRect();

        protected virtual int OnGetItemCount()
        {
            if (DataSource == null || DataSource.Count < 1)
                return 1;
            return DataSource.Count;
        }

        protected virtual int OnGetItemType(int index) { return 0; }

        protected virtual void OnGetListItem(int index, int itemType, GameObject obj) { }

        /// <summary>
        /// 根据主键初始化数据
        /// </summary>
        /// <param name="id"></param>
        public virtual void InitDataSourceWith(int id) { }

        /// <summary>
        /// 根据实体类初始化数据
        /// </summary>
        /// <param name="t"></param>
        public virtual void InitDataSourceWith(T t) { }

        /// <summary>
        /// 刷新
        /// </summary>
        public virtual void Refresh()
        {
            MScrollView.RefreshContent();
        }

        /// <summary>
        /// 滚到指定位置
        /// </summary>
        /// <param name="itemIndex">滚动到位置</param>
        /// <param name="isAnimation">是否有动画</param>
        public virtual void ScrollToIndex(int itemIndex,bool isAnimation = true)
        {
            if (isAnimation) MScrollView.ScrollToListItem(itemIndex);
            else MScrollView.GoToListItem(itemIndex);
        }

        /// <summary>
        /// 滚到顶部
        /// </summary>
        public virtual void ScrollToTop(bool isAnimation = true)
        {
            if (isAnimation) MScrollView.ScrollToListItem(0);
            else MScrollView.GoToListItem(0);
        }

        /// <summary>
        /// 滚到底部
        /// </summary>
        public virtual void ScrollToBottom(bool isAnimation = true)
        {
            if (isAnimation) MScrollView.ScrollToListItem(DataSource.Count - 1);
            else MScrollView.GoToListItem(DataSource.Count - 1);
        }
    }
}
