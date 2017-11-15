using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RenkeiCommon.Const;
using RenkeiCommon.Mapping;
using log4net;
using System.Data;
using System.IO;
using System.Reflection;
using Codeplex.Data;
using System.Dynamic;
using Newtonsoft.Json;

namespace RenkeiCommon
{
    public class Renkei
    {
        #region 変数定義

        /// <summary>
        /// バッチ引数
        /// </summary>
        private string[] Args;

        /// <summary>
        /// Utility
        /// </summary>
        public Utility util;

        /// <summary>
        /// イベント種類
        /// </summary>
        private string ActionType;

        /// <summary>
        /// 報告書フォーマットID
        /// </summary>
        private string FormatId;

        /// <summary>
        /// 報告書No
        /// </summary>
        private string ReportNo;

        /// <summary>
        /// トランザクションマッピング
        /// </summary>
        private ReportMapping reportMapping;

        /// <summary>
        /// ロガー
        /// </summary>
        public ILog logger;

        /// <summary>
        /// 追加クラスのアセンブリ
        /// </summary>
        private static Assembly assembly;

        /// <summary>
        /// コードマスタ会社別
        /// </summary>
        private static Dictionary<string, List<CodeKaishaBetu>> codeKaishaBetuDic = new Dictionary<string, List<CodeKaishaBetu>>();

        /// <summary>
        /// トランザクションマッピングデータ
        /// </summary>
        private static List<ReportMapping> reportMappingData;

        /// <summary>
        /// ベースデータ
        /// </summary>
        private List<BaseColumns> baseData = new List<BaseColumns>();

        /// <summary>
        /// レポートデータ
        /// </summary>
        private List<ReportColumns> reportData = new List<ReportColumns>();

        /// <summary>
        /// 画像データ
        /// </summary>
        private List<ImageColumns> imageData = new List<ImageColumns>();

        /// <summary>
        /// リクエストJson
        /// </summary>
        private List<JsonObject> requestJsonDataLst = new List<JsonObject>();

        #endregion 変数定義

        #region コンストラクタ

        private Renkei()
        { }

        public Renkei(string[] args)
        {
            Args = args;
        }

        #endregion コンストラクタ

        // 快作ビジネス＋→快作レポート
        public void Run(string[] args)
        {
            logger.Info("Renkei#Run() Start");
            try
            {
                // マッピングJson解析
                if (reportMappingData == null)
                {
                    reportMappingData = util.JsonParse(CommConst.MASTER_REPORT_JSON);
                }
                //快作情報カラム名取得
                var reportInfoLst = reportMappingData.Where(x => x.FormatId.Equals(args[0])).Select(x => x).ToList();
                if (reportInfoLst.Count >= 2)
                {
                    logger.Error(Utility.ReleaseMsg(args[0]));
                    throw new Exception();
                }
                reportMapping = reportInfoLst.FirstOrDefault();
                //快作情報データ取得
                var dt = util.GetKayisakuData(args, reportMapping.Condition);
                // マスタマッピングある
                if (reportMapping != null)
                {
                    // 基幹データ元に顧客連携
                    ReportRenkei(dt);
                }
                logger.Debug("Renkei#GetMasterMapping() End");
                //報告データマスタ更新API
                util.ReqUpdReport(args[0], args[1], CreateReportJsonData());
            }
            catch (Exception ex)
            {
                throw;
            }
            logger.Info("Renkei#Run() End");
        }

        //快作からデータ連携
        private void ReportRenkei(DataTable dt)
        {
            var jsonReportDataLst = new List<JsonObject>();
            //reportMapping.ReportInfo.Where(x=>x.BaseData)
            if (dt.Rows.Count > 0)
            {
                var colNmLst = dt.Columns.Cast<DataColumn>().Select(row => row.ColumnName).ToList();
                foreach (DataColumn col in dt.Columns)
                {
                    var Basedata = reportMapping.ReportInfo.BaseData.Where(x => x.ColumnNm.Equals(col.ColumnName)).ToList();
                    var Repotdata = reportMapping.ReportInfo.ReportData.Where(q => q.ColumnNm.Equals(col.ColumnName)).ToList();
                    if (Basedata.ToList().Count > 0)
                    {
                        Basedata.ForEach(y =>
                        {
                            y.Value = dt.Rows[0][col.ColumnName];
                            requestJsonDataLst.Add(new JsonObject { Key = y.RefReportDataKey, Value = y.Value });
                        });
                    }
                    if (Repotdata.ToList().Count > 0)
                    {
                        Repotdata.ForEach(z =>
                        {
                            z.Value = dt.Rows[0][col.ColumnName];
                            jsonReportDataLst.Add(new JsonObject { Key = z.RefReportDataKey, Value = z.Value });
                        });
                    }
                }
                if (jsonReportDataLst != null)
                {
                    requestJsonDataLst.Add(new JsonObject { Key = "report_data", Value = jsonReportDataLst.ToDictionary(dic => dic.Key, dic => dic.Value) });
                }
                if (reportMapping.ReportInfo.ImageData != null && reportMapping.ReportInfo.ImageData.Where(x => x.Count == 2).Count() > 0)
                {
                    imageData = reportMapping.ReportInfo.ImageData.Where(x => x.Count == 2).Select(x => x.Select(y => y)).SelectMany(z => z).ToList();
                }

                CreateImagesData(dt, colNmLst);
            }
        }

        //C#反射给字段赋值
        public void SetObjectValue(object obj, string fieldname, object value)
        {
            Type type = obj.GetType();
            FieldInfo[] fieldInfos = type.GetFields();
            obj.GetType().GetProperty(fieldname).SetValue(obj, value);
        }

        #region 報告データマスタ更新Json作成

        /// <summary>
        /// 報告データマスタ更新Json作成
        /// </summary>
        /// <param name="masterName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string CreateReportJsonData()
        {
            logger.Debug("Renkei#CreateReportJsonData() Start");
            var rtn = string.Empty;
            // リクエストデータ作成
            if (requestJsonDataLst.Count > 0)
            {
                rtn = JsonConvert.SerializeObject(requestJsonDataLst.ToDictionary(x => x.Key, x => x.Value));
            }

            logger.Debug("Renkei#CreateReportJsonData() End");
            return rtn;
        }

        #endregion 報告データマスタ更新Json作成

        #region 報告書の添付画像配列作成

        /// <summary>
        /// 報告書の添付画像配列作成
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colNmLst"></param>
        private void CreateImagesData(DataTable dt, List<string> colNmLst)
        {
            var jsonImageDataLst = new List<JsonObject>();
            var imageFileName = string.Empty;
            imageData.ForEach(z =>
            {
                if (!string.IsNullOrEmpty(z.RefReportDataKey) && colNmLst.Contains(z.ColumnNm) && z.RefReportDataKey.Equals("name"))
                {
                    z.Value = dt.Rows[0][z.ColumnNm];
                    if (z.Value != null)
                    {
                        imageFileName = z.Value.ToString();
                    }
                }
                else if (z.RefReportDataKey.Equals("data"))
                {
                    if (!string.IsNullOrEmpty(reportMapping.RootPath) && !string.IsNullOrEmpty(z.ColumnNm))
                    {
                        if (colNmLst.Contains(z.ColumnNm))
                        {
                            if (dt.Rows[0][z.ColumnNm] != null && !string.IsNullOrEmpty(dt.Rows[0][z.ColumnNm].ToString()))
                            {
                                var path = reportMapping.RootPath + CommConst.YEN_MARK + dt.Rows[0][z.ColumnNm] + CommConst.YEN_MARK + imageFileName;
                                if (File.Exists(path))
                                {
                                    z.Value = util.ImageFromFileToBase64(path);
                                    imageFileName = string.Empty;
                                }
                            }
                        }
                    }
                }
                //else
                //{
                //    z.Value = z.DefaultValue;
                //}
                jsonImageDataLst.Add(new JsonObject { Key = z.RefReportDataKey, Value = z.Value });
            });
            if (jsonImageDataLst.Count > 0)
            {
                var imageDataLst = new List<Dictionary<string, object>>();
                for (var idx = 0; idx < jsonImageDataLst.Count; idx++)
                {
                    var imageData = jsonImageDataLst.Skip(idx * 2).Take(2).ToList();
                    if (imageData[0].Value != null && !string.IsNullOrEmpty(imageData[0].Value.ToString())
                        && imageData[1].Value != null && !string.IsNullOrEmpty(imageData[1].Value.ToString()))
                    {
                        imageDataLst.Add(imageData.ToDictionary(dic => dic.Key, dic => dic.Value));
                    }
                    if ((idx + 1) * 2 >= jsonImageDataLst.Count)
                    {
                        break;
                    }
                }

                if (imageDataLst.Count > 0)
                {
                    requestJsonDataLst.Add(new JsonObject { Key = "images", Value = imageDataLst });
                }
            }
        }

        #endregion 報告書の添付画像配列作成
    }
}