
# Introduction
- This application represents the base of backend implementation for the Football manager app. It is an attempt in building a microservice architecture app.

### Tech used:
- .NET Core - v6.0
- Redis - PUB/SUB
- Neo4j - data persistence and complex cypher queries
- SignalR - client server full duplex communication 

### Structure:
**Shared**

This is a class library containing possible extensions for microservices: redis, neo4j, custom serialization, models, rest api client, and streaming... That means that every microservice can inject dependencies with created methods for chosen extensions.

**Aggregator**

This is a microservice that is used to observe and accumulate data from live match simulation.
It subscribes to specific redis channels and listens to its highlights.

**FutManager**

This app is used for client-server communication, frontend, etc.

**MatchPlayer**

This is a microservice responsible for generating messages and actually simulating football matches, with all the messages and highlight.

**Scheduler**

This is a microservice responsible for scheduling football matches at a given time.

The focus was to build an app that is scalable and can be extended very easily with a ton of new features using just base implementation in Shared class library.
