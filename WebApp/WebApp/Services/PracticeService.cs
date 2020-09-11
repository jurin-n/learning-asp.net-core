using System;
using System.Data.SqlClient;
using System.Transactions;
using WebApp.Models;

namespace WebApp.Services
{
    public class PracticeService
    {
        private AppConfig appConfig;

        public PracticeService(AppConfig appConfig)
        {
            this.appConfig = appConfig;
        }

        public void Add(Practice practice)
        {
            //トランザクション開始
            using (TransactionScope scope = new TransactionScope())
            {
                //using (SqlConnection conn = new SqlConnection(DbHelper.getConnectionString(this.Configuration)))
                using (SqlConnection conn = new SqlConnection(appConfig.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandText = @"
                            INSERT INTO Practice(PracticeId,DateTimeOfImplementation,MenuId,ValueOfUnit)
                            VALUES(@PracticeId,@DateTimeOfImplementation,@MenuId,@ValueOfUnit)
                        ";
                        command.Parameters.AddWithValue("@PracticeId", practice.Id);
                        command.Parameters.AddWithValue("@DateTimeOfImplementation", practice.DateTimeOfImplementation);
                        command.Parameters.AddWithValue("@MenuId", practice.MenuId);
                        command.Parameters.AddWithValue("@ValueOfUnit", practice.ValueOfUnit);
                        command.ExecuteNonQuery();
                    }
                }
                scope.Complete();
            }
        }
    }
}
