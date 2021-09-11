## `DiscovererTests`

|              Method |        Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Allocated |
|-------------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|
|  HardCodedDiscovery |    596.3 ns |  11.72 ns |  12.03 ns |  1.00 |    0.00 | 0.3386 |      1 KB |
| ReflectionDiscovery | 38,240.4 ns | 705.46 ns | 589.09 ns | 64.20 |    1.80 | 2.1362 |      9 KB |

## `ExecutorTests`

|              Method |    Mean |    Error |   StdDev | Ratio | Allocated |
|-------------------- |--------:|---------:|---------:|------:|----------:|
|  HardCodedExecution | 5.028 s | 0.0093 s | 0.0087 s |  1.00 |      4 KB |
| ReflectionExecution | 5.032 s | 0.0088 s | 0.0082 s |  1.00 |     47 KB |

## `ExecutorTests` - 1 10ms test

|              Method |     Mean |    Error |   StdDev | Ratio | Allocated |
|-------------------- |---------:|---------:|---------:|------:|----------:|
|  HardCodedExecution | 15.48 ms | 0.069 ms | 0.064 ms |  1.00 |      1 KB |
| ReflectionExecution | 15.45 ms | 0.047 ms | 0.044 ms |  1.00 |      7 KB |