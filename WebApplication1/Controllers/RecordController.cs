using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IConfiguration _config;
        public RecordController(IConfiguration configurations)
        {
            _config = configurations;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                    select RecordId as ""RecordId"",
                        RecordName as ""RecordName"" ,
                        Country as ""Country"",
                        RecordDate as ""RecordDate"" ,
                        DepartmentName as ""DepartmentName"" ,
                        TransactionType as ""TransactionType"" ,
                        RecordStatus as ""RecordStatus"" ,
                        ClientName as ""ClientName"" ,
                        ClientEmail as ""ClientEmail"" 
                    from Record
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _config.GetConnectionString("RecordAppcon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(NpgsqlCommand mycommand=new NpgsqlCommand(query, mycon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);

        }
    }
}
