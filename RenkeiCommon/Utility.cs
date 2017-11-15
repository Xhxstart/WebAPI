using System;
using Codeplex.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using RenkeiCommon.Const;
using RenkeiCommon.Api;

namespace RenkeiCommon
{
    public class Utility
    {
        #region 変数定義

        public static Configuration Config;

        private static XDocument Message;

        /// <summary>
        /// ロガー
        /// </summary>
        public ILog logger;

        #endregion 変数定義

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Utility(ILog log)
        {
            if (Config == null)
            {
                Config = GetAppConfig(CommConst.APP_COMMON_CONFIG_FILE);
            }
            logger = log;
        }

        #endregion コンストラクタ

        #region App.config取得

        /// <summary>
        /// App.config取得
        /// </summary>
        /// <param name="appConfig"></param>
        /// <returns></returns>
        private Configuration GetAppConfig(string appConfig)
        {
            var configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace(CommConst.FILE_URI, "")) + CommConst.YEN_MARK + CommConst.APP_COMMON_CONFIG_FILE;
            return ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
        }

        #endregion App.config取得

        #region コネクション取得

        public static string GetConnection(string connectName)
        {
            return Utility.Config.ConnectionStrings.ConnectionStrings[connectName].ToString();
        }

        #endregion コネクション取得

        #region Jsonファイル解析

        /// <summary>
        /// Jsonファイル解析
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public dynamic JsonParse(string filePath)
        {
            dynamic rtn;
            using (var reader = new StreamReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace(CommConst.FILE_URI, "")) + CommConst.YEN_MARK + CommConst.JSON_FOLDER + CommConst.YEN_MARK + filePath))
            {
                rtn = DynamicJson.Parse(reader.ReadToEnd(), Encoding.UTF8);
            }
            return rtn;
        }

        #endregion Jsonファイル解析

        /// <summary>
        /// マスタ連携バッチ実行履歴テーブルデータ取得
        /// </summary>
        /// <param name="procId">処理ID</param>
        /// <returns></returns>
        public DataTable GetKayisakuData(string[] args, string condition)
        {
            logger.Debug("Utility#GetMstRenkeiBatchJikkouRireikiTbl() Start");
            var dt = new DataTable();
            var dbAccess = new DbAccess(logger, GetConnection(CommConst.KAYISAKU_CONNECTION));
            try
            {
                var parameters = new object[] { SqlConst.P_FORMAT_ID + CommConst.COMMA + args[0], SqlConst.P_REPORT_ID + CommConst.COMMA + args[1] };
                dt = dbAccess.Reader(SqlConst.QUERY_VIEW_REPORT_SELECT_BY_PROC_ID, parameters);
            }
            finally
            {
                dbAccess.Close();
            }
            logger.Debug("Utility#GetMstRenkeiBatchJikkouRireikiTbl() End");
            return dt;
        }

        #region メッセージ埋込文字変換

        /// <summary>
        /// メッセージ埋込文字変換
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="values">変換内容</param>
        /// <returns></returns>
        public static string ReleaseMsg(string msg, params string[] values)
        {
            //if (values != null)
            //{
            //    values.ToList().Select((value, idx) => new { idx, value }).ToList().ForEach(x =>
            //    {
            //        msg = msg.Replace(CommConst.LEFT_BRACES + x.idx + CommConst.RIGHT_BRACES, x.value);
            //    });
            //}

            return msg;
        }

        #endregion メッセージ埋込文字変換

        #region メッセージ取得

        /// <summary>
        /// メッセージ取得
        /// </summary>
        /// <param name="msgId"></param>
        /// <returns></returns>
        //public static string GetMsg(string msgId)
        //{
        //    var message = string.Empty;
        //    if (Message == null)
        //    {
        //        Message = XDocument.Load(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace(CommConst.FILE_URI, "")) + CommConst.YEN_MARK + CommConst.MESSAGE + CommConst.YEN_MARK + CommConst.MESSAGES);
        //    }
        //    var msg = Message.Descendants(CommConst.MESSAGE).Where(x => msgId.Equals(x.Attribute(CommConst.ID).Value)).Select(x => x.Value).FirstOrDefault();
        //    if (msg != null)
        //    {
        //        message = msgId + CommConst.SEMI_COLON + msg;
        //    }
        //    return message;
        //}

        #endregion メッセージ取得

        #region ダブルコーテーションエスケップ

        //public string EscapeDoubleQuotation(string src)
        //{
        //    if (string.IsNullOrEmpty(src))
        //    {
        //        return string.Empty;
        //    }
        //    else
        //    {
        //        return src.Replace(CommConst.SINGLE_DOUBLE_QUOTATION, CommConst.DOUBLE_DOUBLE_QUOTATION);
        //    }
        //}

        #endregion ダブルコーテーションエスケップ

        #region 画像からBase64に変換

        /// <summary>
        /// 画像からBase64に変換
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ImageFromFileToBase64(string path)
        {
            using (var image = Image.FromFile(path))
            {
                using (var m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    return Convert.ToBase64String(m.ToArray());
                }
            }
        }

        #endregion 画像からBase64に変換

        #region 報告データマスタ更新API

        /// <summary>
        /// 報告データマスタ更新API
        /// </summary>
        /// <param name="masterId">マスタID</param>
        /// <param name="request"></param>
        public void ReqUpdReport(string formatId, string reportId, string request)
        {
            logger.Info("Utility#ReqUpdReportMaster() Start");
            // リクエストURL作成
            var resrcPath = string.Format(WebApiConst.PATH_UPDATE_REPORT, formatId, reportId);
            var url = CreateRequestUrl(resrcPath);
            try
            {
                // リクエスト作成
                var req = (HttpWebRequest)WebRequest.Create(url);
                CreateRequestConfig(req, WebApiConst.METHOD_PUT, request);
                // リクエスト情報(Method:URI)をログ出力
                // レスポンス取得
                var res = (HttpWebResponse)req.GetResponse();
                logger.Info(res.ResponseUri.AbsoluteUri.ToString());
                if (HttpStatusCode.OK.Equals(res.StatusCode))
                {
                    // レスポンス情報取得
                    using (var st = res.GetResponseStream())
                    {
                        // レスポンス情報出力
                        logger.Info(new StreamReader(st).ReadToEnd());
                    }
                }
                else
                {
                    // OK以外は処理失敗としてエラーとする
                    throw new HttpException(res.StatusCode);
                }
            }
            catch (WebException webEx)
            {
                Dictionary<string, object> errRes;
                // HTTPプロトコルエラーかどうか調べる
                var statusCd = ConvWebStatusToHttpStatus(webEx, out errRes);
                if (WebApiConst.NOT_HTTP_ERROR_CODE.Equals(statusCd))
                {
                    // 例外ログを出力
                    logger.Error(webEx.Message);
                    logger.Error(webEx.StackTrace);
                    throw;
                }
                else if (WebApiConst.NOT_HTTP_PROXY_CODE.Equals(statusCd))
                {
                    // ログ出力
                    logger.Error(webEx.Message);
                    logger.Error(webEx.StackTrace);
                    throw;
                }
                else
                {
                    // プロトコルエラー用のログ出力
                    if (errRes.ContainsKey(WebApiConst.ITEM_NAME_JSON_ERROR_TYPE))
                    {
                        // 登録/更新処理エラーログ
                        //OutputMessageErrorHttpUpload(webEx, errRes, statusCd);
                    }
                    else
                    {
                        // その他エラーログ
                        OutputMessageErrorHttp(webEx, errRes, statusCd);
                    }
                    throw new HttpException(statusCd);
                }
            }
            catch (HttpException apiEx)
            {
                OutputMessageErrorHttp(apiEx);
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                throw;
            }
            logger.Info("Utility#ReqUpdReportMaster() End");
        }

        #endregion 報告データマスタ更新API

        /// <summary>
        /// リクエストURLの作成
        /// </summary>
        /// <param name="resrcPath">処理毎のリソースパス</param>
        /// <returns>リクエストURL</returns>
        public string CreateRequestUrl(string resrcPath)
        {
            return string.Format(WebApiConst.URL, GetProtocol(), GetHostName(), GetWebApiVer(), resrcPath);
        }

        #region WebApi関連定義取得

        private static string GetProtocol()
        {
            var protocol = WebApiConst.URL_HTTP;
            if (WebApiConst.URL_HTTPS.ToLower().Equals(Config.AppSettings.Settings[WebApiConst.PROTOCOL].Value))
            {
                protocol = WebApiConst.URL_HTTPS;
            }
            return protocol;
        }

        private static string GetHostName()
        {
            return Config.AppSettings.Settings[WebApiConst.HOST_NAME].Value;
        }

        private static string GetWebApiVer()
        {
            return Config.AppSettings.Settings[WebApiConst.VER].Value;
        }

        private static bool IsProxyUser()
        {
            return WebApiConst.USE.Equals(Config.AppSettings.Settings[WebApiConst.PROXY_SERVER].Value);
        }

        private static string GetProxyAddress()
        {
            return Config.AppSettings.Settings[WebApiConst.PROXY_SERVER_ADDRESS].Value; ;
        }

        private static string GetProxyPort()
        {
            return Config.AppSettings.Settings[WebApiConst.PROXY_SERVER_PORT].Value; ;
        }

        private static string GetProxyUserId()
        {
            return Config.AppSettings.Settings[WebApiConst.PROXY_SERVER_USERID].Value; ;
        }

        private static string GetProxyPassword()
        {
            return Config.AppSettings.Settings[WebApiConst.PROXY_SERVER_PASSWORD].Value; ;
        }

        private static string GetAuthorizationKey()
        {
            return Config.AppSettings.Settings[WebApiConst.AUTHORIZATION_KEY].Value; ;
        }

        private static int GetRequestTimeout()
        {
            var requestTimeout = WebApiConst.REQUEST_TIMEOUT;
            var reqTimeout = Config.AppSettings.Settings[WebApiConst.STR_REQUEST_TIMEOUT].Value;
            if (!string.IsNullOrEmpty(reqTimeout))
            {
                requestTimeout = int.Parse(reqTimeout);
            }
            return requestTimeout;
        }

        #endregion WebApi関連定義取得

        /// <summary>
        /// HTTP処理中の例外発生時のログ出力
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="webEx">発生エラーオブジェクト</param>
        /// <param name="errRes">エラーレスポンス</param>
        /// <param name="errCode">エラー時のHttpStatusCode</param>
        private void OutputMessageErrorHttp(WebException webEx, Dictionary<string, object> errRes, int errCode)
        {
            logger.Error(CreateErrorMessage(errCode));
            // エラーレスポンスに規定のキーがあれば出力
            if (errRes.ContainsKey(WebApiConst.ITEM_NAME_JSON_ERROR_ID))
            {
                logger.Error(errRes[WebApiConst.ITEM_NAME_JSON_MESSAGE]);
            }
            // 例外を出力
            logger.Error(webEx.Message);    // メッセージ
            logger.Error(webEx.StackTrace); // スタックトレース
            logger.Error(webEx.Response.Headers);
            //OutputErrorResponse(errRes);
        }

        /// <summary>
        /// HTTP処理中の例外発生時のログ出力
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="webEx">発生エラーオブジェクト</param>
        /// <param name="errRes">エラーレスポンス</param>
        /// <param name="errCode">エラー時のHttpStatusCode</param>
        //private void OutputMessageErrorHttpUpload(WebException webEx, Dictionary<string, object> errRes, int errCode)
        //{
        //    // 例外を出力
        //    logger.Error(CreateErrorMessage(errCode));
        //    logger.Error(webEx.Message);    // メッセージ
        //    logger.Error(webEx.StackTrace); // スタックトレース
        //    OutputErrorResponse(errRes);   // エラーレスポンスの情報を出力
        //}

        /// <summary>
        /// HTTPエラー発生時のログ出力
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="apiEx">発生エラーオブジェクト</param>
        public void OutputMessageErrorHttp(HttpException apiEx)
        {
            logger.Error(CreateErrorMessage((int)apiEx.ErrCode));
        }

        /// <summary>
        /// エラーレスポンス情報出力
        /// </summary>
        /// <param name="res">取得したレスポンス</param>
        //public void OutputErrorResponse(Dictionary<string, object> res)
        //{
        //    foreach (KeyValuePair<string, object> dict in res)
        //    {
        //        // 取得したレスポンスの項目を全てログに出力する
        //        if (dict.Value is string)
        //        {
        //            logger.Error(string.Format(Utility.GetMsg(MsgConst.KK0020I), dict.Key, dict.Value));
        //        }
        //        else if (dict.Value is Dictionary<string, string>)
        //        {
        //            var work = (Dictionary<string, string>)dict.Value;
        //            foreach (KeyValuePair<string, string> dict2 in work)
        //            {
        //                logger.Error(string.Format(Utility.GetMsg(MsgConst.KK0020I) + "：{2}", dict.Key, dict2.Key, dict2.Value));
        //            }
        //        }
        //        else if (dict.Value is Dictionary<string, object>)
        //        {
        //            var work = (Dictionary<string, object>)dict.Value;
        //            OutputErrorResponse(work);
        //        }
        //    }
        //}

        /// <summary>
        /// ステータスコード変換メソッド
        /// </summary>
        /// <remarks>
        /// WebExceptionのステータスコードをHttpWebResponseのステータスコードに変換
        /// </remarks>
        /// <param name="logger">ロガー</param>
        /// <param name="webEx">WebException</param>
        /// <param name="statusCd">ステータスコード</param>
        /// <returns>エラーコード</returns>
        private int ConvWebStatusToHttpStatus(WebException webEx, out Dictionary<string, object> errRes)
        {
            var ret = WebApiConst.NOT_HTTP_ERROR_CODE;
            errRes = new Dictionary<string, object>();
            // プロトコルエラーか確認
            if (WebExceptionStatus.ProtocolError.Equals(webEx.Status))
            {
                var resEx = (HttpWebResponse)webEx.Response;
                if (WebApiConst.CONTENT_TYPE_JSON.Equals(resEx.ContentType))
                {
                    using (var st = resEx.GetResponseStream())
                    {
                        errRes = (Dictionary<string, object>)DynamicJson.Parse(new StreamReader(st).ReadToEnd());
                    }
                }
                // 数値変換
                ret = (int)resEx.StatusCode;
            }
            else
            {
                // エラーメッセージが『リモート名を解決できませんでした。』の場合は、
                // 専用エラーコードを設定する
                //if (webEx.Message.Contains(Utility.GetMsg(MsgConst.KK0016E)))
                //{
                //    ret = WebApiConst.NOT_HTTP_PROXY_CODE;
                //}
            }
            return ret;
        }

        /// <summary>
        /// エラー状況によるメッセージ作成
        /// </summary>
        /// <param name="code">HTTPエラーコード</param>
        /// <returns>出力するメッセージ</returns>
        private static string CreateErrorMessage(int code)
        {
            var statusCode = (HttpStatusCode)code;
            var msg = string.Empty;
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest: // 400
                    break;

                case HttpStatusCode.NotFound:   // 404
                    break;
                // 上記以外
                default:
                    msg = ReleaseMsg(code.ToString());
                    break;
            }
            return msg;
        }

        /// <summary>
        /// リクエスト設定の作成
        /// </summary>
        /// <param name="req">リクエスト</param>
        /// <param name="method">メソッド</param>
        /// <param name="request">ボディ部</param>
        private void CreateRequestConfig(HttpWebRequest req, string method, string request)
        {
            req.Timeout = GetRequestTimeout();
            req.Method = method;
            // 認証キー取得
            var apiKey = GetAuthorizationKey();
            if (!string.IsNullOrEmpty(apiKey))
            {
                // Base64エンコード
                var cnvKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey));
                // ヘッダに認証追加
                req.Headers.Add(string.Format(WebApiConst.HEADER_CERTIFICATE, cnvKey));
            }
            if (IsProxyUser())
            {
                // 認証情報設定
                var proxy = new WebProxy(GetProxyAddress(), int.Parse(GetProxyPort()));
                proxy.Credentials = new NetworkCredential(GetProxyUserId(), GetProxyPassword());

                req.Proxy = proxy;
            }
            else
            {
                // プロキシを使用しない
                req.Proxy = null;
            }
            // GET以外の場合はヘッダ/パラメータを設定する
            if (WebApiConst.METHOD_PUT.Equals(method))
            {
                req.ContentType = WebApiConst.CONTENT_TYPE_JSON;
                var data = Encoding.UTF8.GetBytes(request);
                req.ContentLength = data.Length;
                using (var stream = req.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
        }
    }
}