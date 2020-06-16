using Amazon;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.S3.Util;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Npgsql;
using S3ToPostgresDataPipeline.Database;
using System;
using System.IO;
using System.Linq;
using System.Threading;

// Assembly attribute to enable the Lambda function's JSON input to be
// converted into a .NET class.
[assembly: LambdaSerializer(
    typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace S3ToPostgresDataPipeline
{
    public static class Program
    {
        public static void Main()
        {
        }
    }

    public class Handler
    {
        private IDatabaseActions _databaseActions;
        
        public Handler(IDatabaseActions databaseActions)
        {
            _databaseActions = databaseActions;
        }

        public Handler() : this(new DatabaseActions()) { }

        public void LoadCsv(S3EventNotification s3Event, ILambdaContext context)
        {
            LambdaLogger.Log("Processing request started");
            try
            {
                var connection = _databaseActions.SetupDatabase(context);
                foreach (var record in s3Event.Records)
                {
                   LambdaLogger.Log("Inside of the s3 events loop");
                    var s3 = record.S3;
                    try
                    {
                        //truncate correct table
                        _databaseActions.TruncateTable(context,Environment.GetEnvironmentVariable("DB_TABLE_NAME"));
                        // load csv data into table
                        _databaseActions.CopyDataToDatabase(context, record.AwsRegion, s3.Bucket.Name, s3.Object.Key);                   
                    }
                    catch (NpgsqlException ex)
                    {
                        LambdaLogger.Log($"Npgsql Exception has occurred - {ex.Message} {ex.InnerException} {ex.StackTrace}");
                        throw ex;
                    }
                    //close db connection
                    connection.Close();
                    LambdaLogger.Log("End of function");
                }
            }
            catch(Exception ex)
            {
                LambdaLogger.Log($"Exception has occurred - {ex.Message} {ex.InnerException} {ex.StackTrace}");
                throw ex;
            }
        }
    }
}
