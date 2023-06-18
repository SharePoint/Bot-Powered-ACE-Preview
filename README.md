# SPFx Bot Powered Adaptive Card Extensions for Viva Connections (Preview)

Welcome to the Bot-Driven Adaptive Card Extension preview program!

<h2>Why Bot-Driven ACEs?</h2>
<h6>Problem Statement</h6>
Currently, the model for building an ACE works for developers that can both invest resources to build a solution from scratch and possess JavaScript and TypeScript knowledge. As a result, developers who have already invested in bot solutions for Microsoft 365 and are more specialized in server-side programming languages, such as Java or C#, are automatically excluded from being able to reap the benefits of an ACE. <br><br>

In addition, currently, within SharePoint, there's no way to provide a seamless silent sign-on (SSO) experience where the solution uses external or third party identity providers (IDPs). This limits the third party resources a developer can access or utilize through an ACE.  

<h6>The Solution: Bot-Driven ACEs</h6>
Bot-Driven ACEs reduce the requirements needed to develop cards from scratch and allow bot developers to integrate their existing investments on a bot to create an ACE. <br><br>

Furthermore, Bot-Driven ACEs prevent bots and other third party code from accessing data from other bots by hosting the bot responses to SharePoint Viva Connections in an iFrame.

<h2>Before Getting Started</h2>

For more information on the bot builder framework, see the [bot builder framework repository](https://github.com/Microsoft/botframework-sdk).

Furthermore, to get started, ensure the account you use to configure the bot is in the same Azure subscription as your tenant.<br>

Note: we're using a fork of the BotBuilder framework due to anticipating changes in the Bot-Driven ACE schema based on feedback during the preview period. Before this feature hits GA, we'll finalize the schema and work with the BotBuilder framework team to merge our changes.  

<h2>Repository Folder Structure</h2>
<ul>
    <li>
        BotBuilder DotNet
        <ul>
            <li>This folder contains a fork of the BotBuilder framework for the CSharp language</li>
            <li>Ignore the ReadMe in this directory and use the instructions in the hands-on-labs as the single source of truth instructions to get started. </li>
        </ul>
    </li>
    <li>
        BotBuilderNodeJs
        <ul>
            <li>This folder contains a fork of the BotBuilder framework for the JavaScript language</li>
            <li>Ignore the ReadMe in this directory and use the instructions in the hands-on-labs as the single source of truth instructions to get started. </li>
        </ul>
    </li>
    <li>
        labs
        <ul>
            <li>This folder contains the available hands-on-labs that will serve as tutorials to key features of Bot-Driven ACEs. The labs are available in both CSharp and JavaScript.</li>
            <li>Follow the instruction of the labs available <a href='https://github.com/SharePoint/BotPoweredACEPreview/wiki'>here</a> to get started. We'll use also this wiki for sharing any updates, which will also be shared with you in direct messages to reduce the impact of potential breaking changes for your testing.</li>
        </ul>
    </li>
</ul>

<h2>Feedback</h2>

When you find something that looks like a bug or you'd like to provide feedback for the engineering - use the https://github.com/SharePoint/BotPoweredACEPreview/issues list for getting your feedback and findings shared with the engineering.

Due to the time zone differences, there might be small delays on getting you a response, but we'll do the best on our side.

 **Your input is invaluable for Microsoft** around these features.

Thank you 👏🚀

Sharing is caring 🧡

> Looking to engage with others who build experiences for hte Microsoft 365 platfrom - join our [Microsoft 365 platform community calls]() and take advantage of our community and open-source efforts, like [>1500 open-source samples]() and [different community projects]().
