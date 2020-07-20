# **NEST with Elastic Search through Docker**

The initial goal with this project was to compare the 2 Elastic Search Libraries in .NET to determine advantages in use. Using Nuget data and Elastic Search engine, we made our own search page. This uses the basic version of Elastic Search, 7.8.

**About Nest**

NEST is a high level .NET Elastic Search Client that provides strongly typed one-to-one mapping .Net queries to Elastic Search queries. It takes advantage of specific .NET features to provide higher level abstractions such as auto mapping of CLR types. Due to that conversion, we expect an overhead which lead us to benchmark if it's a good compromise for more features and capabilities. 

**Tested in:**
- Windows 10
- Visual Studio 2019

**Requirements:**

- [Docker Desktop for Windows](https://docs.docker.com/get-docker/)
- [Nuget Data](https://nusearch.blob.core.windows.net/dump/nuget-data-jul-2017.zip)
- Cloned this repository, locally
- 4GB RAM

## CONTENT

 1. [Docker Installation](Documents/docker-installation.md)
 2. [Preparing the Elastic Stack Container with Security Certificates](Documents/prep-elk-container.md)
 3. [Building the Elastic Stack Container](Documents/build-elk-container.md)
 4. [Securing the Elastic Stack and Changing Settings](Documents/secure-elk-container.md)
 5. [Testing Kibana](Documents/testing-kibana.md)
 6. [Indexing the Documents](Documents/indexing-documents.md)
 7. [Using the Search](Documents/search.md)
 8. [Common Errors and Discussion](Documents/error-discussion.md)


This project was a fork from [NEST 7.x Example](https://github.com/elastic/elasticsearch-net-example/tree/7.x) which was used and modified for our NEST/Elastic Search Training.

*For questions, please email g.mortillero@arcanys.com.*
