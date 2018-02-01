using System;

namespace Test.COM.Entity
{
    public class BaseItem
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 是否可选
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
