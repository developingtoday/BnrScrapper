using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BnrScrapper
{
    public class RateRepository
    {
        private readonly string connection;

        public RateRepository(string conn)
        {
            connection = conn;
        }

        public void InsertBatch(List<RoborHistoric> historics)
        {
            using (var conn = new SqlConnection(this.connection))
            {
                var adapter = new SqlDataAdapter("select r.* from dbo.Rates r", conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                adapter.InsertCommand = new SqlCommand();
                dt.PrimaryKey = new[] { dt.Columns["RateDate"] };
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                var cmdbuild = builder.GetInsertCommand(true);
                var cmdBuildDelete = builder.GetDeleteCommand(true);
                var cmdBuildUpdate = builder.GetDeleteCommand(true);
                //adapter.InsertCommand = builder.GetInsertCommand(true);
                //adapter.UpdateBatchSize = 10000;
                adapter.InsertCommand.Connection = conn;
                adapter.InsertCommand.Parameters.Add("@RateDate", SqlDbType.Date).SourceColumn="RateDate";
                adapter.InsertCommand.Parameters.Add("@Robid3M", SqlDbType.Decimal).SourceColumn = "Robid3M";
                adapter.InsertCommand.Parameters.Add("@Robid6M", SqlDbType.Decimal).SourceColumn = "Robid6M";
                adapter.InsertCommand.Parameters.Add("@Robid9M", SqlDbType.Decimal).SourceColumn = "Robid9M";
                adapter.InsertCommand.Parameters.Add("@Robid12M", SqlDbType.Decimal).SourceColumn = "Robid12M";
                adapter.InsertCommand.Parameters.Add("@Robor3M", SqlDbType.Decimal).SourceColumn = "Robor3M";
                adapter.InsertCommand.Parameters.Add("@Robor6M", SqlDbType.Decimal).SourceColumn = "Robor6M"; 
                adapter.InsertCommand.Parameters.Add("@Robor9M", SqlDbType.Decimal).SourceColumn = "Robor9M";
                adapter.InsertCommand.Parameters.Add("@Robor12M", SqlDbType.Decimal).SourceColumn = "Robor12M";
                //adapter.InsertCommand.CommandText = "insert into dbo.Rates values (@RateDate,@Robid3M,@Robid6M,@Robid9M,@Robid12M,@Robor3M,@Robor6M,@Robor9M,@Robor12M)";
                adapter.InsertCommand.CommandText = cmdbuild.CommandText;
                adapter.UpdateCommand = cmdBuildUpdate;
                adapter.DeleteCommand = cmdBuildDelete;
                foreach (var item in historics)
                {
                    DataRow row;
                    var insert = false;
                    if (!dt.Rows.Contains(item.Data))
                    {
                        insert = true;
                        row = dt.NewRow();
                       
                    }
                    else
                    {
                        row = dt.Rows.Find(item.Data);
                    }

                    row["RateDate"] = item.Data;
                    row[nameof(item.Robid3M)] = item.Robid3M;
                    row[nameof(item.Robid6M)] = item.Robid6M;
                    row[nameof(item.Robid9M)] = item.Robid9M;
                    row[nameof(item.Robid12M)] = item.Robid12M;
                    row[nameof(item.Robor3M)] = item.Robor3M;
                    row[nameof(item.Robor6M)] = item.Robor6M;
                    row[nameof(item.Robor9M)] = item.Robor9M;
                    row[nameof(item.Robor12M)] = item.Robor12M;
                    if(insert) dt.Rows.Add(row);
                }

                adapter.Update(dt);
            }
        }
    }
}
