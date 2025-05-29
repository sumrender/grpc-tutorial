# GrpcGreeter Demo

This is a simple gRPC demonstration project that implements a basic greeting service using C#.

## Project Structure

- `GrpcGreeter/` - Server component
- `GrpcGreeterClient/` - Client component

## Prerequisites

- .NET 6.0 or later
- Postman (version 9.0 or later for gRPC support)

## How to Run

1. Start the server:
   ```bash
   cd GrpcGreeter
   dotnet run
   ```
   The server will start on http://localhost:5057

2. Run the client (in a new terminal):
   ```bash
   cd GrpcGreeterClient
   dotnet run
   ```

## How to Test GrpcGreeter on Postman

1. **Launch Postman and Create New Request**
   - Open Postman
   - Click "New"
   - Select "gRPC Request"

2. **Configure Connection**
   - Enter Server URL: `localhost:5057`
   - Uncheck "Use TLS" (since we're using HTTP)

3. **Import Proto File**
   - In Postman, click "Import" in the gRPC request
   - Select the proto file from: `GrpcGreeter/Protos/greet.proto`
   - You should see the `SayHello` method available

4. **Create and Send Request**
   - Select the `SayHello` method
   - In the "Message" section, enter:
     ```json
     {
       "name": "Postman Test"
     }
     ```
   - Click "Invoke" to send the request

5. **Expected Response**
   You should receive a response like:
   ```json
   {
     "message": "I am grpc server. Text I received from client was: Postman Test"
   }
   ```

### Troubleshooting
- Make sure the gRPC server is running before testing
- Verify you're using HTTP (not HTTPS) in Postman
- Ensure you're using a recent version of Postman that supports gRPC
- If you get connection errors, check if the server is running on the correct port (5057) 