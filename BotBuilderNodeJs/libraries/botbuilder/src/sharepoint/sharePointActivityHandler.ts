/**
 * @module botbuilder
 */
/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import {
    ActivityHandler,
    InvokeResponse,
    TaskModuleRequest,
    TaskModuleResponse,
    TurnContext
} from 'botbuilder-core';

/**
 * The SharePointActivityHandler is derived from ActivityHandler. It adds support for
 * the SharePoint specific events and interactions
 */
export class SharePointActivityHandler extends ActivityHandler {
    /**
     * Invoked when an invoke activity is received from the connector.
     * Invoke activities can be used to communicate many different things.
     * @param context A strongly-typed context object for this turn
     * @returns A task that represents the work queued to execute
     * 
     * Invoke activities communicate programmatic commands from a client or channel to a bot.
     */
    protected async onInvokeActivity(context: TurnContext): Promise<InvokeResponse> {
        try {
            if (!context.activity.name && context.activity.channelId === 'sharepoint') {
                throw new Error('NotImplemented');
            } else {
                switch (context.activity.value.activity) {
                    case 'cardView':
                        return ActivityHandler.createInvokeResponse(
                            await this.OnSharePointTaskGetCardViewAsync(context, (context.activity.value as TaskModuleRequest))
                        );

                    case 'quickView':
                        await this.OnSharePointTaskGetQuickViewAsync(context, (context.activity.value as TaskModuleRequest));
                        return ActivityHandler.createInvokeResponse();

                    case 'propertyPaneConfiguration':
                        return ActivityHandler.createInvokeResponse(
                            await this.OnSharePointTaskGetPropertyPaneConfigurationAsync(context, (context.activity.value as TaskModuleRequest))
                        );

                    case 'setAceProperties':
                        return ActivityHandler.createInvokeResponse(
                            await this.OnSharePointTaskSetPropertyPaneConfigurationAsync(context, (context.activity.value as TaskModuleRequest))
                        );

                    default:
                        return super.onInvokeActivity(context);
                }
            }
        } catch (err) {
            if (err.message === 'NotImplemented') {
                return { status: 501 };
            } else if (err.message === 'BadRequest') {
                return { status: 400 };
            }
            throw err;
        }
    }

    /**
     * Override this in a derived class to provide logic for when a card view is fetched
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    protected async OnSharePointTaskGetCardViewAsync(context: TurnContext, taskModuleRequest: TaskModuleRequest): Promise<TaskModuleResponse>{
        throw new Error('NotImplemented');
    }

    /**
     * Override this in a derived class to provide logic for when a quick view is fetched
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    protected async OnSharePointTaskGetQuickViewAsync(context: TurnContext, taskModuleRequest: TaskModuleRequest): Promise<TaskModuleResponse>{
        throw new Error('NotImplemented');
    }

    /**
     * Override this in a derived class to provide logic for getting configuration pane properties.
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    protected async OnSharePointTaskGetPropertyPaneConfigurationAsync(context: TurnContext, taskModuleRequest: TaskModuleRequest): Promise<TaskModuleResponse>{
        throw new Error('NotImplemented');
    }

    /**
     * Override this in a derived class to provide logic for setting configuration pane properties.
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    protected async OnSharePointTaskSetPropertyPaneConfigurationAsync(context: TurnContext, taskModuleRequest: TaskModuleRequest): Promise<TaskModuleResponse>{
        throw new Error('NotImplemented');
    } 
}