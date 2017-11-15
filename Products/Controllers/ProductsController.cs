using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Products.Models;
using RenkeiCommon;
using RenkeiCommon.Const;
using log4net;

namespace Products.Controllers
{
    public class ProductsController : ApiController
    {
        #region 変数定義

        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion 変数定義

        private Product[] products = new Product[]
      {
            new Product { to_reportplus="1",action_type="C",FormatId = "1", ReportId = "Tomato Soup" },
            new Product {  to_reportplus="1",action_type="C",FormatId = "2", ReportId = "Yo-yo" },
      };

        [HttpPost]
        public IEnumerable<Product> GetAllProducts([FromBody] Product Request)
        {
            try
            {
                var args = new string[] { Request.FormatId, Request.ReportId, Request.to_reportplus, Request.action_type, Request.HokokushoId };
                var renkei = new Renkei(args);
                renkei.util = new Utility(logger);
                renkei.logger = logger;
                renkei.Run(args);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return products;
        }

        [HttpGet]
        public IEnumerable<Product> Get([FromBody] Product Request)
        {
            return products;
        }

        [HttpPut]
        public IHttpActionResult Product(Product ProductRequest)
        {
            //var product = products.FirstOrDefault((p) => p.Id == format_id);
            //if (product == null)
            //{
            //    return NotFound();
            //}

            return Ok(ProductRequest);
        }

        [HttpDelete]
        public IHttpActionResult InputProduct(Product ProductRequest)
        {
            var product = products.FirstOrDefault((p) => p.FormatId == ProductRequest.FormatId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpOptions]
        public string Options()

        {
            return null; // HTTP 200 response with empty body
        }
    }
}