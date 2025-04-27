# Sinch.MessageEncoder

**Sinch.MessageEncoder** is a lightweight C# library for encoding message objects into a compact binary format. It was designed as a proof-of-concept to explore custom binary serialization for "messages" (with headers and payloads) in a Sinch context. The project is **lightweight and fast**, aiming to pack data efficiently for transport or storage.

> **Note**  
> This project was originally created as part of a recruitment exercise, and it was left a bit unfinished when the recruitment process ended.  
> After some digging, it turned out this encoder behaves **fairly similar to MessagePack** (but without any fancy code-generation features). 😉 **Please do your own research!**

## Project Structure

- **src/Sinch.MessageEncoder** – Core library with encoder logic.
- **src/Sinch.MessageEncoder.PoC** – Proof-of-concept console app showcasing basic usage.
- **src/Benchmarks/Sinch.MessageEncoder.Benchmarks** – Benchmark project using BenchmarkDotNet.
- **src/Tests/Sinch.MessageEncoder.MessageBuilder.Tests** – Unit tests for the message builder.
- **src/Tests/Sinch.MessageEncoder.CustomMessages.Tests** – Unit tests for custom message classes.

## Features

- **Message Schema with Attributes** – Easy message class definitions with `[SerializationOrder]` and `[MessageType]`.
- **Headers & Payload Model** – Clean separation between headers and payloads.
- **Fluent Builder API** – Chain calls to build messages easily.
- **Custom Serialization Support** – Plug in custom serializers per property.
- **Efficiency Focus** – Span-based operations for maximum performance.
- **Minimal Dependencies** – Built on pure .NET without heavy libraries.

## Getting Started

### Prerequisites

- [.NET SDK 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) or newer.

### Build the Solution

```bash
dotnet build Sinch.MessageEncoder.sln
```

Or open `Sinch.MessageEncoder.sln` in Visual Studio 2022+ and build.

### Run the PoC

```bash
dotnet run --project src/Sinch.MessageEncoder.PoC/Sinch.MessageEncoder.PoC.csproj
```

Or set **Sinch.MessageEncoder.PoC** as your startup project in Visual Studio.

### Run the Tests

```bash
dotnet test Sinch.MessageEncoder.sln
```

Or run them through Visual Studio's Test Explorer.

### Run Benchmarks

```bash
cd src/Benchmarks/Sinch.MessageEncoder.Benchmarks
dotnet run -c Release
```

(Always run benchmarks in **Release** mode for valid results.)

## Usage Example

```csharp
using Sinch.MessageEncoder;
using Sinch.MessageEncoder.Builders;
using Sinch.MessageEncoder.Messages.Default.Text;

var builder = MessageBuilder<DefaultTextMessageHeaders, DefaultTextMessagePayload>.CreateBuilder();

var messageBytes = builder
    .From(1)
    .To(2)
    .Timestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
    .MsgType(1)
    .AddHeader("sender-name", "Alice")
    .AddHeader("recipient-name", "Bob")
    .EndHeaders()
    .AddPayloadProperty(nameof(DefaultTextMessagePayload.TextMessageBody), "Hello, world!")
    .GetBinary();
```

> Now `messageBytes` contains the serialized message ready for transport or storage!

*Note*: The library focuses mainly on **serialization** for now. Full deserialization support would require a bit more work.

## Contributing

Contributions, ideas, and improvements are welcome!  
Feel free to fork the project, create a branch, and open a pull request. 🚀

## License

This project is licensed under the **MIT License**.  
See the [LICENSE](./LICENSE) file for details.
