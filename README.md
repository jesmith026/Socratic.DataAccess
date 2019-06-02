# **Socratic Data Access**
A simple implementation of a facade for Entity Framework and Dapper utilizing the Unit of Work pattern.

## **Project Status**
#### Socratic.DataAccess.Abstractions 
[![Build Status](https://dev.azure.com/jesmith26/SocraticProgrammer/_apis/build/status/Libraries/Socratic.DataAccess.Abstractions-CI?branchName=dev)](https://dev.azure.com/jesmith26/SocraticProgrammer/_build/latest?definitionId=12&branchName=dev)

#### Socratic.DataAccess
[![Build Status](https://dev.azure.com/jesmith26/SocraticProgrammer/_apis/build/status/Libraries/Socratic.DataAccess-CI?branchName=dev)](https://dev.azure.com/jesmith26/SocraticProgrammer/_build/latest?definitionId=19&branchName=dev)

## **Getting Started**
### Dependencies
- Docker 

To interact with the sample simply navigate to the project directory, i.e. ./Samples/SchoolSample/. Then type the command "docker-compose up". Once the command completes you can send requests to the API via the http://localhost:8080/api/students URL endpoint.

There is a Postman collection in the tests directory which can be used to demonstrate some of the calls available.

Please note that the sample API is **NOT** intended to be an illustration of API best practices, but instead only to demonstrate simple usage of this data access library.

## **About**
Many times in my career I have been asked to choose a side between Dapper and Entity Framework. I, like many others subscribe to the principle of choosing "the right tool for the job". As a result of this I've never been satisfied with implementing a solution which chose between the two. Many times I would approach the discussion of a hybrid solution only to have the conversation fall flat. I believed that this was due to a lack of an existing solution to make these two libraries work together, so that they could each complement the other's weak points. This project is my attempt to begin the creation of such a solution, so that in future discussions it can be referenced as an example of how you don't always need to choose between two seemingly competing technologies. Instead, by developing to the right abstractions you can blend them together to get the best of both worlds.


The latest packages for this project can be found on Nuget along with the other Socratic libraries [here](https://www.nuget.org/packages?q=socratic&prerel=false).