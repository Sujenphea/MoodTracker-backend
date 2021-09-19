# Moodiful
![Logo](./Assets/moodiful_dark.png)
## Introduction
Ever feel anxious and finding nowhere to place your thoughts or rants?
Where else are you looking for?!
This is a mood tracking website to summarise and review your thoughts and your daily habits!

## Documentation
- This repository contains the API for the mood tracker website. 

### Instructions
1. Download the project
2. Open MoodTrackerBackendCosmos.sln with Visual Studio
3. Run the program
4. Interact with GraphQL by appending /graphql to server website
5. Interact with the API provided! :)

### APIs
Types
- User (id, name, github, imageuri)
- Daily (id, dateCreated, description, userid)

Query
- Get Dailies (Dailies)
- Get Dailies by userId (DailiesByUserId(id: string))
- Get Users (Users)
- Get Self (Self)

Mutation
- Add Daily
- Edit Daily
- Add User
- Login

## Technologies
- Database: Microsoft Azure Cosmos Db with Entity Framework
- Deployment: Microsoft Azure Web App
- GraphQL Server: Hot Chocolate
- Public API: http://api.forismatic.com

