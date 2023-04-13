// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { Action } from './action';

/**
 * Sharepoint action button
 */
export class ActionButton {
    private title: string;
    private action: Action;
    /**
     * Initializes a new instance of the ActionButton class
     */
    public ActionButton(){
        // Do nothing
    }

    /**
     * Sets title property of type string
     */
    public set Title(title: string){
        this.title = title;
    }

    /**
     * Gets title property of type string
     */
    public get Title(): string {
        return this.title;
    }

    /**
     * Sets parameters property of type Action
     */
    public set Action (action: Action){
        this.action = action;
    }

    /**
     * Gets parameters property of type Action
     */
    public get Action(): Action {
        return this.action; 
    }
}