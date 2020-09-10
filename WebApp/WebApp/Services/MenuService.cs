using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using WebApp.Common;
using WebApp.Models;

namespace WebApp.Services
{
    public class MenuService
    {
        private AppConfig config;

        internal MenuService(AppConfig config)
        {
            this.config = config;
        }

        internal void Add(Menu menu)
        {
            //トランザクション開始
            using (TransactionScope scope = new TransactionScope())
            {
                //using (SqlConnection conn = new SqlConnection(DbHelper.getConnectionString(this.Configuration)))
                using (SqlConnection conn = new SqlConnection(config.ConnectionString))
                {
                    String sql = @"
                            INSERT INTO Menu(MenuId,Description,Unit)
                            VALUES (@MenuId,@Description,@Unit)
                            ";

                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sql))
                    {

                        command.Connection = conn;
                        //System.Diagnostics.Debug.WriteLine("----------------");
                        command.Parameters.AddWithValue("@MenuId", menu.MenuId);
                        command.Parameters.AddWithValue("@Description", menu.Description);
                        command.Parameters.AddWithValue("@Unit", menu.Unit);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (var audio in menu.AudioFiles)
                {
                    //using (SqlConnection conn = new SqlConnection(DbHelper.getConnectionString(this.Configuration)))
                    using (SqlConnection conn = new SqlConnection(config.ConnectionString))
                    {
                        String sql = @"
                            INSERT INTO AudioFile(MenuId,FileName,Description,S3Url)
                            VALUES (@MenuId,@FileName,@Description,@S3Url)
                            ";

                        conn.Open();
                        using (SqlCommand command = new SqlCommand(sql))
                        {

                            command.Connection = conn;
                            //System.Diagnostics.Debug.WriteLine("----------------");
                            command.Parameters.AddWithValue("@MenuId", menu.MenuId);
                            command.Parameters.AddWithValue("@FileName", audio.FileName);
                            command.Parameters.AddWithValue("@Description", audio.Description);
                            command.Parameters.AddWithValue("@S3Url", ""); //ここでは設定しない。バッチでやるか？
                            command.ExecuteNonQuery();
                        }
                    }
                }

                scope.Complete();
            }
        }

        internal Menu GetMenu(string menuId)
        {

            using (SqlConnection conn = new SqlConnection(config.ConnectionString))
            {

                conn.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;

                    command.Parameters.AddWithValue("@MenuId", menuId);
                    command.CommandText = @"
                                        SELECT
                                        MenuId, Description, Unit
                                        FROM Menu
                                        WHERE MenuId= @MenuId
                                        ";
                    Menu menu;
                    using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    { 
                        if (reader.Read())
                        {
                            menu = new Menu()
                            {
                                MenuId = reader.GetString(0),
                                Description = reader.GetSqlChars(1).ToString(),
                                Unit = reader.GetString(2)
                            };
                        }
                        else
                        {
                            menu = new Menu();
                        }
                    }

                    command.CommandText = @"
                                        SELECT
                                        MenuId, FileName, Description,S3Url
                                        FROM AudioFile
                                        WHERE MenuId= @MenuId
                                        ";

                    using (var reader = command.ExecuteReader())
                    {
                        menu.AudioFiles = new List<AudioFile>();

                        while (reader.Read())
                        {
                            menu.AudioFiles.Add(
                                new AudioFile()
                                {
                                    FileName = reader.GetString(1),
                                    Description = new string(reader.GetSqlChars(2).Value),
                                    S3Url = reader.GetString(3)
                                }
                            );
                        }
                    }
                    return menu;
                }
            }
        }

        internal void PutAudioFilesToS3(String MenuID, IFormFileCollection files)
        {
            using (var client = new AmazonS3Client())
            {
                // ファイルをS3にput
                foreach (var formFile in files)
                {
                    //using (var newMemoryStream = new MemoryStream())
                    using (var stream = formFile.OpenReadStream())
                    {
                        //formFile.CopyTo(newMemoryStream);
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = stream,
                            Key = formFile.FileName,
                            BucketName = "com.jurin-n.audio-files/webapp/"+ MenuID
                        };

                        var fileTransferUtility = new TransferUtility(client);
                        fileTransferUtility.Upload(uploadRequest);
                    }
                }
            }
        }
    }
}
