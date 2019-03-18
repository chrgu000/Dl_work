using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     目前可以识别的语种.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         目前支持英文,中文,繁体中文,日语，韩语
    ///     </para>
    /// </remarks>
    [Serializable]
    public enum Language
    {
        /// <summary>
        ///     英 语
        /// </summary>
        Eng = 0,

        /// <summary>
        ///     简 体
        /// </summary>
        Sim,

        /// <summary>
        ///     繁 体
        /// </summary>
        Tra,

        /// <summary>
        ///     日 语
        /// </summary>
        Jpn,

        /// <summary>
        ///     韩 语
        /// </summary>
        Kor
    }
}
