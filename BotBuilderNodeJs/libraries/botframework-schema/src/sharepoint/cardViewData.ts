// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ActionButton } from './actionButton';

/**
 * Sharepoint Card View Data object
 */
export class CardViewData {
    private actionButtons: [ActionButton];
    private primaryText: string;
    private description: string;
    /**
     * Initializes a new instance of the CardViewData class
     */
    public CardViewData(){
        // Do nothing
    }

    /**
     * Sets actionButtons property of type [ActionButton]
     */
    public set ActionButtons(actionButtons: [ActionButton]){
        this.actionButtons = actionButtons;
    }

    /**
     * Gets actionButtons property of type [ActionButton]
     */
    public get ActionButtons(): [ActionButton] {
        return this.actionButtons;
    }

    /**
     * Sets primaryText property of type string
     */
    public set PrimaryText(primaryText: string){
        this.primaryText = primaryText;
    }

    /**
     * Gets primaryText property of type string
     */
    public get PrimaryText(): string {
        return this.primaryText;
    }

    /**
     * Sets description property of type string
     */
    public set Description(description: string){
        this.description = description;
    }

    /**
     * Gets description property of type string
     */
    public get Description(): string {
        return this.description;
    }
}