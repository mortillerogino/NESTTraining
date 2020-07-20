## VIII. COMMON ERRORS & DISCUSSION

Initially this was run through a non-docker local setup. But for portability and ease of use, we contained our elastic-kibana stack to a docker container.

Our First attempt was to just connect the code to a ready-made [Elk stack container made by deviantony](https://github.com/deviantony/docker-elk). Apart from making adjustments to the connection as this repository was using a trial version (one with monitoring), it was easy to connect. 

However, we encountered some limitations, as we attempt to index our multiple documents. As we ran our code, we come accross the error 429, or too many requests error. It occurs when the number of requests to the Elasticsearch cluster exceeds the bulk queue size.

We determined that the write requests were just too many for one node to handle. So we decided to write our own docker-compose file, which will contain only the ones we need, i.e., a multiple node Elastic Search and Kibana.