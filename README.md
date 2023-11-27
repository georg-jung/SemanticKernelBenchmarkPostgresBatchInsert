# Implement proper PostgreSQL batch insert in `SemanticKernel.Connectors.Memory.Postgres`

See <https://github.com/microsoft/semantic-kernel/issues/3705>.

## Results

Results from a GitHub Actions runner can be seen [here](https://github.com/georg-jung/SemanticKernelBenchmarkPostgresBatchInsert/actions/runs/7005427316/job/19055135965).

<details>
<summary>GitHub Actions benchmark results (10x speedup)</summary>

    // * Summary *

    BenchmarkDotNet v0.13.10, Ubuntu 22.04.3 LTS (Jammy Jellyfish)
    AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
    .NET SDK 8.0.100
      [Host] : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
      After  : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
      Before : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method | Job    | BuildConfiguration | Mean      | Error     | StdDev   | Median    | Allocated  |
|------- |------- |------------------- |----------:|----------:|---------:|----------:|-----------:|
| Insert | After  | LocalBuild         |  51.03 ms |  1.418 ms | 4.069 ms |  49.41 ms |  492.39 KB |
| Insert | Before | Default            | 556.18 ms | 10.228 ms | 9.567 ms | 555.52 ms | 4914.77 KB |

    // * Warnings *
    MultimodalDistribution
      Benchmarks.Insert: After -> It seems that the distribution can have several modes (mValue = 2.89)
</details>

I also performed these benchmarks on my local machine, where the impact was even bigger:

<details>
<summary>Local benchmark results (47x speedup)</summary>

    // * Summary *

    BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2715/23H2/2023Update/SunValley3)
    AMD Ryzen 7 PRO 4750U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
    .NET SDK 8.0.100
      [Host] : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
      After  : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
      Before : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method | Job    | BuildConfiguration | Mean       | Error     | StdDev    | Gen0      | Allocated  |
|------- |------- |------------------- |-----------:|----------:|----------:|----------:|-----------:|
| Insert | After  | LocalBuild         |   121.0 ms |   3.52 ms |  10.15 ms |         - |  497.16 KB |
| Insert | Before | Default            | 5,761.0 ms | 114.58 ms | 140.72 ms | 2000.0000 | 4994.88 KB |

    // * Warnings *
    MultimodalDistribution
      Benchmarks.Insert: After -> It seems that the distribution can have several modes (mValue = 3)

</details>
