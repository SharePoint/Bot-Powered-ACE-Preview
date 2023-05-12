// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { Action } from './action';

/**
 * Sharepoint action button
 */
export class ActionButton {
    private title: string;
    private action: Action;
    private id: string;
    private style: ActionButton.ActionStyle;
    
    /**
     * Initializes a new instance of the ActionButton class
     */
    public ActionButton() {
        // Do nothing
    }

    /**
     * Sets title property of type string
     */
    public set Title(title: string) {
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
    public set Action (action: Action) {
        this.action = action;
    }

    /**
     * Gets parameters property of type Action
     */
    public get Action(): Action {
        return this.action; 
    }

    /**
     * Sets id property of type string
     */
    public set Id (id: string) {
        this.id = id;
    }

    /**
     * Gets id property of type string
     */
    public get Id(): string {
        return this.id; 
    }

    /**
     * Sets style property of type string
     */
    public set Style (style: ActionButton.ActionStyle) {
        this.style = style;
    }

    /**
     * Gets style property of type string
     */
    public get Style(): ActionButton.ActionStyle {
        return this.style; 
    }
}

export namespace ActionButton
{
    export enum ActionStyle {
        Default = "default",
        Positive = "positive",
        Destructive = "destructive"
    }
}