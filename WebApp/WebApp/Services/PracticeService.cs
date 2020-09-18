using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        internal IList<Practice> GetPractices()
        {
            using (SqlConnection conn = new SqlConnection(appConfig.ConnectionString))
            {

                conn.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"
                                        SELECT
                                             p.PracticeId
                                            ,p.DateTimeOfImplementation
                                            ,p.MenuId
                                            ,p.ValueOfUnit
                                            ,m.Unit
                                            ,m.Description
                                        FROM Practice p
                                        INNER JOIN Menu m
                                        ON p.MenuId = m.MenuId
                                        ORDER BY p.DateTimeOfImplementation DESC
                                        ";

                    var practices = new List<Practice>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            practices.Add(
                                new Practice()
                                {
                                    Id = reader.GetString(0),
                                    DateTimeOfImplementation = reader.GetDateTimeOffset(1),
                                    MenuId = reader.GetString(2),
                                    ValueOfUnit = reader.GetInt32(3),
                                    Unit = reader.GetString(4),
                                    Description = new string(reader.GetSqlChars(5).Value)
                                }
                            );
                        }
                    }
                    return practices;
                }
            }
        }


        internal IList<MenuLog> GetMenuLogs(String MenuId, String Start, String End)
        {
            using (SqlConnection conn = new SqlConnection(appConfig.ConnectionString))
            {

                conn.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"
                                        SELECT 
                                          m.MenuId
                                         ,p.DateTimeOfImplementation
                                         ,p.ValueOfUnit
                                         ,m.Unit
                                        FROM Menu m
                                        INNER JOIN Practice p
                                        ON
                                          m.MenuId = p.MenuId
                                        WHERE m.MenuId=@MenuId
                                          AND FORMAT(p.DateTimeOfImplementation, 'yyyy-MM-dd') >= @Start
                                          AND FORMAT(p.DateTimeOfImplementation, 'yyyy-MM-dd') <= @End
                                        ORDER BY p.DateTimeOfImplementation
                                        ";
                    command.Parameters.AddWithValue("@MenuId", MenuId);
                    command.Parameters.AddWithValue("@Start", Start);
                    command.Parameters.AddWithValue("@End", End);

                    var menuLogs = new List<MenuLog>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuLogs.Add(
                                new MenuLog()
                                {
                                    MenuId = reader.GetString(0),
                                    DateTimeOfImplementation = reader.GetDateTimeOffset(1),
                                    ValueOfUnit = reader.GetInt32(2),
                                    Unit = reader.GetString(3)
                                }
                            );
                        }
                    }
                    return menuLogs;
                }
            }
        }
    }
}
