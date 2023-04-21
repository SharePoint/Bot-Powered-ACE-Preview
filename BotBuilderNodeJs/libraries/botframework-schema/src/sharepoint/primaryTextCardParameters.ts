// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { BaseCardParameters } from './baseCardParameters';
import { ICardParameters } from './ICardParameters';

/**
 * Sharepoint primary text card view parameters
 */
export class PrimaryTextCardParameters extends BaseCardParameters implements ICardParameters {
    private primaryText: string;
    private description: string;

    /**
     * Initializes a new instance of the PrimaryTextCardParameters class
     */
    public PrimaryTextCardParameters(){
        // Do nothing
    }

    /**
     * Sets primary text property of type string
     */
    public set PrimaryText(primaryText: string){
        this.primaryText = primaryText;
    }

    /**
     * Gets primary text property of type string
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