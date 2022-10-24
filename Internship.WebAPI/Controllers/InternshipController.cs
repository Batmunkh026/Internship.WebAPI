using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Internship.Utils.DBConnection;
using Internship.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Internship.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class InternshipController : ControllerBase
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = false,
        };
        DBConnection dbconn;
        private readonly AppConfig _settings;
        public InternshipController(IConfiguration configuration)
        {
            AppConfig options = configuration.GetSection("Config").Get<AppConfig>();
            _settings = options;
            dbconn = new DBConnection(options.dbhost, options.schema);
        }

        /// <summary>
        /// get Last 10 smart cards
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<ResponseModel>> Get()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (dbconn.idbStatOK())
                {
                    DataTable dt = dbconn.getTable($"select card_no from ddish_cards where ROWNUM <=10");
                    if(dt.Rows.Count != 0)
                    {
                        List<SmartCard> _cards = new List<SmartCard>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            _cards.Add(new SmartCard { cardNo = dr["card_no"].ToString() });
                            //SmartCard crd = new SmartCard();
                            //crd.cardNo = dr["card_no"].ToString();
                            //_cards(crd);
                        }
                        response.isSuccess = true;
                        response.cards = _cards;
                    }
                    else
                    {
                        response.errorCode = Convert.ToString((int)HttpStatusCode.NotFound);
                        response.resultMessage = "Data not found !!!";
                    }
                }
                else
                {
                    response.errorCode = Convert.ToString((int)HttpStatusCode.InternalServerError);
                    response.resultMessage = "Internal Error";
                }
            }
            catch(Exception ex)
            {
                response.errorCode = Convert.ToString((int)HttpStatusCode.InternalServerError);
                response.resultMessage = ex.Message;
            }
            return Ok(response);
        }

        /// <summary>
        /// V2 get Last 10 smart cards
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ApiVersion("2.0")]
        public async Task<ActionResult<ResponseModel>> GetV2()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (dbconn.idbStatOK())
                {
                    DataTable dt = dbconn.getTable($"select card_no from ddish_cards where ROWNUM <={_settings.rowNumber}");
                    if (dt.Rows.Count != 0)
                    {
                        List<SmartCard> _cards = new List<SmartCard>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            _cards.Add(new SmartCard { cardNo = dr["card_no"].ToString() });
                        }
                        response.isSuccess = true;
                        response.cards = _cards;
                    }
                    else
                    {
                        response.errorCode = Convert.ToString((int)HttpStatusCode.NotFound);
                        response.resultMessage = "Data not found !!!";
                    }
                }
                else
                {
                    response.errorCode = Convert.ToString((int)HttpStatusCode.InternalServerError);
                    response.resultMessage = "Internal Error";
                }
            }
            catch (Exception ex)
            {
                response.errorCode = Convert.ToString((int)HttpStatusCode.InternalServerError);
                response.resultMessage = ex.Message;
            }
            return Ok(response);
        }
    }
}
