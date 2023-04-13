# Bot Functional Test - From Scratch to CI

## Introduction

This article walks you through making and developing functional tests for bots from scratch to CI.

We will be covering the basics of making a simple echo bot to test, write functional tests using [Mocha](https://mochajs.org/) and create an [Azure CI](https://docs.microsoft.com/en-us/azure/devops/pipelines/get-started/what-is-azure-pipelines?view=azure-devops) to Deploy the bot and running tests.

At the end, you will learn how to:

- Create a test bot
- Create a functional test using [Mocha](https://mochajs.org/) as a test suite
- Set up an [Azure CI](https://docs.microsoft.com/en-us/azure/devops/pipelines/get-started/what-is-azure-pipelines?view=azure-devops) for Deploying a bot and running the functional tests

You can download the project files used in this article in the [functional-tests folder](https://github.com/microsoft/botbuilder-js/tree/main/libraries/functional-tests) included within the BotBuilder-JS repository.

## Create a test bot

### Prerequisites

- [Visual Studio Code](https://www.visualstudio.com/downloads)
- [Node.js](https://nodejs.org/)
- [Bot Framework Emulator](https://aka.ms/bot-framework-emulator-readme)

To create your test bot:

1. Create the next directory for your functional test project.

   ```tex
   bot-functional-test
   └───test-bot
   	└───bots
   ```

2. Add the next files to the `test-bot` folder.

   **package.json**

   ```json
   {
     "name": "testbot",
     "version": "1.0.0",
     "description": "a test bot for working locally",
     "main": "index.js",
     "scripts": {
       "test": "echo \"Error: no test specified\" && exit 1"
     },
     "author": "",
     "license": "MIT",
     "dependencies": {
       "botbuilder": "^4.1.6",
       "restify": "^8.5.1",
       "dotenv": "^6.2.0"
     }
   }
   ```

   In the `bots` folder, add the **myBot.js** file.

   ```javascript
   /**
    * Copyright (c) Microsoft Corporation. All rights reserved.
    * Licensed under the MIT License.
    */
   
   const { ActivityHandler } = require('botbuilder');
   
   class MyBot extends ActivityHandler {
       constructor(conversationState) {
           super();
           this.conversationState = conversationState;
           this.conversationStateAccessor = this.conversationState.createProperty('test');
           this.onMessage(async (context, next) => {
   
               var state = await this.conversationStateAccessor.get(context, { count: 0 });
   
               await context.sendActivity(`you said "${ context.activity.text }" ${ state.count }`);
   
               state.count++;
               await this.conversationState.saveChanges(context, false);
   
               await next();
           });
           this.onMembersAdded(async (context, next) => {
               const membersAdded = context.activity.membersAdded;
               for (let cnt = 0; cnt < membersAdded.length; cnt++) {
                   if (membersAdded[cnt].id !== context.activity.recipient.id) {
                       await context.sendActivity(`welcome ${ membersAdded[cnt].name }`);
                   }
               }
               await next();
           });        
       }
   }
   
   exports.MyBot = MyBot;
   ```

   **index.js**

   ```javascript
   // Copyright (c) Microsoft Corporation. All rights reserved.
   // Licensed under the MIT License.
   
   const restify = require('restify');
   const path = require('path');
   
   const { BotFrameworkAdapter, MemoryStorage, UserState, ConversationState, InspectionState, InspectionMiddleware } = require('botbuilder');
   const { MyBot } = require('./bots/myBot')
   
   const ENV_FILE = path.join(__dirname, '.env');
   require('dotenv').config({ path: ENV_FILE });
   
   const adapter = new BotFrameworkAdapter({
       appId: process.env.MicrosoftAppId,
       appPassword: process.env.MicrosoftAppPassword
   });
   
   var memoryStorage = new MemoryStorage();
   var inspectionState = new InspectionState(memoryStorage);
   
   var userState = new UserState(memoryStorage);
   var conversationState = new ConversationState(memoryStorage);
   
   adapter.use(new InspectionMiddleware(inspectionState, userState, conversationState, { appId: process.env.MicrosoftAppId, appPassword: process.env.MicrosoftAppPassword }));
   
   adapter.onTurnError = async (context, error) => {
       console.error(`\n [onTurnError]: ${ error }`);
       await context.sendActivity(`Oops. Something went wrong!`);
   };
   
   var bot = new MyBot(conversationState);
   
   console.log('welcome to test bot - a local test tool for working with the emulator');
   
   let server = restify.createServer();
   server.listen(process.env.port || process.env.PORT || 3978, function() {
       console.log(`\n${ server.name } listening to ${ server.url }`);
   });
   
   server.post('/api/mybot', (req, res) => {
       adapter.processActivity(req, res, async (turnContext) => {
           await bot.run(turnContext);
       });
   });
   
   server.post('/api/messages', (req, res) => {
       adapter.processActivity(req, res, async (turnContext) => {
           await bot.run(turnContext);
       });
   });
   ```



To deploy the bot in Azure, we will need the next files. 

The **deployment template**, that is a file used it to automatize the process of creation the resources relate to the bot.

The **.deployment** script, that is a file used to automatize the installation of the dependencies needed in the project when the bot is deployed. 

To add the deployment files:

1. In the `test-bot` directory, add the next file

   **.deployment**
   
   ```tex
   [config]
   SCM_DO_BUILD_DURING_DEPLOYMENT=true
   ```

2. Create a new folder `deploymentTemplates` in the `test-bot` directory. 

   ```tex
   bot-functional-test
   └───test-bot
   	└───bots
       	└───deploymentTemplates
   ```

3. Create a **template.json** file inside the `deploymentTemplates`. Then, copy the content

   <details>
   <summary>Click to expand template code</summary>
   <p>
   
   ```json
   {
       "$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
       "contentVersion": "1.0.0.0",
       "parameters": {
           "groupLocation": {
               "type": "string",
               "metadata": {
                   "description": "Specifies the location of the Resource Group."
               }
           },
           "groupName": {
               "type": "string",
               "metadata": {
                   "description": "Specifies the name of the Resource Group."
               }
           },
           "appId": {
               "type": "string",
               "metadata": {
                   "description": "Active Directory App ID, set as MicrosoftAppId in the Web App's Application Settings."
               }
           },
           "appSecret": {
               "type": "string",
               "metadata": {
                   "description": "Active Directory App Password, set as MicrosoftAppPassword in the Web App's Application Settings."
               }
           },
           "botId": {
               "type": "string",
               "metadata": {
                   "description": "The globally unique and immutable bot ID. Also used to configure the displayName of the bot, which is mutable."
               }
           },
           "botSku": {
               "type": "string",
               "metadata": {
                   "description": "The pricing tier of the Bot Service Registration. Acceptable values are F0 and S1."
               }
           },
           "newAppServicePlanName": {
               "type": "string",
               "metadata": {
                   "description": "The name of the App Service Plan."
               }
           },
           "newAppServicePlanSku": {
               "type": "object",
               "defaultValue": {
                   "name": "S1",
                   "tier": "Standard",
                   "size": "S1",
                   "family": "S",
                   "capacity": 1
               },
               "metadata": {
                   "description": "The SKU of the App Service Plan. Defaults to Standard values."
               }
           },
           "newAppServicePlanLocation": {
               "type": "string",
               "metadata": {
                   "description": "The location of the App Service Plan. Defaults to \"westus\"."
               }
           },
           "newWebAppName": {
               "type": "string",
               "defaultValue": "",
               "metadata": {
                   "description": "The globally unique name of the Web App. Defaults to the value passed in for \"botId\"."
               }
           }
       },
       "variables": {
           "appServicePlanName": "[parameters('newAppServicePlanName')]",
           "resourcesLocation": "[parameters('newAppServicePlanLocation')]",
           "webAppName": "[if(empty(parameters('newWebAppName')), parameters('botId'), parameters('newWebAppName'))]",
           "siteHost": "[concat(variables('webAppName'), '.azurewebsites.net')]",
           "botEndpoint": "[concat('https://', variables('siteHost'), '/api/mybot')]"
       },
       "resources": [
           {
               "name": "[parameters('groupName')]",
               "type": "Microsoft.Resources/resourceGroups",
               "apiVersion": "2018-05-01",
               "location": "[parameters('groupLocation')]",
               "properties": {
               }
           },
           {
               "type": "Microsoft.Resources/deployments",
               "apiVersion": "2018-05-01",
               "name": "storageDeployment",
               "resourceGroup": "[parameters('groupName')]",
               "dependsOn": [
                   "[resourceId('Microsoft.Resources/resourceGroups/', parameters('groupName'))]"
               ],
               "properties": {
                   "mode": "Incremental",
                   "template": {
                       "$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
                       "contentVersion": "1.0.0.0",
                       "parameters": {},
                       "variables": {},
                       "resources": [
                           {
                               "comments": "Create a new App Service Plan",
                               "type": "Microsoft.Web/serverfarms",
                               "name": "[variables('appServicePlanName')]",
                               "apiVersion": "2018-02-01",
                               "location": "[variables('resourcesLocation')]",
                               "sku": "[parameters('newAppServicePlanSku')]",
                               "properties": {
                                   "name": "[variables('appServicePlanName')]"
                               }
                           },
                           {
                               "comments": "Create a Web App using the new App Service Plan",
                               "type": "Microsoft.Web/sites",
                               "apiVersion": "2015-08-01",
                               "location": "[variables('resourcesLocation')]",
                               "kind": "app",
                               "dependsOn": [
                                   "[resourceId('Microsoft.Web/serverfarms/', variables('appServicePlanName'))]"
                               ],
                               "name": "[variables('webAppName')]",
                               "properties": {
                                   "name": "[variables('webAppName')]",
                                   "serverFarmId": "[variables('appServicePlanName')]",
                                   "siteConfig": {
                                       "appSettings": [
                                           {
                                               "name": "WEBSITE_NODE_DEFAULT_VERSION",
                                               "value": "10.14.1"
                                           },
                                           {
                                               "name": "MicrosoftAppId",
                                               "value": "[parameters('appId')]"
                                           },
                                           {
                                               "name": "MicrosoftAppPassword",
                                               "value": "[parameters('appSecret')]"
                                           }
                                       ],
                                       "cors": {
                                           "allowedOrigins": [
                                               "https://botservice.hosting.portal.azure.net",
                                               "https://hosting.onecloud.azure-test.net/"
                                           ]
                                       }
                                   }
                               }
                           },
                           {
                               "apiVersion": "2017-12-01",
                               "type": "Microsoft.BotService/botServices",
                               "name": "[parameters('botId')]",
                               "location": "global",
                               "kind": "bot",
                               "sku": {
                                   "name": "[parameters('botSku')]"
                               },
                               "properties": {
                                   "name": "[parameters('botId')]",
                                   "displayName": "[parameters('botId')]",
                                   "endpoint": "[variables('botEndpoint')]",
                                   "msaAppId": "[parameters('appId')]",
                                   "developerAppInsightsApplicationId": null,
                                   "developerAppInsightKey": null,
                                   "publishingCredentials": null,
                                   "storageResourceId": null
                               },
                               "dependsOn": [
                                   "[resourceId('Microsoft.Web/sites/', variables('webAppName'))]"
                               ]
                           }
                       ],
                       "outputs": {}
                   }
               }
           }
       ]
   }
   ```
   
   </p>
   </details>




To test the bot locally

1. Install the node modules, open a terminal and run the next command in the test-bot folder.

   ```bash
   npm install
   ```

2. Start and test the bot.

   Open a terminal in the directory where you created the index.js file, and start it with the next command.

   ```bash
   node index.js
   ```

   Then, start the [Bot Framework Emulator](https://aka.ms/bot-framework-emulator-readme) and click on the **Open bot** button. 

   Add the route of the bot endpoint `http://localhost:3978/api/messages` and click on **Connect**.

   Once connected, the bot will send you a welcome message.

    ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/bf-emulator-connected.png)



## Create Functional Test

A functional test is a testing process that aims to validate if the behavior of an application matches the business requirements.

In this case, we created a bot that simply "echoes" back to the user whatever the user says to the bot. 

The purpose of the functional test will be to verify if the bot created complies with the behavior described below.

```
user: Contoso
bot: you said "Contoso" 0
```

The logic of the functional test will consist of three parts:

One, create a DirectLine client to connect the test with the bot using the [swagger-client](https://www.npmjs.com/package/swagger-client) package.

Two, use the client to create a conversation then, send a message and retrieve the bot response. 

Three, make the assertion of the bot message.

To create the functional test:

1. Add the next files in the root of the project folder.

   **package.json**

   ```json
   {
       "name": "functional-tests",
       "version": "1.0.0",
       "description": "Test that hits services",
       "main": "",
       "dependencies": {
         "mocha": "^7.0.0",
         "swagger-client": "^2.1.18"
       },
       "directories": {
         "test-bot": "test-bot"
       },
       "scripts": {
         "functional-test":"mocha functional.test.js"
       },
       "keywords": [],
       "author": "",
       "license": "MIT"
   }
   ```

   **directline-swagger.json**

   Find the Direct Line API definition in the functional test folder from the **BotBuilder-JS** repository. [Here](https://github.com/microsoft/botbuilder-js/blob/main/libraries/functional-tests/tests/directline-swagger.json) 

   **functional.test.js**
   
   ``````javascript
   /**
    * Copyright (c) Microsoft Corporation. All rights reserved.
    * Licensed under the MIT License.
    */
   
   const assert = require('assert');
   const directLineSpec = require('./directline-swagger.json');
   const Swagger = require('swagger-client');
   
   const directLineClientName = 'DirectLineClient';
   const userMessage = 'Contoso';
   const directLineSecret = process.env.DIRECT_LINE_KEY || null;
   
   const auths = {
       AuthorizationBotConnector: new Swagger.ApiKeyAuthorization('Authorization', 'BotConnector ' + directLineSecret, 'header'),
   };
   
   function getDirectLineClient() {    
       return new Swagger({
           spec: directLineSpec,
           usePromise: true,
           authorizations: auths
       });
   }
   
   async function sendMessage(client, conversationId) {       
       let status;
       do{
           await client.Conversations.Conversations_PostMessage({
               conversationId: conversationId,
               message: {
                   from: directLineClientName,
                   text: userMessage
               }
           }).then((result) => {
               status = result.status;
           }).catch((err)=>{
               status = err.status;
           }); 
       }while(status == 502);
   }
   
   function getMessages(client, conversationId) {    
       let watermark = null;
       return client.Conversations.Conversations_GetMessages({ conversationId: conversationId, watermark: watermark })
           .then((response) => {            
               return response.obj.messages.filter((message) => message.from !== directLineClientName);       
           });
   }
   
   function getConversationId(client) {
       return client.Conversations.Conversations_NewConversation()
           .then((response) => response.obj.conversationId);
   }
   
   describe('Test Azure Bot', function(){
       this.timeout(60000);    
       it('Check deployed bot answer', async function(){
           const directLineClient = await getDirectLineClient();    
           const conversationId = await getConversationId(directLineClient);
           await sendMessage(directLineClient, conversationId);
           const messages = await getMessages(directLineClient, conversationId);
           const result = messages.filter((message) => message.text.includes('you said'));                
           assert(result[0].text == `you said "${ userMessage }" 0`, `test fail`);
       });
   });
   
   ``````



As you can see in the test code.

```javascript
const directLineSecret = process.env.DIRECT_LINE_KEY || null;
```

To run the test, you will need a `directLineSecret` value, which is a token used for the bot connector authorization schema to make requests to the bot.

To get this value you will need that your bot is been deployed in Azure and have a [DirectLine Channel](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-channel-directline?view=azure-bot-service-4.0) configured.

The test gets the value from the process environment variables as it will be running in an [Azure DevOps pipeline](https://docs.microsoft.com/en-us/azure/devops/pipelines/get-started/what-is-azure-pipelines?view=azure-devops).



## Set up an Azure pipeline

This section will guide you through configuring a Azure pipeline that you can use to automatically build and test your code. 

### Prerequisites

- [Microsoft Azure](https://azure.microsoft.com/free/) subscription
- [git](https://git-scm.com/)
- Familiarity with [Azure CLI and ARM templates](https://docs.microsoft.com/azure/azure-resource-manager/resource-group-overview)

Before use the [Azure DevOps services](https://docs.microsoft.com/es-es/azure/devops/user-guide/what-is-azure-devops?view=azure-devops) to setup an [Azure pipeline](https://docs.microsoft.com/en-us/azure/devops/pipelines/get-started/what-is-azure-pipelines?view=azure-devops) to run the functional test, you must to have the `bot-functional-test` project source code in a GitHub repository.  

To create a GitHub repository

1. Add the next file into the `bot-functional-test` root directory

   **.gitignore**

   ```tex
   # Dependency directories
   node_modules/
   
   # Related to Teams Scenarios work
   *.zip
   *.vscode
   ```

2. Follow the next guides

   - [Creating a new repository](https://help.github.com/en/github/creating-cloning-and-archiving-repositories/creating-a-new-repository)
   - [Adding an existing project to GitHub using the command line](https://help.github.com/en/github/importing-your-projects-to-github/adding-an-existing-project-to-github-using-the-command-line)

   

To set up an Azure Pipeline

1. Create an Azure DevOps organization following the next [guide](https://docs.microsoft.com/en-us/azure/devops/pipelines/get-started/what-is-azure-pipelines?view=azure-devops), if you have one, you can skip this step.

2. Create an Azure DevOps project following the next [guide](https://docs.microsoft.com/en-us/azure/devops/organizations/projects/create-project?view=azure-devops&tabs=preview-page#create-a-project), if you have one, you can skip this step.

3. Create a new build pipeline. Then, select the ***use classic editor*** option. 

   ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/new-pipeline-use-classic.-editor.png)

4. Add the GitHub repository of the `functional-test` project. Then, click on **Empty job**

   - Note: You need to authorize the connection between Azure DevOps and GitHub repository. 
   
   ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/choose-repository-select-empty-job.png)

5. In the **Variables tab**, add the next variables: **AppId**, **AppSecret**, **BotName**

   - Note: The `AppId` and `AppSecret` values refers to an App Registration. You can create one using the portal [here](https://go.microsoft.com/fwlink/?linkid=2083908)

   ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/add-pipeline-variables.png)

   Set the variables **AppId**, **AppSecret** as locked variables. 

   ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/lock-sensitive-var-data.png)

7. Add an **Azure CLI** **task** to generate the *web.config* file necessary to deploy a bot source code to Azure. Configure the task with an *Azure subscription* and select the *script inline* options.

   The script looks likes:

   ```powershell
   call az bot prepare-deploy --code-dir "$(System.DefaultWorkingDirectory)/test-bot" --lang Javascript
   ```

   ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/prepare-to-deploy-task.png)

8. Add a **PowerShell** **task** to compress the bot source code. Configure the task with the *'Inline'* script option.

   The script looks likes:

   ```powershell
   $DirToCompress = "$(System.DefaultWorkingDirectory)/test-bot"
   $DirtoExclude =@("node_modules", "deploymentTemplates")
   $files = Get-ChildItem -Path $DirToCompress -Exclude $DirtoExclude
   $ZipFileResult ="$(System.DefaultWorkingDirectory)/test-bot.zip"
   Compress-Archive -Path $files -DestinationPath $ZipFileResult
   ```

   ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/compress-bot-source-code-task.png)

8. In the **Task tab**, Add an **Bot Deployment task** to deploy the bot zip file and connect it to the *DirectLine* channel. It will generate a *DirectLineCreate.json* file with a secret key. The key is used to start a conversation with the bot in the test logic. 

   Install the task extension following the next [guide](https://docs.microsoft.com/en-us/azure/devops/marketplace/install-extension?view=azure-devops&tabs=browser). Then, complete the next fields. 

   

   - **Azure Subscription**
     - Select an Azure Service Connection. This configuration allows to the Azure pipeline to create and manage Azure resources. You can follow this [guide](https://www.azuredevopslabs.com/labs/devopsserver/azureserviceprincipal/) to create an Azure service connection

   - **Resource Group**
     - Use the *BotName* pipeline variable create before. you can access to its content with the next syntax $("BotName")

   - **Location**
     - Select the location in where place the resources.

   - **Template**  
     - Add the path to the Deployment template file of the bot ` test-bot/deploymentTemplates/template.json` 

   - **Zipped Bot**
     - Add the path to the bot source code compressed before, As the zip file is in the root directory, you can just add the name of the file. `test-bot.zip`

   - **Channels**
     - Select the `Direct Line` channel. 

   ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/bot-deployment-task.png)

   Click on `“…”` next to the **Override Parameters** textbox. 
   
   In the **Override Template Parameters** popup window, click on **Add** button to get a new row.
   Then, complete grid with the next values. 
   
   | Name                      | Value        |
   | ------------------------- | ------------ |
   | appId                     | $(AppId)     |
   | appSecret                 | $(AppSecret) |
   | groupLocation             | centralus    |
   | groupName                 | $(BotName)   |
   | botId                     | $(BotName)   |
   | botSku                    | F0           |
   | newAppServicePlanName     | $(BotName)   |
   | newAppServicePlanLocation | centralus    |
   | newWebAppName             | $(BotName)   |
   
   Finally, click on **OK** to save the values.

9. Add the **PowerShell task** to read the *.json* file generated in the previous step and get the secret key to connect to the bot. Configure the task with the *'Inline'* script option.

   The script looks likes:

   ```powershell
   $json = Get-Content '$(System.DefaultWorkingDirectory)\DirectLineCreate.json' | Out-String | ConvertFrom-Json
   
   $key = $json.properties.properties.sites.key
   
   echo "##vso[task.setvariable variable=DIRECT_LINE_KEY;]$key"
   ```

   ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/get-direct-line-key-task.png)

10. Configure the Pipeline to run the *functional-tests*

    1. Add a Node Task
       1. Configure the *Version Spec* field to `10.x`
    2. Add NPM install task
       1. Use the default options
    3. Add NPM Custom command task
       1. Command: `custom`
       2. Command and arguments: `run functional-test`

    ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/run-functional-test-tasks.png)

11. After the Tests run, add a new **Azure CLI Task** to delete the resource group we've created.

    The script looks likes

    ```powershell
    call az group delete -n "$(BotName)" --yes
    ```

    ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/delete-resources-task.png)

    Is strongly recommend setting this task to run even if any of the previous tasks have failed or the build has been canceled. With this setting, we will ensure that the resources will be deleted from Azure even if the build fails at any step.

    ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/delete-resource-run-option-task.png)

12. Click on **save and queue** button to run the build pipeline.

    ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/save-and-queue.png)

    After running all the tasks, click on the NPM custom task. The log of the task displays the outcome of the functional test. 

    ![alt text](https://github.com/southworks/botbuilder-js/blob/add/deploy-bot-deploy-section/docs/functional-tests/images/pipeline-process-result.png)
