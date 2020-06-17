using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Npgsql;

namespace S3ToPostgresDataPipeline.Database
{
    public interface IDatabaseActions
    {
        void TruncateTable(ILambdaContext context, string tableName);
        int CopyDataToDatabase(ILambdaContext context, string awsRegion, string bucketName, string objectKey);
        NpgsqlConnection SetupDatabase(ILambdaContext context);
    }
}
