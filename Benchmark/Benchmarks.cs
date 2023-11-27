using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Microsoft.SemanticKernel.Connectors.Memory.Postgres;
using Microsoft.SemanticKernel.Memory;
using Npgsql;

[Config(typeof(Config))]
[MemoryDiagnoser]
public class Benchmarks {
    private const string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=sk-pg";
    private static PostgresMemoryStore store;
    private static List<MemoryRecord> records;

    [GlobalSetup]
    public async Task Setup() {
        await using (var con = new NpgsqlConnection(ConnectionString))
        {
            await con.OpenAsync();
            await using var cmd = con.CreateCommand();
            cmd.CommandText = "CREATE EXTENSION IF NOT EXISTS vector;";
            await cmd.ExecuteNonQueryAsync();
        }

        store = new PostgresMemoryStore(ConnectionString, 512);
        await store.DeleteCollectionAsync("test");
        await store.CreateCollectionAsync("test");
        var vec = CreateVector();
        var vecMem = vec.AsMemory();
        records = Enumerable.Range(0, 1000).Select(i => CreateRecord(i, vecMem)).ToList();
    }

    private float[] CreateVector() {
        return Enumerable.Range(0, 512).Select(i => (float) i).ToArray();
    }

    private MemoryRecord CreateRecord(int i, ReadOnlyMemory<float> vec)
        => MemoryRecord.LocalRecord(i.ToString(), i.ToString(), null, vec);

    [Benchmark]
    public async Task Insert() {
        await foreach (var key in store.UpsertBatchAsync("test", records)) {
        }
    }

    private class Config : ManualConfig
    {
        public Config()
        {
            var baseJob = Job.Default;
            var beforeJob = baseJob.WithId("Before");
            var afterJob = baseJob.WithCustomBuildConfiguration("LocalBuild").WithId("After");
            AddJob(beforeJob);
            AddJob(afterJob);
        }
    }
}
