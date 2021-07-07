using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Способ разрешения конфликта имён при загрузке
    /// </summary>
    public enum DownloadPermissionType
    {
        /// <summary>
        /// Разрешение еще не запрошено
        /// </summary>
        None,
        /// <summary>
        /// Разрешена перезапись
        /// </summary>
        Rewrite,
        /// <summary>
        /// Разрешено переименовывание
        /// </summary>
        Rename
    }
}
