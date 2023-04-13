// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ActionParameters } from './actionParameters';

/**
 * Sharepoint action
 */
export class Action {
    private type: string;
    private parameters: ActionParameters;
    /**
     * Initializes a new instance of the Action class
     */
    public Action(){
        // Do nothing
    }

    /**
     * Sets type property of type string
     */
    public set Type(type: string){
        this.type = type;
    }

    /**
     * Gets type property of type string
     */
    public get Type(): string {
        return this.type;
    }

    /**
     * Sets parameters property of type ActionParameters
     */
    public set Parameters (parameters: ActionParameters){
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type ActionParameters
     */
    public get Parameters(): ActionParameters {
        return this.parameters; 
    }
}