## V. TESTING KIBANA

It's time for us to muddle the waters of your newly built elastic stack. Your container should now be up and running. 

To use Kibana, open a browser and navigate to https://localhost:5601. If you have changed your KIBANA_PORT on .env file, change the 5601 to that. Also, notice that this url has https instead of http.

You might get this page below.

![image: connection-not-private.png](../Images/connection-not-private.png)

This is okay. Our certificicates our unsigned so this is expected. To continue, just click on the Advanced button and once the extra options appear, click on the Proceed to localhost (unsafe) link.

You'll be redirected to a login page. The username will be elastic and its password should be coming from the generated passwords.

![image: kibana-loading.png](../Images/kibana-loading.png)

Kibana is the user interface that you'll need to interact with Elastic Search. Think of it like SSMS to your SQL Server. There are lot of interesting features here but for the purpose of this tutorial, we will just be using the dev tools. This is where we will be running elastic search queries.

The dev tools is accessible through the menu, under Management.

![image: dev-tools.png](../Images/dev-tools.png)

Once everything is started, you can go to Kibana through https://localhost:{your current Kibana port on the .env file}. By default, this is 5601.

The login credentials can be found from the generated passwords still, the **elastic username and its corresponding password.**

As an example, we'll run a query to check on the health of our 3 nodes.

    GET /_cluster/health

To run the command, click on the Play button. This button will send the request to our elastic search server. Which in turn should be like this below.

![image: cluster-health.png](../Images/cluster-health.png)

As you can observer, our server is as good as green. This is critical because we will be indexing a big number of documents.
