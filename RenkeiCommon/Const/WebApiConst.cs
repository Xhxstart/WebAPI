namespace RenkeiCommon.Const
{
    public class WebApiConst
    {
        /// <summary>
        /// レスポンス(JSON)：error_type
        /// </summary>
        /// <remarks>
        /// マスタアップロードのエラーレスポンスで受け取る
        /// </remarks>
        public const string ITEM_NAME_JSON_ERROR_TYPE = "error_type";

        /// <summary>
        /// レスポンス(JSON)：error_id
        /// </summary>
        /// <remarks>
        /// 共通のエラーレスポンスで受け取る
        /// </remarks>
        public const string ITEM_NAME_JSON_ERROR_ID = "error_Id";

        /// <summary>
        /// レスポンス(JSON)：message
        /// </summary>
        /// <remarks>
        /// 報告書作成依頼ステータス取得のレスポンスで受け取る
        /// </remarks>
        public const string ITEM_NAME_JSON_MESSAGE = "message";

        /// <summary>
        /// WebAPIとの通信で使用するリソースパス：報告データマスタアップロード
        /// </summary>
        /// <remarks>
        /// WEBAPI_URLの{3}で使用
        /// </remarks>
        public const string PATH_UPLOAD_REPORT = "templates/{0}";

        /// <summary>
        /// 報告書情報取得
        /// </summary>
        public const string PATH_GET_REPORT_INFO = "formats/{0}/reports/{1}.json";

        /// <summary>
        /// WebAPIとの通信で使用するURL
        /// </summary>
        /// <remarks>
        /// {0}：{http} or {https}
        /// {1}：hostname
        /// {2}：WebAPIバージョン
        /// {3}：リソースパス
        /// </remarks>
        public const string URL = "{0}://{1}/kaisakuapi/{2}/{3}";

        /// <summary>
        /// WebAPIとの通信で使用するURL：メソッド(GET)
        /// </summary>
        /// <remarks>
        /// WEBAPI_URLの{1}で使用
        /// </remarks>
        public const string METHOD_GET = "GET";

        /// <summary>
        /// WebAPIとの通信で使用するURL：メソッド(PUT)
        /// </summary>
        /// <remarks>
        /// WEBAPI_URLの1}で使用
        /// </remarks>
        public const string METHOD_PUT = "PUT";

        /// <summary>
        /// WebAPIとの通信で使用するContent-Type：application/json;charset=UTF-8
        /// </summary>
        public const string CONTENT_TYPE_JSON = "application/json;charset=UTF-8";

        /// <summary>
        /// Protocol
        /// </summary>
        public const string PROTOCOL = "Protocol";

        /// <summary>
        /// HostName
        /// </summary>
        public const string HOST_NAME = "HostName";

        /// <summary>
        /// WebApiVer
        /// </summary>
        public const string VER = "WebApiVer";

        /// <summary>
        /// ProxyServer
        /// </summary>
        public const string PROXY_SERVER = "ProxyServer";

        /// <summary>
        /// ProxyServerAddress
        /// </summary>
        public const string PROXY_SERVER_ADDRESS = "ProxyServerAddress";

        /// <summary>
        /// ProxyServerPort
        /// </summary>
        public const string PROXY_SERVER_PORT = "ProxyServerPort";

        /// <summary>
        /// ProxyServerUserId
        /// </summary>
        public const string PROXY_SERVER_USERID = "ProxyServerUserId";

        /// <summary>
        /// ProxyServerPassword
        /// </summary>
        public const string PROXY_SERVER_PASSWORD = "ProxyServerPassword";

        /// <summary>
        /// Use
        /// </summary>
        public const string USE = "Use";

        /// <summary>
        /// WebAPIとの通信で使用するURL：http
        /// </summary>
        /// <remarks>
        /// WEBAPI_URLの{0}で使用
        /// </remarks>
        public const string URL_HTTP = "http";

        /// <summary>
        /// WebAPIとの通信で使用するURL：https
        /// </summary>
        /// <remarks>
        /// WEBAPI_URLの{0}で使用
        /// </remarks>
        public const string URL_HTTPS = "https";

        /// <summary>
        /// WebAPIとの通信時、ヘッダに追加する認証情報
        /// </summary>
        /// <remarks>
        /// APIキーを指定する
        /// </remarks>
        public const string HEADER_CERTIFICATE = "X-KAISAKU-Authorization: {0}";

        /// <summary>
        /// AuthorizationKey
        /// </summary>
        public const string AUTHORIZATION_KEY = "AuthorizationKey";

        /// <summary>
        /// RequestTimeout
        /// </summary>
        public const string STR_REQUEST_TIMEOUT = "RequestTimeout";

        /// <summary>
        /// HTTPエラー以外の場合のエラーコード：-1
        /// </summary>
        public const int NOT_HTTP_ERROR_CODE = -1;

        /// <summary>
        /// HTTPプロキシ未使用のエラーコード：-10
        /// </summary>
        public const int NOT_HTTP_PROXY_CODE = -10;

        /// <summary>
        /// リクエストタイムアウト時間（ミリ秒）
        /// </summary>
        public const int REQUEST_TIMEOUT = 600000;

        /// <summary>
        /// マスタアップロードのパラメータ：master_name
        /// </summary>
        public const string ITEM_NAME_MASTER_PARAM_NAME = "master_name";

        /// <summary>
        /// マスタアップロードのパラメータ：items
        /// </summary>
        public const string ITEM_NAME_MASTER_PARAM_ITEMS = "items";

        /// <summary>
        /// 取引先コード（WebApi）
        /// </summary>
        public const string TORIHIKISAKI_CD = "取引先コード";

        /// <summary>
        /// 取引先略称（WebApi）
        /// </summary>
        public const string TORIHIKISAKI_SNM = "取引先略称";

        /// <summary>
        /// 連携対象
        /// </summary>
        public const string RENKEI_TARGET = "連携対象";

        /// <summary>
        /// 取引先分類（WebApi）
        /// </summary>
        public const string TORIHIKISAKI_TYPE = "取引先分類";

        /// <summary>
        /// 本人格No
        /// </summary>
        public const string HONJINKAKU_NO = "法人格NO";

        /// <summary>
        /// 法人格名
        /// </summary>
        public const string HONJINKAKU_NAME = "法人格名";

        /// <summary>
        /// 法人格略称
        /// </summary>
        public const string HONJINKAKU_SNM = "法人格略称";

        /// <summary>
        /// 顧客マスタ連携（On）
        /// </summary>
        public const string KOKYAKU_RENKEI_ON = "1";

        /// <summary>
        /// 顧客管理NO
        /// </summary>
        public const string KOKYAKU_NO = "顧客管理NO";

        /// <summary>
        /// 顧客名
        /// </summary>
        public const string KOKYAKU_NM = "顧客名";

        /// <summary>
        /// 顧客略称
        /// </summary>
        public const string KOKYAKU_SNM = "顧客略称";

        public const string KOKYAKUKANRI_NO = "KOKYAKUKANRI_NO";

        /// <summary>
        /// WebAPIとの通信で使用するリソースパス：報告書登録
        /// </summary>
        public const string PATH_INSERT_REPORT = "formats/{0}";

        /// <summary>
        /// WebAPIとの通信で使用するリソースパス：報告書更新
        /// </summary>
        public const string PATH_UPDATE_REPORT = "formats/{0}/reports/{1}";

        /// <summary>
        /// WebAPIとの通信で使用するリソースパス：報告書削除
        /// </summary>
        public const string PATH_DELETE_REPORT = "formats/{0}/reports/{1}";
    }
}