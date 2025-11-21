# Library API (sample)

Prereqs:
- .NET 7+ SDK (or .NET 8)
- (Optional) SQLite CLI for inspection, not required.

Projects:
- Library.Api (HTTP + gRPC host)
- Library.Service (gRPC service; can be combined with Api for demo)
- Library.Core (shared models)
- Library.Tests (xUnit)

Run locally:
1. cd Library.Api
2. dotnet run

This will:
- Create `library.db` SQLite database in the working folder (if not exists)
- Seed sample data
- Start gRPC server and HTTP API

Sample HTTP endpoints:
GET https://localhost:7145/api/analytics/most-borrowed?limit=5
GET https://localhost:7145/api/analytics/top-borrowers?limit=5
GET https://localhost:7145/api/analytics/reading-pace?userId=1&bookId=1
GET https://localhost:7145/api/analytics/also-borrowed/1?limit=5

Testing:
From repo root:
dotnet test

Notes:
- gRPC proto at /protos/library.proto. Generate C# code by adding Grpc.Tools to projects and appropriate project configuration.
- To separate API and Service into different processes, run Library.Service on a dedicated port and update gRPC client address in Library.Api.
