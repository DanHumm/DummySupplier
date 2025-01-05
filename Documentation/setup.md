# Setup
This section contains the following folders which contains useful information for understanding, intergration, and consuming my supplier API:

- Certificate Authority Certificate: Contains the CA certificate (CA Cert) used for HTTPS communication.
- Code Documentation PDF: Contains the code documentation to better understand under the hood what is happening.
- Control Flow Diagrams Example: Example control flow diagrams for various API endpoints.
- Entity Relationship Diagram: Illustrates the relationships between entities in the database.
- Postman Collection: Contains the Postman collection (api.postman.collection) for testing the API endpoints should you wish to do so.

Since the API is in development and currently runs in a development mode only, a swagger endpoint "{CONTAINER_IP}/swagger" is available for a full description and consumption of my API.

## Importing the CA Certificate
To ensure secure communication with the API, you need to import the provided CA certificate into your browser's trusted root store to form the HTTPS connection since this is an internal application (if this was a real website in the wild, you would use a trusted certificate authority such as LetsEncrypt which would eliminate this). Follow the instructions below for your operating system:

### Windows

- Double-click the CA Cert file.
- Click "Install Certificate...".
- Select "Local Machine" and click "Next".
- Select "Place all certificates in the following store".
- Click "Browse..." and select "Trusted Root Certification Authorities". 6. Click "OK" and then "Next".  
- Click "Finish".

### Mac:

- Double-click the CA Cert file.
- In the Keychain Access window, select "System" under Keychains.
- Drag and drop the CA Cert file into the "System" keychain.
- Enter your administrator password when prompted.

### Linux:

Instructions may vary depending on your Linux distribution. Generally, you'll need to copy the CA Cert file to the appropriate directory and update the certificate store. Consult your distribution's documentation for specific instructions.

### Docker

Depending on the file system your container is adopting, you are generally looking for the location of ca-certificates.

In your DockerFile edit and add the following:
```
 COPY ca.crt {PATH_TO_CA-CERTIFICATES}/ca.crt
 chmod 644 {PATH_TO_CA-CERTIFICATES}/ca.crt ## ADD THIS IF YOU GET PERMISSIONS ERRORS 
 RUN update-ca-certificates
 RUN echo "{CONTAINER_IP} dairydoughsupplies.internal" >> /etc/hosts ## Modify CONTAINER_IP to reflect the IP of my docker container running in your docker network
 ```

- ca.crt: The file in its default state is ca.crt. If you rename it when you download, you will need to change these entries to reflect.
- {PATH_TO_CA-CERTIFICATES}: Depending on your base image/file system, this is where your containers trusted root certification store is.
- chmod 644: Modifies file permissions if you run into permissions errors from inherited permissions from the download.
- update-ca-certificates: Updates store to recognize new certificate we added
- RUN echo: This line echos the host name for the application into your hosts file so the application can be accessed using the name it is associated with in its certificate. Without this, the certificate name/host name wont match and you will get SSL error CERT INVALID.


The commands may vary depending on the file system/base image used to please refer to documentation for your case for correct commands/directories.

## Adding the domain name to your Hosts file

You need to map the domain name i have set for it to the IP address of the **container** in order to be able to communicate with the application using HTTPS since its certificate was generated for the domain name i selected "dairydoughsupplies.internal". Follow one of these instructions based on what OS you are running:

## Windows

- Open Notepad as administrator.
- In Notepad, open the following file: C:\Windows\System32\drivers\etc\hosts
- Add the following line at the end of the file, replacing {CONTAINER_IP} with the actual IP address of your container:
  {CONTAINER_IP} dairydoughsupplies.internal
- Save the file.

## Mac or Linux

- Open a terminal window.
- Open the hosts file using a text editor with administrator privileges (e.g., sudo nano /etc/hosts).
- Add the following line at the end of the file, replacing {CONTAINER_IP} with the actual IP address of your container:
  {CONTAINER_IP} dairydoughsupplies.internal
- Save the file.

## Docker
Check out the Docker instructions for adding the certificate, there is a line there about adding to the hosts file from the DockerFile but please be aware the location could be different depending on the base image/file system so if you run into errors bare this in mind.

# How to download and use it in your browser
1. Contact for a user account, an invite code will be generated which you will be able to use to register. Please wait to receive your invite code before pulling the package.
2. Pull the docker package, you can find the latest package for it [here](https://github.com/orgs/BUAdvDev2024/packages?repo_name=Logistics-Management-System).
3. List the images using the command ```docker images```
4. Run the image using ```docker run {image-id}```
5. Find the IP of container. If your using Linux, then ```docker ps``` to get container id then ```docker inspect <container_id> | grep "IPAddress"```
6. Download the Certificate Authority Certificate i made for it [here](https://github.com/BUAdvDev2024/Logistics-Management-System/tree/main/Docs/DairyDoughSupplies/Certificate).
7. Follow the instructions for importing the certificate into your browser as well as adding the right entry in your hosts file. You can find instructions on how to do this within my [introduction document](https://github.com/BUAdvDev2024/Logistics-Management-System/blob/main/Docs/DairyDoughSupplies/introduction.md).
8. If the container is running, the ca.crt is imported, and a valid entry has been entered into your hosts file, you should be able to access it using https://dairydoughsupplies.internal

Since this application is configured by default as the development prototype, you will be able to access the swagger endpoint which describes the API endpoints. The API follows RestFUL principles so if you get confused about what different endpoints might do check out heading 2 of this [useful resource](https://restfulapi.net/rest-api-best-practices/).

**Please note:** there are some admin only endpoints which as a standard user you will receive a 403. This is to be expected when you are attempting to use an administrative endpoint with a standard user account.

# Test Users

There are two available test users for this:

<details>
  <summary>Test user accounts</summary>

  **Standard User**

  username: testuser
  password: testUSER123!

  **Admin User**

  username: dan
  password: Password123!

</details>