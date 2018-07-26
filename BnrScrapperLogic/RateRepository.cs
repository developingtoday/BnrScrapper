using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BnrScrapperLogic
{
    public class RateRepository : IRateRepository
    {
        private readonly string connection;

        public RateRepository(string conn)
        {
            connection = conn;
        }

        public void InsertBatchEuroRonRate(List<EuroRonRate> rates)
        {
            using (var conn=new SqlConnection(this.connection))
            {
                var adapter = new SqlDataAdapter("select r.* from dbo.EuroRonRates r", conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                adapter.InsertCommand = new SqlCommand();
                dt.PrimaryKey = new[] { dt.Columns["RateDate"] };
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                var cmdbuild = builder.GetInsertCommand(true);
                cmdbuild.UpdatedRowSource = UpdateRowSource.None;
                var cmdBuildDelete = builder.GetDeleteCommand(true);
                cmdBuildDelete.UpdatedRowSource = UpdateRowSource.None;
                var cmdBuildUpdate = builder.GetUpdateCommand(true);
                adapter.InsertCommand = cmdbuild;
                adapter.UpdateCommand = cmdBuildUpdate;
                adapter.DeleteCommand = cmdBuildDelete;
                cmdBuildUpdate.UpdatedRowSource = UpdateRowSource.None;
                adapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                adapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
               // adapter.UpdateBatchSize = 1000;
                foreach (var item in rates)
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
                    row["Value"] = item.Valoare;
                    if (insert) dt.Rows.Add(row);
                }

                adapter.Update(dt);
            }
        }

        public List<RoborHistoric> GetRobors(DateTime from, DateTime to)
        {
            using (var sqlConn = new SqlConnection(this.connection))
            {
                using (var cmd=sqlConn.CreateCommand())
                {
                    cmd.CommandText = "select r.* from dbo.Rates r where @from<=r.RateDate and r.RateDate<=@to";
                    cmd.Parameters.Add("@from", SqlDbType.DateTime).Value=from;
                    cmd.Parameters.Add("@to", SqlDbType.DateTime).Value=to;
                    sqlConn.Open();
                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            var lst = new List<RoborHistoric>();
                            while (reader.Read())
                            {
                               
                                var obj = new RoborHistoric();
                                obj.Data = reader.GetDateTime(reader.GetOrdinal("RateDate"));
                                obj.Robid12M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robid12M)));
                                obj.Robor12M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robor12M)));
                                obj.Robid3M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robid3M)));
                                obj.Robor3M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robor3M)));
                                obj.Robid6M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robid6M)));
                                obj.Robor6M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robor6M)));
                                obj.Robid9M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robid9M)));
                                obj.Robor9M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robor9M)));
                                lst.Add(obj);
                                
                            }
                            return lst;
                        }
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                }
            }
        }

        public RoborHistoric GetRoborRecent()
        {
            using (var sqlConn = new SqlConnection(this.connection))
            {
                using (var cmd = sqlConn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Top 1 x.* from dbo.Rates x order by x.RateDate desc";
                    sqlConn.Open();
                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            var lst = new List<RoborHistoric>();
                            while (reader.Read())
                            {

                                var obj = new RoborHistoric();
                                obj.Data = reader.GetDateTime(reader.GetOrdinal("RateDate"));
                                obj.Robid12M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robid12M)));
                                obj.Robor12M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robor12M)));
                                obj.Robid3M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robid3M)));
                                obj.Robor3M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robor3M)));
                                obj.Robid6M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robid6M)));
                                obj.Robor6M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robor6M)));
                                obj.Robid9M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robid9M)));
                                obj.Robor9M = reader.GetDecimal(reader.GetOrdinal(nameof(obj.Robor9M)));
                                lst.Add(obj);

                            }
                            return lst.FirstOrDefault()??new RoborHistoric();
                        }
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                }
            }
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
                cmdbuild.UpdatedRowSource = UpdateRowSource.None;
                var cmdBuildDelete = builder.GetDeleteCommand(true);
                cmdBuildDelete.UpdatedRowSource = UpdateRowSource.None;
                var cmdBuildUpdate = builder.GetUpdateCommand(true);
                cmdBuildUpdate.UpdatedRowSource = UpdateRowSource.None;
                //adapter.InsertCommand = builder.GetInsertCommand(true);
                adapter.UpdateBatchSize = 1;
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
                adapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                adapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
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
