using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RenkeiCommon.Const
{
    public class SqlConst
    {
        /// <summary>
        /// カラム（FORMAT_ID）
        /// </summary>
        public const string COL_FORMAT_ID = "FORMAT_ID";

        /// <summary>
        /// カラム（REPORT_NO）
        /// </summary>
        public const string COL_REPORT_NO = "REPORT_NO";

        /// <summary>
        /// カラム（更新日時）
        /// </summary>
        public const string COL_UPD_TM = "UPD_TM";

        /// <summary>
        /// カラム（登録日時）
        /// </summary>
        public const string COL_INS_TM = "INS_TM";

        /// <summary>
        /// SQLパラメータ（@formatId）
        /// </summary>
        public const string P_FORMAT_ID = "@formatId";

        /// <summary>
        /// SQLパラメータ（@reportNo）
        /// </summary>
        public const string P_REPORT_ID = "@reportId";

        /// <summary>
        /// SQLパラメータ（@insTm）
        /// </summary>
        public const string P_INS_TM = "@insTm";

        /// <summary>
        /// SQLパラメータ（@insTm）
        /// </summary>
        public const string P_UPD_TM = "@updTm";

        /// <summary>
        /// デフォルト条件
        /// </summary>
        public const string DEFAULT_CONDITION = "( 1 = 1 )";

        /// <summary>
        /// UPDATE
        /// </summary>
        public const string UPDATE = "UPDATE";

        /// <summary>
        /// INSERT
        /// </summary>
        public const string INSERT = "INSERT";

        /// <summary>
        /// DELETE
        /// </summary>
        public const string DELETE = "DELETE";

        /// <summary>
        /// INTO
        /// </summary>
        public const string INTO = "INTO";

        /// <summary>
        /// VALUES
        /// </summary>
        public const string VALUES = "VALUES";

        /// <summary>
        /// SET
        /// </summary>
        public const string SET = "SET";

        /// <summary>
        /// WHERE
        /// </summary>
        public const string WHERE = "WHERE";

        /// <summary>
        /// AND
        /// </summary>
        public const string AND = "AND";

        /// <summary>
        /// FROM
        /// </summary>
        public const string FROM = "FROM";

        /// <summary>
        /// SELECT
        /// </summary>
        public const string SELECT = "SELECT";

        /// <summary>
        /// 削除フラグ
        /// </summary>
        public const string DEL_FLG_0 = "0";

        /// <summary>
        /// フラグ0
        /// </summary>
        public const string DEFAULT_FLAG_0 = "0";

        /// <summary>
        /// フラグ1
        /// </summary>
        public const string DEFAULT_FLAG_1 = "1";

        /// <summary>
        /// SELECT CAST(SCOPE_IDENTITY() AS decimal)
        /// </summary>
        public const string SCOPE_IDENTITY = "SELECT CAST(SCOPE_IDENTITY() AS decimal)";

        /// <summary>
        /// マスタ連携バッチ実行履歴テーブル
        /// </summary>
        public const string QUERY_VIEW_REPORT_SELECT_BY_PROC_ID = "SELECT * FROM VIEW_REPORT WHERE FORMAT_ID = " + P_FORMAT_ID + " AND REPORT_ID = " + P_REPORT_ID;
    }
}