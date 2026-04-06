# gRPC Learning and Demonstration Projects

A comprehensive collection of gRPC projects showcasing Protocol Buffers, unary RPC, server streaming, client streaming, and multi-language interoperability (.NET and JavaScript).

## 📋 Project Overview

| Project | Type | Language | Purpose |
|---------|------|----------|---------|
| **example** | Console App | C# | Protobuf serialization/deserialization demo |
| **groomserver** | gRPC Server | C# / ASP.NET Core | Central message hub and room management |
| **groomclient** | Console App | C# | Room registration client (unary RPC) |
| **groomadmin** | Console App | C# | Message monitoring client (server streaming) |
| **newsbot** | Console App | Node.js | News broadcasting client (client streaming) |

## 🏗️ Architecture

```
┌─────────────────┐  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐
│  groomclient    │  │  groomadmin  │  │   newsbot    │  │   example   │
│  (C# Unary RPC) │  │(C# Streaming)│  │ (JS Streaming)  │ (Protobuf)  │
└────────┬────────┘  └──────┬───────┘  └──────┬───────┘  └─────────────┘
         │                  │                  │
         │ RegisterToRoom   │ StartMonitoring  │ SendNewsFlash
         │                  │                  │
         └──────────────────┴──────────────────┘
                            │
                            ▼
                    ┌──────────────────┐
                    │  groomserver     │
                    │  (gRPC Server)   │
                    │  Port: 5071      │
                    │                  │
                    │ • Room Mgmt      │
                    │ • Message Queue  │
                    │ • Broadcasting   │
                    └──────────────────┘
```

## 🚀 Quick Start

### Prerequisites
- .NET 10.0 SDK
- Node.js 18+ (for newsbot)
- Visual Studio or VS Code

### Setup

1. **Clone/Open the solution:**
   ```bash
   cd /Users/hmarques/workspace/csharp/grpc
   ```

2. **Build the solution:**
   ```bash
   dotnet build grpc.sln
   ```

3. **Install newsbot dependencies:**
   ```bash
   cd newsbot
   npm install
   ```

### Running the Projects

#### 1. Start the gRPC Server
```bash
cd groomserver
dotnet run
# Server will be available at localhost:5071
```

#### 2. Register a Room (in a new terminal)
```bash
cd groomclient
dotnet run
# Enter a room name when prompted
```

#### 3. Monitor Messages (in another terminal)
```bash
cd groomadmin
dotnet run
# Will stream all messages from the server
```

#### 4. Send News (in another terminal)
```bash
cd newsbot
npm install
node client.js
# Sends 10 random news items to the server
```

#### 5. Run Protobuf Demo
```bash
cd example
dotnet run
# Demonstrates Protocol Buffer serialization
```

## 📖 Project Details

### example - Protocol Buffers Demo
- **Purpose:** Learn Protocol Buffers serialization and deserialization
- **Features:**
  - Creates an Employee object with complex fields
  - Serializes to binary format using `employee.proto`
  - Deserializes and displays the data
  - Demonstrates Protobuf features: `oneof`, enums, maps, repeated fields
- **Proto Definition:** `employee.proto`

### groomserver - gRPC Server
- **Purpose:** Central message hub with room management and message broadcasting
- **Endpoints:**
  - `RegisterToRoom` (Unary RPC) - Register a client to a specific room
  - `SendNewsFlash` (Client Streaming) - Accept stream of news items from clients
  - `StartMonitoring` (Server Streaming) - Stream messages to monitoring clients
- **Features:**
  - Internal message queue for message management
  - Multi-room support
  - Real-time message broadcasting
- **Proto Definition:** `groom.proto`
- **Port:** 5071

### groomclient - Room Registration Client
- **Purpose:** Demonstrate unary RPC communication
- **Features:**
  - Connect to groomserver
  - Register user to a room
  - Simple interactive CLI
- **Proto Definition:** `groom.proto`
- **Technology:** C# / Grpc.Net.Client

### groomadmin - Message Monitoring Client
- **Purpose:** Demonstrate server streaming RPC
- **Features:**
  - Connect to groomserver
  - Receive real-time message stream
  - Monitor all messages in the system
  - Display message details (content, user, timestamp)
- **Proto Definition:** `groom.proto`
- **Technology:** C# / Grpc.Net.Client

### newsbot - News Broadcasting Client
- **Purpose:** Demonstrate client streaming RPC and multi-language gRPC interoperability
- **Features:**
  - Connect to groomserver from Node.js
  - Send stream of news items at 1-second intervals
  - Multiple predefined news categories (stocks, weather, events, etc.)
  - Shows .NET server ↔ JavaScript client communication
- **Proto Definition:** `groom.proto`
- **Technology:** Node.js / @grpc/grpc-js
- **Main File:** `client.js`

## 🔧 Key Technologies

- **gRPC** - High-performance RPC framework
- **Protocol Buffers (Proto3)** - Efficient data serialization
- **.NET 10.0** - Modern C# runtime
- **ASP.NET Core** - Web framework for gRPC server
- **Node.js** - JavaScript runtime for cross-language demo
- **Visual Studio Solution** - `grpc.sln` for integrated development

## 📚 Learning Path

1. **Start with `example`** - Understand Protocol Buffers basics
2. **Run `groomserver`** - Start the server
3. **Try `groomclient`** - Learn unary RPC
4. **Run `groomadmin`** - Explore server streaming
5. **Run `newsbot`** - Experience client streaming and cross-language communication

## 🔗 Proto Definitions

### groom.proto
Located in each project, defines:
- `Room` message
- `Item` message
- `Message` message (with user, content, timestamp)
- `Groom` service with three RPC methods

### employee.proto
Used by `example` project, defines:
- `Employee` message with complex fields
- `Address` message
- Enum types and maps

## 📝 License

This project is for learning purposes.

## 🤝 Contributing

Feel free to extend these projects with new features, additional clients, or new RPC patterns (bidirectional streaming, etc.).

## Bi-directional streaming in gRoom
![alt text](image.png)