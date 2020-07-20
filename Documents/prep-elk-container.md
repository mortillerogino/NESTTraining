## II. PREPARING THE ELASTIC STACK CONTAINER WITH SECURITY

You can expand on this later, but for the purposes of this training, we'll stick with a basic authentication setup. Granted, we'll need to install some certificates. 

You'll need git bash or powershell to run the create certificates command. You'll have to navigate to the Docker directory, assuming you already cloned this repository. If not, clone it locally now. 

The best way to open the directory in powershell, is navigate to that Docker directory, and shift right click on an empty space. There will be multiple options, but what you need is the "**Open Powershell window here**".

Click that command and it should show you powershell (identified by the blue background) 
with the cursor on the directory already. You will need to run the command below.

    docker-compose -f create-certs.yml run --rm create_certs

To provide more information, this command downloads and extracts security certificates for your machine to run elastic search. Elasticsearch nodes and Kibana use these certificates to identify themselves when communicating with other nodes.

After you press enter, it should show you the following:

![image: cert-installed.png](../Images/cert-installed.png)

As you can see, this installs several certificates. We will be using 3 nodes and Kibana, so this installs a certificate for each. This is all configured on the **create-certs.yml** file. We'll reserve how it's coded for another time. 

That's it for the security certificates. Now we move on to the main event!