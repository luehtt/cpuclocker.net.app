# CPUClocker
CPUClocker is a console app to monitor computer running Windows using .NET 5.0, InfluxDb, and Kafka.\
Coder: thanhtoan.inu@gmail.com\
Github: http://github.com/luehtt
  

# Installation

### Install .NET 5.0
1. Download and Install **.NET Runtime**.
 [Download .NET 5.0 (Linux, macOS, and Windows) ](https://dotnet.microsoft.com/download/dotnet/5.0)\
Please note that the right link is in the right under **Run apps - Runtime**.\
Can either **.NET Desktop Runtime** or **.NET Runtime** since this app in an **Console app**.

### Install InfluxDb
1. Download and Extract it to somewhere such as D:\Lib\influxDb.
2. Open \influxdb.config and change some var if need.\
`dir = "/var/lib/influxdb/meta"`\
`dir = "/var/lib/influxdb/data"`\
`wal-dir = "/var/lib/influxdb/wal"`\
The default location for that is C:\Users\{PCName}\.influxdb.
3. Open influxd.exe to start InfluxDB server and let it running.
4. Open influx.exe to connect to InfluxDB server.
5. Enter this command to create user influxDB with all privileges.\
`create user influxdb with password 'password' with all privileges`
6. Enter this command to create a database.\
`create database cpuclocker`

### Install Kafka (Optional)
> Apache Kafka is an open-source distributed event streaming platform used by thousands of companies for high-performance data pipelines, streaming analytics, data integration, and mission-critical applications.

Installing Kafka is optional, the program can be run without it.
1. Download and Extract it to somewhere such as D:\Lib\kafka.\
[Apache Kafka - Download](https://kafka.apache.org/downloads)
2. Open \config\zookeeper.properties and edit these line.\
`dataDir=D:\Lib\kafka\zookeeper`\
Create the folder zookeeper if there is not.
3. Open \config\server.properties and edit these line.\
`log.dirs=D:\Git\kafka\logs`\
Create the folder logs if there is not. Add this line to config Kafka host.\
`listeners=PLAINTEXT://localhost:9092`
4. Download Java SE Runtime Environment 8 and Install.\
Kafka running depend on Java.\
[Java SE Runtime Environment 8 - Downloads](https://www.oracle.com/java/technologies/javase-jre8-downloads.html)

# Running
1. Open influxd.exe to start InfluxDB server and let it running.
2. Open cmd, go to kafka installed folder, start zookeeper server, and let it running.\
`D:`\
`cd Lib/kafka/bin/windows`\
`zookeeper-server-start.bat ../../config/zookeeper.properties`
3. Do the same cmd, start kafka server, and let it running.\
`zookeeper-server-start.bat ../../config/server.properties`
4. Open AppConfig.json in CPUClocker app folder and set these values.\
If Kafka is not used, set UsingKafka to false.
6. Run CPUClocker.exe and enjoy.
