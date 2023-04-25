// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ICardActionParameters } from './ICardActionParameters';

/**
 * Sharepoint action
 */
export class Action {
    private type: Action.ActionType;
    private parameters: ICardActionParameters;
    
    /**
     * Initializes a new instance of the Action class
     */
    public Action() {
        // Do nothing
    }

    /**
     * Sets type property of type Action.ActionType
     */
    public set Type(type: Action.ActionType) {
        this.type = type;
    }

    /**
     * Gets type property of type Action.ActionType
     */
    public get Type(): Action.ActionType {
        return this.type;
    }

    /**
     * Sets parameters property of type ICardActionParameters
     */
    public set Parameters (parameters: ICardActionParameters) {
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type ICardActionParameters
     */
    public get Parameters(): ICardActionParameters {
        return this.parameters; 
    }
}

export namespace Action {
    export enum ActionType {
        QuickView = 'QuickView',
        Submit = 'Submit',
        ExternalLink = 'ExternalLink',
        SelectMedia = 'VivaAction.SelectMedia',
        GetLocation = 'VivaAction.GetLocation',
        ShowLocation = 'VivaAction.ShowLocation',
        Execute = 'Execute'
    }
}