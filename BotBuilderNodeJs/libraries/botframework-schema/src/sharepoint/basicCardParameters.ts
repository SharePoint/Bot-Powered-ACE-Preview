// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { BaseCardParameters } from './baseCardParameters';

/**
 * Sharepoint basic card view parameters
 */
export class BasicCardParameters extends BaseCardParameters {
    private primaryText: string;

    /**
     * Initializes a new instance of the BasicCardParameters class
     */
    public BasicCardParameters() {
        // Do nothing
    }

    /**
     * Sets primary text property of type string
     */
    public set PrimaryText(primaryText: string) {
        this.primaryText = primaryText;
    }

    /**
     * Gets primary text property of type string
     */
    public get PrimaryText(): string {
        return this.primaryText;
    }
}