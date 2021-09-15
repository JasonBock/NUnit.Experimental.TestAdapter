## `GeneratorTests` with driver creation and execution

|   Method |     Mean |    Error |   StdDev |   Median | Allocated |
|--------- |---------:|---------:|---------:|---------:|----------:|
| Generate | 792.7 us | 100.4 us | 288.0 us | 673.7 us |     51 KB |

## `GeneratorTests` with driver execution

|   Method |     Mean |    Error |   StdDev |   Median | Allocated |
|--------- |---------:|---------:|---------:|---------:|----------:|
| Generate | 712.6 us | 72.27 us | 202.7 us | 639.8 us |     51 KB |

## `GeneratorTests` comparing driver execution with a generator that does nothing

|            Method |       Mean |     Error |    StdDev |   Gen 0 |  Gen 1 | Allocated |
|------------------ |-----------:|----------:|----------:|--------:|-------:|----------:|
|          Generate | 132.490 us | 2.5039 us | 3.5910 us | 10.0098 | 2.4414 |     41 KB |
| GenerateDoNothing |   3.128 us | 0.0306 us | 0.0256 us |  0.3319 |      - |      1 KB |

...or...

|            Method |      Mean |     Error |    StdDev |   Gen 0 |  Gen 1 | Allocated |
|------------------ |----------:|----------:|----------:|--------:|-------:|----------:|
|          Generate | 82.132 us | 1.6387 us | 1.5329 us | 10.0098 | 2.4414 |     41 KB |
| GenerateDoNothing |  1.997 us | 0.0372 us | 0.0348 us |  0.3319 |      - |      1 KB |