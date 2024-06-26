Certainly! Here's an expanded version of the README file with additional information about the multi-threading aspect of using Kafka partitions:

---

# dotnet-kafka-parallel-consumer

This sample demonstrates the utilization of [.NET Channels](https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/) to create in-memory queues, facilitating one worker per Kafka topic partition, thus enabling efficient multi-threading for processing Kafka messages.

## Key Features

- **Multi-Threading**: The application leverages Kafka topic partitions to allow for concurrent processing of messages. Each partition is assigned to a dedicated worker thread, enabling parallel execution and efficient resource utilization.
  
- **Asynchronous Processing**: By using .NET Channels, message processing is handled asynchronously, ensuring optimal performance and responsiveness. As messages are received from Kafka, they are enqueued into the corresponding channel and processed by the associated worker thread concurrently.

## Requirements

To run this project, ensure you have the following prerequisites installed:

- **.NET 6**: Ensure you have .NET 8 SDK installed on your machine.
- **Docker**: Docker is required to run Kafka locally. Ensure Docker Desktop or Docker Engine is installed.

## Quickstart

To get started quickly, follow these steps:

1. **Start Kafka**:
   Run the following command in your terminal to start Kafka using Docker Compose:
   ```
   docker-compose up
   ```

2. **Run the Worker**:
   Execute the following command in your terminal to run the worker application:
   ```
   dotnet run
    ```

## Output 
I have a system with 10 partitions. When we start the application, it creates 10 threads. Each thread is responsible for processing messages from its assigned partition. The thread handler and worker function as observers, continuously monitoring the partitions during runtime. If we add or remove partitions, the system will automatically adjust the number of threads in the thread worker accordingly.

![image](https://github.com/m2sachinpatil/KafkaParallelConsumer/assets/51775632/3e1f501b-56aa-4391-bd90-2e48f2d695d2)

---

This README provides an overview of the project's key features, focusing on its multi-threading capabilities for processing Kafka messages efficiently. Feel free to adjust or expand upon it further!
